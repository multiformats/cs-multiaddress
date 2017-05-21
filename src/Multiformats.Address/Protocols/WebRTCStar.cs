using System;

namespace Multiformats.Address.Protocols
{
    public class WebRTCStar : MultiaddressProtocol
    {
        public WebRTCStar()
            : base("libp2p-webrtc-star", 275, 0)
        {
        }

        public override void Decode(string value)
        {
        }

        public override void Decode(byte[] bytes)
        {
        }

        public override byte[] ToBytes() => EmptyBuffer;
    }
}