using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Budgeter.API.Services
{
    public class QueryStringBuilder<T>
    {
        private string _baseQuery;
        public QueryStringBuilder(string baseQuery)
        {
            _baseQuery = baseQuery;
        }
    }
    public class WhereQuery<T>
    {
        public WhereQuery()
        {
            Expressions = new List<Expression>();
        }
        public IList<Expression> Expressions { get; }

        public Expression Equals<TMember>(Expression<Func<T, TMember>> memberExpr, TMember value)
        {
            Expressions.Add(Expression.Equal(memberExpr.Body, Expression.Constant(value)));
            return Expressions.LastOrDefault();
        }

        public Expression Equals<TMember>(Expression<Func<T, TMember>> memberExpr, Expression<Func<T, TMember>> memberExpr2)
        {
            Expressions.Add(Expression.Equal(memberExpr.Body, memberExpr2.Body));
            return Expressions.LastOrDefault();
        }
    }
}
