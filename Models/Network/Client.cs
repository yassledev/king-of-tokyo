using KingOfTokyo.Models.Network;
using KingOfTokyo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTokyo.Models.Network
{
    public static class Client
    {
        public static readonly Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] buffer;
        public static bool IsConnected => Socket.Connected;
        public static void Connect(string ip, int port)
        {
            try
            {
                Socket.Connect(IPAddress.Parse(ip), port);
                buffer = new byte[Socket.ReceiveBufferSize];
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, AsyncReceive, null);
            }
            catch (SocketException e)
            {
                throw e;
            }
        }

        private static void AsyncReceive(IAsyncResult AR)
        {
            try
            {
                int bufferSize = Socket.EndReceive(AR);
                if (bufferSize == 0) return;

                var data = new byte[bufferSize];
                Array.Copy(buffer, data, bufferSize);

                // Let controller
                ClientController.AcceptPacket(new Packet(data));

                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, AsyncReceive, null);
            }
            catch (SocketException e)
            {
                Disconnect();
            }
        }

        public static void SendRequest(Command command, string message)
        {
            Packet paquet = new Packet(command, message);
            Socket.Send(paquet.GetBytes());
        }

        public static void SendObject(Command command, Object o)
        {
            Packet p = new Packet(command, o);
            Socket.Send(p.GetBytes());
        }

        public static void Disconnect()
        {
            try
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            catch (Exception)
            {

            }
            
        }

        public static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

    }
}
