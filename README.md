# Multiformats.Address

[![Build Status](https://travis-ci.org/tabrath/cs-multiaddress.svg?branch=master)](https://travis-ci.org/tabrath/cs-multiaddress)
[![Build status](https://ci.appveyor.com/api/projects/status/4edkkka63u76r6vs?svg=true)](https://ci.appveyor.com/project/tabrath/cs-multiaddress)
[![NuGet Badge](https://buildstats.info/nuget/Multiformats.Address)](https://www.nuget.org/packages/Multiformats.Address/)
[![NuGet Badge](https://buildstats.info/nuget/Multiformats.Address.Net)](https://www.nuget.org/packages/Multiformats.Address.Net/)

C# implementation of [multiformats/multiaddr](https://github.com/multiformats/multiaddr).
Consider this as work in progress as the API is not carved in stone yet.

## Usage
``` cs
var ma = Multiaddress.Decode("/ip4/127.0.0.1/udp/1234");
var addresses = ma.Split();
var joined = Multiaddress.Join(addresses);
var tcp = ma.Protocols.Get<TCP>();
```

There's some extension methods included that let's you create multiaddresses of IPEndPoints, and create IPEndPoints from multiaddresses.
Some let's you create sockets directly from IP4/IP6, TCP/UDP multiaddresses.

``` cs
var socket = ma.CreateSocket();
var localEndPoint = socket.GetLocalMultiaddress();
var remoteEndPoint = socket.GetRemoteMultiaddress();
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
