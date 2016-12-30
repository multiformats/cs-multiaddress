using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Multiformats.Address.Net;
using NUnit.Framework;

namespace Multiformats.Address.Tests
{
    [TestFixture]
    public class ConvertTests
    {
        [Test]
        public void IPEndPoint_GivenIPv4Tcp_ReturnsValid()
        {
            var ep = new IPEndPoint(IPAddress.Loopback, 1337);
            var ma = ep.ToMultiaddress(ProtocolType.Tcp);
            var result = ma.ToString();

            Assert.That(result, Is.EqualTo("/ip4/127.0.0.1/tcp/1337"));
        }
        
        [Test]
        public void IPEndPoint_GivenIPv6Tcp_ReturnsValid()
        {
            var ep = new IPEndPoint(IPAddress.IPv6Loopback, 1337);
            var ma = ep.ToMultiaddress(ProtocolType.Tcp);
            var result = ma.ToString();

            Assert.That(result, Is.EqualTo("/ip6/::1/tcp/1337"));
        }

        [Test]
        public void IPEndPoint_GivenIPv4Udp_ReturnsValid()
        {
            var ep = new IPEndPoint(IPAddress.Loopback, 1337);
            var ma = ep.ToMultiaddress(ProtocolType.Udp);
            var result = ma.ToString();

            Assert.That(result, Is.EqualTo("/ip4/127.0.0.1/udp/1337"));
        }

        [Test]
        public void IPEndPoint_GivenIPv6Udp_ReturnsValid()
        {
            var ep = new IPEndPoint(IPAddress.IPv6Loopback, 1337);
            var ma = ep.ToMultiaddress(ProtocolType.Udp);
            var result = ma.ToString();

            Assert.That(result, Is.EqualTo("/ip6/::1/udp/1337"));
        }

        [Test]
        public void Multiaddress_GivenIPv4Tcp_ReturnsValidEndPoint()
        {
            var ma = Multiaddress.Decode("/ip4/127.0.0.1/tcp/1337");
            ProtocolType p;
            var ep = ma.ToEndPoint(out p);

            Assert.That(ep.AddressFamily, Is.EqualTo(AddressFamily.InterNetwork));
            Assert.That(ep.Address, Is.EqualTo(IPAddress.Loopback));
            Assert.That(ep.Port, Is.EqualTo(1337));
            Assert.That(p, Is.EqualTo(ProtocolType.Tcp));
        }

        [Test]
        public void Multiaddress_GivenIPv4Udp_ReturnsValidEndPoint()
        {
            var ma = Multiaddress.Decode("/ip4/127.0.0.1/udp/1337");
            ProtocolType p;
            var ep = ma.ToEndPoint(out p);

            Assert.That(ep.AddressFamily, Is.EqualTo(AddressFamily.InterNetwork));
            Assert.That(ep.Address, Is.EqualTo(IPAddress.Loopback));
            Assert.That(ep.Port, Is.EqualTo(1337));
            Assert.That(p, Is.EqualTo(ProtocolType.Udp));
        }

        [Test]
        public void Multiaddress_GivenIPv6Tcp_ReturnsValidEndPoint()
        {
            var ma = Multiaddress.Decode("/ip6/::1/tcp/1337");
            ProtocolType p;
            var ep = ma.ToEndPoint(out p);

            Assert.That(ep.AddressFamily, Is.EqualTo(AddressFamily.InterNetworkV6));
            Assert.That(ep.Address, Is.EqualTo(IPAddress.IPv6Loopback));
            Assert.That(ep.Port, Is.EqualTo(1337));
            Assert.That(p, Is.EqualTo(ProtocolType.Tcp));
        }

        [Test]
        public void Multiaddress_GivenIPv6Udp_ReturnsValidEndPoint()
        {
            var ma = Multiaddress.Decode("/ip6/::1/udp/1337");
            ProtocolType p;
            var ep = ma.ToEndPoint(out p);

            Assert.That(ep.AddressFamily, Is.EqualTo(AddressFamily.InterNetworkV6));
            Assert.That(ep.Address, Is.EqualTo(IPAddress.IPv6Loopback));
            Assert.That(ep.Port, Is.EqualTo(1337));
            Assert.That(p, Is.EqualTo(ProtocolType.Udp));
        }

        [Test]
        public void Socket_GivenMultiaddress_CreatesSocket()
        {
            var ma = Multiaddress.Decode("/ip4/127.0.0.1/tcp/1337");
            using (var socket = ma.CreateSocket())
            {
                Assert.That(socket.AddressFamily, Is.EqualTo(AddressFamily.InterNetwork));
                Assert.That(socket.ProtocolType, Is.EqualTo(ProtocolType.Tcp));
                Assert.That(socket.SocketType, Is.EqualTo(SocketType.Stream));
            }
        }

        [Test]
        public void Socket_GivenListenerAndConnection_Connects()
        {
            var ma = Multiaddress.Decode("/ip4/127.0.0.1/tcp/1337");
            using (var listener = ma.CreateListener())
            {
                listener?.BeginAccept(ar =>
                {
                    try
                    {
                        var conn = ((Socket)ar.AsyncState).EndAccept(ar);
                        Thread.Sleep(100);
                        conn?.Dispose();
                    }
                    catch { }
                }, listener);

                using (var connection = ma.CreateConnection())
                {
                    Assert.That(connection?.Connected ?? false, Is.True);
                }
            }
        }

        [TestCase("/ip4/127.0.0.1/udp/1234", true)]
        [TestCase("/ip4/127.0.0.1/tcp/1234", true)]
        [TestCase("/ip4/127.0.0.1/udp/1234/tcp/1234", true)]
        [TestCase("/ip4/127.0.0.1/tcp/12345/ip4/1.2.3.4", true)]
        [TestCase("/ip6/::1/tcp/80", true)]
        [TestCase("/ip6/::1/udp/80", true)]
        [TestCase("/ip6/::1", true)]
        [TestCase("/tcp/1234/ip4/1.2.3.4", false)]
        [TestCase("/tcp/1234", false)]
        [TestCase("/tcp/1234/udp/1234", false)]
        [TestCase("/ip4/1.2.3.4/ip4/2.3.4.5", true)]
        [TestCase("/ip6/::1/ip4/2.3.4.5", true)]
        public void TestThinWaist(string addr, bool expected)
        {
            var m = Multiaddress.Decode(addr);

            Assert.That(m.IsThinWaist(), Is.EqualTo(expected));
        }

#if !__MonoCS__
        [Test]
        public void CanGetInterfaceAddresses()
        {
            var addrs = MultiaddressTools.GetInterfaceMultiaddresses();

            Assert.That(addrs.Count(), Is.GreaterThan(1));
        }
#endif

        private void TestAddr(Multiaddress m, Multiaddress[] input, Multiaddress[] expect)
        {
            var actual = m.Match(input);

            Assert.That(actual, Is.EqualTo(expect));
        }

        [Test]
        public void TestAddrMatch()
        {
            var a = new[]
            {
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/2345"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/tcp/2345"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/tcp/2345"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/udp/1234"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/udp/1234"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/ip6/::1"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/ip6/::1"),
                Multiaddress.Decode("/ip6/::1/tcp/1234"),
                Multiaddress.Decode("/ip6/::1/tcp/2345"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/tcp/2345"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/tcp/2345"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/udp/1234"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/udp/1234"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/ip6/::1"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/ip6/::1"),
            };

            TestAddr(a[0], a, new []
            {
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234"), 
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/2345"), 
            });

            TestAddr(a[2], a, new[]
            {
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/tcp/2345"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/tcp/2345"),
            });

            TestAddr(a[4], a, new[]
            {
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/udp/1234"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/udp/1234"),
            });

            TestAddr(a[6], a, new[]
            {
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/ip6/::1"),
                Multiaddress.Decode("/ip4/1.2.3.4/tcp/1234/ip6/::1"),
            });

            TestAddr(a[8], a, new[]
            {
                Multiaddress.Decode("/ip6/::1/tcp/1234"),
                Multiaddress.Decode("/ip6/::1/tcp/2345"),
            });

            TestAddr(a[10], a, new[]
            {
                Multiaddress.Decode("/ip6/::1/tcp/1234/tcp/2345"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/tcp/2345"),
            });

            TestAddr(a[12], a, new[]
            {
                Multiaddress.Decode("/ip6/::1/tcp/1234/udp/1234"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/udp/1234"),
            });

            TestAddr(a[14], a, new[]
            {
                Multiaddress.Decode("/ip6/::1/tcp/1234/ip6/::1"),
                Multiaddress.Decode("/ip6/::1/tcp/1234/ip6/::1"),
            });
        }
    }
}
