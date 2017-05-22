using System;

namespace Multiformats.Address.Protocols
{
    public class WebRTCDirect : MultiaddressProtocol
    {
        public WebRTCDirect()
            : base("libp2p-webrtc-direct", 276, 0)
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