using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersAPI_Test3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersAPI_Test3
{
    [TestClass]
    public class ApiTestCategory
    {
        private ApiLogicCategory _apiLogicCategory;
        public ApiTestCategory()
        {
            _apiLogicCategory = new ApiLogicCategory();
        }

        [TestMethod]
        public void GetCategories()
        {
            _apiLogicCategory.GetCategoriesList_Api();
            _apiLogicCategory.GetCategoriesList_DB();
            Assert.IsTrue(_apiLogicCategory.ValidateCategories_Api_vs_DB());

        }

        [TestMethod]
        public void InsertNewCategory()
        {
            _apiLogicCategory.GenerateToken();
            Categories newCategoryApi = _apiLogicCategory.InsertNewCategoryAPI();
            _apiLogicCategory.ValidateNewCategory(newCategoryApi);
            _apiLogicCategory.DeleteCategoryDB(newCategoryApi);
        }

    }
}
