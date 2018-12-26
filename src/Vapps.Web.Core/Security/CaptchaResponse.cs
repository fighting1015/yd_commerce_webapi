using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Web.Security
{
    public class CaptchaResponse
    {
        public bool IsValid { get; set; }

        public int ErrorCodes { get; set; }

        public string ErrorMsg { get; set; }

        public CaptchaResponse()
        {

        }
    }
}
