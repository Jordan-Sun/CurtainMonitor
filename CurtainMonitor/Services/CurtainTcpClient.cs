using CurtainMonitor.Models;
using System;
using System.Runtime.Serialization;
using System.Net.Sockets;
using System.Diagnostics;

namespace CurtainMonitor.Services
{
    internal enum CurtainState
    {
        Stop = -1,
        Lower = 0,
        Raise = 1,
    }
    public class CurtainTcpClient : IClient
    {
        private TcpClient client;
        private NetworkStream stream;
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
            stream = null;
            sequence = 0;
            state = CurtainState.Stop;
        }

        public bool Connect(String server, int port)
        {
            try
            {
                client = new TcpClient(server, port);
                stream = client.GetStream();
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
            try
            {
                CurtainPacket.SendPacket(stream, packet);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to serialize. Reason: " + e.Message);
                stream?.Close();
                client?.Close();
                client = null;
                Recipient?.OnControllerStatusChanged();
            }
        }
        
        public void Raise(int turns = alwaysTurn)
        {
            if (IsConnected && state != CurtainState.Raise)
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
            if (IsConnected && state != CurtainState.Lower)
            {
                Send(new CurtainPacket()
                {
                    timestamp = Timespec.Current(),
                    sequence = ++sequence,
                    direction = (int) CurtainState.Lower,
                    turns = turns
                });
                state = CurtainState.Lower;
            }
        }
        public void Stop()
        {
            if (IsConnected && state != CurtainState.Stop)
            {
                Send(new CurtainPacket()
                {
                    timestamp = Timespec.Current(),
                    sequence = ++sequence,
                    direction = (int) CurtainState.Raise,
                    turns = stopTurn
                });
                state = CurtainState.Stop;
            }
        }

        ~CurtainTcpClient()
        {
            stream?.Close();
            client?.Close();
        }
    }
}
