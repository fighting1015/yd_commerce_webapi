using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using Vapps.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vapps.Configuration;
using Abp.Runtime.Security;
using Vapps.Advert.AdvertAccounts;

namespace Vapps.Migrations.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly VappsDbContext _context;

        public DefaultSettingsCreator(VappsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            AddSettingIfNotExists(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault, "true");

            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "dev@vapps.hk");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "devvapps");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "smtp.ym.163.com");
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "dev@vapps.hk");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, SimpleStringCipher.Instance.Encrypt("vapps510000") );

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-CN");

            //Ship
            AddSettingIfNotExists(AppSettings.Shipping.ApiId, "1337086");
            AddSettingIfNotExists(AppSettings.Shipping.ApiSecret, "73f47271-8ba4-4625-9089-34d97b7cb693");
            AddSettingIfNotExists(AppSettings.Shipping.ApiUrl, "http://api.kdniao.com/Ebusiness/EbusinessOrderHandle.aspx");
            
            //Advert
            AddSettingIfNotExists(AdvertSettings.ToutiaoAdsAppId, "1620174454145053");
            AddSettingIfNotExists(AdvertSettings.ToutiaoAdsAppSecret, "41091c14794c7d24cd248a987f452fad1d8b4e15");
            AddSettingIfNotExists(AdvertSettings.TenantAdsAppId, "1107493946");
            AddSettingIfNotExists(AdvertSettings.TenantAdsAppSecret, "u1UbBRGXOTbEzdzv");
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}