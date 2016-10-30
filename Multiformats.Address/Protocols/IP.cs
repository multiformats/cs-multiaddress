using System.Net;

namespace Multiformats.Address.Protocols
{
    public abstract class IP : Protocol
    {
        protected IP(string name, int code, int size)
            : base(name, code, size)
        {
        }

        public override void Decode(string value) => Value = IPAddress.Parse(value);
        public override void Decode(byte[] bytes) => Value = new IPAddress(bytes);
        public override byte[] ToBytes() => (Value as IPAddress).GetAddressBytes();
        public override string ToString() => Value?.ToString() ?? string.Empty;
    }

}
