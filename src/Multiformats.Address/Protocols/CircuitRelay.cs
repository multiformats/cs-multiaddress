using Multiformats.Base;
using Multiformats.Hash;

namespace Multiformats.Address.Protocols
{
    public class CircuitRelay : MultiaddressProtocol
    {
        public CircuitRelay()
            : base("libp2p-circuit-relay", 290, -1)
        {
        }

        public CircuitRelay(string address)
            : this(Multihash.Parse(address))
        {
        }

        public CircuitRelay(Multihash address)
            : this()
        {
            Value = address;
        }

        public override void Decode(string value) => Value = Multihash.Parse(value);
        public override void Decode(byte[] bytes) => Value = Multihash.Decode(bytes);
        public override byte[] ToBytes() => (Multihash)Value;
        public override string ToString() => ((Multihash)Value)?.ToString(MultibaseEncoding.Base58Btc) ?? string.Empty;
    }
}
