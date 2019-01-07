using System;

namespace Memory.API {
	public class APIObject : IDisposable {
		private readonly int id;
		private bool isDisposed = false;

		public APIObject(int id) {
			this.id = id;
			MagicAPI.Allocate(id);
		}

		~APIObject() {
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing) {
			if (!isDisposed) {
				if (disposing) {
				}

				MagicAPI.Free(id);
				isDisposed = true;
			}
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}