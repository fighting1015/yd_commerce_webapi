using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vapps.Helpers
{
    public static class CommonHelper
    {
        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            stringsToValidate.Each(str =>
            {
                if (string.IsNullOrEmpty(str)) result = true;
            });

            return result;
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }

        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new Exception("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// 验证有效的电子邮件格式的字符串
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// 检查是否有效的手机号码(大陆)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidPhoneNumber(string phone)
        {
            if (String.IsNullOrEmpty(phone))
                return false;

            phone = phone.Trim();
            var result = Regex.IsMatch(phone, @"(13\d|14[57]|15[^4,\D]|17[678]|18\d)\d{8}|170[059]\d{7}", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }


        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetVappsCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetVappsCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.GetTypeInfo().IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
            }
            return value;
        }

        public static TypeConverter GetVappsCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();

            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        /// <summary>
        /// Generate random digit code
        /// 根据长度生成随机数字代码
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// 根据范围生成随机数
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var randomNumberBuffer = new byte[10];
                random.GetBytes(randomNumberBuffer);

                return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
            }
        }

        /// <summary>
        /// Compare two arrasy
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Get difference in years
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            //this assumes you are looking for the western idea of age and not using East Asian reckoning.
            int age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
                age--;
            return age;
        }

        /// <summary>
        /// 从图片地址下载图片并转为字节
        /// </summary>
        /// <param name="url">图片网址</param>
        /// <returns></returns>
        public static async Task<byte[]> SavePictureFromUrlAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Byte[] buffer = new Byte[1024];
                    int current = 0;
                    while ((current = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        ms.Write(buffer, 0, current);
                    }
                    return ms.ToArray();
                }
            }
        }

        public static string UrlEncodeBase64(string base64Data)
        {
            return new string(UrlEncodeBase64(base64Data.ToCharArray()));
        }

        public static char[] UrlEncodeBase64(char[] base64Data)
        {
            for (int i = 0; i < base64Data.Length; i++)
            {
                switch (base64Data[i])
                {
                    case '+':
                        base64Data[i] = '@';
                        break;

                    case '/':
                        base64Data[i] = '#';
                        break;
                }
            }
            return base64Data;
        }

        public static string UrlDecodeBase64(string base64Data)
        {
            return new string(UrlDecodeBase64(base64Data.ToCharArray()));
        }

        public static char[] UrlDecodeBase64(char[] base64Data)
        {
            for (int i = 0; i < base64Data.Length; i++)
            {
                switch (base64Data[i])
                {
                    case '@':
                        base64Data[i] = '+';
                        break;

                    case '$':
                        base64Data[i] = '/';
                        break;
                }
            }
            return base64Data;
        }

        /// <summary>
        /// 根据GUID获取16位的唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列
        /// </summary>
        /// <returns></returns>
        public static long GuidToLongId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 生成22位唯一的数字 并发可用
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueId()
        {
            Random d = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            string strUnique = DateTime.Now.ToString("yyyyMMddHHmmssffff") + d.Next(1000, 9999);
            return strUnique;
        }

        public static object Parse(Type dataType, string ValueToConvert)
        {
            TypeConverter obj = TypeDescriptor.GetConverter(dataType);
            object value = obj.ConvertFromString(null, CultureInfo.InvariantCulture, ValueToConvert);
            return value;
        }

        /// <summary>
        /// 获取总秒数
        /// </summary>
        /// <param name="durationString"></param>
        /// <returns></returns>
        private static int GetTotalScond(string durationString)
        {
            var duration = 0;
            var items = durationString.Split(':');

            var hours = Convert.ToInt32(items[0]);
            var minute = Convert.ToInt32(items[1]);
            var second = Convert.ToInt32(items[2]);

            if (hours > 0)
                duration += hours * 3600;

            if (minute > 0)
                duration += minute * 60;

            if (second > 0)
                duration += second;

            return duration;
        }

        #region Url


        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(string pageURL)
        {
            Uri uri = new Uri(pageURL);
            string queryString = uri.Query;

            return GetQueryString(queryString, null, true);
        }

        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(string queryString, Encoding encoding, bool isEncoded)
        {
            queryString = queryString.Replace("?", "");
            NameValueCollection result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    if (isEncoded)
                    {
                        result[MyUrlDeCode(key, encoding)] = MyUrlDeCode(value, encoding);
                    }
                    else
                    {
                        result[key] = value;
                    }
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 解码URL.
        /// </summary>
        /// <param name="encoding">null为自动选择编码</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MyUrlDeCode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码                     
                string code = WebUtility.UrlDecode(str.ToUpper());
                //将已经解码的字符再次进行编码.
                string encode = WebUtility.UrlEncode(code).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return WebUtility.UrlDecode(str);
        }

        #endregion

        /// <summary>
        /// 根据Ip解析地址
        /// </summary>
        /// <returns></returns>
        public async static Task<Ip2AddressEntity> AnalysisIp2AddressAsync(string ip)
        {
            Ip2AddressEntity ip2AddressEntity = new Ip2AddressEntity();
            var getIpInfoUrl = QueryHelpers.AddQueryString("http://ip.taobao.com/service/getIpInfo.php", "ip", ip);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                    client.Timeout = TimeSpan.FromSeconds(120);
                    client.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

                    var response = await client.GetStringAsync(getIpInfoUrl);
                    //response.EnsureSuccessStatusCode();

                    //var returnResultString = await response.Content.ReadAsStringAsync();
                    var deserializeResult = JsonConvert.DeserializeObject<RequestAnalysisDataResult>(response);

                    if (deserializeResult.Code == 0)
                        ip2AddressEntity = JsonConvert.DeserializeObject<Ip2AddressEntity>(response);
                    else
                        ip2AddressEntity.Code = deserializeResult.Code;
                }
            }
            catch (WebException webEx)
            {
                //log it
                string logDebug = $"获取客户端IP地址出错. {ip},{webEx.Status},{webEx.Message},{webEx.StackTrace},{webEx.Source}";
                return null;
            }

            ReplaceSuffix(ip2AddressEntity);

            return ip2AddressEntity;
        }

        public static void ReplaceSuffix(Ip2AddressEntity ip2AddressEntity)
        {
            ip2AddressEntity.AddressData.Region = ip2AddressEntity.AddressData.Region.Replace("省", "");
            ip2AddressEntity.AddressData.Region = ip2AddressEntity.AddressData.Region.Replace("市", "");
        }

        /// <summary>
        /// 根据Ip解析地址
        /// </summary>
        /// <returns></returns>
        public async static Task<Longitude2Address> AnalysisLongitude2AddressAsync(string longitude)
        {
            if (longitude.IsNullOrEmpty())
                return null;

            Longitude2Address ip2AddressEntity = new Longitude2Address();
            var getIpInfoUrl = QueryHelpers.AddQueryString("https://apis.map.qq.com/ws/geocoder/v1/", "location", longitude);

            getIpInfoUrl = QueryHelpers.AddQueryString(getIpInfoUrl, "key", "3ZBBZ-S4OWQ-N4U5Y-GYQRZ-FKASV-TOFLQ");
            getIpInfoUrl = QueryHelpers.AddQueryString(getIpInfoUrl, "coord_type", "1");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                    client.Timeout = TimeSpan.FromSeconds(120);
                    client.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

                    var response = await client.GetStringAsync(getIpInfoUrl);

                    ip2AddressEntity = JsonConvert.DeserializeObject<Longitude2Address>(response);

                    return ip2AddressEntity;
                }
            }
            catch (WebException webEx)
            {
                //log it
                string logDebug = $"获取客户端IP地址出错. {longitude},{webEx.Status},{webEx.Message},{webEx.StackTrace},{webEx.Source}";
                return ip2AddressEntity;
            }
        }
    }
}
