using Abp.Dependency;
using Abp.Localization;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vapps.Helpers;

namespace Vapps
{
    public static class EnumExtensions
    {
        public static List<SelectListItem> EnumToSelectListItem<TEnum>(this TEnum enumObj, string sourceName,
           int[] valuesToExclude = null, bool includeDefault = false) where TEnum : struct
        {
            return EnumToSelectListItem(typeof(TEnum), sourceName, valuesToExclude, includeDefault);
        }

        /// <summary>
        /// 枚举类型转换Json数据
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="valuesToExclude"></param>
        /// <param name="includeDefault"></param>
        /// <returns></returns>
        public static List<SelectListItem> EnumToSelectListItem(Type moduleType, string enumName,
            string sourceName, int[] valuesToExclude = null, bool includeDefault = false)
        {
            var enumType = GetEnumTypeInfo(moduleType, enumName);

            return EnumToSelectListItem(enumType, sourceName, valuesToExclude, includeDefault);
        }

        /// <summary>
        /// 枚举类型转换Json数据
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="valuesToExclude"></param>
        /// <param name="includeDefault"></param>
        /// <returns></returns>
        public static List<SelectListItem> EnumToSelectListItem(Type enumType, string sourceName,
            int[] valuesToExclude = null, bool includeDefault = false)
        {
            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");
            var localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();

            var allEnumValues = Enum.GetValues(enumType);
            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (var enumValue in allEnumValues)
            {
                if (valuesToExclude != null && valuesToExclude.Contains(Convert.ToInt32(enumValue)))
                    continue;

                listItems.Add(new SelectListItem()
                {
                    Value = Convert.ToInt32(enumValue).ToString(),
                    Text = enumValue.GetLocalizedEnum(enumType, localizationManager, sourceName)
                });
            }

            // 添加默认值
            if (includeDefault)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = 0.ToString(),
                    Text = localizationManager.GetString(sourceName, "Please_Choose")
                });
            }

            var result = listItems.OrderBy(t => Convert.ToInt32(t.Value)).ToList();

            return result;
        }

        /// <summary>
        /// 获取枚举的本地化值
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedEnum(this object enumValue, Type enumType, ILocalizationManager localizationManager, string sourceName)
        {
            if (localizationManager == null)
                throw new ArgumentNullException("localizationManager");

            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("T must be an enumerated type");

            //localized value
            string resourceName = string.Format("Enums.{0}.{1}", enumType.ToString(),
                //Convert.ToInt32(enumValue)
                enumValue.ToString());

            string result = localizationManager.GetString(sourceName, resourceName);

            //如果没有翻译,则获取默认值
            if (String.IsNullOrEmpty(result))
                result = CommonHelper.ConvertEnum(enumValue.ToString());

            return result;
        }

        /// <summary>
        /// 获取枚举的本地化值
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationManager localizationManager, string sourceName = "ServerSide")
        {
            if (localizationManager == null)
                throw new ArgumentNullException("localizationManager");

            if (!typeof(T).GetTypeInfo().IsEnum) throw new ArgumentException("T must be an enumerated type");

            //localized value
            string resourceName = string.Format("Enums.{0}.{1}",
                typeof(T).ToString(),
                //Convert.ToInt32(enumValue)
                enumValue.ToString());

            string result = localizationManager.GetString(sourceName, resourceName);

            //set default value if required
            if (String.IsNullOrEmpty(result))
                result = CommonHelper.ConvertEnum(enumValue.ToString());

            return result;
        }

        public static Type GetEnumTypeInfo(Type moduleType, string enumName)
        {
            var subTypeQuery = from t in moduleType.GetAssembly().GetTypes()
                               where t.Name == enumName
                               select t;

            return subTypeQuery.FirstOrDefault();
        }


        public static bool IsSubClassOf(Type type, Type baseType)
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
    }
}
