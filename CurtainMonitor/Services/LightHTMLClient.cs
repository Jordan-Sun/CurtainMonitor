using CurtainMonitor.Models;
using System;
using System.Text;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurtainMonitor.Services
{
    class HueError
    {
        public int type { get; set; }
        public string address { get; set; }
        public string description { get; set; }
    }
    class HueSuccess
    {
        public string username { get; set; }
    }
    class HueObject
    {
        public HueError error { get; set; }
        public HueSuccess success { get; set; }
    }
    
    public class LightHTMLClient
    {
        public HttpClient Client;
        public string Username;
        private bool isOn;
        public bool IsConnected
        {
            get
            {
                return (Client != null);
            }
        }

        public IStatusChanged Recipient;

        public LightHTMLClient()
        {
            Client = null;
            isOn = false;
        }

        public bool Connect(string server)
        {
            try
            {
                Client = new HttpClient();
                Client.BaseAddress = new Uri("http://" + server + "/");
                isOn = true;
                Debug.WriteLine("Connected to light at " + server);
                return true;
            }
            catch (Exception e)
            {
                Client = null;
                isOn = false;
                Debug.WriteLine("Failed to resolve uri. Reason: " + e.Message);
                return false;
            }
            finally
            {
                Recipient?.OnControllerStatusChanged();
            }
        }

        public async Task<string> NewUser()
        {
            string result = "";
            if (Client is null)
            {
                Debug.WriteLine("Failed to toggle light. Reason: Not connected");
                return result;
            }
            string json = "{\"devicetype\":\"real_time_systems#smart_curtain\"}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            try
            {
                Debug.WriteLine("Sending new user request to " + Client.BaseAddress + "api with content" + content);
                response = await Client.PostAsync(Client.BaseAddress + "api", content);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to create new user. Reason: " + e.Message);
            }
            if (response is null)
            {
                return result;
            }
            try
            {
                string stringContent = response.Content.ReadAsStringAsync().Result;
                System.Collections.Generic.IList<HueObject> parsedContent = JsonConvert.DeserializeObject<System.Collections.Generic.IList<HueObject>>(stringContent);
                if (parsedContent.Count > 0)
                {
                    foreach (HueObject obj in parsedContent)
                    {
                        if (obj.success != null)
                        {
                            result = obj.success.username;
                        }
                        else if (obj.error != null)
                        {
                            if (obj.error.type == 101)
                            {
                                result = "Link Button Not Pressed";
                            }
                            else
                            {
                                Debug.WriteLine("Failed to create new user. Reason: " + obj.error.description);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to read response. Reason: " + e.Message);
            }
            return result;
        }

        public async Task Toggle(bool? toState = null)
        {
            if (Client is null || Username is null)
            {
                Debug.WriteLine("Failed to toggle light. Reason: Not connected");
                return;
            }
            bool previousState = isOn;
            isOn = toState ?? (!isOn);
            string json = "{\"on\":" + (isOn ? "true" : "false")+ "}";
            Debug.WriteLine("Sending toggle request to " + Client.BaseAddress + "api/" + Username + "/lights/1/state with content" + json);
            try
            {
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await Client.PutAsync(Client.BaseAddress + "api/" + Username + "/lights/1/state", content);
            }
            catch (Exception e)
            {
                isOn = previousState;
                Debug.WriteLine("Failed to toggle light. Reason: " + e.Message);
            }
        }
    }            
}
