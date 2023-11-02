using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Media;
using QEAMApp.Core;
using System.ComponentModel;
using System.Threading;

namespace QEAMApp.MVVM.Model
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private string DEFAULT_GATEWAY;
        private string _baseUri;

        public ApiService(string HostAddress = "http://localhost", int Port = 8080)
        {
            DEFAULT_GATEWAY = $"{HostAddress}:{Port}";
            _baseUri = $"{DEFAULT_GATEWAY}/api/";
            _client = new HttpClient();
        }

        public void ChangeDefaultGateWay(string UpdatedDefaultGateway)
        {
            DEFAULT_GATEWAY = UpdatedDefaultGateway ;
            _baseUri = $"http://{DEFAULT_GATEWAY}/api/";
        }

        private static (bool?, Attendee?) Exit(bool? flag = false)
        {
            return (flag, null);
        }

        public async Task<bool> GetServerInfo(string Address, int Port, double maxTimeoutSeconds = 0.5)
        {
            string url = $"{_baseUri}status_info";
            bool IsConnected = false;

            try
            {
                // Create a CancellationTokenSource with the specified timeout
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(maxTimeoutSeconds)))
                {
                    // Assign the CancellationToken from the CancellationTokenSource to the GetAsync method
                    HttpResponseMessage response = await _client.GetAsync(url, cts.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        IsConnected = true;
                        string json = await response.Content.ReadAsStringAsync();
                        Dictionary<string, object>? result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        if (result is null) return IsConnected;
                        Address = (string)result["address"];
                        Port = (int)result["port"];
                        return IsConnected;
                    }
                    else
                    {
                        return IsConnected;
                    }
                }
            }
            catch (Exception)
            {
                return IsConnected;
            }
        }
        public async Task<(bool?, Attendee?)> Authenticate(string id, double maxTimeoutSeconds = 2)
        {
            string url = $"{_baseUri}authenticate/{id}";

            try
            {
                // Create a CancellationTokenSource with the specified timeout
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(maxTimeoutSeconds)))
                {
                    // Assign the CancellationToken from the CancellationTokenSource to the GetAsync method
                    HttpResponseMessage response = await _client.GetAsync(url, cts.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Dictionary<string, object>? result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        if (result is null) return Exit();
                        bool IsFound = (bool)result["success"];
                        if (IsFound)
                        {
                            JArray data2 = JArray.Parse(result["data"] + "");
                            var firstElement = data2.FirstOrDefault();
                            byte membership = (byte)firstElement!["membership"]!["data"]![0]!;
                            byte position = (byte)firstElement!["position"]!["data"]![0]!;

                            Attendee attendee = new Attendee(
                                _fn: firstElement["fn"] + "",
                                _mi: firstElement["mi"] + "",
                                _ln: firstElement["ln"] + "",
                                _uid: firstElement["uid"] + "",
                                _membership: membership,
                                _position: position,
                                _institution: firstElement["institution"] + "",
                                _pn: "0" + firstElement["pn"],
                                _day1: new DayContent(firstElement.ToObject<Dictionary<string, object>>(), new string[] { "amd1", "lunchd1", "pmd1", "checkind1", "checkoutd1" }),
                                _day2: new DayContent(firstElement.ToObject<Dictionary<string, object>>(), new string[] { "amd2", "lunchd2", "pmd2", "checkind2", "checkoutd2" }),
                                _day3: new DayContent(firstElement.ToObject<Dictionary<string, object>>(), new string[] { "amd3", "lunchd3", "pmd3", "checkind3", "checkoutd3" })
                            );

                            return (true, attendee);
                        }
                        else if (!IsFound) return Exit();
                    }
                    return Exit();
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Exclamation.Play();
                AutoClosingMessageBox.Show("No Server Connected.", "SERVER 404", 5000);
                return Exit(flag: null);
            }
        }

    }
}
