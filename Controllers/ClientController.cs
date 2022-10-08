using KingOfTokyo.Models.Dices;
using KingOfTokyo.Models.Game;
using KingOfTokyo.Models.Monsters;
using KingOfTokyo.Models.Network;
using KingOfTokyo.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KingOfTokyo.Controllers
{
    public static class ClientController
    {
        public static CharacterSelection WindowCharacter { get; set; }
        public static GameBoard WindowGame { get; set; }

        public static void LocalPacket(Command command, Object o)
        {
            Packet packet = new Packet(command, o);
            AcceptPacket(packet);
        }

       public static void AcceptPacket(Packet packet)
        {
            switch (packet.Command)
            {
                // Lobby Packet
                case Command.ServeurMessage:
                    break;
                case Command.CreateGame:
                    break;
                case Command.JoinGame:
                    JoinGame(packet);
                    break;
                case Command.UpdatePlayer:
                    UpdatePlayer(packet);
                    break;
                case Command.UpdateMe:
                    UpdateMe(packet);
                    break;
                case Command.QuitGame:
                    QuitGame(packet);
                    break;
                // Game Packet
                case Command.StartGame:
                    StartGame(packet);
                    break;
                case Command.StartTurn:
                    Jeu.StartTurn();
                    break;
                case Command.Roll:
                    Roll(packet);
                    break;
                case Command.SaveDice:
                    SaveDice(packet);
                    break;
                case Command.EndRoll:
                    EndRoll(packet);
                    break;
                case Command.EndCard:
                    EndCard(packet);
                    break;
                case Command.EndTurn:
                    EndTurn(packet);
                    break;
                case Command.StayNamek:
                    ExitNamek(false);
                    break;
                case Command.ExitNamek:
                    ExitNamek(true);
                    break;
                case Command.BuyCard:
                    BuyCard(packet);
                    break;
            }
        }

        public static void BuyCard(Packet packet)
        {
            int id = int.Parse(Encoding.UTF8.GetString(packet.Data));
            Jeu.BuyCard(id);

            WindowGame.showCards();
            WindowGame.CreateCharacterList();
            WindowGame.GenerateDeckPlayer();
        }

        public static void ExitNamek(bool value)
        {
            Jeu.ExitNamek(value);
            WindowGame.CreateCharacterList();
            WindowGame.UpdateGame();
        }

        public static void EndTurn(Packet packet)
        {
            Jeu.EndTurn();
            WindowGame.CreateCharacterList();
            WindowGame.UpdateGame();
        }

        public static void EndCard(Packet packet)
        {
            Jeu.EndCard();
            WindowGame.UpdateGame();
        }

        public static void EndRoll(Packet packet)
        {
            Jeu.EndRoll();
            WindowGame.CreateCharacterList();
            WindowGame.UpdateGame();
        }

        public static void SaveDice(Packet packet)
        {
            int id = int.Parse(Encoding.UTF8.GetString(packet.Data));
            DiceRoller.Dices[id].Save = !DiceRoller.Dices[id].Save;
            WindowGame.uc_Dice.UpdateDice(id);
        }

        public static void StartGame(Packet packet)
        {
            WindowCharacter.StartGame();
        }

        public static void Roll(Packet packet)
        {
            DiceRoller.Dices = (List<Dice>)Helpers.ByteArrayToObject(packet.Data);
            DiceRoller.ResolveDice();
            Jeu.MaxRoll--;
            WindowGame.UpdateDiceView();
        }

        public static void UpdateMe(Packet packet)
        {
            Player p = (Player)Helpers.ByteArrayToObject(packet.Data);
            Jeu.Players[p.Id] = p;
            WindowCharacter.SelectedPlayer = p.Id;
            WindowCharacter.UpdateOneView(p.Id);
            WindowCharacter.SetSelectedView(p.Id);
        }
        public static void UpdatePlayer(Packet packet)
        {
            Player p = (Player)Helpers.ByteArrayToObject(packet.Data);
            Jeu.Players[p.Id] = p;
            WindowCharacter.UpdateOneView(p.Id);
        }

        public static void JoinGame(Packet packet)
        {
            Jeu.Players = (List<Player>)Helpers.ByteArrayToObject(packet.Data);
            WindowCharacter.UpdateView();
        }
        public static void QuitGame(Packet packet)
        {
            Jeu.Players = (List<Player>)Helpers.ByteArrayToObject(packet.Data);

            // Reset player id
            if (WindowCharacter.SelectedPlayer > Jeu.Players.Count - 1)
                WindowCharacter.SelectedPlayer--;

            WindowCharacter.UpdateView();
            WindowCharacter.SetSelectedView(WindowCharacter.SelectedPlayer);
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
