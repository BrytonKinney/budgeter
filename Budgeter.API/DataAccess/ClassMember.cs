using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Budgeter.API.DataAccess
{
    [Serializable]
    public class ClassMember<T>
    {
        public ClassMember(Expression<Func<T, object>> memberAccessor, string columnName)
        {
            Accessor = memberAccessor;
            MemberAccessDelegate = Accessor.Compile();
            ColumnName = columnName;
        }

        public Expression<Func<T, object>> Accessor { get; set; }
        public Func<T, object> MemberAccessDelegate { get; set; }
        public string ColumnName { get; set; }
    }
}
