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
            : this(Multihash.FromB58String(address))
        {
        }

        public CircuitRelay(Multihash address)
            : this()
        {
            Value = address;
        }

        public override void Decode(string value) => Value = Multihash.FromB58String(value);
        public override void Decode(byte[] bytes) => Value = Multihash.Decode(bytes);
        public override byte[] ToBytes() => (Multihash)Value;
        public override string ToString() => ((Multihash)Value)?.B58String() ?? string.Empty;
    }
}
