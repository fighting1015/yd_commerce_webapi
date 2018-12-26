using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Enums;

namespace Vapps.Web.Authentication.External.QQ
{
    public static class QQHelper
    {
        /// <summary>
        /// Gets the QQ user ID.
        /// </summary>
        public static string GetOpenId(JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("openid");
        }

        /// <summary>
        /// Gets the user's min age.
        /// </summary>
        public static GenderType GetGender(JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var gender = user.Value<string>("gender");

            if (gender == "男")
            {
                return GenderType.M;
            }
            else if (gender == "女")
            {
                return GenderType.F;
            }
            else
            {
                return GenderType.Unknown;
            }
        }

        /// <summary>
        /// Gets the user's max age.
        /// </summary>
        public static string GetProfilePicture(JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("figureurl_qq_2");
        }

        /// <summary>
        /// Gets the user's birthday.
        /// </summary>
        public static string GetNickname(JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return user.Value<string>("nickname");
        }

        /// <summary>
        /// Gets the QQ email.
        /// </summary>
        public static string GetProvince(JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("province");
        }

        /// <summary>
        /// Gets the user's first name.
        /// </summary>
        public static string GetCity(JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("city");
        }
    }
}
