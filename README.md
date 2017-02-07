# Multiformats.Address (cs-multiaddress)

[![](https://img.shields.io/badge/project-multiformats-blue.svg?style=flat-square)](https://github.com/multiformats/multiformats)
[![](https://img.shields.io/badge/freenode-%23ipfs-blue.svg?style=flat-square)](https://webchat.freenode.net/?channels=%23ipfs)
[![Travis CI](https://img.shields.io/travis/multiformats/cs-multiaddress.svg?style=flat-square&branch=master)](https://travis-ci.org/multiformats/cs-multiaddress)
[![AppVeyor](https://img.shields.io/appveyor/ci/tabrath/cs-multiaddress/master.svg?style=flat-square)](https://ci.appveyor.com/project/tabrath/cs-multiaddress)
[![NuGet](https://buildstats.info/nuget/Multiformats.Address)](https://www.nuget.org/packages/Multiformats.Address/)
[![](https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square)](https://github.com/RichardLitt/standard-readme)
[![Codecov](https://img.shields.io/codecov/c/github/multiformats/cs-multiaddress/master.svg?style=flat-square)](https://codecov.io/gh/multiformats/cs-multiaddress)
[![Libraries.io](https://img.shields.io/librariesio/github/multiformats/cs-multiaddress.svg?style=flat-square)](https://libraries.io/github/multiformats/cs-multiaddress)
[![Quality Gate](http://sonar.dispatch.no/api/badges/gate?key=cs-multiaddress&metric=alert_status)](http://sonar.dispatch.no/dashboard/index/cs-multiaddress)


> [Multiaddr](https://github.com/multiformats/multiaddr) implementation in C#.

Consider this as work in progress as the API is not carved in stone yet.

## Table of Contents

- [Install](#install)
- [Usage](#usage)
- [Supported addresses](#supported-addresses)
- [Maintainers](#maintainers)
- [Contribute](#contribute)
- [License](#license)

## Install

	PM> Install-Package Multiformats.Address

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
* DNS
* DNS4
* DNS6
* IPv4
* IPv6
* IPFS
* P2P
* Onion
* SCTP
* TCP
* UDP
* UDT
* Unix
* WebSocket
* WebSocketSecure

## Maintainers

Captain: [@tabrath](https://github.com/tabrath).

## Contribute

Contributions welcome. Please check out [the issues](https://github.com/multiformats/cs-multiaddress/issues).

Check out our [contributing document](https://github.com/multiformats/multiformats/blob/master/contributing.md) for more information on how we work, and about contributing in general. Please be aware that all interactions related to multiformats are subject to the IPFS [Code of Conduct](https://github.com/ipfs/community/blob/master/code-of-conduct.md).

Small note: If editing the README, please conform to the [standard-readme](https://github.com/RichardLitt/standard-readme) specification.

## License

[MIT](LICENSE) � 2017 Trond Br�then
