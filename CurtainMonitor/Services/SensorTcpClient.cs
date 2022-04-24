using CurtainMonitor.Models;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CurtainMonitor.Services
{
    public class SensorTcpClient : IClient
    {
        private TcpClient client;
        private NetworkStream stream;
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
            stream = null;
            controller = source;
        }

        ~SensorTcpClient()
        {
            if (stream != null)
            {
                stream.Close();
            }
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
                stream = client.GetStream();
                Task.Factory.StartNew( async() =>
                {
                    DateTime expire = DateTime.Now;
                    do
                    {
                        try
                        {
                            Read();
                            expire = expire.AddSeconds(1);
                            if (expire < DateTime.Now)
                            {
                                expire = DateTime.Now.AddSeconds(1);
                            }
                            await Task.Delay(expire - DateTime.Now);
                        }
                        catch (Exception e)
                        {
                            /* failed to read from sensor, probably disconnected */
                            Debug.WriteLine("Failed to read. Reason: " + e.Message);
                            client.Close();
                            client = null;
                            Recipient?.OnControllerStatusChanged();
                            break;
                        }
                    } while (true);
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
            try
            {
                SensorPacket packet = SensorPacket.ReadPacket(stream);
                Debug.WriteLine("Time: " + packet.timestamp.tv_sec + "." + packet.timestamp.tv_nsec + " Seq: " + packet.sequence + " Full: " + packet.full + " Infrared: " + packet.infrared + " Visible: " + packet.visible);
                if (lastReceived is SensorPacket lastReceivedPacket)
                {
                    latency = packet.timestamp - lastReceivedPacket.timestamp;
                }
                lastReceived = packet;
                controller.OnNewData();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
        }
    }
}
