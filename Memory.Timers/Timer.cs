using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Memory.Timers {
	public class Timer {
		private static readonly List<Tuple<string, DisposableWatch>> watches = new List<Tuple<string, DisposableWatch>>();

		public static string Report {
			get {
				var rep = "";
				DisposableWatch parent = null;
				long lvlTime = 0;
				foreach (var pair in watches) {
					var watch = pair.Item2;
					var pad = "";
					var p = watch.Parent;
					while (p != null) {
						pad += "    ";
						p = p.Parent;
					}

					if (watch == parent) {
						rep += $"{(pad + "    Rest").PadRight(20)}: {watch.Time - lvlTime}\n";
					}

					if (watch.Parent != parent) {
						lvlTime = 0;
					}

					parent = watch.Parent;
					if (pair.Item1 == "st") {
						rep += $"{(pad + watch.Name).PadRight(20)}: {watch.Time}\n";
						lvlTime += watch.Time;
					}
				}

				watches.Clear();

				return rep;
			}
		}

		public static IDisposable Start(string name = "*") {
			var watch = new DisposableWatch(new Stopwatch(), watches, name);
			watches.Add(Tuple.Create("st", watch));
			return watch;
		}
	}

	internal class DisposableWatch : IDisposable {
		private readonly Stopwatch watch;
		private readonly List<Tuple<string, DisposableWatch>> watches;
		private bool isDisposed;

		public long Time { get; private set; }
		public DisposableWatch Parent { get; private set; }
		public string Name { get; }

		public DisposableWatch(Stopwatch watch, List<Tuple<string, DisposableWatch>> watches, string name) {
			this.watch = watch;
			this.watches = watches;
			Name = name;
			watch.Start();
		}

		public void Dispose() {
			if (isDisposed) {
				return;
			}

			isDisposed = true;

			Time = watch.ElapsedMilliseconds;
			if (watches.Count == 1) {
				return;
			}

			int n = 0;
			for (int i = 0; i < watches.Count; i++) {
				if (watches[i].Item2 == this) {
					n = i;
				}
			}

			for (int i = n - 1; i >= 0; i--) {
				var w = watches[i].Item2;
				if (!w.isDisposed) {
					Parent = w;
					break;
				}
			}

			watches.Add(Tuple.Create("end", this));
		}
	}
}