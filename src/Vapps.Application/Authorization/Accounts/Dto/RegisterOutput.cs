namespace Vapps.Authorization.Accounts.Dto
{
    public class RegisterOutput
    {
        /// <summary>
        /// ÄÜ·ñµÇÂ½
        /// </summary>
        public bool CanLogin { get; set; }

        /// <summary>
        /// Ëæ»úÃÜÂë
        /// </summary>
        public string RandomPassword { get; set; }
    }
}