using System.Linq.Expressions;

namespace DotNetCore.Helpers
{
    public class ExpressionHelper
    {
        public static Expression<Func<T, bool>>? MergeAnd<T>(List<Expression<Func<T, bool>>> expressions)
        {
            Expression<Func<T, bool>>? result = null;

            foreach (var exp in expressions)
            {
                if (result == null)
                {
                    result = exp;
                }
                else
                {
                    result = AndExpression2(result, exp);
                }

            }

            return result;
        }

        public static Expression<Func<T, bool>> AndExpression<T> //check which is better
                    (Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invoked = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.AndAlso(left.Body, invoked), left.Parameters);
        }

        public static Expression<Func<T, bool>> AndExpression2<T>( //check which is better
            Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var visitor = new ParameterReplaceVisitor()
            {
                Target = right.Parameters[0],
                Replacement = left.Parameters[0],
            };

            var rewrittenRight = visitor.Visit(right.Body);
            var andExpression = Expression.AndAlso(left.Body, rewrittenRight);
            return Expression.Lambda<Func<T, bool>>(andExpression, left.Parameters);
        }

        private class ParameterReplaceVisitor : ExpressionVisitor
        {
            public ParameterExpression? Target { get; set; }
            public ParameterExpression? Replacement { get; set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == Target ? Replacement : base.VisitParameter(node);
            }
        }
    }
}
