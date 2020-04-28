using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using GY.AutoVK.Utils;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;

namespace GY.AutoVK
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class AutoVk : RocketPlugin<Config>
    {
        public static AutoVk Instance;
        protected override void Load()
        {
            Instance = this;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            U.Events.OnPlayerConnected += EventsOnOnPlayerConnected;

            if (!System.IO.Directory.Exists("GY.DataBase"))
            {
                System.IO.Directory.CreateDirectory("GY.DataBase");
            }
        }

        protected override void Unload()
        {
            Instance = null;
            U.Events.OnPlayerConnected -= EventsOnOnPlayerConnected;
        }

        private static void EventsOnOnPlayerConnected(UnturnedPlayer player)
        {
            DataBaseUtil.CheckPlayer(player.CSteamID.m_SteamID);
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"group_not_found", "Группа для выдачи бонуса не найдена, обратитесь к администратору!"},
            {"invalid_id", "Вы ввели неверный ID своей сраницы!"},
            {"player_added", "Вы получли бонус за вступление в группу ВК!"},
            {"not_subscribed", "Вы не найдены в списке участников группы!"},
            {"has_permission", "Вы уже состоите в бонусной группе!"},
            {"vk_confirm", "Код активации бонуса: {0}. Используйте команду /Activate [Код] на сервере для получения бонуса!"},
            {"group_leave", "Вы вышли из группы сервера, бонус за вступление был аннулирован!"},
            {"group_invite", "{0}, вступи в группу сервера и получи уникальный бонус, пиши /VK!"},
            {"cant_send_msg", "Мы не можем отправить код активации вам в лс, разрешите группе сервера присылать вам сообщения!"},
            {"code_invalid", "Введенный вами код неверный или неактивный!"},
            {"activation_send", "Проверьте личные сообщения ВК, группа сервера отправила вам код подтверждения!"},
            {"code_not_found", "Используйте команду /Vk [Ваш ID VK] для получения кода активации!"},
            {"command_invalid", "Команда введена неверно, используйте: {0}!"},
            {"message_thx", "Спасибо за активацию, обращаем ваше внимание, что при выходе из группы бонус будет аннулирован!"}
        };
    }
}