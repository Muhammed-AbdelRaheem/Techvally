using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Domain.Models
{
    public static class Extantion
    {
        //public static string TaskToken = "5fb76f6a-546b-47dc-adbc-";
        //public static string TaskName = "Update_Requests";

        public static Func<List<int>, bool> MatchRate(int? rate = 0)
        {
            return e => (Math.Round(CalucRating(e.Sum(), e.Count)) == rate);
        }


        public static decimal CalculatePriceWithTax(this decimal price, int tax)
        {
            double taxRate = (double)tax / 100;
            var res = decimal.Round(price + (price * (decimal)taxRate), 2, MidpointRounding.AwayFromZero);
            return res;
        }
        public static decimal CalculateTaxFromPriceWithTax(this decimal priceWithTax, int tax)
        {
            decimal taxRate = (decimal)tax / 100;
            var res = decimal.Round(priceWithTax - (priceWithTax / (1 + taxRate)), 2, MidpointRounding.AwayFromZero);
            return res;
        }
        public static decimal CalculatePriceWithDiscountPercent(this decimal price, decimal discount)
        {
            var res = discount / 100;
            return res <= 0 ? 0 : decimal.Round(price - (price * res), 2, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculatePriceWithDiscount(this decimal price, decimal discount)
        {
            var res = price - discount;
            return res <= 0 ? 0 : decimal.Round(price - discount, 2, MidpointRounding.AwayFromZero);
        }

        public static CultureInfo GetCultureInfo(string code)
        {
            return new CultureInfo(code);
        }
        public static DateTime ToSaudiDate(this DateTime dateTime)
        {
            return dateTime.AddHours(3);
        }

        public static string ToUnixTimeMilliseconds(this DateTime dateTime)
        {

            DateTimeOffset dto = new DateTimeOffset(dateTime.ToUniversalTime());
            var date = dto.ToUnixTimeMilliseconds().ToString();
            return date;
        }

        public static DateTime AddUtcTime(int time)
        {
            return DateTime.UtcNow.AddHours(time);
   
        }


        public static DateTime AddUtcTime()
        {
            return DateTime.UtcNow.AddHours(6);

        }
        public static string GetWebsiteYear(int startYear)
        {
            var currentYear = AddUtcTime().Year;
            if (startYear == currentYear)
            {
                return $"{currentYear}";

            }
            return $"{startYear} - {currentYear}";

        }
        public static string GetClientToken()
        {
            return Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
        }
        public static string GetClientId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
      
        public static string GenerateToken()
        {
            int maxSize = 32;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return Convert.ToBase64String(data);
        }
        public static string RNGCharacterMask()
        {
            int maxSize = 15;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }

        public static List<DateTime> GetWeekdayInRange(DateTime today, DateTime from, DateTime to,
            DayOfWeek day, int durationBeforeOrder, DateTime timeFrom, bool allDay)
        {
            const int daysInWeek = 7;
            var result = new List<DateTime>();
            TimeSpan time = timeFrom.TimeOfDay;
            DateTime dateTodayWithTime = today.Date.Add(time);
            var daysToAdd = ((int)day - (int)from.DayOfWeek + daysInWeek) % daysInWeek;

            do
            {
                from = from.AddDays(daysToAdd);
                if (from.Date <= to.Date)
                {
                    if (from.Date == today.Date && (allDay || today <= dateTodayWithTime.AddHours(-durationBeforeOrder)))
                    {

                        result.Add(today);
                    }
                    else if (from.Date != today.Date && from.Date >= today.Date && (allDay || today <= from.Date.Add(time).AddHours(-durationBeforeOrder)))
                    {
                        result.Add(from);
                    }

                    daysToAdd = daysInWeek;
                }

            } while (from.Date < to.Date);
            return result;
        }

       
        public class SerializeURLClientObject
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        public static string SelectByLanguage(string language, string en, string ar, string fr)
        {

            if (language == "ar")
            {
                return ar;
            }
            else if (language == "en")
            {
                return en;
            }
            else if (language == "fr")
            {
                return fr;
            }
            else
            {
                return en;
            }
        }
   

        public static string ConvertStringToEnum<T>(string status)
        {
            Enum name = (Enum)Enum.Parse(typeof(T), status);
            return name.DisplayName();
        }
        public static string ConvertHtmlToPlainTxt(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", "");
        }
        public static double? GetDiscount(this double? price, double? discount)
        {
            if (discount != null && discount > 0)
            {
                double? discounted_price = (price - (price * discount / 100));
                return discounted_price;

            }
            return price;
        }
        public static decimal VatPrice(decimal price, int vat)
        {
            return (price / 100) * vat;
        }
        public static double CalculateIncloudVat(double price, double vat)
        {
            double CR = (vat + 100) / 100;
            return price * CR;
        }
        public static decimal VatCost(decimal incVAT, int vat)
        {
            decimal cPrice = (decimal)(vat + 100) / 100;
            decimal tCost = incVAT * cPrice;
            return tCost - incVAT;
        }

        public static decimal GetPrice(decimal vat, decimal priceIncludeVat)
        {
            return priceIncludeVat - vat;
        }

        public static double? GetAmount(this double? price, double? discount)
        {
            if (discount != null && discount > 0)
            {
                double? discounted_price = (price * discount / 100);
                return discounted_price;
            }
            return price;
        }
       


        public static string DisplayName(this Enum enuemValue)
        {
            return enuemValue.GetType()
                .GetMember(enuemValue.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }
        public static string GetDisplayName2(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attributes = field.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

            return attributes == null ? value.ToString() :
                (attributes.Length > 0 ? attributes[0].GetName() : value.ToString());
        }
     


        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
        public static double CalucRating(int rateSum, int rateCount)
        {
            if (rateSum > 0 && rateCount > 0)
            {
                var ratingPercent = ((rateSum * 100) / rateCount) / 5;
                return (((double)ratingPercent / 100) * 5);
            }
            return 0;

        }


        public static string GenerateSignature(string passPhrase, Dictionary<string, string> data)
        {
            data.Remove("signature");
            var dataString = data.OrderBy(x => x.Key)
                .Select(x => $"{x.Key}={x.Value}")
                .Aggregate((x, y) => $"{x}{y}");

            var dataBytes = Encoding.UTF8.GetBytes($"{passPhrase}{dataString}{passPhrase}");
            var signature = SHA256.Create().ComputeHash(dataBytes);
            return BitConverter.ToString(signature).Replace("-", string.Empty).ToLower();
        }



        public static bool ValidateSignature(string signature, string passPhrase, Dictionary<string, string> data)
        {
            var generatedSignature = GenerateSignature(passPhrase, data);
            return signature == generatedSignature;
        }


       
        public static String GetHash(String text, String key)
        {
            
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
 
    }

}
