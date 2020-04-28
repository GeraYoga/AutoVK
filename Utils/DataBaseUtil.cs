using System;
using System.Runtime.InteropServices;
using LiteDB;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Steamworks;
using UnityEngine;
using static GY.AutoVK.AutoVk;

namespace GY.AutoVK.Utils
{
    public static class DataBaseUtil
    {
        public static VkModel GetPlayer(ulong steam)
        {
            using (var db = new LiteDatabase(@"GY.DataBase/AutoVk.db"))
            {
                var collection = db.GetCollection<VkModel>("vk_info");
                var member = collection.FindOne(m => m.Steam == steam);
                
                if (member != null)
                {
                    return member;
                }
            }

            return null;
        }
        
        public static string GetOrAddCode(ulong steam, string vk)
        {
            using (var db = new LiteDatabase(@"GY.DataBase/AutoVk.db"))
            {
                var collection = db.GetCollection<VkModel>("vk_info");
                var member = collection.FindOne(m => m.Steam == steam);
                var code = GeneratorUtil.GenerateCode();
                if (member == null)
                {
                    collection.Insert(new VkModel
                    {
                        Code = code, Steam = steam, Vkontakte = vk, Activated = false
                    });
                }
                else
                {
                    member.Code = code;
                    member.Vkontakte = vk;
                    member.Activated = false;
                    collection.Update(member);
                }
                return code;
            }
        }

        public static void UpdateState(ulong steam, bool isActivated)
        {
            using (var db = new LiteDatabase(@"GY.DataBase/AutoVk.db"))
            {
                var collection = db.GetCollection<VkModel>("vk_info");
                var member = collection.FindOne(m => m.Steam == steam);
                member.Activated = isActivated;
                collection.Update(member);
            }
        }
        
        
        public static async void CheckPlayer(ulong steam)
        {
            using (var db = new LiteDatabase(@"GY.DataBase/AutoVk.db"))
            {
                var player = UnturnedPlayer.FromCSteamID((CSteamID) steam);
                var collection = db.GetCollection<VkModel>("vk_info");
                var member = collection.FindOne(m => m.Steam == steam);

                if (member == null)
                {
                    UnturnedChat.Say(player, Instance.Translate("group_invite", player.CharacterName), Color.cyan);
                    return;
                }
                
                var flag = await RequestUtil.IsMemberOfGroup(member.Vkontakte);

                if (flag) return;
                R.Permissions.RemovePlayerFromGroup(Instance.Configuration.Instance.RewardGroup, new RocketPlayer(steam.ToString()));
                UpdateState(steam, false);
                UnturnedChat.Say(player, Instance.Translate("group_leave"), Color.red);
            }
        }
    }

    public class VkModel
    {
        public ulong Steam { get; set; }
        public string Vkontakte { get; set; }
        public string Code { get; set; }
        public bool Activated { get; set; }
        public Guid Id { get; set; }
    }
}