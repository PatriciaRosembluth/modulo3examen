using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OrdersAPI_Test3.Helpers;
using OrdersAPI_Test3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersAPI_Test3
{
    public class ApiLogicProduct
    {
        private string baseUri = ConfigurationManager.AppSettings["BaseUri"] ?? "http://localhost:33991/";
        private string _username = ConfigurationManager.AppSettings["UserName"] ?? "user001@gmail.com";
        private string _password = ConfigurationManager.AppSettings["Password"] ?? "Password1.";
        private List<Products> _productListApi;
        private List<Products> _productListDB;

        private ClientApiHelper _clientApiHelper;
        private OrdersEntities db;

        public ApiLogicProduct()
        {
            _clientApiHelper = new ClientApiHelper(baseUri);
            db = new OrdersEntities();
        }

        public List<Products> GetProductsList_Api()
        {
            JArray result = _clientApiHelper.GetAsync("api/Products");
            _productListApi = new List<Products>();
            foreach (var item in result)
            {
                _productListApi.Add(new Products()
                {
                    Id = Convert.ToInt32(item["Id"]),
                    Name = item["Name"].ToString(),
                    Price = Convert.ToDecimal(item["Price"]),
                    Rank = Convert.ToInt32(item["Rank"]),
                    Category_Id = Convert.ToInt32(item["Category_Id"])
                });
            }
            return _productListApi;
        }

        public List<Products> GetProductsList_DB()
        {
            _productListDB = db.Products.ToList();
            return _productListDB;
        }

        public bool ValidateProducts_Api_vs_DB()
        {
            var result = _productListApi.SequenceEqual(_productListDB, new CompareProductsTo());
            return result;
        }
        public void GenerateToken()
        {
            _clientApiHelper.GetToken(_username, _password);
        }

        public Products InsertNewProductAPI()
        {
            Products _newProduct = new Products()
            { Name = "newProduct" + DateTime.Now.Millisecond,
              Price = 5,
              Rank = 3,
              Category_Id = 2};
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Name", _newProduct.Name);
            parameters.Add("Price", _newProduct.Price.ToString());
            parameters.Add("Rank", _newProduct.Rank.ToString());
            parameters.Add("Category_Id", _newProduct.Category_Id.ToString());

            dynamic data = _clientApiHelper.PostAsync("api/Products", parameters, true);

            Products productResult = new Products();
            productResult.Id = data["Id"];
            productResult.Name = data["Name"];
            productResult.Price = data["Price"];
            productResult.Rank = data["Rank"];
            productResult.Category_Id = data["Category_Id"];
            
            return productResult;
        }

        public void ValidateNewProduct(Products productApi)
        {
            Products productDB = db.Products.Find(productApi.Id);
            if (productDB == null)
            {
                Assert.Fail("Product was not found");
            }
            Assert.IsTrue(productApi.Name.Equals(productDB.Name), "Description is not equal");
        }

        public void DeleteProductDB(Products productApi)
        {
            db.Products.Remove(db.Products.Find(productApi.Id));
            db.SaveChanges();
        }
    }
}
