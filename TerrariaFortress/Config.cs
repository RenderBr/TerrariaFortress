using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TShockAPI;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace TerrariaFortress
{
    public class Config
    {
		public string gameModeName = "Terraria Fortress";

		public List<Kit> kits;

		public int matchTime; //minutes

		public int playerCountToStart;

		public Vector2 blueSpawnPoint;

		public Vector2 redSpawnPoint;

		public Vector2 spawnPosition;

		public string sorterRegionName = "sorter";
		public string border1 = "b1";
		public string border2 = "b2";

        public void Write()
		{
			string path = Path.Combine(TShock.SavePath, "TerrariaFortress.json");
			File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
		}
		public static Config Read()
		{
			string filepath = Path.Combine(TShock.SavePath, "TerrariaFortress.json");
			try
			{
				Config config = new Config();

				if (File.Exists(filepath))
				{
					config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filepath));
				}

				File.WriteAllText(filepath, JsonConvert.SerializeObject(config, Formatting.Indented));
				return config;
			}
			catch (Exception ex)
			{
				TShock.Log.ConsoleError(ex.ToString());
				return new Config();
			}
		}

	}
}
