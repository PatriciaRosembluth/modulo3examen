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
    public class ApiLogicCategory
    {
        private string baseUri = ConfigurationManager.AppSettings["BaseUri"] ?? "http://localhost:33991/";
        private string _username = ConfigurationManager.AppSettings["UserName"] ?? "user001@gmail.com";
        private string _password = ConfigurationManager.AppSettings["Password"] ?? "Password1.";
        private List<Categories> _categoryListApi;
        private List<Categories> _categoryListDB;

        private ClientApiHelper _clientApiHelper;
        private OrdersEntities db;
        public ApiLogicCategory()
        {
            _clientApiHelper = new ClientApiHelper(baseUri);
            db = new OrdersEntities();
        }

        public List<Categories> GetCategoriesList_Api()
        {
            JArray result = _clientApiHelper.GetAsync("api/Categories");
            _categoryListApi = new List<Categories>();
            foreach (var item in result)
            {
                _categoryListApi.Add(new Categories()
                {
                    Id = Convert.ToInt32(item["Id"]),
                    Description = item["Description"].ToString()
                });
            }
            return _categoryListApi;
        }

        public List<Categories> GetCategoriesList_DB()
        {
            _categoryListDB = db.Categories.ToList();
            return _categoryListDB;
        }

        public bool ValidateCategories_Api_vs_DB()
        {
            var result = _categoryListApi.SequenceEqual(_categoryListDB, new CompareCategoriesTo());
            return result;
        }

        public void GenerateToken()
        {
            _clientApiHelper.GetToken(_username, _password);
        }

        public Categories InsertNewCategoryAPI()
        {
            Categories _newCateogory = new Categories()
            { Description = "newCategory" + DateTime.Now.Millisecond };
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Description", _newCateogory.Description);

            dynamic data = _clientApiHelper.PostAsync("api/Categories", parameters, true);

            Categories categoryResult = new Categories();
            categoryResult.Id = data["Id"];
            categoryResult.Description = data["Description"];
            return categoryResult; 
        }

        public void ValidateNewCategory(Categories categoryApi)
        {
            Categories categoryDB = db.Categories.Find(categoryApi.Id);
            if (categoryDB == null)
            {
                Assert.Fail("Category was not found");
            }
           Assert.IsTrue(categoryApi.Description.Equals(categoryDB.Description), "Description is not equal");
        }

        public void DeleteCategoryDB(Categories categoryApi)
        {
            db.Categories.Remove(db.Categories.Find(categoryApi.Id));
            db.SaveChanges();
        }
    }
}
