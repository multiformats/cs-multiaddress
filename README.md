# Multiformats.Address

[![Build Status](https://travis-ci.org/tabrath/cs-multiaddress.svg?branch=master)](https://travis-ci.org/tabrath/cs-multiaddress)
[![Build status](https://ci.appveyor.com/api/projects/status/4edkkka63u76r6vs?svg=true)](https://ci.appveyor.com/project/tabrath/cs-multiaddress)
[![NuGet Badge](https://buildstats.info/nuget/Multiformats.Address)](https://www.nuget.org/packages/Multiformats.Address/)
[![NuGet Badge](https://buildstats.info/nuget/Multiformats.Address.Net)](https://www.nuget.org/packages/Multiformats.Address.Net/)

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
