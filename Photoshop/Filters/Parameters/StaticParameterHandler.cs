using System;
using System.Linq;
using System.Reflection;

namespace MyPhotoshop {
	public class StaticParameterHandler<TParameters> : IParemeterHandler<TParameters> where TParameters : IParameters, new() {
		private static readonly PropertyInfo[] properties;
		private static readonly ParameterInfo[] descriptions;

		static StaticParameterHandler() {
			properties = typeof(TParameters).GetProperties()
				.Where(p => Attribute.GetCustomAttribute(p, typeof(ParameterInfo)) != null)
				.ToArray();

			descriptions = properties
				.Select(p => Attribute.GetCustomAttribute(p, typeof(ParameterInfo)))
				.OfType<ParameterInfo>()
				.ToArray();
		}

		public ParameterInfo[] GetDesсription() {
			return descriptions;
		}

		public TParameters CreateParameters(double[] values) {
			if (values.Length != properties.Length) {
				throw new ArgumentException("Неверное число параметров");
			}

			var parameters = new TParameters();
			for (int i = 0; i < properties.Length; i++) {
				properties[i].SetValue(parameters, values[i], null);
			}

			return parameters;
		}
	}
}