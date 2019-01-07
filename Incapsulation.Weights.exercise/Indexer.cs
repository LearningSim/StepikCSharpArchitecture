using System;

namespace Incapsulation.Weights {
	public class Indexer {
		private double[] array;
		public double this[int index] {
			get {
				if(index < 0 || index >= Length) {
					throw new IndexOutOfRangeException();
				}
				return array[startIndex + index];
			}
			set {
				array[startIndex + index] = value;
			}
		}

		public int Length => len;
		private int startIndex;
		private int len;

		public Indexer(double[] array, int startIndex, int len) {
			this.array = array;
			this.startIndex = startIndex;
			this.len = len;

			if(startIndex < 0 || len < 0 || len > array.Length) {
				throw new ArgumentException();
			}
		}
	}
}
