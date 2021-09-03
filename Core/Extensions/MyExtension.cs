using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Extensions
{
    public static class MyExtension
    {       
        public static string getUniqueCode()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            string uniquecode = BitConverter.ToUInt32(buffer, 12).ToString();
            return uniquecode;
        }

        public static DateTime getLocalTime(this DateTime utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time"));
        }
        public static string getCleanedNumber(this string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }

        public static string generatePromoCode(string phone)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var alpha = new string(Enumerable.Repeat(alphabet, 5).Select(s => s[random.Next(s.Length)]).ToArray());
            var numer = new string(Enumerable.Repeat(phone, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            var result = alpha + numer;
            string promocodes = new string(result.ToCharArray().OrderBy(s => (random.Next(2) % 2) == 0).ToArray());
            return promocodes;
        }
    }

}
