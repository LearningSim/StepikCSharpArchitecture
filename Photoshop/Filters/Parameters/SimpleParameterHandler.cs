using System;
using System.Linq;

namespace MyPhotoshop {
	public class SimpleParameterHandler<TParameters> : IParemeterHandler<TParameters> where TParameters : IParameters, new() {
		public ParameterInfo[] GetDesсription() {
			var props = typeof(TParameters).GetProperties();

			return props
				.Select(prop => Attribute.GetCustomAttribute(prop, typeof(ParameterInfo)))
				.OfType<ParameterInfo>()
				.ToArray();
		}

		public TParameters CreateParameters(double[] values) {
			var parameters = new TParameters();
			var props = parameters.GetType().GetProperties()
				.Where(p => Attribute.GetCustomAttribute(p, typeof(ParameterInfo)) != null)
				.ToList();

			if (values.Length != props.Count) {
				throw new ArgumentException("Неверное число параметров");
			}

			for (int i = 0; i < props.Count; i++) {
				props[i].SetValue(parameters, values[i], null);
			}

			return parameters;
		}
	}
}