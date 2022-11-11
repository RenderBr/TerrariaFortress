using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using TerrariaFortress;
using TShockAPI;

namespace TerrariaFortress
{
    public class Leaderboard
    {
        public static string gameModeName = Main.Config.gameModeName;
        public static int playerCount = TShock.Players.Length;
        public static string RepeatLineBreaks(int number)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < number; i++)
            {
                sb.Append("\r\n");
            }

            return sb.ToString();
        }

        public static void Initialize()
        {
            string message = ($"{RepeatLineBreaks(59)} {gameModeName} \r\n Players: {playerCount}");

            TSPlayer.All.SendData(PacketTypes.Status, message, 1);
        }

 
    }
}
