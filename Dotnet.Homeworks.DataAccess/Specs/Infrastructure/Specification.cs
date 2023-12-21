using System.Linq.Expressions;

namespace Dotnet.Homeworks.DataAccess.Specs.Infrastructure;

public class Specification<T> : IQueryableFilter<T> where T : class
{
    protected Expression<Func<T, bool>> SpecExpression { get; }

    public Specification(Expression<Func<T, bool>> specExpression)
    {
        SpecExpression = specExpression;
    }

    public IQueryable<T> Apply(IQueryable<T> query)
        => query.Where(SpecExpression);

    public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
            => spec.SpecExpression;

    public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
        => new Specification<T>(Or(spec1.SpecExpression, spec2.SpecExpression));

    public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
        => new Specification<T>(And(spec1.SpecExpression, spec2.SpecExpression));

    private static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        => Compose(a, b, Expression.OrElse);

    private static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        => Compose(a, b, Expression.AndAlso);

    private static Expression<T> Compose<T>(Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
        var bindedBody = ParameterRebinder.ReplaceParameters(
            first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f), 
            second.Body);

        return Expression.Lambda<T>(merge(first.Body, bindedBody), first.Parameters);
    }

    private class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _bindMap;

        ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _bindMap = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_bindMap.TryGetValue(p, out var replacement))
                p = replacement;

            return base.VisitParameter(p);
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
    }
}