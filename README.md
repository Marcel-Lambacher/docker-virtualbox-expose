# Attention:

This feature is available in the offical Docker for windows toolkit now and therefore this repository is not unter maintenance anymore!

# docker-virtualbox-expose

This projects provides a set of services which integrates VirtualBox and Docker seamless into the Windows host.
Exposed Docker containers will automatically create a port forwarding for VirtualBox to the host OS.
The host OS can directly access the docker services within the VirtualBox machine through the host IP address.

![Architecture Diagram](../master/docs/Architecture-Overview.png)

## Prerequisites

To run this toolset the following prerequisites are required:
- VirtualBox installed on your Windows host OS
- A docker compatible VM configured in VirtualBox (e.g Debian)
- Docker Engine installed on the guest VM

## Installation

## Usage

Start a docker container and attach the following label:
```
$ docker container run --label vm-expose --port 80:80 --port 8080:8080 <some-container>
```
Using the `vm-expose` label without a value will create a port forwarding for all exposed ports (i.e. 80 and 8080).
The windows host is now able to access the exposed services directly using the localhost addresss.
We can also define specific ports we want to expose:
```
$ docker container run --label vm-expose="80, 22" --port 80:80 --port 21:21 --port 22:22 <some-container>
```
This command will expose only port 80 and 22 to the host system.

## Contributing

## Changelog

## Roadmap

## License

This project is licensed under the MIT License - see the [LICENSE](../master/LICENSE) file for details.
