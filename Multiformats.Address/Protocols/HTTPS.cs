using System;

namespace Multiformats.Address.Protocols
{
    public class HTTPS : Protocol
    {
        public HTTPS()
            : base("https", 480, 0)
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