using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace GY.AutoVK.Utils
{
    public static class GeneratorUtil
    {
        private static readonly char[] Codes = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            
        public static string GenerateCode()
        {
            var builder = new StringBuilder();
            
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    builder.Append(Codes.ElementAt(Random.Range(1, Codes.Length)));
                }
                builder.Append("-");
            }

            return builder.ToString().TrimEnd('-');
        }
    }
}