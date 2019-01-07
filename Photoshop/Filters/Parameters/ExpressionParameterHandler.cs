using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyPhotoshop {
	public class ExpressionParameterHandler<TParameters> : IParemeterHandler<TParameters>
		where TParameters : IParameters, new() {
		private static readonly ParameterInfo[] descriptions;
		private static Func<double[], TParameters> parser;

		static ExpressionParameterHandler() {
			descriptions = typeof(TParameters).GetProperties()
				.Select(p => Attribute.GetCustomAttribute(p, typeof(ParameterInfo)))
				.OfType<ParameterInfo>()
				.ToArray();

			CreateParser();
		}

		private static void CreateParser() {
			var type = typeof(TParameters);
			var newExpression = Expression.New(type);
			var vals = Expression.Parameter(typeof(double[]), "vals");
			var properties = type.GetProperties()
				.Where(p => Attribute.GetCustomAttribute(p, typeof(ParameterInfo)) != null)
				.ToArray();
			var bindings = new List<MemberBinding>();

			for (int i = 0; i < properties.Length; i++) {
				Expression call = Expression.ArrayIndex(
					vals,
					Expression.Constant(i)
				);
				var binding = Expression.Bind(properties[i], call);
				bindings.Add(binding);
			}

			var body = Expression.MemberInit(newExpression, bindings);
			var ex = Expression.Lambda<Func<double[], TParameters>>(
				body, vals
			);
			parser = ex.Compile();
		}

		public ParameterInfo[] GetDesсription() {
			return descriptions;
		}

		public TParameters CreateParameters(double[] values) {
			return parser(values);
		}
	}
}