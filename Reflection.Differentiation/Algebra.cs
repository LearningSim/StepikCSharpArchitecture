using System.Linq.Expressions;
using System.Reflection;

namespace Reflection.Differentiation;

public class Algebra
{
    public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function) =>
        Expression.Lambda<Func<double, double>>(Differentiate(function.Body), function.Parameters);

    private static Expression Differentiate(Expression exp) => exp switch
    {
        ConstantExpression => Expression.Constant(0d),
        ParameterExpression => Expression.Constant(1d),
        BinaryExpression { NodeType: ExpressionType.Add } add => Expression.Add(
            Differentiate(add.Left),
            Differentiate(add.Right)
        ),
        BinaryExpression { NodeType: ExpressionType.Multiply } mult => Expression.Add(
            Expression.Multiply(Differentiate(mult.Left), mult.Right),
            Expression.Multiply(mult.Left, Differentiate(mult.Right))
        ),
        MethodCallExpression call => call.Method.Name switch
        {
            "Sin" => Expression.Multiply(
                Differentiate(call.Arguments[0]),
                Call(Math.Cos, call.Arguments[0])
            ),
            "Cos" => Expression.Multiply(
                Differentiate(call.Arguments[0]),
                Expression.Negate(Call(Math.Sin, call.Arguments[0]))
            ),
            _ => throw new ArgumentException(call.Method.Name)
        },
        _ => throw new ArgumentException(exp.ToString())
    };

    private static Expression Call(Func<double, double> func, Expression arg) =>
        Expression.Call(func.GetMethodInfo(), arg);
}