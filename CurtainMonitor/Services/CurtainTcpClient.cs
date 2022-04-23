using CurtainMonitor.Models;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Diagnostics;

namespace CurtainMonitor.Services
{
    internal enum CurtainState
    {
        Stop = -1,
        Raise = 0,
        Lower = 1,
    }
    public class CurtainTcpClient : IClient
    {
        private TcpClient client;
        public bool IsConnected
        {
            get
            {
                return client?.Connected ?? false;
            }
        }
        private uint sequence;
        private CurtainState state;
        private const int stopTurn = 0;
        private const int alwaysTurn = -1;

        public IStatusChanged Recipient;

        public CurtainTcpClient()
        {
            client = null;
            sequence = 0;
            state = CurtainState.Stop;
        }

        public bool Connect(String server, int port)
        {
            try
            {
                client = new TcpClient(server, port);
                sequence = 0;
                state = CurtainState.Stop;
                Debug.WriteLine("Connected to curtain at " + server + " on port " + port);
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

        private void Send(CurtainPacket packet)
        {
            NetworkStream stream = client.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, packet);
            }
            catch (SerializationException e)
            {
                Debug.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                stream.Close();
            }
        }
        
        public void Raise(int turns = alwaysTurn)
        {
            if (turns <= 0)
            {
                return;
            }
            if (client.Connected && state != CurtainState.Raise)
            {
                Send(new CurtainPacket()
                {
                    timestamp = Timespec.Current(),
                    sequence = ++sequence,
                    direction = (int) CurtainState.Raise,
                    turns = turns
                });
                state = CurtainState.Raise;
            }
        }
        public void Lower(int turns = alwaysTurn)
        {
            if (turns <= 0)
            {
                return;
            }
            if (client.Connected && state != CurtainState.Lower)
            {
                Send(new CurtainPacket()
                {
                    timestamp = Timespec.Current(),
                    sequence = ++sequence,
                    direction = (int)CurtainState.Stop,
                    turns = turns
                });
                state = CurtainState.Lower;
            }
        }
        public void Stop()
        {
            if (client.Connected && state != CurtainState.Stop)
            {
                Send(new CurtainPacket()
                {
                    timestamp = Timespec.Current(),
                    sequence = ++sequence,
                    direction = (int)CurtainState.Stop,
                    turns = stopTurn
                });
                state = CurtainState.Stop;
            }
        }

        ~CurtainTcpClient()
        {
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
