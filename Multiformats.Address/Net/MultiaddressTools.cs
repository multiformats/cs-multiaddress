using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Multiformats.Address.Net
{
    public static class MultiaddressTools
    {
        public static IEnumerable<Multiaddress> GetInterfaceMultiaddresses()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .SelectMany(MultiaddressExtensions.GetMultiaddresses);
        }
    }
}
