using CurtainMonitor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CurtainMonitor.Services
{
    public class LightHTMLClient : IClient
    {
        public HttpClient client;
        private bool isOn;
        public bool IsConnected
        {
            get
            {
                return (client != null);
            }
        }

        public IStatusChanged Recipient;

        public LightHTMLClient()
        {
            client = null;
            isOn = false;
        }

        public bool Connect(string server, int _)
        {
            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(server);
                isOn = false;
                Debug.WriteLine("Connected to light at " + server);
                return true;
            }
            catch (Exception e)
            {
                client = null;
                isOn = false;
                Debug.WriteLine("Failed to resolve uri. Reason: " + e.Message);
                return false;
            }
            finally
            {
                Recipient?.OnControllerStatusChanged();
            }
        }

        public async Task Toggle(bool? toState = null)
        {
            if (client is null)
            {
                return;
            }
            bool previousState = isOn;
            isOn = toState ?? (!isOn);
            string json = "{\"on\":{\"on\":" + (isOn ? "true" : "false")+ "}}";
            try
            {
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(client.BaseAddress, content);
            }
            catch (Exception e)
            {
                isOn = previousState;
                Debug.WriteLine("Failed to toggle light. Reason: " + e.Message);
            }
        }
    }            
}
