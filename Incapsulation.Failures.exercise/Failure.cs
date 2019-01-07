using System;

namespace Incapsulation.Failures {
	public struct Failure {
		public Failure(FailureType type, int deviceId, DateTime date) {
			Type = type;
			DeviceId = deviceId;
			Date = date;
		}

		public bool IsSerious {
			get {
				return Type == FailureType.UnexpectedShutdown || Type == FailureType.HardwareFailure;
			}
		}

		public FailureType Type {
			get;
			private set;
		}

		public int DeviceId {
			get;
			private set;
		}

		public DateTime Date {
			get;
			private set;
		}
	}
}
