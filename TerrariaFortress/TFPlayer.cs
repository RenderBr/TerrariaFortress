using System;
using System.Collections.Generic;
using System.Text;
using TShockAPI;
using TerrariaFortress;

namespace TerrariaFortress
{
    public class TFPlayer
    {

        public TSPlayer TSPlayer { get; set; } 
        public string Name { get; set; }     

        public Team Team { get; set; }

        public static TFPlayer GetByUsername(string name)
        {
            return Main.players.Find(p => p.Name == name);
        }

        public TFPlayer(TSPlayer player)
        {
            this.TSPlayer = player;
            this.Name = player.Name;
        }
    }
}
