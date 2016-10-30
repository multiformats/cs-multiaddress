using System;

namespace Multiformats.Address.Protocols
{
    public class UDT : Protocol
    {
        public UDT()
            : base("udt", 302, 0)
        {
        }

        public override void Decode(string value)
        {
        }

        public override void Decode(byte[] bytes)
        {
        }

        public override byte[] ToBytes()
        {
            return Array.Empty<byte>();
        }
    }
}