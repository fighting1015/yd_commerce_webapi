using System.ComponentModel.DataAnnotations;

namespace Vapps.Web.Models.TokenAuth
{
    public class ExternalUnBindingModel
    {
        /// <summary>
        /// 认证类型
        /// </summary>
        [Required]
        public string AuthProvider { get; set; }
    }
}
