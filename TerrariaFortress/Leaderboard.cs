using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Terraria;
using TerrariaFortress;
using TShockAPI;

namespace TerrariaFortress
{
    public class Leaderboard
    {
        public static Timer matchTimeLeft = Main._delayTimer;
        public static string gameModeName = Main.Config.gameModeName;
        public static string RepeatLineBreaks(int number)
        {
            string sb = "";
            for (int i = 0; i < number; i++)
            {
                sb += "\r\n";
            }

            return sb.ToString();
        }

        public static async void Initialize()
        {
            var winner = "Winning Team: Tied!";
            var winScore = 0;
            if (TeamManager.Blue().score < TeamManager.Red().score)
                winner = "[c/ff4848:W][c/ff4a4a:i][c/ff4d4d:n][c/ff5050:n][c/ff5353:i][c/ff5656:n][c/ff5959:g] [c/ff5f5f:T][c/ff6262:e][c/ff6565:a][c/ff6868:m][c/ff6b6b::] [c/ff7171:R][c/ff7474:e][c/ff7777:d]";
                winScore = TeamManager.Red().score;
            if (TeamManager.Red().score < TeamManager.Blue().score)
                winner = "[c/0080c0:W][c/0780c3:i][c/0f80c7:n][c/1680cb:n][c/1e80ce:i][c/2580d2:n][c/2d80d6:g] [c/3c80dd:T][c/4380e1:e][c/4b80e5:a][c/5280e8:m][c/5a80ec::] [c/6980f3:B][c/7080f7:l][c/7880fb:u][c/8080ff:e]";
                winScore = TeamManager.Blue().score;
            if(TeamManager.Red().score == 0 && TeamManager.Blue().score == 0)
                winner = "Winning Team: Tied!";
                winScore = TeamManager.Red().score;

            string message = ($"{RepeatLineBreaks(10)} [c/2596be:[{gameModeName}][c/2596be:]] \r\n Players: {Main.players.Count} \r\n {winner} ({winScore}) \r\n Time Elapsed: {(int)Math.Round((DateTime.Now.Subtract(Main.startTime).TotalSeconds))} seconds {RepeatLineBreaks(59)}");

            TSPlayer.All.SendData(PacketTypes.Status, message, 0);
            await Task.Delay(1000);
            Initialize();
        }

 
    }
}
