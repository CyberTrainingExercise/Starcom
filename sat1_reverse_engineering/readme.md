# Tiger FTP System

Custom concurrent FTP server and client written in python using TCP sockets.

This code is being used to teach reverse engineering in a Cyber Training Exercise (CTX2).

## Mission

The FTP server is designed with a vulnerability. It can still take an "abort" command despite the fact that the client no longer supports the abort command. The trainees will be given a the server and client code and asked to find a bug in the code that will allow for a remote takedown.

Various levels of difficulty are possible:
    1. Give them the all the code, even the code with the abort feature. Simply have them read the documentation until they figure out how to connect and send commands.
    2. Give them all the code, except the abort features in the client and shared library. Have them read the documentation and write a few lines to produce a valid packet to disable the FTP server.
    3. Give them all the code, except the abort features in the client and shared library. Do not give them any documentation, they have to just use the code.
    4. Give them all the code, except the abort features in the client and shared library. Do not give them any documentation and run the code through a python obfuscator.

Additional challenge can come from making the `passwords.txt` file secret.

## Build System

Please run `setup.sh`

Then run `./TigerS` and `./TigerC`

Feel free to include your first command to `TigerC` in the command line like so:
`./TigerC connect localhost user pass`

The server is in the root directory and the client is in a directory below. Due to some annoying quirks with python I was not able to get them to run in the same level of directory without duplicating the `tigershared.py` code. I figured it would be cleaner to have the shared code just in the project once so I left it this way.
### Project Structure

There are 3 main files, `tigers.py`, `tigerc.py`, and `tigershared.py`.

These correspond to the server application, client application, and shared library.

Additionally, `tigerc_noabort.py` and `tigershared.py` is the client and shared library without the abort feature implemented.

#### Server Application

The server runs a main thread which accepts connections, on each reception of a connection is starts a new thread which handles the client that was trying to connect. The client thread handles all requests from the client and closes itself when the client exits.

The sever reads the client usernames and passwords from the file `passwords.txt`

#### Client Application

The client applications has a main thread which runs reception of data from the server. The client also has a thread to handle reception of data from the user. This split was made primarily to help split the responsibilities into neat little silos of code and to avoid an issues with slow processing of user or server input.

Client input data is parsed and then a command is chosen and the subsequents steps for that command are run. After this the thread checks if it should stop and then continues or stops based on that.

In addition to the dotted decimal of a specific IP address, a domain name such as `localhost` is accepted by the client as well.

#### Shared Libary

This library contains helpful functions for that both the client and server need for the FTP protocol they are using. This includes the identification, parsing, and creation of packets for the FTP protocol.

### Transport Protocol

This project uses TCP as the transport protocol.
### Concurrency

Concurrency was implemented using multithreading with the standard threading library that python provides. As detailed in the Server Application section, the server creates a new thread for each client and uses that to respond to that clients requests.

## Command and Control Protocol

The protocol is fairly simple. It has a header with a packet type, a data length, and then the data. The details of these can be found below.

Custom FTP protocol packet:
- x to n byte protocol packet length
- Byte 1: command type
- Bytes 2 to x: data length
- Bytes x+1 to n: data

Command types and their data:
1. connection <username> <password>
2. get <file>
3. put <file>
4. exit

Protocol types, their data, and their usage:
1. connection <username> <password>         used as a request for the client to connect to the server
2. get <file>                               used as a request for a file, responsed too by x transfer packets and a finish transfer packet
3. put <file>                               used as a request to put a file, followed by x transfer packets and a finish transfer packet
4. exit                                     used to end connection with the client and the server
5. transfer <filedata>                      used to transfer file data
6. finish transfer                          used to denote the end of a file transfer
7. message <message>                        used to send a message to the client, client displays these to the user
8. abort                                    used to abort the servers satcom connection