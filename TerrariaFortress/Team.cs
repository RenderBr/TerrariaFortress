using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TerrariaFortress
{
    public class Team
    {
        public List<TFPlayer> TFPlayers = new List<TFPlayer>();

        public string team { get; set; }

        public Vector2 spawnPoint;

        public int score;

        public Team(string team)
        {
            this.team = team;
        }

  
    }

    public class TeamManager
    {
        public static Team Blue()
        {
            return Main.Teams.Find(x => x.team == "BLU");
        }

        public static Team Red()
        {
            return Main.Teams.Find(x => x.team == "RED");
        }
    }
}
