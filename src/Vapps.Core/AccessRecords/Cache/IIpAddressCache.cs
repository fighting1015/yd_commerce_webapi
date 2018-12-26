using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Helpers;

namespace Vapps.AccessRecords.Cache
{
    public interface IIpAddressCache
    {
        AddressData Get(string ip);

        AddressData GetOrNull(string ip);
    }
}
