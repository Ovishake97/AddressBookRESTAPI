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

            public AddressBook(string name, string city, string state, string phone, string email)
            {
                this.name = name;
                this.city = city;
                this.state = state;
                this.phone = phone;
                this.email = email;
            }
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
        /// Adding a list of
        /// 
        /// </summary>
        [TestMethod]
        public void OnCallingPostAddsMultipleAddress() {
            List<AddressBook> addressBook = new List<AddressBook>();
            addressBook.Add(new AddressBook("Mahesh","Kochi","Kerala","8993839","mahesh@gmail.com"));
            addressBook.Add(new AddressBook("Prasoon", "Mathura", "UP", "837731", "prasoon@gmail.com"));
            addressBook.Add(new AddressBook("Ketaki", "Salem", "TN", "7383939", "ketaki@gmail.com"));
            List<AddressBook> result = new List<AddressBook>();
            addressBook.ForEach(address => {
                RestRequest request = new RestRequest(" /AddressBook/list", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("name", address.name);
                jsonObj.Add("city", address.city);
                jsonObj.Add("state", address.state);
                jsonObj.Add("phone", address.phone);
                jsonObj.Add("email", address.email);
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                AddressBook book = JsonConvert.DeserializeObject<AddressBook>(response.Content);
                result.Add(book);
            }
            );
            Assert.AreEqual(3, result.Count);
        }
    }
}
