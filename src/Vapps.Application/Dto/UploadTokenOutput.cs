using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Dto
{
    public class UploadTokenOutput
    {
        public string Token { get; set; }

        public DateTime ExpirationOnUtc { get; set; }
    }
}
