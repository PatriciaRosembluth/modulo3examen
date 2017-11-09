using OrdersAPI_Test3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersAPI_Test3.Helpers
{
    public class CompareProductsTo : IEqualityComparer<Products>
    {
        public bool Equals(Products x, Products y)
        {
            return x.Id.Equals(y.Id) && x.Name.Equals(y.Name) && x.Price.Equals(y.Price) && x.Rank.Equals(y.Rank) && x.Category_Id.Equals(y.Category_Id);
        }

        public int GetHashCode(Products obj)
        {
            throw new NotImplementedException();
        }
    }
}
