using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Budgeter.API.DataAccess
{
    public class DataMapper<T>
    {
        public Lazy<HashSet<ClassMember<T>>> Mappings { get; } = new Lazy<HashSet<ClassMember<T>>>(() =>
                                                                        {
                                                                            return new HashSet<ClassMember<T>>();
                                                                        });

        protected void Column<TMember>(Expression<Func<T, object>> member, string columnName)
        {
            Mappings.Value.Add(new ClassMember<T>(member, columnName));
        }
    }
}
