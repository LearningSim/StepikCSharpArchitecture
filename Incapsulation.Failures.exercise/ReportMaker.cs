using System;
using System.Collections.Generic;

namespace Incapsulation.Failures {

	public class Common {
		public static bool Earlier(DateTime v, DateTime date) {
			if (v.Year != date.Year) {
				return v.Year < date.Year;
			}
			if (v.Month != date.Month) {
				return v.Month < date.Month;
			}
			return v.Day < date.Day;
		}
	}

	public class ReportMaker {
		public static List<string> FindDevicesFailedBeforeDateObsolete(
			int day,
			int month,
			int year,
			int[] failureTypes,
			int[] deviceId,
			object[][] times,
			List<Dictionary<string, object>> devices) {

			List<Failure> failures = new List<Failure>();
			for (int i = 0; i < failureTypes.Length; i++) {
				failures.Add(new Failure(
					(FailureType)failureTypes[i],
					deviceId[i],
					new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0])
				));
			}

			List<Device> devs = new List<Device>();
			foreach (var device in devices) {
				devs.Add(new Device (
					(int)device["DeviceId"],
					device["Name"] as string
				));
			}

			return FindDevicesFailedBeforeDate(new DateTime(year, month, day), failures, devs);
		}

		public static List<string> FindDevicesFailedBeforeDate(DateTime date, List<Failure> failures, List<Device> devices) {
			var problematicDevices = new HashSet<int>();
			foreach (var failure in failures) {
				if (failure.IsSerious && Common.Earlier(failure.Date, date)) {
					problematicDevices.Add(failure.DeviceId);
				}
			}

			var result = new List<string>();
			foreach (var device in devices) {
				if (problematicDevices.Contains(device.Id)) {
					result.Add(device.Name);
				}
			}
			return result;
		}

	}
}
