using System;
using System.IO;
using BinaryEncoding;
using SimpleBase;

namespace Multiformats.Address.Protocols
{
    public class Onion : Protocol
    {
        public Onion()
            : base("onion", 444, 96)
        {
        }

        public Onion(string s)
            : this()
        {
            Value = s;
        }

        public override void Decode(string value)
        {
            var s = value;
            var addr = s.Split(':');
            if (addr.Length != 2)
                throw new Exception("Failed to parse addr");

            if (addr[0].Length != 16)
                throw new Exception("Failed to parse addr");

            Base32.Rfc4648.Decode(addr[0].ToUpper());

            var i = ushort.Parse(addr[1]);
            if (i < 1)
                throw new Exception("Failed to parse addr");

            Value = value;
        }

        public override void Decode(byte[] bytes)
        {
            var addr = Base32.Rfc4648.Encode(bytes.Slice(0, 10), false).ToLower();
            var port = Binary.BigEndian.GetUInt16(bytes, 10);

            Value = $"{addr}:{port}";
        }

        public override byte[] ToBytes()
        {
            var s = (string) Value;
            var addr = s.Split(':');
            if (addr.Length != 2)
                throw new Exception("Failed to parse addr");

            if (addr[0].Length != 16)
                throw new Exception("Failed to parse addr");

            var onionHostBytes = Base32.Rfc4648.Decode(addr[0].ToUpper());
            var i = ushort.Parse(addr[1]);
            if (i < 1)
                throw new Exception("Failed to parse addr");

            using (var stream = new MemoryStream())
            {
                stream.Write(onionHostBytes, 0, onionHostBytes.Length);
                Binary.BigEndian.Write(stream, i);
                return stream.ToArray();
            }
        }
    }
}