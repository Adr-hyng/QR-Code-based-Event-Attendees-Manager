using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace QEAMApp.MVVM.Model
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly string _baseUri = "http://localhost:5000/api/";

        public ApiService()
        {
            _client = new HttpClient();
        }

        public async Task<(bool, Account)> Authenticate(string id)
        {
            string url = $"{_baseUri}authenticate/{id}";

            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                bool IsFound = (bool)result["success"];
                if (IsFound)
                {
                    JArray data2 = JArray.Parse(result["data"].ToString());
                    var firstElement = data2.FirstOrDefault();
                    Account attendee = new Account(
                        _fn: firstElement["fn"].ToString(),
                        _mi: firstElement["mi"].ToString(),
                        _ln: firstElement["ln"].ToString(),
                        _uid: firstElement["uid"].ToString(),
                        _membership: (byte)firstElement["membership"]["data"][0],
                        _position: (byte)firstElement["position"]["data"][0],
                        _institution: firstElement["institution"].ToString(),
                        _pn: "0" + firstElement["pn"]
                        );
                    return (true, attendee);
                } else if(!IsFound)
                {
                    return (false, null);
                }
            }
            return (false, null);
        }
    }
}
