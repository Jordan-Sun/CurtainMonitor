using CurtainMonitor.Models;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using Xamarin.Forms;
using System.Diagnostics;

namespace CurtainMonitor.Services
{
    public class SensorTcpClient : IClient
    {
        private TcpClient client;
        private SensorPacket? lastReceived;
        private Timespec? latency;
        private ISensorController controller;
        public int? Full
        {
            get
            {
                return lastReceived?.full;
            }
        }
        public int? Infrared
        {
            get
            {
                return lastReceived?.infrared;
            }
        }
        public int? Visible
        {
            get
            {
                return lastReceived?.visible;
            }
        }
        public Timespec? Latency
        {
            get
            {
                return latency;
            }
        }

        public IStatusChanged Recipient;

        public bool IsConnected
        {
            get
            {
                return client?.Connected ?? false;
            }
        }

        public SensorTcpClient(ISensorController source)
        {
            client = null;
            controller = source;
        }

        ~SensorTcpClient()
        {
            if (client != null)
            {
                client.Close();
            }
        }

        public bool Connect(String server, int port)
        {
            try
            {
                client = new TcpClient(server, port);
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    /* read from sensor every second */
                    try
                    {
                        Read();
                        return true; /* run again */
                    }
                    catch
                    {
                        /* failed to read from sensor, probably disconnected */
                        client.Close();
                        client = null;
                        Recipient?.OnControllerStatusChanged();
                        return false; /* stop */
                    }
                });
                Debug.WriteLine("Connected to sensor at " + server + " on port " + port);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to connect. Reason: " + e.Message);
                return false;
            }
            finally
            {
                Recipient?.OnControllerStatusChanged();
            }
        }

        public void Read()
        {
            NetworkStream stream = client.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                SensorPacket packet = (SensorPacket) formatter.Deserialize(stream);
                if (lastReceived is SensorPacket lastReceivedPacket)
                {
                    latency = packet.timestamp - lastReceivedPacket.timestamp;
                }
                lastReceived = packet;
                controller.OnNewData();
            }
            catch (SerializationException e)
            {
                Debug.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
