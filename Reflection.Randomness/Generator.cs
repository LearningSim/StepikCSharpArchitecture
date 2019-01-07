using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reflection.Randomness {
	[AttributeUsage(AttributeTargets.Property)]
	public class FromDistribution : Attribute {
		public IContinousDistribution Distribution { get; }

		public FromDistribution(Type type, params object[] args) {
			try {
				Distribution = (IContinousDistribution) Activator.CreateInstance(type, args);
			}
			catch (MissingMethodException) {
				throw new ArgumentException($"Ctor with parameters {string.Join(", ", args)} for type {type} not found");
			}
		}
	}

	public class Generator<T> where T : new() {
		private readonly List<Update<T>> updates = new List<Update<T>>();

		public T Generate(Random rnd) {
			var props = typeof(T).GetProperties()
				.Where(p => Attribute.IsDefined(p, typeof(FromDistribution)))
				.ToArray();

			var attrs = props
				.Select(prop => Attribute.GetCustomAttribute(prop, typeof(FromDistribution)))
				.OfType<FromDistribution>()
				.ToArray();

			var generated = new T();
			for (int i = 0; i < props.Length; i++) {
				props[i].SetValue(generated, attrs[i].Distribution.Generate(rnd));
			}

			foreach (var update in updates) {
				if (update.Property.CanWrite) {
					update.Property.SetValue(generated, update.Distribution.Generate(rnd));
				}
			}

			return generated;
		}

		public Update<T> For(Expression<Func<T, double>> property) {
			var update = new Update<T>(this, property);
			updates.Add(update);
			return update;
		}
	}

	public class Update<T> where T : new() {
		public PropertyInfo Property { get; }
		public IContinousDistribution Distribution { get; private set; }

		public Generator<T> Generator { get; set; }

		public Update(Generator<T> generator, Expression<Func<T, double>> property) {
			var exp = property.Body as MemberExpression;
			// exp.Member is property itself,
			// exp.Expression is an object which contains exp.Member
			if (exp == null || exp.Expression.Type != typeof(T)) {
				throw new ArgumentException("Incorrect lambda");
			}

			Property = (PropertyInfo) exp.Member;
			Generator = generator;
		}

		public Generator<T> Set(IContinousDistribution distribution) {
			Distribution = distribution;
			return Generator;
		}
	}
}