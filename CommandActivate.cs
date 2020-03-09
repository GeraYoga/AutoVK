using System.Collections.Generic;
using GY.AutoVK.Utils;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using static GY.AutoVK.AutoVk;

namespace GY.AutoVK
{
    public class CommandActivate : IRocketCommand
    {
        public async void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer) caller;
            
            if (R.Permissions.GetGroup(Instance.Configuration.Instance.RewardGroup).Members.Contains(player.CSteamID.ToString()))
            {
                UnturnedChat.Say(caller, Instance.Translate("has_permission"), Color.yellow);
                return;
            }
            
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, Instance.Translate("command_invalid", "/Activate [Код]"), Color.red);
                return;
            }
            
            var getInfo = DataBaseUtil.GetPlayer(player.CSteamID.m_SteamID);

            if (getInfo == null)
            {
                UnturnedChat.Say(caller, Instance.Translate("code_not_found"), Color.red);
                return;
            }

            var isMember = await RequestUtil.IsMemberOfGroup(getInfo.Vkontakte);

            if (!isMember)
            {
                UnturnedChat.Say(caller, Instance.Translate("not_subscribed"), Color.red);
                return;
            }
            
            if (getInfo.Code != command[0])
            {
                UnturnedChat.Say(caller, Instance.Translate("code_invalid"), Color.red);
                return;
            }
            
            R.Permissions.AddPlayerToGroup(Instance.Configuration.Instance.RewardGroup, caller);
            DataBaseUtil.UpdateState(player.CSteamID.m_SteamID, true);
            await RequestUtil.SendMessage(getInfo.Vkontakte, "", "message_thx");
            UnturnedChat.Say(caller, Instance.Translate("player_added"), Color.magenta);
            
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "Activate";
        public string Help => "";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>{"gy.vk.activate"};
    }
}