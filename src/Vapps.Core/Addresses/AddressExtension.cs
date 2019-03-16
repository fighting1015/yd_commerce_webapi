using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Addresses
{
    public static class AddressExtension
    {

        public static string GetFullAddress(this Address address)
        {
            return $"{address.Province}{address.City}{address.District}{address.DetailAddress}";
        }
    }
}
