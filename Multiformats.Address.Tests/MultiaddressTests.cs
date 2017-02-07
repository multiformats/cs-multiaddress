using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using Multiformats.Address.Protocols;
using NUnit.Framework;
using Org.BouncyCastle.Utilities.Encoders;

namespace Multiformats.Address.Tests
{
    [TestFixture]
    public class MultiaddressTests
    {
        [TestCase("/ip4")]
        [TestCase("/ip4/::1")]
        [TestCase("/ip4/fdpsofodsajfdoisa")]
        [TestCase("/ip6")]
        [TestCase("/udp")]
        [TestCase("/tcp")]
        [TestCase("/sctp")]
        [TestCase("/udp/65536")]
        [TestCase("/tcp/65536")]
        [TestCase("/onion/9imaq4ygg2iegci7:80")]
        [TestCase("/onion/aaimaq4ygg2iegci7:80")]
        [TestCase("/onion/timaq4ygg2iegci7:0")]
        [TestCase("/onion/timaq4ygg2iegci7:-1")]
        [TestCase("/onion/timaq4ygg2iegci7")]
        [TestCase("/onion/timaq4ygg2iegci@:666")]
        [TestCase("/udp/1234/sctp")]
        [TestCase("/udp/1234/udt/1234")]
        [TestCase("/udp/1234/utp/1234")]
        [TestCase("/ip4/127.0.0.1/udp/jfodsajfidosajfoidsa")]
        [TestCase("/ip4/127.0.0.1/udp")]
        [TestCase("/ip4/127.0.0.1/tcp/jfodsajfidosajfoidsa")]
        [TestCase("/ip4/127.0.0.1/tcp")]
        [TestCase("/ip4/127.0.0.1/ipfs")]
        [TestCase("/ip4/127.0.0.1/ipfs/tcp")]
        [TestCase("/unix")]
        [TestCase("/ip4/1.2.3.4/tcp/80/unix")]
        [TestCase("/dns")]
        [TestCase("/dns4")]
        [TestCase("/dns6")]
        public void TestConstructFails(string addr)
        {
            Assert.Throws(Is.InstanceOf<Exception>(), () => Multiaddress.Decode(addr));
        }

        [TestCase("/ip4/1.2.3.4")]
        [TestCase("/ip4/0.0.0.0")]
        [TestCase("/ip6/::1")]
        [TestCase("/ip6/2601:9:4f81:9700:803e:ca65:66e8:c21")]
        [TestCase("/onion/timaq4ygg2iegci7:1234")]
        [TestCase("/onion/timaq4ygg2iegci7:80/http")]
        [TestCase("/udp/0")]
        [TestCase("/tcp/0")]
        [TestCase("/sctp/0")]
        [TestCase("/udp/1234")]
        [TestCase("/tcp/1234")]
        [TestCase("/sctp/1234")]
        [TestCase("/udp/65535")]
        [TestCase("/tcp/65535")]
        [TestCase("/ipfs/QmcgpsyWgH8Y8ajJz1Cu72KnS5uo2Aa2LpzU7kinSupNKC")]
        [TestCase("/p2p/QmcgpsyWgH8Y8ajJz1Cu72KnS5uo2Aa2LpzU7kinSupNKC")]
        [TestCase("/udp/1234/sctp/1234")]
        [TestCase("/udp/1234/udt")]
        [TestCase("/udp/1234/utp")]
        [TestCase("/tcp/1234/http")]
        [TestCase("/tcp/1234/https")]
        [TestCase("/ipfs/QmcgpsyWgH8Y8ajJz1Cu72KnS5uo2Aa2LpzU7kinSupNKC/tcp/1234")]
        [TestCase("/p2p/QmcgpsyWgH8Y8ajJz1Cu72KnS5uo2Aa2LpzU7kinSupNKC/tcp/1234")]
        [TestCase("/ip4/127.0.0.1/udp/1234")]
        [TestCase("/ip4/127.0.0.1/udp/0")]
        [TestCase("/ip4/127.0.0.1/tcp/1234")]
        [TestCase("/ip4/127.0.0.1/tcp/1234/")]
        [TestCase("/ip4/127.0.0.1/ipfs/QmcgpsyWgH8Y8ajJz1Cu72KnS5uo2Aa2LpzU7kinSupNKC")]
        [TestCase("/ip4/127.0.0.1/ipfs/QmcgpsyWgH8Y8ajJz1Cu72KnS5uo2Aa2LpzU7kinSupNKC/tcp/1234")]
        [TestCase("/unix/a/b/c/d/e")]
        [TestCase("/unix/stdio")]
        [TestCase("/ip4/1.2.3.4/tcp/80/unix/a/b/c/d/e/f")]
        [TestCase("/ip4/127.0.0.1/ipfs/QmcgpsyWgH8Y8ajJz1Cu72KnS5uo2Aa2LpzU7kinSupNKC/tcp/1234/unix/stdio")]
        [TestCase("/dns/www.google.com")]
        [TestCase("/dns4/www.google.com")]
        [TestCase("/dns6/www.google.com")]
        public void TestConstructSucceeds(string addr)
        {
            Assert.DoesNotThrow(() => Multiaddress.Decode(addr));
        }

        [Test]
        public void TestEqual()
        {
            var m1 = Multiaddress.Decode("/ip4/127.0.0.1/udp/1234");
            var m2 = Multiaddress.Decode("/ip4/127.0.0.1/tcp/1234");
            var m3 = Multiaddress.Decode("/ip4/127.0.0.1/tcp/1234");
            var m4 = Multiaddress.Decode("/ip4/127.0.0.1/tcp/1234");

            Assert.That(m1, Is.Not.EqualTo(m2));
            Assert.That(m2, Is.Not.EqualTo(m1));
            Assert.That(m2, Is.EqualTo(m3));
            Assert.That(m3, Is.EqualTo(m2));
            Assert.That(m1, Is.EqualTo(m1));
            Assert.That(m2, Is.EqualTo(m4));
            Assert.That(m4, Is.EqualTo(m3));
        }

        [TestCase("/ip4/127.0.0.1/udp/1234", "047f0000011104d2")]
        [TestCase("/ip4/127.0.0.1/tcp/4321", "047f0000010610e1")]
        [TestCase("/ip4/127.0.0.1/udp/1234/ip4/127.0.0.1/tcp/4321", "047f0000011104d2047f0000010610e1")]
        public void TestStringToBytes(string s, string h)
        {
            var b1 = Hex.Decode(h);
            var b2 = Multiaddress.Decode(s).ToBytes();

            Assert.That(b2, Is.EqualTo(b1));
        }

        [TestCase("/ip4/127.0.0.1/udp/1234", "047f0000011104d2")]
        [TestCase("/ip4/127.0.0.1/tcp/4321", "047f0000010610e1")]
        [TestCase("/ip4/127.0.0.1/udp/1234/ip4/127.0.0.1/tcp/4321", "047f0000011104d2047f0000010610e1")]
        [TestCase("/onion/aaimaq4ygg2iegci:80", "bc030010c0439831b48218480050")]
        public void TestBytesToString(string s1, string h)
        {
            var b = Hex.Decode(h);
            var s2 = Multiaddress.Decode(s1).ToString();

            Assert.That(s2, Is.EqualTo(s1));
        }

        [TestCase("/ip4/1.2.3.4/udp/1234", "/ip4/1.2.3.4", "/udp/1234")]
        [TestCase("/ip4/1.2.3.4/tcp/1/ip4/2.3.4.5/udp/2", "/ip4/1.2.3.4", "/tcp/1", "/ip4/2.3.4.5", "/udp/2")]
        [TestCase("/ip4/1.2.3.4/utp/ip4/2.3.4.5/udp/2/udt", "/ip4/1.2.3.4", "/utp", "/ip4/2.3.4.5", "/udp/2", "/udt")]
        public void TestBytesSplitAndJoin(string s, params string[] res)
        {
            var m = Multiaddress.Decode(s);
            var split = m.Split().ToArray();

            Assert.That(split.Length, Is.EqualTo(res.Length));

            for (var i = 0; i < split.Length; i++)
            {
                Assert.That(split[i].ToString(), Is.EqualTo(res[i]));
            }

            var joined = Multiaddress.Join(split);
            Assert.That(joined, Is.EqualTo(m));
        }

        [Test]
        public void TestEncapsulate()
        {
            var m = Multiaddress.Decode("/ip4/127.0.0.1/udp/1234");
            var m2 = Multiaddress.Decode("/udp/5678");

            var b = m.Encapsulate(m2);
            Assert.That(b.ToString(), Is.EqualTo("/ip4/127.0.0.1/udp/1234/udp/5678"));

            var m3 = Multiaddress.Decode("/udp/5678");
            var c = b.Decapsulate(m3);
            Assert.That(c.ToString(), Is.EqualTo("/ip4/127.0.0.1/udp/1234"));

            var m4 = Multiaddress.Decode("/ip4/127.0.0.1");
            var d = c.Decapsulate(m4);
            Assert.That(d.ToString(), Is.EqualTo(""));
        }

        [Test]
        public void TestGetValue()
        {
            var a =
                Multiaddress.Decode(
                    "/ip4/127.0.0.1/utp/tcp/5555/udp/1234/utp/ipfs/QmbHVEEepCi7rn7VL7Exxpd2Ci9NNB6ifvqwhsrbRMgQFP");

            Assert.That(a.Protocols.OfType<IP4>().FirstOrDefault().ToString(), Is.EqualTo("127.0.0.1"));
            Assert.That(a.Protocols.OfType<UTP>().FirstOrDefault()?.ToString(), Is.EqualTo(""));
            Assert.That(a.Protocols.OfType<TCP>().FirstOrDefault().ToString(), Is.EqualTo("5555"));
            Assert.That(a.Protocols.OfType<UDP>().FirstOrDefault().ToString(), Is.EqualTo("1234"));
            Assert.That(a.Protocols.OfType<IPFS>().FirstOrDefault().ToString(), Is.EqualTo("QmbHVEEepCi7rn7VL7Exxpd2Ci9NNB6ifvqwhsrbRMgQFP"));

            a = Multiaddress.Decode("/ip4/0.0.0.0/unix/a/b/c/d");
            Assert.That(a.Protocols.OfType<IP4>().FirstOrDefault().ToString(), Is.EqualTo("0.0.0.0"));
            Assert.That(a.Protocols.OfType<Unix>().FirstOrDefault().ToString(), Is.EqualTo("a/b/c/d"));
        }
    }
}
