
using System.Collections.Generic;
using System.Net;
using GY.AutoVK.Utils;
using JetBrains.Annotations;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using static GY.AutoVK.AutoVk;

namespace GY.AutoVK
{
    public class CommandVk : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "VK";
        public string Help => "Вступите в группу ВК и получите бонус!";
        public string Syntax => "/VK - открыть браузер | /VK [id вашей страницы]";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> {"gy.vk"};

        public async void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer) caller;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            
            if (R.Permissions.GetGroup(Instance.Configuration.Instance.RewardGroup) == null)
            {
                UnturnedChat.Say(caller, Instance.Translate("group_not_found"), Color.red);
                return;
            }

            switch (command.Length)
            {
                case 0:
                    player.Player.sendBrowserRequest(Instance.Configuration.Instance.BrowserRequestDesc, $"https://vk.com/{Instance.Configuration.Instance.VkGroupId}");
                    break;
                case 1:

                    if (R.Permissions.GetGroup(Instance.Configuration.Instance.RewardGroup).Members.Contains(player.CSteamID.ToString()))
                    {
                        UnturnedChat.Say(caller, Instance.Translate("has_permission"), Color.yellow);
                        return;
                    }

                    var vk = await RequestUtil.GetPlayerVk(command[0]);
                    
                    
                    if (vk == null)
                    {
                        UnturnedChat.Say(caller, Instance.Translate("invalid_id"), Color.red);
                        return;
                    }

                    var newReg = DataBaseUtil.GetOrAddCode(player.CSteamID.m_SteamID, vk);
                    
                    Reg(caller, vk, newReg);

                    break;
                default:
                    UnturnedChat.Say(caller, Instance.Translate("command_invalid", "/Vk [Ваш ID Vk]"), Color.red);
                    break;
            }
        }

        private static async void Reg(IRocketPlayer caller, string vk, string code)
        {
            var isSuccessful = await RequestUtil.SendConfirmMsg(vk, code);

            if (!isSuccessful)
            {
                UnturnedChat.Say(caller, Instance.Translate("cant_send_msg"), Color.red);
                return;
            }

            UnturnedChat.Say(caller, Instance.Translate("activation_send"), Color.magenta);
        }
    }
}