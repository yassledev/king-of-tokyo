using KingOfTokyo.Models.Monsters;
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
    public static class Serveur
    {
        private static readonly Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static List<Socket> Sockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        public static bool IsOpen;
        public static void Setup(int port)
        {
            try
            {
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(10);
                server.BeginAccept(AsyncAccept, null);
            }
            catch (SocketException e)
            {
                throw e;
            }
        }

        public static void Close()
        {
            foreach(Socket socket in Sockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            server.Close();
        }

        private static void AsyncAccept(IAsyncResult AR)
        {
            Socket socket;
            try
            {
                socket = server.EndAccept(AR);
                Sockets.Add(socket);
                socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, AsyncReceive, socket);
                server.BeginAccept(AsyncAccept, null);
            } 
            catch (Exception)
            {
                return;
            }
        }

        private static void AsyncReceive(IAsyncResult AR)
        {
            // Récupère le socket du client
            Socket socket = (Socket)AR.AsyncState;

            // Verifie si le client envoie des données
            int bufferSize;
            try
            {
                bufferSize = socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                ServerController.AcceptPacket(socket, new Packet(Command.QuitGame, ""));
                socket.Close();
                Sockets.Remove(socket);
                return;
            }

            byte[] receive = new byte[bufferSize];
            Array.Copy(buffer, receive, bufferSize);

            // Server controller
            ServerController.AcceptPacket(socket, new Packet(receive));

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, AsyncReceive, socket);

        }

        public static void SendToAll(Command command, string text)
        {
            Packet packet = new Packet(command, text);
            foreach (Socket s in Sockets)
            {
                s.Send(packet.GetBytes());
            }
        }

        public static void SendAllOthers(Socket socket, string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            foreach (Socket s in Sockets)
            {
                if (socket != s)
                    s.Send(data);
            }
        }

        public static void SendObjectAllOthers(Socket me, Command command, Object o)
        {
            byte[] objData = Helpers.ObjectToByteArray(o);
            Packet packet = new Packet(command, objData);
            foreach (Socket s in Sockets)
            {
                s.Send(packet.GetBytes());
            }
        }

        public static void SendPacketToAll(Packet packet)
        {
            foreach (Socket s in Sockets)
            {
                s.Send(packet.GetBytes());
            }
        }

        public static void SendObjectToAll(Command command, Object o)
        {
            byte[] objData = Helpers.ObjectToByteArray(o);
            Packet packet = new Packet(command, objData);
            foreach (Socket s in Sockets)
            {
                s.Send(packet.GetBytes());
            }
        }
    }
}
