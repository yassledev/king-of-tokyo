using KingOfTokyo.Models.Monsters;
using KingOfTokyo.Models.Network;
using KingOfTokyo.Models.Game;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using KingOfTokyo.Models.Dices;
using System.Windows;

namespace KingOfTokyo.Controllers
{
    public static class ServerController
    {
        public static Dictionary<Socket, Player> SocketPlayers = new Dictionary<Socket, Player>();

        public static void AcceptPacket(Socket socket, Packet packet)
        {
            switch (packet.Command)
            {
                // Lobby Packet
                case Command.ServeurMessage:
                    Serveur.SendAllOthers(socket, Encoding.ASCII.GetString(packet.Data));
                    break;
                case Command.CreateGame:
                    CreateGame(socket, packet);
                    break;
                case Command.JoinGame:
                    JoinGame(socket, packet);
                    break;
                case Command.UpdatePlayer:
                    UpdatePlayer(socket, packet);
                    break;
                case Command.QuitGame:
                    QuitGame(socket, packet);
                    break;
                case Command.StartGame:
                    StartGame(socket, packet);
                    break;
                // Game Packet
                case Command.Roll:
                    Roll(socket, packet);
                    break;
                case Command.SaveDice:
                    SaveDice(socket, packet);
                    break;
                case Command.EndRoll:
                    Serveur.SendPacketToAll(packet);
                    break;
                case Command.EndCard:
                    Serveur.SendPacketToAll(packet);
                    break;
                case Command.EndTurn:
                    Serveur.SendPacketToAll(packet);
                    break;
                case Command.StayNamek:
                    Serveur.SendPacketToAll(packet);
                    break;
                case Command.ExitNamek:
                    Serveur.SendPacketToAll(packet);
                    break;
                case Command.BuyCard:
                    Serveur.SendPacketToAll(packet);
                    break;
            }
        }
        public static void SaveDice(Socket socket, Packet packet)
        {
            int id = Int32.Parse(Encoding.UTF8.GetString(packet.Data));
            Serveur.SendPacketToAll(packet);
        }

        public static void StartGame(Socket socket, Packet packet)
        {
            Serveur.SendToAll(Command.StartGame, "Start");
        }

        public static void Roll(Socket socket, Packet packet)
        {
            Jeu.RollS();
            Serveur.SendObjectToAll(Command.Roll, DiceRoller.Dices);
        }

        public static void CreateGame(Socket socket, Packet packet)
        {
            // Deserialize Player object
            Player player = (Player)Helpers.ByteArrayToObject(packet.Data);
            Jeu.Host = player;
            Jeu.Players.Add(player);
        }
        public static void JoinGame(Socket socket, Packet packet)
        {
            // Deserialize Player
            List<string> Characters = new List<string> { "goku", "vegeta", "beerus", "friza", "hit", "black" };

            // Set Id to player
            int id = Jeu.Players.Count;
            int id_character = 0;

            for(int i = 0; i < Characters.Count; i++)
            {
                if(!Array.Exists(Jeu.Players.ToArray(), (Player p) => p.Pseudo == Characters[i]))
                {
                    id_character = i;
                    break;
                }
            }

            Player player = new Player(id, Characters[id]);

            // Check if player host game
            if (Jeu.Host == null) Jeu.Host = player;
            Jeu.Players.Add(player);

            // Send players list to all player
            Serveur.SendObjectToAll(Command.JoinGame, Jeu.Players);

            // Send Player updated to player
            socket.Send(new Packet(Command.UpdateMe, Helpers.ObjectToByteArray(player)).GetBytes());

            SocketPlayers.Add(socket, player);
        }

        public static void QuitGame(Socket socket, Packet packet)
        {

            if (!SocketPlayers.ContainsKey(socket))
                return;

            Player p = SocketPlayers[socket];

            // Remove player from list
            Jeu.Players.RemoveAt(p.Id);
            SocketPlayers.Remove(socket);
            Serveur.Sockets.Remove(socket);

            // Reset player ID
            for (int i = 0; i < Jeu.Players.Count; i++)
                Jeu.Players[i].Id = i;

            // Send new list to all player
            Serveur.SendObjectToAll(Command.QuitGame, Jeu.Players);
        }

        public static void UpdatePlayer(Socket socket, Packet packet)
        {
            Player player = (Player)Helpers.ByteArrayToObject(packet.Data);

            Jeu.Players[player.Id] = player;

            Serveur.SendObjectToAll(Command.UpdatePlayer, player);
        }

    }
}
