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
using System.Text;

namespace QEAMApp.MVVM.Model
{
    public class ApiService: INotifyPropertyChanged
    {
        private readonly HttpClient _client;
        private string DEFAULT_GATEWAY;
        private string _baseUri;
        private bool _debugMode = false;

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
                using CancellationTokenSource cts = new(TimeSpan.FromSeconds(maxTimeoutSeconds));
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
            catch (Exception)
            {
                return IsConnected;
            }
        }

        public async Task<(bool?, Attendee?)> Authenticate(string id, double maxTimeoutSeconds = 0.5)
        {
            string url = $"{_baseUri}authenticate/{id}";

            try
            {
                // Create a CancellationTokenSource with the specified timeout
                using CancellationTokenSource cts = new(TimeSpan.FromSeconds(maxTimeoutSeconds));
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

                        Attendee attendee = new(
                            _fn: firstElement["fn"] + "",
                            _mi: firstElement["mi"] + "",
                            _ln: firstElement["ln"] + "",
                            _uid: firstElement["uid"] + "",
                            _membership: membership,
                            _position: position,
                            _institution: firstElement["institution"] + "",
                            _pn: "0" + firstElement["pn"],
                            _day1: new DayContent(firstElement.ToObject<Dictionary<string, object>>()!, new string[] { "amd1", "lunchd1", "pmd1", "checkind1", "checkoutd1" }),
                            _day2: new DayContent(firstElement.ToObject<Dictionary<string, object>>()!, new string[] { "amd2", "lunchd2", "pmd2", "checkind2", "checkoutd2" }),
                            _day3: new DayContent(firstElement.ToObject<Dictionary<string, object>>()!, new string[] { "amd3", "lunchd3", "pmd3", "checkind3", "checkoutd3" })
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
                return Exit(flag: null);
            }
        }

        public async Task<(bool, Attendee?)> UpdateAttendee(string id, string column, string value)
        {
            string url = $"{_baseUri}update_attendee/{id}";

            try
            {
                var data = new Dictionary<string, string>
                    {
                        { "column", column },
                        { "value", value }
                    };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Attendee updated successfully.");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());

                    // Call Authenticate method to get the updated attendee
                    var (isAuthenticated, updatedAttendee) = await Authenticate(id);

                    if (isAuthenticated.HasValue && isAuthenticated.Value)
                    {
                        return (true, updatedAttendee);
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve updated attendee.");
                        return (false, null);
                    }
                }
                else
                {
                    Console.WriteLine("Failed to update attendee.");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while making the request.");
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public bool DebugMode
        {
            get
            {
                return _debugMode;
            }
            set
            {
                if (_debugMode != value)
                {
                    _debugMode = value;
                    OnPropertyChanged(nameof(DebugMode));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
