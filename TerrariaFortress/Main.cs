using System;
using System.Collections.Generic;
using Terraria;
using TerrariaApi.Server;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Timers;
using TShockAPI;
using TerrariaFortress;
using TShockAPI.Hooks;
using Terraria.Localization;

namespace TerrariaFortress
{
    /// <summary>
    /// The main plugin class should always be decorated with an ApiVersion attribute. The current API Version is 2.1
    /// </summary>
    [ApiVersion(2, 1)]
    public class Main : TerrariaPlugin
    {
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        public override string Name => "TerrariaFortress";

        /// <summary>
        /// The version of the plugin in its current state.
        /// </summary>
        public override Version Version => new Version(1, 0, 0);

        /// <summary>
        /// The author(s) of the plugin.
        /// </summary>
        public override string Author => "Average";

        /// <summary>
        /// A short, one-line, description of the plugin's purpose.
        /// </summary>
        public override string Description => "Average's TerrariaFortress base plugin";
        public bool gameStarted = false;
        public static List<Team> Teams = new List<Team>();
        public static List<TFPlayer> players = new List<TFPlayer>();
        public Config Config { get; private set; }
        /// <summary>
        /// The plugin's constructor
        /// Set your plugin's order (optional) and any other constructor logic here
        /// </summary>
        public Main(Terraria.Main game) : base(game)
        {

        }

        /// <summary>
        /// Performs plugin initialization logic.
        /// Add your hooks, config file read/writes, etc here
        /// </summary>
        public override void Initialize()
        {
            ServerApi.Hooks.NetGreetPlayer.Register(this, PlayerJoin);
            ServerApi.Hooks.GameInitialize.Register(this, WorldLoaded);
            GeneralHooks.ReloadEvent += Reload;
            PlayerHooks.PlayerChat += Chat;
            RegionHooks.RegionEntered += Spawned;

            Commands.ChatCommands.Add(new Command("tf.team", BlueTeam, "blu", "blue"));
            Commands.ChatCommands.Add(new Command("tf.team", RedTeam, "red"));
            Commands.ChatCommands.Add(new Command("tf.team", Select, "select"));
            Commands.ChatCommands.Add(new Command("tf.manager", setSpawn, "ss"));


        }

        public void Reload(ReloadEventArgs args)
        {
            Config = Config.Read();
        }

        public void Spawned(RegionHooks.RegionEnteredEventArgs args)
        {
            TSPlayer Player = args.Player;
            TFPlayer tF = TFPlayer.GetByUsername(Player.Name);

            if(args.Region.Name != Config.sorterRegionName)
            {
                return;
            }

            if(tF.Team == TeamManager.Blue())
            {
                Player.Teleport((int)Config.blueSpawnPoint.X*16, (int)Config.blueSpawnPoint.Y*16);
                return;
            }
            if (tF.Team == TeamManager.Red())
            {
                Player.Teleport((int)Config.redSpawnPoint.X * 16, (int)Config.redSpawnPoint.Y * 16);
                return;
            }
            if (tF.Team.team == "none")
            {
                Player.Teleport((int)Config.spawnPosition.X * 16, (int)Config.spawnPosition.Y * 16);
                return;
            }

        }
        
        public void setSpawn(CommandArgs args)
        {
            TSPlayer Player = args.Player;

            if(args.Parameters.Count == 0)
            {
                Player.SendErrorMessage("Enter a team to set the spawn for! /ss b/r");
                return;
            }

            string Team = args.Parameters[0].ToString();
            var newPosX = new int();
            var newPosY = new int();

            if(Team.ToLower() == TeamManager.Blue().team.ToLower() || Team.ToLower() == "b" || Team.ToLower() == "blue")
            {
                newPosX = args.Player.TileX+1;
                newPosY = args.Player.TileY - 1;

                Player.SendSuccessMessage($"You have successfully set the blue team's spawn at: X: {newPosX} Y: {newPosY}");
                Config.blueSpawnPoint = new Vector2(newPosX, newPosY);
                Config.Write();
                return;
            }

            if (Team.ToLower() == TeamManager.Red().team.ToLower() || Team.ToLower() == "r")
            {
                newPosX = args.Player.TileX + 1;
                newPosY = args.Player.TileY - 1;
                Player.SendSuccessMessage($"You have successfully set the red team's spawn at: X: {newPosX} Y: {newPosY}");
                Config.redSpawnPoint = new Vector2(newPosX, newPosY);
                Config.Write();
                return;
            }

                newPosX = args.Player.TileX + 1;
                newPosY = args.Player.TileY - 1;
                Player.SendSuccessMessage($"You have successfully set the spawn point at: X: {newPosX} Y: {newPosY}");
                Config.spawnPosition = new Vector2(newPosX, newPosY);
                Config.Write();
                return;
        }

        public void Select(CommandArgs args)
        {
            TSPlayer Player = args.Player;
            TFPlayer tfP = TFPlayer.GetByUsername(Player.Name);
            List<Kit> kits = Config.kits;
            var kitString = "";

            foreach(Kit kit in kits)
            {
                if(kit.name == kits[kits.Count - 1].name)
                {
                    kitString += kit.name + " ";
                }
                else
                {
                    kitString += kit.name + ", ";
                }
            }

            if(args.Parameters.Count == 0)
            {
                Player.SendErrorMessage("Enter a character to play as: " + kitString);
                return;
            }

            string Char = args.Parameters[0];
            
            for (var i = 0; i < Player.TPlayer.inventory.Length; i++)
            {
                Player.TPlayer.inventory[i].TurnToAir();
                NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.inventory[i].Name, NetworkText.Mode.Literal), Player.Index, i, 0);
            }

            if (tfP.Team.Equals(TeamManager.Blue()))
            {
                for (var i = 0; i < Player.TPlayer.dye.Length; i++)
                {
                    Player.TPlayer.dye[i] = TShock.Utils.GetItemById(ItemID.BlueDye);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.dye[i].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Dye0 + i, 0);
                }
            }



            if (tfP.Team.Equals(TeamManager.Red()))
            {
                for (var i = 0; i < Player.TPlayer.dye.Length; i++)
                {
                    Player.TPlayer.dye[i] = TShock.Utils.GetItemById(ItemID.RedDye);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.dye[i].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Dye0 + i, 0);
                }
            }


            if (kits.Find(x => x.name == Char).name == Char)
            {
                var kit = kits.Find(x => x.name == Char);
                foreach(Tuple<int,int> item in kit.items)
                {
                    if(TShock.Utils.GetItemById(item.Item1).FitsAmmoSlot() == true)
                    {
                        var Item = TShock.Utils.GetItemById(item.Item1);
                        Item.stack = item.Item2;
                        Player.TPlayer.inventory[(int)ItemSlot.AmmoSlot1] = Item;
                        NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.inventory[(int)ItemSlot.AmmoSlot1].Name, NetworkText.Mode.Literal), Player.Index, (int)ItemSlot.AmmoSlot1, 0);

                    }
                    else
                    {
                        Player.GiveItem(item.Item1, item.Item2);
                    }

                }

                Player.TPlayer.armor[0] = TShock.Utils.GetItemById(kit.armor[0]);
                NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.armor[0].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Armor0, 0);

                Player.TPlayer.armor[1] = TShock.Utils.GetItemById(kit.armor[1]);
                NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.armor[1].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Armor0+1, 0);

                Player.TPlayer.armor[2] = TShock.Utils.GetItemById(kit.armor[2]);
                NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.armor[2].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Armor0+2, 0);


                for (var i = 0; i < kit.accessories.Count; i++)
                {
                    Player.TPlayer.armor[3+i] = TShock.Utils.GetItemById(kit.accessories[i]);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.armor[2+i].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Armor0+3+i, 0);
                }
            }
            else
            {
                Player.SendErrorMessage("Invalid character! Select a character: " +kitString);
                return;
            }

        }

        private void WorldLoaded(EventArgs args)
        {
            Config = Config.Read();
            Team BlueTeam = new Team("BLU");
            BlueTeam.spawnPoint = Config.blueSpawnPoint;
            Team RedTeam = new Team("RED");
            RedTeam.spawnPoint = Config.redSpawnPoint;
            Teams.Add(BlueTeam);
            Teams.Add(RedTeam);
            Console.WriteLine(Config.kits[0].name);
        }

        private void Chat(PlayerChatEventArgs args)
        {
            TSPlayer Player = args.Player;
            TFPlayer TFp = TFPlayer.GetByUsername(Player.Name);
            if(TFp.Team.team == "none")
            {
                args.TShockFormattedText = args.TShockFormattedText;

            }
            Console.WriteLine(TFp.Team.team);
            if (TFp.Team.Equals(TeamManager.Red()))
            {
                args.TShockFormattedText = "[c/ff5959:[][c/ff5959:R][c/ff5959:E][c/ff5959:D][c/ff5959:]] " + args.TShockFormattedText;
            }
            if (TFp.Team.Equals(TeamManager.Blue()))
            {
                args.TShockFormattedText = "[c/1188ff:[][c/1188ff:B][c/1188ff:L][c/1188ff:U][c/1188ff:]] " + args.TShockFormattedText;
            }
        }

        private void PlayerJoin(GreetPlayerEventArgs args)
        {
            TSPlayer Player = TShock.Players[args.Who];
            TFPlayer tfp = new TFPlayer(Player);
            tfp.Team = new Team("none");
            Player.TPlayer.Spawn_SetPosition((int)Config.spawnPosition.X, (int)Config.spawnPosition.Y);
            players.Add(tfp);
            AskPlayerJoinTeam(Player);
            for (var i = 0; i < Player.TPlayer.inventory.Length; i++)
            {
                Player.TPlayer.inventory[i].TurnToAir();
                NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.inventory[i].Name, NetworkText.Mode.Literal), Player.Index, i, 0);
            }
        }

        public void KitsAdmin(CommandArgs args)
        {

        }

        public void BlueTeam(CommandArgs args)
        {
            TSPlayer Player = args.Player;
            TFPlayer tFPlayer = TFPlayer.GetByUsername(Player.Name);
            tFPlayer.Team = TeamManager.Blue();
            TeamManager.Blue().TFPlayers.Add(tFPlayer);
            Player.TPlayer.ChangeSpawn((int)Config.blueSpawnPoint.X, (int)Config.blueSpawnPoint.Y);
            Player.Teleport((int)Config.blueSpawnPoint.X * 16, (int)Config.blueSpawnPoint.Y * 16);
            Player.SendMessage($"[{Config.gameModeName}] You are now in the blue team!", Color.LightBlue);

            if (tFPlayer.Team.Equals(TeamManager.Blue()))
            {
                for (var i = 0; i < Player.TPlayer.dye.Length; i++)
                {
                    Player.TPlayer.dye[i] = TShock.Utils.GetItemById(ItemID.BlueDye);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.dye[i].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Dye0 + i, 0);
                }
            }



            if (tFPlayer.Team.Equals(TeamManager.Red()))
            {
                for (var i = 0; i < Player.TPlayer.dye.Length; i++)
                {
                    Player.TPlayer.dye[i] = TShock.Utils.GetItemById(ItemID.RedDye);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.dye[i].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Dye0 + i, 0);
                }
            }

            return;
        }

        public void RedTeam(CommandArgs args)
        {
            TSPlayer Player = args.Player;
            TFPlayer tFPlayer = TFPlayer.GetByUsername(Player.Name);
            tFPlayer.Team = TeamManager.Red();
            TeamManager.Red().TFPlayers.Add(tFPlayer);
            Player.TPlayer.ChangeSpawn((int)Config.redSpawnPoint.X, (int)Config.redSpawnPoint.Y);
            Player.Teleport((int)Config.redSpawnPoint.X * 16, (int)Config.redSpawnPoint.Y * 16);
            Player.SendMessage($"[{Config.gameModeName}] You are now in the red team!", Color.IndianRed);

            if (tFPlayer.Team.Equals(TeamManager.Blue()))
            {
                for (var i = 0; i < Player.TPlayer.dye.Length; i++)
                {
                    Player.TPlayer.dye[i] = TShock.Utils.GetItemById(ItemID.BlueDye);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.dye[i].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Dye0 + i, 0);
                }
            }



            if (tFPlayer.Team.Equals(TeamManager.Red()))
            {
                for (var i = 0; i < Player.TPlayer.dye.Length; i++)
                {
                    Player.TPlayer.dye[i] = TShock.Utils.GetItemById(ItemID.RedDye);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new NetworkText(Player.TPlayer.dye[i].Name, NetworkText.Mode.Literal), Player.Index, PlayerItemSlotID.Dye0 + i, 0);
                }
            }
            return;
        }

        public void AskPlayerJoinTeam(TSPlayer player)
        {
            player.SendMessage($"[{Config.gameModeName}] Join a team to start! Use /blu or /red!", Color.OrangeRed);
        }
        
        #region item slot indexes
        public enum ItemSlot
        {
            InvRow1Slot1, InvRow1Slot2, InvRow1Slot3, InvRow1Slot4, InvRow1Slot5, InvRow1Slot6, InvRow1Slot7, InvRow1Slot8, InvRow1Slot9, InvRow1Slot10,
            InvRow2Slot1, InvRow2Slot2, InvRow2Slot3, InvRow2Slot4, InvRow2Slot5, InvRow2Slot6, InvRow2Slot7, InvRow2Slot8, InvRow2Slot9, InvRow2Slot10,
            InvRow3Slot1, InvRow3Slot2, InvRow3Slot3, InvRow3Slot4, InvRow3Slot5, InvRow3Slot6, InvRow3Slot7, InvRow3Slot8, InvRow3Slot9, InvRow3Slot10,
            InvRow4Slot1, InvRow4Slot2, InvRow4Slot3, InvRow4Slot4, InvRow4Slot5, InvRow4Slot6, InvRow4Slot7, InvRow4Slot8, InvRow4Slot9, InvRow4Slot10,
            InvRow5Slot1, InvRow5Slot2, InvRow5Slot3, InvRow5Slot4, InvRow5Slot5, InvRow5Slot6, InvRow5Slot7, InvRow5Slot8, InvRow5Slot9, InvRow5Slot10,
            CoinSlot1, CoinSlot2, CoinSlot3, CoinSlot4, AmmoSlot1, AmmoSlot2, AmmoSlot3, AmmoSlot4, HandSlot,
            ArmorHeadSlot, ArmorBodySlot, ArmorLeggingsSlot, AccessorySlot1, AccessorySlot2, AccessorySlot3, AccessorySlot4, AccessorySlot5, AccessorySlot6, UnknownSlot1,
            VanityHeadSlot, VanityBodySlot, VanityLeggingsSlot, SocialAccessorySlot1, SocialAccessorySlot2, SocialAccessorySlot3, SocialAccessorySlot4, SocialAccessorySlot5, SocialAccessorySlot6, UnknownSlot2,
            DyeHeadSlot, DyeBodySlot, DyeLeggingsSlot, DyeAccessorySlot1, DyeAccessorySlot2, DyeAccessorySlot3, DyeAccessorySlot4, DyeAccessorySlot5, DyeAccessorySlot6, Unknown3,
            EquipmentSlot1, EquipmentSlot2, EquipmentSlot3, EquipmentSlot4, EquipmentSlot5,
            DyeEquipmentSlot1, DyeEquipmentSlot2, DyeEquipmentSlot3, DyeEquipmentSlot4, DyeEquipmentSlot5
        };
        #endregion
        /// <summary>
        /// Performs plugin cleanup logic
        /// Remove your hooks and perform general cleanup here
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, PlayerJoin);
                //unhook
                //dispose child objects
                //set large objects to null
            }
            base.Dispose(disposing);
        }
    }
}
