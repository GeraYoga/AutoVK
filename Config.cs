using Rocket.API;

namespace GY.AutoVK
{
    public class Config : IRocketPluginConfiguration
    {
        public string VkGroupId;
        public string BrowserRequestDesc;
        public string RewardGroup;
        public string AccessToken;
        public void LoadDefaults()
        {
            BrowserRequestDesc = "Вступи в группу ВК и получи бонус!";
            VkGroupId = "plugins_gy";
            RewardGroup = "BonusGroup";
            AccessToken = "https://vk.com/dev/access_token - инструкция";

        }
    }
}