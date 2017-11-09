using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OrdersAPI_Test3.Helpers
{
    public class ClientApiHelper
    {
        private Uri _baseUri;
        private string _token;

        public ClientApiHelper(string uri)
        {
            _baseUri = new Uri(uri);
        }
        public JArray GetAsync(string partialUri)
        {
            JArray result = new JArray();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(_baseUri + partialUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var responseString = responseContent.ReadAsStringAsync().Result;
                    result = JArray.Parse(responseString);
                }
                else
                {
                    Assert.Fail("Test Failed response: " + response);
                }
            }
            return result;
        }

        public void GetToken(string _username, string _password, string partialUri = "token")
        {
            dynamic jsonResponse;
            Uri baseAddress = new Uri(_baseUri.AbsoluteUri);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = baseAddress;
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(baseAddress, partialUri)))
                {
                    request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "grant_type", "password" },
                        { "username", _username},
                        { "password", _password }
                    });
                    var respose = httpClient.PostAsync(request.RequestUri, request.Content).Result;
                    string result = respose.Content.ReadAsStringAsync().Result;
                    jsonResponse = JObject.Parse(result);
                }
            }
            _token = jsonResponse["access_token"].ToString();
        }

        public dynamic PostAsync(string partialUri, Dictionary<string, string> parameters, bool needToken)
        {
            dynamic jsonResponse = null;
            using (HttpClient httpClient = new HttpClient())
            {
                if (needToken)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                
                //need token
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _baseUri + partialUri))
                {
                    request.Content = new FormUrlEncodedContent(parameters);
                    var response = httpClient.PostAsync(request.RequestUri, request.Content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        jsonResponse = JObject.Parse(result);
                    }
                    else
                    {
                        //Assert.Fail("Test Failed: "+ response);
                        return response.StatusCode.ToString();
                    }
                }
            }
            return jsonResponse;
        }
    }
}
