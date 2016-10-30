using System.Globalization;
using BinaryEncoding;

namespace Multiformats.Address.Protocols
{
    public abstract class Number : Protocol
    {
        protected Number(string name, int code)
            : base(name, code, 16)
        {
        }

        public override void Decode(string value) => Value = ushort.Parse(value, NumberStyles.Number);
        public override void Decode(byte[] bytes) => Value = Binary.BigEndian.GetUInt16(bytes, 0);
        public override byte[] ToBytes() => Binary.BigEndian.GetBytes((ushort)Value);
    }
}
