using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace AddressBookJsonServer
{
    [TestClass]
    public class RestSharpTest
    {
        RestClient client;
        [TestInitialize]
        public void SetUp() {
            client = new RestClient("http://localhost:4000");
        }
        public class AddressBook
        {
            public string name { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
        }
        public IRestResponse GetAddressBook()
        {
            RestRequest request = new RestRequest("/AddressBook/list", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
        /// Addressbook data are retrieved from the json file
        /// and are stored in a list. The list count matches with the
        /// number of data
        [TestMethod]
        public void OnCallingGetAddressBook()
        {
            IRestResponse response = GetAddressBook();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            List<AddressBook> addresses = JsonConvert.DeserializeObject<List<AddressBook>>(response.Content);
            Assert.AreEqual(4, addresses.Count);
            addresses.ForEach(addressData =>
            {
                Console.WriteLine("name: "+addressData.name);
                Console.WriteLine("city: "+addressData.city);
            }
            );
        }
    }
}
