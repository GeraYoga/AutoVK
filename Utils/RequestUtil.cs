using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static GY.AutoVK.AutoVk;

namespace GY.AutoVK.Utils
{
    public static class RequestUtil
    {
        public static async Task<string> GetPlayerVk(string input)
        {
            var token = Instance.Configuration.Instance.AccessToken;
            const string url = "https://api.vk.com/method/users.get";
            var wc = new WebClient();
            wc.QueryString.Add("user_ids", $"{input}");
            wc.QueryString.Add("version", "5.92");
            wc.QueryString.Add("v", "5.92");
            wc.QueryString.Add("access_token", $"{token}");
            var response = await wc.UploadValuesTaskAsync(url, wc.QueryString);
            var responseString = Encoding.UTF8.GetString(response);
            if (!responseString.StartsWith("{\"response\":[{\"id\":")) {return null;}
            var playerid = responseString.Split(',').First().Split(':').Last();
            return playerid;
        }
        
        public static async Task<bool> SendMessage(string vk, string code, string key)
        {
            var token = Instance.Configuration.Instance.AccessToken;
            const string url = "https://api.vk.com/method/messages.send";
            var wc = new WebClient();
            wc.QueryString.Add("user_id", $"{vk}");
            wc.QueryString.Add("message", Instance.Translate(key, code));
            wc.QueryString.Add("access_token", $"{token}");
            wc.QueryString.Add("version", "5.92");
            wc.QueryString.Add("v", "5.92");
            wc.QueryString.Add("random_id", $"{DateTime.Now.Millisecond}");
            var response = await wc.UploadValuesTaskAsync(url, wc.QueryString);
            var responseString = Encoding.UTF8.GetString(response);
            return !responseString.StartsWith("{\"error\"");
        }

        public static async Task<bool> IsMemberOfGroup(string vk)
        {
            var token = Instance.Configuration.Instance.AccessToken;
            const string url = "https://api.vk.com/method/groups.isMember";
            var wc = new WebClient();
            wc.QueryString.Add("user_id", $"{vk}");
            wc.QueryString.Add("group_id", $"{Instance.Configuration.Instance.VkGroupId}");
            wc.QueryString.Add("extended", "1");
            wc.QueryString.Add("access_token", $"{token}");
            wc.QueryString.Add("version", "5.92");
            wc.QueryString.Add("v", "5.92");
            wc.QueryString.Add("random_id", $"{DateTime.Now.Millisecond}");
            var response = await wc.UploadValuesTaskAsync(url, wc.QueryString);
            var responseString = Encoding.UTF8.GetString(response);
            return responseString.Split(',').First().Split(':').Last() == "1";
        }
    }
}