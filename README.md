# Multiformats.Address
C# implementation of [multiformats/multiaddr](https://github.com/multiformats/multiaddr).

## Usage
``` cs
var ma = Multiaddress.Decode("/ip4/127.0.0.1/udp/1234");
var addresses = ma.Split();
var joined = Multiaddress.Join(addresses);
var tcp = ma.Protocols.OfType<TCP>().SingleOrDefault();
```

## Supported addresses

* DCCP
* HTTP
* HTTPS
* IPv4
* IPv6
* IPFS
* Onion
* SCTP
* TCP
* UDP
* UDT
* Unix
* WebSocket
