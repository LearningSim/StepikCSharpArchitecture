using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Reflection.Differentiation {
	public class Algebra {
		public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> func) {
			return Expression.Lambda<Func<double, double>>(Differentiate(func.Body, func.Parameters[0]), func.Parameters[0]);
		}

		public static Expression Differentiate(Expression body, ParameterExpression zed) {
			if (body.NodeType == ExpressionType.Constant) {
				return Expression.Constant(0.0);
			}

			if (body.NodeType == ExpressionType.Parameter) {
				return Expression.Constant(1.0);
			}
			
			var binary = body as BinaryExpression;
			if (binary != null) {
				if (body.NodeType == ExpressionType.Add) {
					return Expression.Add(Differentiate(binary.Left, zed), Differentiate(binary.Right, zed));
				}

				if (body.NodeType == ExpressionType.Multiply) {
					if (binary.Left.NodeType == binary.Right.NodeType && binary.Right.NodeType == ExpressionType.Parameter) {
						return Expression.Multiply(Expression.Constant(2.0), zed);
					}

					return ProcessCommutatively(
						binary,
						(l, r) => {
							if (l.NodeType == ExpressionType.Constant && r.NodeType == ExpressionType.Parameter) {
								return l;
							}

							if (l.NodeType == ExpressionType.Multiply) {
								var last = Expression.Multiply(l, r);
								var b = CrackMultiply(last);
								double factor = 1;
								int zCount = 0;
								while (b != null) {
									if (b.NotMultiply.NodeType == ExpressionType.Constant) {
										factor *= GetValue((ConstantExpression) b.NotMultiply);
									}
									else if (b.NotMultiply.NodeType == ExpressionType.Parameter) {
										zCount++;
									}

									last = b.Multiply;
									b = CrackMultiply(b.Multiply);
								}

								if (last.Left.NodeType == ExpressionType.Constant) {
									factor *= GetValue((ConstantExpression) last.Left);
								}
								else if (last.Left.NodeType == ExpressionType.Parameter) {
									zCount++;
								}

								if (last.Right.NodeType == ExpressionType.Constant) {
									factor *= GetValue((ConstantExpression) last.Right);
								}
								else if (last.Right.NodeType == ExpressionType.Parameter) {
									zCount++;
								}

								Expression multiplier = zed;
								for (int i = 0; i < zCount - 2; i++) {
									multiplier = Expression.Multiply(multiplier, zed);
								}

								return Expression.Multiply(Expression.Constant(factor * zCount), multiplier);
							}

							return null;
						}
					);
				}
			}

			var method = body as MethodCallExpression;
			if (method != null) {
				var arg = method.Arguments[0];
				var left = Differentiate(arg, zed);
				if (method.Method.Name == "Cos") {
					var sin = Expression.Call(
						new Func<double, double>(Math.Sin).GetMethodInfo(),
						arg
					);
					var prod = Expression.Multiply(left, Expression.Negate(sin));
					return prod;
				}

				if (method.Method.Name == "Sin") {
					var cos = Expression.Call(
						new Func<double, double>(Math.Cos).GetMethodInfo(),
						arg
					);
					return Expression.Multiply(left, cos);
				}
			}

			return zed;
		}

		private static Expression
			ProcessCommutatively(BinaryExpression binary, Func<Expression, Expression, Expression> func) {
			return func(binary.Left, binary.Right) ?? func(binary.Right, binary.Left);
		}

		private static CrackedMultiply CrackMultiply(BinaryExpression binary) {
			if (binary.Left.NodeType == ExpressionType.Multiply) {
				return new CrackedMultiply((BinaryExpression) binary.Left, binary.Right);
			}

			if (binary.Right.NodeType == ExpressionType.Multiply) {
				return new CrackedMultiply((BinaryExpression) binary.Right, binary.Left);
			}

			return null;
		}

		private static double GetValue(ConstantExpression exp) {
			return (double) exp.Value;
		}
	}

	class CrackedMultiply {
		public BinaryExpression Multiply { get; }
		public Expression NotMultiply { get; }

		public CrackedMultiply(BinaryExpression multiply, Expression notMultiply) {
			Multiply = multiply;
			NotMultiply = notMultiply;
		}
	}
}