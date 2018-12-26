using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Reflection.Extensions;
using System;
using System.Linq;
using System.Reflection;
using Vapps.Security;

namespace Vapps.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AdminPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 管理权限
            var admin = context.GetPermissionOrNull(AdminPermissions.Self);
            if (admin == null)
            {
                admin = context.CreatePermission(AdminPermissions.Self, L(AdminPermissions.Self));
                var configType = typeof(AdminPermissions).GetTypeInfo();
                var baseAttributeInfo = GetPermissionAttributeInfo(configType, AdminPermissions.Self);
                GeneratePermission(configType, admin, baseAttributeInfo, VappsConsts.ServerSideLocalizationSourceName);
            }

            // 机构中心权限
            var businessPermission = context.GetPermissionOrNull(BusinessCenterPermissions.Self);
            if (businessPermission == null)
            {
                businessPermission = context.CreatePermission(BusinessCenterPermissions.Self, BL(BusinessCenterPermissions.Self));
                var configType = typeof(BusinessCenterPermissions).GetTypeInfo();
                var baseAttributeInfo = GetPermissionAttributeInfo(configType, BusinessCenterPermissions.Self);
                GeneratePermission(configType, businessPermission, baseAttributeInfo, VappsConsts.BusinessLocalizationSourceName);
            }
        }

        /// <summary>
        /// 遍历类生成嵌套菜单
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <param name="parentPermission"></param>
        /// <param name="parentAttributeInfo"></param>
        /// <param name="sourceName"></param>
        public void GeneratePermission(TypeInfo typeInfo, Permission parentPermission, PermissionAttribute parentAttributeInfo,string sourceName)
        {
            if (typeInfo == null || parentPermission == null) return;
            var instance = Activator.CreateInstance(typeInfo.AsType());

            var innerClass = typeInfo.DeclaredMembers
            .Where(p => p.MemberType == MemberTypes.NestedType
            && p.GetType().FullName == "System.RuntimeType"
            && ((dynamic)p).FullName.StartsWith($"{typeInfo.FullName}+")).ToList();
            foreach (var inner in innerClass)
            {
                var permissionName = $"{parentPermission.Name}.{inner.Name}";
                var attributeInfo = GetPermissionAttributeInfo(inner, permissionName, parentAttributeInfo);
                var permission = parentPermission.CreateChildPermission(permissionName, new LocalizableString(attributeInfo.Description, sourceName), multiTenancySides: GetMultiTenancySides(attributeInfo));

                GeneratePermission(inner as TypeInfo, permission, attributeInfo, sourceName);
            }

            var fields = typeInfo.DeclaredMembers.Where(p => p.MemberType == MemberTypes.Field);
            foreach (FieldInfo field in fields)
            {
                if (field.Name == "Self") continue;
                string value = field.GetValue(instance).ToString();
                var attributeInfo = GetPermissionAttributeInfo(field, value, parentAttributeInfo);

                parentPermission.CreateChildPermission(value, new LocalizableString(attributeInfo.Description, sourceName), multiTenancySides: GetMultiTenancySides(attributeInfo));
            }
        }

        public MultiTenancySides GetMultiTenancySides(PermissionAttribute attributeInfo)
        {
            if (!attributeInfo.DependOnEnableMultiTenancy)
                return attributeInfo.MultiTenancySides;

            if (_isMultiTenancyEnabled)
                return attributeInfo.MultiTenancySides;
            else
                return MultiTenancySides.Tenant;
        }


        public static PermissionAttribute GetPermissionAttributeInfo(MemberInfo memberInfo, string permissionName,
            PermissionAttribute parentAttributeInfo = null)
        {
            Object obj = GetAttributeClass(memberInfo, typeof(PermissionAttribute));

            PermissionAttribute permissionAttribute;
            if (obj == null)
            {
                if (parentAttributeInfo != null)
                    permissionAttribute = new PermissionAttribute(permissionName, parentAttributeInfo.MultiTenancySides, parentAttributeInfo.DependOnEnableMultiTenancy);
                else
                    permissionAttribute = new PermissionAttribute(permissionName);
            }
            else
            {
                //使用父类作用域覆盖子类作用域
                permissionAttribute = (PermissionAttribute)obj;
                if (parentAttributeInfo != null && parentAttributeInfo.MultiTenancySides == MultiTenancySides.Host)
                    permissionAttribute.MultiTenancySides = parentAttributeInfo.MultiTenancySides;
            }

            return permissionAttribute;
        }

        /// <summary>
        /// 获取指定属性类的实例
        /// </summary>
        /// <param name="subitem">类子项</param>
        /// <param name="attributeType">DescriptionAttribute属性类或其自定义属性类 类型，例如：typeof(DescriptionAttribute)</param>
        private static Object GetAttributeClass(MemberInfo subitem, Type attributeType)
        {
            //FieldInfo fieldinfo = subitem.GetType().GetField(subitem.ToString());

            //if (fieldinfo == null)
            //    return null;

            Object[] objs = subitem.GetCustomAttributes(attributeType, false).ToArray();
            if (objs == null || objs.Length == 0)
            {
                return null;
            }
            return objs[0];
        }


        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, VappsConsts.ServerSideLocalizationSourceName);
        }

        private static ILocalizableString BL(string name)
        {
            return new LocalizableString(name, VappsConsts.BusinessLocalizationSourceName);
        }
    }
}
