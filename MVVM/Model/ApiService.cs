using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Media;
using QEAMApp.Core;

namespace QEAMApp.MVVM.Model
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private const string DEFAULT_GATEWAY = "http://localhost:5000";
        private readonly string _baseUri = $"{DEFAULT_GATEWAY}/api/";

        public ApiService()
        {
            _client = new HttpClient();
        }

        private static (bool, Attendee?) Exit()
        {
            return (false, null);
        }

        public async Task<(bool, string?, int?)> GetServerInfo()
        {
            string url = $"{_baseUri}status_info";
            string Address = "";
            int Port = 5000;
            bool IsConnected = false;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    IsConnected = true;
                    string json = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object>? result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (result is null) return (IsConnected, Address, Port);
                    Address = (string)result["address"];
                    Port = (int)result["port"];
                    return (IsConnected, Address, Port);
                }
                else
                {
                    return (IsConnected, Address, Port);
                }
            }
            catch (Exception)
            {
                return (IsConnected, Address, Port);
            }
        }


        public async Task<(bool, Attendee?)> Authenticate(string id)
        {
            string url = $"{_baseUri}authenticate/{id}";

            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object>? result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (result is null) return Exit();
                    bool IsFound = (bool) result["success"];
                    if (IsFound)
                    {
                        JArray data2 = JArray.Parse((string)result["data"]);
                        JToken? firstElement = data2.FirstOrDefault();
                        byte membership = (byte) firstElement!["membership"]!["data"]![0]!;
                        byte position = (byte) firstElement!["position"]!["data"]![0]!;
                        
                        Attendee attendee = new(
                            _fn: firstElement["fn"] + "",
                            _mi: firstElement["mi"] + "",
                            _ln: firstElement["ln"] + "",
                            _uid: firstElement["uid"] + "",
                            _membership: membership,
                            _position: position,
                            _institution: firstElement["institution"] + "",
                            _pn: "0" + firstElement["pn"]
                            );
                        return (true, attendee);
                    }
                    else if (!IsFound) return Exit();
                }
                return Exit();
            }
            catch (Exception)
            {
                SystemSounds.Exclamation.Play();
                AutoClosingMessageBox.Show("No Server Connected.", "SERVER 404", 5000);
                return Exit();
            }
        }
    }
}
