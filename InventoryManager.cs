using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.Social;
using TShockAPI;
using TShockAPI.DB;

namespace InventoryManager
{
    public class InventoryManager
    {
        public class Inventory
        {
            public Inventory(string name) => this.name = name;
            public struct Item
            {
                public int type;
                public int stack;
                public byte prefix;
            }

            public readonly string name;
            public bool male;
            public bool[] hideVisibleAccessory = new bool[10];
            public readonly Item[] inventory = new Item[59];
            public readonly Item[] armor = new Item[20];
            public int[] dyes = new int[10];
            public int[] miscEquips = new int[5];
            public int[] miscDyes = new int[5];
            public int skinVariant;
            public int hair;
            public Color eyeColor;
            public Color skinColor;
            public Color pantsColor;
            public Color underShirtColor;
            public Color shirtColor;
            public Color hairColor;
            public Color shoeColor;
            public byte hideMisc;
            public bool superCart;
        }

        public delegate void LoadD(int playerIndex, Player oldPlayer);
        public static event LoadD LoadInventory;

        public InventoryManager(TSPlayer player) => Player = player;
        public TSPlayer Player { get; private set; }
        public int UserId => Player?.Account?.ID ?? 0;

        public void SSC()
        {
            if (Main.ServerSideCharacter)
                return;
            MemoryStream stream = new();
            BinaryWriter writer = new(stream);
            var currentPosition = 3L;
            writer.BaseStream.Position = 2L;
            writer.Write(7);
            writer.BaseStream.Position = currentPosition;
            writer.Write((int)Main.time);
            BitsByte bitsByte = 0;
            bitsByte[0] = Main.dayTime;
            bitsByte[1] = Main.bloodMoon;
            bitsByte[2] = Main.eclipse;
            writer.Write(bitsByte);
            writer.Write((byte)Main.moonPhase);
            writer.Write((short)Main.maxTilesX);
            writer.Write((short)Main.maxTilesY);
            writer.Write((short)Main.spawnTileX);
            writer.Write((short)Main.spawnTileY);
            writer.Write((short)Main.worldSurface);
            writer.Write((short)Main.rockLayer);
            writer.Write(Main.worldID);
            writer.Write(Main.worldName);
            writer.Write((byte)Main.GameMode);
            writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
            writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
            writer.Write((byte)Main.moonType);
            writer.Write((byte)WorldGen.treeBG1);
            writer.Write((byte)WorldGen.treeBG2);
            writer.Write((byte)WorldGen.treeBG3);
            writer.Write((byte)WorldGen.treeBG4);
            writer.Write((byte)WorldGen.corruptBG);
            writer.Write((byte)WorldGen.jungleBG);
            writer.Write((byte)WorldGen.snowBG);
            writer.Write((byte)WorldGen.hallowBG);
            writer.Write((byte)WorldGen.crimsonBG);
            writer.Write((byte)WorldGen.desertBG);
            writer.Write((byte)WorldGen.oceanBG);
            writer.Write((byte)WorldGen.mushroomBG);
            writer.Write((byte)WorldGen.underworldBG);
            writer.Write((byte)Main.iceBackStyle);
            writer.Write((byte)Main.jungleBackStyle);
            writer.Write((byte)Main.hellBackStyle);
            writer.Write(Main.windSpeedTarget);
            writer.Write((byte)Main.numClouds);
            for (int i = 0; i < 3; i++)
            {
                writer.Write(Main.treeX[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                writer.Write((byte)Main.treeStyle[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                writer.Write(Main.caveBackX[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                writer.Write((byte)Main.caveBackStyle[i]);
            }
            WorldGen.TreeTops.SyncSend(writer);
            writer.Write(Main.maxRaining);
            BitsByte bitsByte2 = 0;
            bitsByte2[0] = WorldGen.shadowOrbSmashed;
            bitsByte2[1] = NPC.downedBoss1;
            bitsByte2[2] = NPC.downedBoss2;
            bitsByte2[3] = NPC.downedBoss3;
            bitsByte2[4] = Main.hardMode;
            bitsByte2[5] = NPC.downedClown;
            bitsByte2[6] = true;
            bitsByte2[7] = NPC.downedPlantBoss;
            writer.Write(bitsByte2);
            BitsByte bitsByte3 = 0;
            bitsByte3[0] = NPC.downedMechBoss1;
            bitsByte3[1] = NPC.downedMechBoss2;
            bitsByte3[2] = NPC.downedMechBoss3;
            bitsByte3[3] = NPC.downedMechBossAny;
            bitsByte3[4] = Main.cloudBGActive >= 1f;
            bitsByte3[5] = WorldGen.crimson;
            bitsByte3[6] = Main.pumpkinMoon;
            bitsByte3[7] = Main.snowMoon;
            writer.Write(bitsByte3);
            BitsByte bitsByte4 = 0;
            bitsByte4[1] = Main.fastForwardTimeToDawn;
            bitsByte4[2] = Main.slimeRain;
            bitsByte4[3] = NPC.downedSlimeKing;
            bitsByte4[4] = NPC.downedQueenBee;
            bitsByte4[5] = NPC.downedFishron;
            bitsByte4[6] = NPC.downedMartians;
            bitsByte4[7] = NPC.downedAncientCultist;
            writer.Write(bitsByte4);
            BitsByte bitsByte5 = 0;
            bitsByte5[0] = NPC.downedMoonlord;
            bitsByte5[1] = NPC.downedHalloweenKing;
            bitsByte5[2] = NPC.downedHalloweenTree;
            bitsByte5[3] = NPC.downedChristmasIceQueen;
            bitsByte5[4] = NPC.downedChristmasSantank;
            bitsByte5[5] = NPC.downedChristmasTree;
            bitsByte5[6] = NPC.downedGolemBoss;
            bitsByte5[7] = BirthdayParty.PartyIsUp;
            writer.Write(bitsByte5);
            BitsByte bitsByte6 = 0;
            bitsByte6[0] = NPC.downedPirates;
            bitsByte6[1] = NPC.downedFrost;
            bitsByte6[2] = NPC.downedGoblins;
            bitsByte6[3] = Sandstorm.Happening;
            bitsByte6[4] = DD2Event.Ongoing;
            bitsByte6[5] = DD2Event.DownedInvasionT1;
            bitsByte6[6] = DD2Event.DownedInvasionT2;
            bitsByte6[7] = DD2Event.DownedInvasionT3;
            writer.Write(bitsByte6);
            BitsByte bitsByte7 = 0;
            bitsByte7[0] = NPC.combatBookWasUsed;
            bitsByte7[1] = LanternNight.LanternsUp;
            bitsByte7[2] = NPC.downedTowerSolar;
            bitsByte7[3] = NPC.downedTowerVortex;
            bitsByte7[4] = NPC.downedTowerNebula;
            bitsByte7[5] = NPC.downedTowerStardust;
            bitsByte7[6] = Main.forceHalloweenForToday;
            bitsByte7[7] = Main.forceXMasForToday;
            writer.Write(bitsByte7);
            BitsByte bitsByte8 = 0;
            bitsByte8[0] = NPC.boughtCat;
            bitsByte8[1] = NPC.boughtDog;
            bitsByte8[2] = NPC.boughtBunny;
            bitsByte8[3] = NPC.freeCake;
            bitsByte8[4] = Main.drunkWorld;
            bitsByte8[5] = NPC.downedEmpressOfLight;
            bitsByte8[6] = NPC.downedQueenSlime;
            bitsByte8[7] = Main.getGoodWorld;
            writer.Write(bitsByte8);
            BitsByte bitsByte9 = 0;
            bitsByte9[0] = Main.tenthAnniversaryWorld;
            bitsByte9[1] = Main.dontStarveWorld;
            bitsByte9[2] = NPC.downedDeerclops;
            bitsByte9[3] = Main.notTheBeesWorld;
            bitsByte9[4] = Main.remixWorld;
            bitsByte9[5] = NPC.unlockedSlimeBlueSpawn;
            bitsByte9[6] = NPC.combatBookVolumeTwoWasUsed;
            bitsByte9[7] = NPC.peddlersSatchelWasUsed;
            writer.Write(bitsByte9);
            BitsByte bitsByte10 = 0;
            bitsByte10[0] = NPC.unlockedSlimeGreenSpawn;
            bitsByte10[1] = NPC.unlockedSlimeOldSpawn;
            bitsByte10[2] = NPC.unlockedSlimePurpleSpawn;
            bitsByte10[3] = NPC.unlockedSlimeRainbowSpawn;
            bitsByte10[4] = NPC.unlockedSlimeRedSpawn;
            bitsByte10[5] = NPC.unlockedSlimeYellowSpawn;
            bitsByte10[6] = NPC.unlockedSlimeCopperSpawn;
            bitsByte10[7] = Main.fastForwardTimeToDusk;
            writer.Write(bitsByte10);
            BitsByte bitsByte11 = 0;
            bitsByte11[0] = Main.noTrapsWorld;
            bitsByte11[1] = Main.zenithWorld;
            writer.Write(bitsByte11);
            writer.Write((byte)Main.sundialCooldown);
            writer.Write((byte)Main.moondialCooldown);
            writer.Write((short)WorldGen.SavedOreTiers.Copper);
            writer.Write((short)WorldGen.SavedOreTiers.Iron);
            writer.Write((short)WorldGen.SavedOreTiers.Silver);
            writer.Write((short)WorldGen.SavedOreTiers.Gold);
            writer.Write((short)WorldGen.SavedOreTiers.Cobalt);
            writer.Write((short)WorldGen.SavedOreTiers.Mythril);
            writer.Write((short)WorldGen.SavedOreTiers.Adamantite);
            writer.Write((sbyte)Main.invasionType);
            if (SocialAPI.Network != null)
            {
                writer.Write(SocialAPI.Network.GetLobbyId());
            }
            else
            {
                writer.Write(0L);
            }
            writer.Write(Sandstorm.IntendedSeverity);
            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = 0L;
            writer.Write((short)currentPosition);
            writer.BaseStream.Position = currentPosition;
            Player.SendRawData(stream.ToArray());
        }
        public bool Rename(string name, string newName, out bool exists)
        {
            exists = Find(newName);
            if (Find(name) && !exists)
                return DB.db.Query("UPDATE SavedInventoryData SET Name = @0 WHERE UserID = @1 AND Name = @2", newName, UserId, name) > 0;
            return false;
        }
        public bool Delete(string name)
        {
            return DB.db.Query("DELETE FROM SavedInventoryData WHERE UserID = @0 AND Name = @1", UserId, name) > 0;
        }
        public bool Save(string name)
        {
            try
            {
                static string RGB(Color color)
                {
                    return string.Format("{0},{1},{2}", color.R, color.G, color.B);
                }
                string hideVisibleAccessory = string.Join(",", Player.TPlayer.hideVisibleAccessory.Select(i => i.ToInt()));
                string inventory = string.Join("~", Player.TPlayer.inventory.Select(i => $"{i.type},{i.stack},{i.prefix}"));
                string armor = string.Join("~", Player.TPlayer.armor.Select(i => $"{i.type},{i.prefix}"));
                string dye = string.Join(",", Player.TPlayer.dye.Select(i => i.type));
                string miscEquips = string.Join(",", Player.TPlayer.miscEquips.Select(i => i.type));
                string miscDyes = string.Join(",", Player.TPlayer.miscDyes.Select(i => i.type));
                var eyeColor = RGB(Player.TPlayer.eyeColor);
                var skinColor = RGB(Player.TPlayer.skinColor);
                var hairColor = RGB(Player.TPlayer.hairColor);
                var shirtColor = RGB(Player.TPlayer.shirtColor);
                var underShirtColor = RGB(Player.TPlayer.underShirtColor);
                var pantsColor = RGB(Player.TPlayer.pantsColor);
                var shoeColor = RGB(Player.TPlayer.shoeColor);
                var male = Player.TPlayer.Male;
                var skinVariant = Player.TPlayer.skinVariant;
                var hair = Player.TPlayer.hair;
                DB.db.Query("DELETE FROM Inventories WHERE UserID = @0 AND Name = @1", UserId, name);
                return DB.db.Query($"INSERT INTO Inventories VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20)", 
                    Player.Name, 
                    UserId, 
                    name, 
                    inventory, 
                    armor, 
                    dye,
                    hideVisibleAccessory,
                    miscEquips,
                    miscDyes,
                    male,
                    hair,
                    skinVariant,
                    skinColor,
                    eyeColor,
                    hairColor,
                    shirtColor,
                    underShirtColor,
                    pantsColor,
                    shoeColor,
                    Player.TPlayer.hideMisc.value,
                    Player.TPlayer.unlockedSuperCart.ToInt()) > 0;
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError("[InvenoryManager Error]: {0}", ex.Message);
                return false;
            }
        }
        public bool Load(string name)
        {
            var inventory = GetPlayerInventories().Find(i => i.name == name);
            if (inventory != null)
            {
                SSC();
                var player = (Player)Player.TPlayer.Clone();
                for (int i = 0; i < 59; i++)
                {
                    Player.TPlayer.inventory[i].SetDefaults(inventory.inventory[i].type);
                    Player.TPlayer.inventory[i].stack = inventory.inventory[i].stack;
                    Player.TPlayer.inventory[i].prefix = inventory.inventory[i].prefix;
                    NetMessage.SendData(5, Player.Index, -1, null, Player.Index, i, inventory.inventory[i].prefix);
                }
                for (int i = 0; i < 20; i++)
                {
                    Player.TPlayer.armor[i].SetDefaults(inventory.armor[i].type);
                    Player.TPlayer.armor[i].Prefix(inventory.armor[i].prefix);
                    NetMessage.SendData(5, Player.Index, -1, null, Player.Index, i + 59, inventory.armor[i].prefix);
                }
                for (int i = 0; i < 10; i++)
                {
                    Player.TPlayer.dye[i].SetDefaults(inventory.dyes[i]);
                    NetMessage.SendData(5, Player.Index, -1, null, Player.Index, i + 79);
                }
                for (int i = 0; i < 5; i++)
                {
                    Player.TPlayer.miscEquips[i].SetDefaults(inventory.miscEquips[i]);
                    Player.TPlayer.miscDyes[i].SetDefaults(inventory.miscDyes[i]);
                    NetMessage.SendData(5, Player.Index, -1, null, Player.Index, i + 89);
                    NetMessage.SendData(5, Player.Index, -1, null, Player.Index, i + 94);
                }
                Player.TPlayer.shoeColor = inventory.shoeColor;
                Player.TPlayer.pantsColor = inventory.pantsColor;
                Player.TPlayer.underShirtColor = inventory.underShirtColor;
                Player.TPlayer.shirtColor = inventory.shirtColor;
                Player.TPlayer.hairColor = inventory.hairColor;
                Player.TPlayer.hair = inventory.hair;
                Player.TPlayer.Male = inventory.male;
                Player.TPlayer.eyeColor = inventory.eyeColor;
                Player.TPlayer.skinColor = inventory.skinColor;
                Player.TPlayer.skinVariant = inventory.skinVariant;
                Player.TPlayer.hideVisibleAccessory = inventory.hideVisibleAccessory;
                Player.TPlayer.hideMisc = inventory.hideMisc;
                NetMessage.SendData(4, -1, -1, null, Player.Index);
                LoadInventory?.Invoke(Player.Index, player);
                return true;
            }
            return false;
        }
        public bool Find(string name)
        {
            return GetPlayerInventories().Find(i => i.name == name) != null;
        }
        public List<Inventory> GetPlayerInventories()
        {
            List<Inventory> inventories = new();
            using var reader = DB.db.QueryReader("SELECT * FROM Inventories WHERE UserID = @0", UserId);
            while (reader.Read())
            {
                var Inventory = reader.Get<string>("Inventory").Split("~");
                var Armor = reader.Get<string>("Armor").Split("~");
                var HideVisibleAccessory = reader.Get<string>("HideVisibleAccessory").Split(",");
                var Dyes = reader.Get<string>("Dye").Split(",");
                var MiscDyes = reader.Get<string>("MiscDyes").Split(",");
                var MiscEquips = reader.Get<string>("MiscEquips").Split(",");
                var EyeColor = reader.Get<string>("EyeColor").Split(',');
                var SkinColor = reader.Get<string>("SkinColor").Split(',');
                var PantsColor = reader.Get<string>("PantsColor").Split(',');
                var ShirtColor = reader.Get<string>("ShirtColor").Split(',');
                var UnderShirtColor = reader.Get<string>("UnderShirtColor").Split(',');
                var ShoeColor = reader.Get<string>("ShoeColor").Split(',');
                var HairColor = reader.Get<string>("HairColor").Split(',');

                Inventory inventory = new(reader.Get<string>("Name"));
                for (int i = 0; i < Inventory?.Length; i++)
                {
                    var item = Inventory[i].Split(',');
                    if (item.IndexInRange(2))
                    {
                        inventory.inventory[i].type = int.Parse(item[0]);
                        inventory.inventory[i].stack = int.Parse(item[1]);
                        inventory.inventory[i].prefix = byte.Parse(item[2]);
                    }
                }
                for (int i = 0; i < Armor?.Length; i++)
                {
                    var item = Armor[i].Split(',');
                    if (item.IndexInRange(1))
                    {
                        inventory.armor[i].type = int.Parse(item[0]);
                        inventory.armor[i].prefix = byte.Parse(item[1]);
                    }
                }
                inventory.dyes = Dyes.Select(i => int.Parse(i)).ToArray();
                inventory.miscDyes = MiscDyes.Select(i => int.Parse(i)).ToArray();
                inventory.miscEquips = MiscEquips.Select(i => int.Parse(i)).ToArray();
                inventory.hideVisibleAccessory = HideVisibleAccessory.Select(i => i == "1").ToArray();
                inventory.eyeColor = new(byte.Parse(EyeColor[0]), byte.Parse(EyeColor[1]), byte.Parse(EyeColor[2]));
                inventory.skinColor = new(byte.Parse(SkinColor[0]), byte.Parse(SkinColor[1]), byte.Parse(SkinColor[2]));
                inventory.hairColor = new(byte.Parse(HairColor[0]), byte.Parse(HairColor[1]), byte.Parse(HairColor[2]));
                inventory.shirtColor = new(byte.Parse(ShirtColor[0]), byte.Parse(ShirtColor[1]), byte.Parse(ShirtColor[2]));
                inventory.underShirtColor = new(byte.Parse(UnderShirtColor[0]), byte.Parse(UnderShirtColor[1]), byte.Parse(UnderShirtColor[2]));
                inventory.pantsColor = new(byte.Parse(PantsColor[0]), byte.Parse(PantsColor[1]), byte.Parse(PantsColor[2]));
                inventory.shoeColor = new(byte.Parse(ShoeColor[0]), byte.Parse(ShoeColor[1]), byte.Parse(ShoeColor[2]));
                inventory.male = reader.Get<string>("Male") == "1";
                inventory.skinVariant = reader.Get<int>("SkinVariant");
                inventory.hair = reader.Get<int>("Hair");
                inventory.hideMisc = (byte)reader.Get<int>("HideMisc");
                inventory.superCart = reader.Get<string>("SuperCart") == "1";
                inventories.Add(inventory);
            }
            return inventories;
        }
    }
}
