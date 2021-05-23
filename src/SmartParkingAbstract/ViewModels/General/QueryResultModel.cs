using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.General
{
    public class QueryResultModel<T>
    {
        public decimal Page { get; set; }
        public decimal TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }

        public static QueryResultModel<T> Empty()
        {
            return new QueryResultModel<T>(new List<T>());
        }

        public QueryResultModel(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}
