using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Roleplay.Login
{
    class Character : Script
    {
        private static readonly string[] headOverlayNames = { "blemishes", "facialHair", "eyebrows", "ageing", "makeup", "blush", "complexion", "sunDamage", "lipstick", "molesFreckles", "chestHair", "bodyBlemishes", "addBodyBlemishes" };
        private static readonly string[] faceFeatureNames = {
            "f_noseWidth", "f_noseHeight", "f_noseLength", "f_noseBridge", "f_noseTip",
            "f_noseShift", "f_browHeight", "f_browWidth", "f_cheekboneHeight", "f_cheekboneWidth",
            "f_cheeksWidth", "f_eyes", "f_lips", "f_jawWidth", "f_jawHeight",
            "f_chinLength", "f_chinPosition", "f_chinWidth", "f_chinShape", "f_neckWidth"
        };

        private static readonly Vector3 spawn = new Vector3(-1167.994, -700.4285, 21.89281);

        private static HeadOverlay CreateHeadOverlay (Byte index, Byte color, Byte secondaryColor, float opacity)
        {
            return new HeadOverlay {
                Index = index,
                Color = color,
                SecondaryColor = secondaryColor,
                Opacity = opacity
            };
        }

        [RemoteEvent("OnPlayerCharacterAttempt")]
        public void OnPlayerCharacterAttempt(Client c, string vorname, string nachname)
        {
            if (vorname.Length < 3 || nachname.Length < 3)
            {
                c.SendNotification("Dein Vor-/Nachname muss mindestens 3 Buchstaben enthalten");
                return;
            }

            string specialChar = @"^\|!#$%&/()=?»«@£§€{}.-;:[]'<>_,1234567890 ";
            foreach (var item in specialChar)
            {
                if (vorname.Contains(item) || nachname.Contains(item))
                {
                    c.SendNotification("Dein Charakter darf keine Sonderzeichen enthalten!");
                    return;
                }
            }

            if (vorname == nachname)
            {
                c.SendNotification("Dein Vorname darf nicht wie dein Nachname klingen!");
                return;
            }

            vorname = vorname[0].ToString().ToUpper() + vorname.Substring(1);
            nachname = nachname[0].ToString().ToUpper() + nachname.Substring(1);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("UPDATE characters SET first_name = @vn, last_name = @nn, created = @cc WHERE id=@aid", conn);
            cmd.Parameters.AddWithValue("@aid", c.GetData("character_id"));
            cmd.Parameters.AddWithValue("@vn", vorname);
            cmd.Parameters.AddWithValue("@nn", nachname);
            cmd.Parameters.AddWithValue("@cc", true);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    c.SendNotification("Dieser Vor-/Nachname ist bereits ~r~vergeben~w~!");
                }

                DatabaseAPI.API.GetInstance().FreeConnection(conn);
                return;
            }

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            NAPI.Player.SetPlayerName(c, vorname + nachname);

            c.TriggerEvent("FinishCharacter");

            c.TriggerEvent("namehud", c);
        }

        [RemoteEvent("login.character.select")]
        public static void SelectCharacter(Client c, int id)  {
            Console.WriteLine("selectCharacter: " + id);

            int account_id = c.GetData("account_id");

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM characters WHERE id = @id AND account_id = @a_id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@a_id", account_id) ;
            MySqlDataReader r = cmd.ExecuteReader();   
            if (!r.Read())  {
                r.Close();
                DatabaseAPI.API.GetInstance().FreeConnection(conn);
            }
            NAPI.Player.SpawnPlayer(c, new Vector3(r.GetFloat("p_x"), r.GetFloat("p_y"), r.GetFloat("p_z")));
            if (r.GetUInt32("dim") != 0) {
                //c.Dimension = r.GetUInt32("dim");
                //c.SetData("houseid", c.Dimension);
                c.SetData("dimension", r.GetUInt32("dim"));
                c.SetData("houseid", r.GetUInt32("dim"));
            }  else
            {
                c.SetData("dimension", r.GetUInt32("dim"));
                //c.Dimension = r.GetUInt32("dim");
            }
            c.SetData("h_key", r.GetInt32("h_key"));
            c.SetData("fraktionrank", r.GetInt32("fraktionrank"));
            c.SetData("fraktion", r.GetInt32("fraktion"));
            c.SetData("vehicles", r.GetInt32("vehicles"));
            c.SetData("PlayerPaydayTimer", r.GetInt32("payday"));
            c.SetData("createdc", r.GetBoolean("created"));
            c.SetData("jailtime", r.GetInt32("jailtime"));

            c.Name = r.GetString("first_name") + r.GetString("last_name");
            r.Close();

            cmd = new MySqlCommand("SELECT * FROM characters_customization WHERE character_id = @character_id", conn);
            cmd.Parameters.AddWithValue("@character_id", id);
            r = cmd.ExecuteReader();
            if (r.Read())
            {
                c.SetData("character_id", id);

                // Populate the head
                HeadBlend headBlend = new HeadBlend
                {
                    ShapeFirst = r.GetByte("h_ShapeFirst"),
                    ShapeSecond = r.GetByte("h_ShapeSecond"),
                    ShapeThird = r.GetByte("h_ShapeThird"),
                    SkinFirst = r.GetByte("h_SkinFirst"),
                    SkinSecond = r.GetByte("h_SkinSecond"),
                    SkinThird = r.GetByte("h_SkinThird"),
                    ShapeMix = r.GetFloat("h_ShapeMix"),
                    SkinMix = r.GetFloat("h_SkinMix"),
                    ThirdMix = r.GetFloat("h_ThirdMix")
                };

                // Add the face features

                float[] faceFeatures = new float[faceFeatureNames.Length];
                for (int i = 0; i < faceFeatureNames.Length; i++)
                {
                    faceFeatures[i] = r.GetFloat(faceFeatureNames[i]);
                }

                // Populate the head overlays
                Dictionary<int, HeadOverlay> headOverlays = new Dictionary<int, HeadOverlay>();

                for (int i = 0; i < headOverlayNames.Length; i++)
                {
                    string s = headOverlayNames[i];
                    headOverlays.Add(i, CreateHeadOverlay(r.GetByte("o_i_" + s), r.GetByte("o_c_" + s), r.GetByte("o_c2_" + s), r.GetFloat("o_o_" + s)));
                }

                c.SetData("hair", r.GetInt32("hair"));
                c.SetData("isMale", r.GetBoolean("sex"));

                // Update the character's skin
                c.SetCustomization(r.GetBoolean("sex"), headBlend,
                r.GetByte("eyeColor"), r.GetByte("hairColor"), r.GetByte("hightlightColor"),
                faceFeatures, headOverlays, new Decoration[] { });
            }
            r.Close();

            MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM characters_clothes WHERE character_id = @character_id", conn);
            cmd2.Parameters.AddWithValue("@character_id", id);
            r = cmd2.ExecuteReader();
            if (r.Read())
            {
                //Klamotten der Spieler                 
                c.SetData("shoes", r.GetInt32("shoes"));                   
                c.SetData("legs", r.GetInt32("legs"));                
                c.SetData("tops", r.GetInt32("tops"));
                c.SetData("torsos", r.GetInt32("torsos"));
                c.SetData("undershirts", r.GetInt32("undershirts"));
            }
            r.Close();

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            if (c.GetData("createdc") == false)
            {
                c.TriggerEvent("StartCharBrowser");
            }

            c.Dimension = c.GetData("dimension");

            MoneyAPI.API.SyncCash(c);
            BankAPI.API.SyncCash(c);
            InventoryAPI.API.SyncItems(c);
            Player.PlayerUpdate.SyncPlayer(c);
            ApplyPlayerClothes(c);

            c.TriggerEvent("namehud", c);
            Player.PlayerTime.OnStartPayday(c);
            if (Init.Init.LSPDDoorLock == 1)
            {
                c.TriggerEvent("LSPDGateOpen");
            }
            c.SendNotification("Du kannst ganz im Notfall dich mit '/dc' disconnecten.");
        }

        private static void BindHead(MySqlCommand cmd,  string type, HeadOverlay headOverlay)
        {
            cmd.Parameters.AddWithValue("@o_i_" + type, headOverlay.Index);
            cmd.Parameters.AddWithValue("@o_c_" + type, headOverlay.Color);
            cmd.Parameters.AddWithValue("@o_c2_" + type, headOverlay.SecondaryColor);
            cmd.Parameters.AddWithValue("@o_o_" + type, headOverlay.Opacity);
        }


        [RemoteEvent("login.character.create")]
        public static void CreateCharacter(Client c, int hair, bool isMale, string headBlendJStr, byte eyeColor, byte hairColor, byte hightlightColor, string faceFeaturesStr, string headOverlaysJStr, string decorationJStr)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 99999);
            int randomNumber2 = random.Next(0, 99999);

            HeadBlend headBlend = JsonConvert.DeserializeObject<HeadBlend>(headBlendJStr);
            float[] faceFeatures = JsonConvert.DeserializeObject<float[]>(faceFeaturesStr);
            Dictionary<int, HeadOverlay> headOverlays = JsonConvert.DeserializeObject<Dictionary<int, HeadOverlay>>(headOverlaysJStr);
            Decoration[] decorations = JsonConvert.DeserializeObject<Decoration[]>(decorationJStr);

            MySqlConnection conn = DatabaseAPI.API.GetInstance().GetConnection();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO characters (first_name, last_name, created, account_id, p_x, p_y, p_z, cash) VALUES (@first_name, @last_name, @created, @account_id, @p_x, @p_y, @p_z, @cash)", conn);
            Console.WriteLine("Created cmd");
            cmd.Parameters.AddWithValue("@first_name", "None" + randomNumber);
            cmd.Parameters.AddWithValue("@last_name", "None" + randomNumber2);
            cmd.Parameters.AddWithValue("@created", false);
            cmd.Parameters.AddWithValue("@p_x", spawn.X);
            cmd.Parameters.AddWithValue("@p_y", spawn.Y);
            cmd.Parameters.AddWithValue("@p_z", spawn.Z);
            cmd.Parameters.AddWithValue("@account_id", c.GetData("account_id"));
            cmd.Parameters.AddWithValue("@cash", 8500);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                if(ex.Number == 1062)
                {
                    Log.WriteDError("Vor-/Nachname '" + "None" + randomNumber + " None" + randomNumber2 + "' existiert bereits!");
                    c.SendNotification("[~r~FEHLER~w~]: Bitte versuche es erneut.");
                    return;
                }
            }

            //DatabaseAPI.API.GetInstance().FreeConnection(conn);

            cmd = new MySqlCommand("SELECT LAST_INSERT_ID() AS id", conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            int characterId = -1;
            if (reader.Read()) {
                characterId = reader.GetInt32("id");
            }
            Console.WriteLine("Char Id: " + characterId);
            reader.Close();

            cmd = new MySqlCommand("INSERT INTO characters_customization (" +
                "character_id, sex, h_ShapeFirst, h_ShapeSecond, h_ShapeThird, h_SkinFirst, h_SkinSecond, h_SkinThird, h_ShapeMix, h_SkinMix, h_ThirdMix, " +
                "eyeColor, hair, hairColor, hightlightColor, " +
                "f_noseWidth, f_noseHeight, f_noseLength, f_noseBridge, f_noseTip, " +
                "f_noseShift, f_browHeight, f_browWidth, f_cheekboneHeight, f_cheekboneWidth, " +
                "f_cheeksWidth, f_eyes, f_lips, f_jawWidth, f_jawHeight, " +
                "f_chinLength, f_chinPosition, f_chinWidth, f_chinShape, f_neckWidth, " +
                "o_i_blemishes, o_c_blemishes, o_c2_blemishes, o_o_blemishes, " +
                "o_i_facialHair, o_c_facialHair, o_c2_facialHair, o_o_facialHair, " +
                "o_i_eyebrows, o_c_eyebrows, o_c2_eyebrows, o_o_eyebrows, " +
                "o_i_ageing, o_c_ageing, o_c2_ageing, o_o_ageing, " +
                "o_i_makeup, o_c_makeup, o_c2_makeup, o_o_makeup, " +
                "o_i_blush, o_c_blush, o_c2_blush, o_o_blush, " +
                "o_i_complexion, o_c_complexion, o_c2_complexion, o_o_complexion, " +
                "o_i_sunDamage, o_c_sunDamage, o_c2_sunDamage, o_o_sunDamage, " +
                "o_i_lipstick, o_c_lipstick, o_c2_lipstick, o_o_lipstick, " +
                "o_i_molesFreckles, o_c_molesFreckles, o_c2_molesFreckles, o_o_molesFreckles, " +
                "o_i_chestHair, o_c_chestHair, o_c2_chestHair, o_o_chestHair, " +
                "o_i_bodyBlemishes, o_c_bodyBlemishes, o_c2_bodyBlemishes, o_o_bodyBlemishes, " +
                "o_i_addBodyBlemishes, o_c_addBodyBlemishes, o_c2_addBodyBlemishes, o_o_addBodyBlemishes" +
                ")VALUES(" +
                "@character_id, @sex, @h_ShapeFirst, @h_ShapeSecond, @h_ShapeThird, @h_SkinFirst, @h_SkinSecond, @h_SkinThird, @h_ShapeMix, @h_SkinMix, @h_ThirdMix, " +
                "@eyeColor, @hair, @hairColor, @hightlightColor, " +
                "@f_noseWidth, @f_noseHeight, @f_noseLength, @f_noseBridge, @f_noseTip, " +
                "@f_noseShift, @f_browHeight, @f_browWidth, @f_cheekboneHeight, @f_cheekboneWidth, " +
                "@f_cheeksWidth, @f_eyes, @f_lips, @f_jawWidth, @f_jawHeight, " +
                "@f_chinLength, @f_chinPosition, @f_chinWidth, @f_chinShape, @f_neckWidth, " +
                "@o_i_blemishes, @o_c_blemishes, @o_c2_blemishes, @o_o_blemishes, " +
                "@o_i_facialHair, @o_c_facialHair, @o_c2_facialHair, @o_o_facialHair, " +
                "@o_i_eyebrows, @o_c_eyebrows, @o_c2_eyebrows, @o_o_eyebrows, " +
                "@o_i_ageing, @o_c_ageing, @o_c2_ageing, @o_o_ageing, " +
                "@o_i_makeup, @o_c_makeup, @o_c2_makeup, @o_o_makeup, " +
                "@o_i_blush, @o_c_blush, @o_c2_blush, @o_o_blush, " +
                "@o_i_complexion, @o_c_complexion, @o_c2_complexion, @o_o_complexion, " +
                "@o_i_sunDamage, @o_c_sunDamage, @o_c2_sunDamage, @o_o_sunDamage, " +
                "@o_i_lipstick, @o_c_lipstick, @o_c2_lipstick, @o_o_lipstick, " +
                "@o_i_molesFreckles, @o_c_molesFreckles, o_c2_molesFreckles, o_o_molesFreckles, " +
                "@o_i_chestHair, @o_c_chestHair, @o_c2_chestHair, @o_o_chestHair, " +
                "@o_i_bodyBlemishes, @o_c_bodyBlemishes, @o_c2_bodyBlemishes, @o_o_bodyBlemishes, " +
                "@o_i_addBodyBlemishes, @o_c_addBodyBlemishes, @o_c2_addBodyBlemishes, @o_o_addBodyBlemishes" +
                ")", 
            conn);

            cmd.Parameters.AddWithValue("@character_id", characterId);
            cmd.Parameters.AddWithValue("@sex", isMale);

            cmd.Parameters.AddWithValue("@h_ShapeFirst", headBlend.ShapeFirst);
            cmd.Parameters.AddWithValue("@h_ShapeSecond", headBlend.ShapeSecond);
            cmd.Parameters.AddWithValue("@h_ShapeThird", headBlend.ShapeThird);
            cmd.Parameters.AddWithValue("@h_SkinFirst", headBlend.SkinFirst);
            cmd.Parameters.AddWithValue("@h_SkinSecond", headBlend.SkinSecond);
            cmd.Parameters.AddWithValue("@h_SkinThird", headBlend.SkinThird);
            cmd.Parameters.AddWithValue("@h_ShapeMix", headBlend.ShapeMix);
            cmd.Parameters.AddWithValue("@h_SkinMix", headBlend.SkinMix);
            cmd.Parameters.AddWithValue("@h_ThirdMix", headBlend.ThirdMix);

            cmd.Parameters.AddWithValue("@hair", hair);

            cmd.Parameters.AddWithValue("@eyeColor", eyeColor);
            cmd.Parameters.AddWithValue("@hairColor", hairColor);
            cmd.Parameters.AddWithValue("@hightlightColor", hightlightColor);

            for (int i = 0; i < faceFeatureNames.Length; i++)
            {
                cmd.Parameters.AddWithValue("@" + faceFeatureNames[i], faceFeatures[0]);
            }

            HeadOverlay headOverlay;
            for (int i = 0; i < headOverlayNames.Length; i++)
            {
                string s = headOverlayNames[i];
                if (headOverlays.TryGetValue(i, out headOverlay))
                {
                    BindHead(cmd, s, headOverlay);
                } else
                {
                    BindHead(cmd, s, CreateHeadOverlay(0, 0, 0, 0));
                    Console.WriteLine("headOverlays Missing Key: " + s);
                }
            }
            cmd.ExecuteNonQuery();

            cmd = new MySqlCommand("INSERT INTO characters_clothes (character_id, torsos, shoes, legs, tops, undershirts) VALUES (@charid, @torsos, @shoes, @legs, @tops, @us)", conn);
            cmd.Parameters.AddWithValue("@charid", characterId);
            cmd.Parameters.AddWithValue("@shoes", 4);
            if (!isMale)
            {
                cmd.Parameters.AddWithValue("@tops", 0);
                cmd.Parameters.AddWithValue("@torsos", 4);
                cmd.Parameters.AddWithValue("@us", 2);
            } else
            {
                cmd.Parameters.AddWithValue("@tops", 13);
                cmd.Parameters.AddWithValue("@torsos", 11);
                cmd.Parameters.AddWithValue("@us", 15);
            }
            cmd.Parameters.AddWithValue("@legs", 4);
            cmd.ExecuteNonQuery();

            c.TriggerEvent("toggleCreator", false);

            DatabaseAPI.API.GetInstance().FreeConnection(conn);

            SelectCharacter(c, characterId);
        }

        [RemoteEvent("login.character.creator")]
        public static void CharacterCreator(Client c)
        {
            c.Position = new Vector3(402.8664, -996.4108, -99.00027);

            c.TriggerEvent("toggleCreator", true);
        }

        [RemoteEvent("creator_GenderChange")]
        public static void CharacterCreatorGenderChange(Client c, Boolean male)
        {
            if (male)
            {
                c.SetSkin(PedHash.FreemodeMale01);
            } else
            {
                c.SetSkin(PedHash.FreemodeFemale01);
            }
            c.TriggerEvent("creator_GenderChanged");
            c.Position = new Vector3(402.8664, -996.4108, -99.00027);
        }

        [RemoteEvent("creator_leave")]
        public static void CharacterCreatorLeave(Client c)
        {
            c.Position = new Vector3(-1167.994, -700.4285, 21.89281);
            c.TriggerEvent("toggleCreator", false);
        }

        
        public static void ApplyPlayerClothes(Client c)
        {
            if (!c.HasData("character_id"))
            {
                c.Kick();
                return;
            }

            //Klamotten
            c.SetClothes(6, c.GetData("shoes"), 0);
            c.SetClothes(4, c.GetData("legs"), 0);
            c.SetClothes(11, c.GetData("tops"), 0);
            c.SetClothes(3, c.GetData("torsos"), 0);
            c.SetClothes(8, c.GetData("undershirts"), 0);

            //Haare
            c.SetClothes(2, c.GetData("hair"), 0);
        }

        /*
        public static void ApplyPlayerTattoos(Client player)
        {
            // Get the tattoos from the player
            int playerId = player.GetData(EntityData.PLAYER_SQL_ID);
            List<TattooModel> playerTattoos = Globals.tattooList.Where(t => t.player == playerId).ToList();

            foreach (TattooModel tattoo in playerTattoos)
            {
                // Add each tattoo to the player
                Decoration decoration = new Decoration();
                decoration.Collection = NAPI.Util.GetHashKey(tattoo.library);
                decoration.Overlay = NAPI.Util.GetHashKey(tattoo.hash);
                player.SetDecoration(decoration);
            }
        }

        private static int[] GetOverlayData(SkinModel skinModel, int index)
        {
            int[] overlayData = new int[2];

            switch (index)
            {
                case 0:
                    overlayData[0] = skinModel.blemishesModel;
                    overlayData[1] = 0;
                    break;
                case 1:
                    overlayData[0] = skinModel.beardModel;
                    overlayData[1] = skinModel.beardColor;
                    break;
                case 2:
                    overlayData[0] = skinModel.eyebrowsModel;
                    overlayData[1] = skinModel.eyebrowsColor;
                    break;
                case 3:
                    overlayData[0] = skinModel.ageingModel;
                    overlayData[1] = 0;
                    break;
                case 4:
                    overlayData[0] = skinModel.makeupModel;
                    overlayData[1] = 0;
                    break;
                case 5:
                    overlayData[0] = skinModel.blushModel;
                    overlayData[1] = skinModel.blushColor;
                    break;
                case 6:
                    overlayData[0] = skinModel.complexionModel;
                    overlayData[1] = 0;
                    break;
                case 7:
                    overlayData[0] = skinModel.sundamageModel;
                    overlayData[1] = 0;
                    break;
                case 8:
                    overlayData[0] = skinModel.lipstickModel;
                    overlayData[1] = skinModel.lipstickColor;
                    break;
                case 9:
                    overlayData[0] = skinModel.frecklesModel;
                    overlayData[1] = 0;
                    break;
                case 10:
                    overlayData[0] = skinModel.chestModel;
                    overlayData[1] = skinModel.chestColor;
                    break;
            }

            return overlayData;
        }

        */

    }
}
