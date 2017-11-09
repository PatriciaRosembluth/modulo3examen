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
    public class ApiTestProduct
    {
        private ApiLogicProduct _apiLogicProduct;
        public ApiTestProduct()
        {
            _apiLogicProduct = new ApiLogicProduct();
        }

        [TestMethod]
        public void GetProducts()
        {
            _apiLogicProduct.GetProductsList_Api();
            _apiLogicProduct.GetProductsList_DB();
            Assert.IsTrue(_apiLogicProduct.ValidateProducts_Api_vs_DB());

        }

        [TestMethod]
        public void InsertNewProduct()
        {
            _apiLogicProduct.GenerateToken();
            Products newProductApi = _apiLogicProduct.InsertNewProductAPI();
            _apiLogicProduct.ValidateNewProduct(newProductApi);
            _apiLogicProduct.DeleteProductDB(newProductApi);
        }

        [TestMethod]
        public void InsertNewProductWithoutToken()
        {
            var response = _apiLogicProduct.InsertNewProductWithoutTokenAPI();
            Assert.AreEqual(response, "Unauthorized");
        }
    }
}
