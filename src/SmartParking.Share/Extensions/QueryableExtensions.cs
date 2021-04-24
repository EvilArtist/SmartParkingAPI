using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Share.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PagedBy<T>(this IQueryable<T> query, int page, int pageSize)
        {
            int skip = Math.Max((page - 1) * pageSize, 0);
            int take = Math.Max(1, pageSize);
            return query.Skip(skip).Take(take);
        }
    }
}
