using KingOfTokyo.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KingOfTokyo.Models.Network
{
    public class Packet
    {

        public Command Command { get; set; }
        public byte[] Data { get; set; }
        public Packet(byte[] buffer)
        {
            Command = (Command)buffer[0];
            Data = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, Data, 0, Data.Length);
        }
        public Packet(Command command, byte[] buffer)
        {
            Command = (Command)command;
            Data = buffer;
        }
        public Packet(Command command, string data)
        {
            Command = (Command)command;
            Data = Encoding.ASCII.GetBytes(data);
        }

        public Packet(Command command, Object o)
        {
            Command = (Command)command;
            Data = Helpers.ObjectToByteArray(o);
        }

        public byte[] GetBytes()
        {

            byte[] buffer = new byte[Data.Length + 1];
            buffer[0] = (byte)Command;
            Array.Copy(Data, 0, buffer, 1, Data.Length);
            return buffer;
        }

        public static byte[] GetBytes(Command command, byte[] buffer)
        {
            Packet p = new Packet(command, buffer);
            return p.GetBytes();
        }
        override
        public string ToString()
        {
            return "[" + Command + "," + Data + "]";
        }

    }
}
