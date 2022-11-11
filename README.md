# Terraria Fortress
**Plugin progress to completion: 15%** **ETA: Late-November? We'll see!**

Support me & this plugin's (along with several others) development on Ko.Fi: [Here!](https://ko-fi.com/averageterraria)

# **REQUIREMENTS**
These are required by this plugin to function correctly.
- SSC enabled! **Initial startup kit should be DELETED**
- Do not set pvp-mode to always, the plugin will handle this for you.
- An entire server for this to work on. (so either a dimensions setup, where this is a seperate server or... just run a standalone server for this? lolol)
- A world that would make sense for a gamemode like this... include two team spawn points (red and blue themed), a lobby (in the sky), a battlegrounds, 
- TShock V5, duh...

# Introduction & Implemented Stuff
This will be a Team Fortress 2-styled minigame plugin! This plugin was actually just started earlier today, and I've only pushed out the first commit but within (hopefully not too much time) time, I am planning a fully-working kit-based PVP game mode. My plugin will be themed around Team Fortress' characters and whatnot, such as the Scout, Sniper, etc. BUT- the way I'm developing this plugin, you could ideally use this plugin and completely change the gamemode, with your own loadouts/setups. Instead of taking on the TF2 style that I will be using in my server, you could make this into a more generic KitPvP type of situation. Right now, here are the plans for the plugin/what I've done so far:

- Loadout builder - within the config file you may set up custom loadouts. You can add items such as: any inventory items, the three armor pieces, and accessories! (planning on making it possible for prefix-setting, for potentially better gameplay-balancing). Ammo will automatically be put into your ammo slots. Loadouts/classes can also have an attributed health value. This will change the player's max health when they /select the class.
- Teams - there are two teams, both named after Team Fortress teams, RED and BLU. Firstly, each player on joining gets the option to join either team. For this period, they are teleported to a user-defined spawn location (/ss s). This would be considered the "mini-game lobby area", if you will. The player can use /red or /blu (and yes they can also type in /blue) to switch to either team. After, they will have an additional prefix, representing either [RED] or [BLU]. This will save on user relogs.
- Each team has a user defined spawn location (/ss b/r) where the player will then be warped to after entering the respective command. The player will also be equipped with the team's respective dye, visually allowing other players to see the team you are on.
- Inventories **ARE MANAGED ON SSC ONLY**- and actually, this plugin ONLY works with SSC enabled. Otherwise, player's inventories can not really be managed. This type of SSC implementation completely changes things however, it is not like a survival inventory SSC. At the beginning, they are reset fully. The player should have NOTHING when registering. Once the player /select <character/loadout/whatever you wanna call 'em>, their inventory will be managed appropriately. Armor will be automatically put on, along with any accessories defined in the config. The dyes will be kept, despite the inventory fully resetting each time a player uses /select (this is to prevent players from mixing loadouts). As stated earlier, ammo will also go into the designated slot.
- A weird mechanic relating to this plugin **is the "sorter" region**. Browsing the config, you will see this and likely wonder what that means. Essentially, (as of right now, this whole thing is subject to deletion) I got stuck in a certain part of the development with player spawn points. No matter how I implemented it, it would just use the normal world spawn point, so what I have done here is, set the **world** spawn point right inside of a "sorter" region. What this sorter region does it... well... sort the players out to their respective spawns. Users without a team will go to the "spawnpoint" as defined in the config, and the users with a team will go their team spawn points, which can again be set in the config. (/reload does work! but a command may be added for defining a region later)
- Game Instance Mechanics. There is a match time, it auto resets, and auto-starts when there is a certain amount of players (configurable). At the start of each match, every player is warped to their respective spawn point.
- Border regions. Border1 and Border2 will warp the player back to their spawn points.

# Planned Features
- Control points (Maybe? Yeah probably...)
- Removing "sorter" region (if i can figure out a way lmao)
- Loadout-defined buffs
- A command for defining loadouts
- Possibly integrate block destruction and building? (resetting the map after each round and allowing tile placements)
- Leaderboard-type thing (shows winning team, top player(s), time left in round)
- DB-stored player stats (playtime, kill count, death count, etc.)
- Integrate a currency of some kind, potentially for ranking up (and unlocking other classes) (#fuck timeranks :>)
- Integrate CE item support (prob not but i'd like to)
- Integrate random events & disasters (mob boss spawns, meteor drops, acid rain, bombs fall from the sky)
- potential level system, which could maybe be a buildup of your character life? or defense... or ... idk something that doesn't completely throw off the balance (TBD)
- PvP check for an item that isn't in kits (kick this player, prob illegally spawned item)
- Integrate a team chat command /team <message>
- Clans integration - Clans for... some reason? Why the hell not lol
- Eventually.. my pre-defined config that i use for this server
- And much more to come...

I'm really excited because this is my biggest properly STANDALONE project, that I guess could be used by the public without issue? Or at least not too much issue... haha

Hopefully this helps some of you out there! My Discord is Average#1305 in case you need to submit any bug reports or feedback! Thank you :D
