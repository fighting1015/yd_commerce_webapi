using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization;
using Abp.Runtime.Validation;
using Abp.Localization;
using Abp.Dependency;
using Abp.Zero;
using System.Linq;

namespace Vapps.Authorization.Permissions
{
    public static class PermissionManagerExtensions
    {
        /// <summary>
        /// 根据名称获取所有权限
        /// Throws <see cref="AbpValidationException"/> 没有找到任何权限时
        /// </summary>
        public static IEnumerable<Permission> GetPermissionsFromNamesByValidating(this IPermissionManager permissionManager, IEnumerable<string> permissionNames)
        {
            var permissions = new List<Permission>();
            var undefinedPermissionNames = new List<string>();

            foreach (var permissionName in permissionNames)
            {
                var permission = permissionManager.GetPermissionOrNull(permissionName);
                if (permission == null)
                {
                    undefinedPermissionNames.Add(permissionName);
                }

                permissions.Add(permission);
            }

            if (undefinedPermissionNames.Count <= 0)
                return permissions;

            var localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
            throw new AbpValidationException(string.Format(L(localizationManager, "Permission.Undefined"), undefinedPermissionNames.Count))
            {
                ValidationErrors = undefinedPermissionNames.Select(permissionName =>
                new ValidationResult(string.Format(
                    L(localizationManager, "Permission.UndefinedName"), permissionName))).ToList()
            };
        }

        private static string L(ILocalizationManager localizationManager, string name)
        {
            return localizationManager.GetString(AbpZeroConsts.LocalizationSourceName, "Permission.Undefined");
        }
    }
}

