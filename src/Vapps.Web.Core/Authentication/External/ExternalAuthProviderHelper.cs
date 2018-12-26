using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vapps.Web.Authentication.External.Wechat;

namespace Vapps.Web.Authentication.External
{
    public static class ExternalAuthProviderHelper
    {
        /// <summary>
        /// 使用反射获取所有继承了ExternalAuthProviderApiBase的子类
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllExternalAuthProviderTypeInfo()
        {
            var subTypeQuery = from t in typeof(VappsWebCoreModule).GetAssembly().GetTypes()
                               where IsSubClassOf(t, typeof(ExternalAuthProviderApiBase))
                               select t;

            return subTypeQuery;
        }

        private static bool IsSubClassOf(Type type, Type baseType)
        {
            var b = type.GetTypeInfo().BaseType;
            while (b != null)
            {
                if (b.Equals(baseType))
                {
                    return true;
                }
                b = b.GetTypeInfo().BaseType;
            }
            return false;
        }

        public static string GetUniversalProvider(this string normalProviderKey)
        {
            string universalProviderKey;
            switch (normalProviderKey)
            {
                case WeChatMPAuthProviderApi.Name:
                    universalProviderKey = WeChatAuthProviderApi.Name;
                    break;
                default:
                    universalProviderKey = normalProviderKey;
                    break;
            }

            return universalProviderKey;
        }
    }
}
