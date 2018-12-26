using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.Authorization.Users.Dto
{
    public class GetUsersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否确认邮箱(可空)
        /// </summary>
        public bool? IsEmailConfirmed { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 是否确认手机(可空)
        /// </summary>
        public bool? IsPhoneConfirmed { get; set; }

        /// <summary>
        /// 激活(可空)
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public int[] RoleIds { get; set; }

        /// <summary>
        /// 只获取锁定用户
        /// </summary>
        public bool OnlyLockedUsers { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}