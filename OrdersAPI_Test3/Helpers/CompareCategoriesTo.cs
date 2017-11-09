using OrdersAPI_Test3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersAPI_Test3.Helpers
{
    public class CompareCategoriesTo : IEqualityComparer<Categories>
    {
        public bool Equals(Categories x, Categories y)
        {
            return x.Id.Equals(y.Id) && x.Description.Equals(y.Description);
        }

        public int GetHashCode(Categories obj)
        {
            throw new NotImplementedException();
        }
    }
}
