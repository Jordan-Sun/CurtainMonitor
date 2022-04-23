using CurtainMonitor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Diagnostics;

namespace CurtainMonitor.Services
{
    public class LightHTMLClient : IClient
    {
        public HttpClient client;
        private bool isConnected;
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
        }

        public IStatusChanged Recipient;

        public LightHTMLClient()
        {
            client = new HttpClient();
            isConnected = false;
        }

        public bool Connect(string server, int _)
        {
            try
            {
                client.BaseAddress = new Uri(server);
                isConnected = true;
                Debug.WriteLine("Connected to light at " + server);
                return true;
            }
            catch (Exception e)
            {
                isConnected = false;
                Debug.WriteLine("Failed to resolve uri. Reason: " + e.Message);
                return false;
            }
            finally
            {
                Recipient?.OnControllerStatusChanged();
            }
        }
    }
}
