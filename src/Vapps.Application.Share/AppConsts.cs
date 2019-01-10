namespace Vapps
{
    /// <summary>
    /// Some consts used in the application.
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// 默认分页大小
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// 允许的最大分页大小
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";

        /// <summary>
        /// Day count for notifying user before expiration of subscription
        /// </summary>
        public const int SubscriptionExpireNootifyDayCount = 7;

        public const int ResizedMaxProfilPictureBytesUserFriendlyValue = 1024;

        public const int MaxProfilPictureBytesUserFriendlyValue = 5;

        public const string TokenValidityKey = "token_validity_key";

        public static string UserIdentifier = "user_identifier";
    }
}
