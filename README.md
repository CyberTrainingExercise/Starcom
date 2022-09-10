## Operation Starcom

3 satellites will be running as a set of virtual machines on a master laptop, each of them will have a set of vulnerabilities that will allow remote access from the cadets. A UI display will show the status of all the satellites and their connection with the DLAB (where the cadets are hacking from).

1. One of the satellites (starcom0) will be running a vulnerable web application that contains logs of passwords. Directory brute forcing and log examining will lead to an SSH login being obtained.

2. Another satellite (starcom1) will be taken offline as it is running a vulnerable FTP server. This server is very simple and written in python. The cadets will reverse engineer the code and write a few lines into their client to exploit the vulnerability.

3. Lastly, the final satellite (starcom2) will only be vulnerable when access to a physical USB is obtained. This will be done by a mission scenario with a high value target who is carrying it. After this, the satellite will be taken offline via John the Ripper and SSH.

## Authors Note

I wanted to make a special note on this CTX about realism. The technology of this CTX is real in that these technologies are used to professionally hack things, but it is not real in that satellites are not accessible via SSH/Dirb/FTP over the open internet. In addition, most satellites aren't running a webserver, an FTP server, or a linux operating system.

With all that in mind, this CTX aims to provide value by giving cadets a look into what pentesting common systems looks like. It also aims to provide value by providing them with an engaging look into a space themed CTX.

### Objectives

Estimated Time: 1.5 hours

Technical Objectives:
1. Port scanning
1. Reverse engineering
1. FTP servers
1. Attacking web services
1. SSH

Tools:
1. Machine capable of running at least 2 VMs and a Unity Executable
1. Machine capable of running Kali
1. Machine capable of running Python3 and a code editor
1. USB Stick

Setup:

### Scenario

Team is briefed with the following.

```
Welcome to the Starcom Satellite Command Center.

While you were taking control of the DLAB the Yuktobanian soldiers activated their satellite based missile system. From now on, if you or any of your team step outside of the DLAB and nearby buildings, you will be shot by enemy missiles.

However, allied forces have been gathering intel on this satellite system and believe that it contains a set of vulnerabilities and can be taken offline by a skilled team of hackers. When each satellite is taken offline, there will be a 10 minute window displayed on the screen where you can go outside. The entire satellite system takes 30 minutes to orbit with each satellite providing 10 minutes of coverage.

1. One of the satellites (starcom0) is reported to be running a vulnerable web application that details the status of the missile system. You are tasked with taking this satellite offline. Intel believes the following tools will be all you need to complete the job.
- IP address of starcom0: [insert IP here]
- nmap
- dirb
- ssh
- Machine with Kali

2. Another satellite (starcom1) will be taken offline as it is running a vulnerable FTP server. The exact vulnerability in unknown, but we have managed to gain access to the source code. You will need to reverse engineer the server, add code to the client, and exploit the vulnerability.
- IP address of starcom1: [insert IP here]
- Machine with VSCode, Python, and the FTP server and client code

3. Lastly, the final satellite (starcom2) will only be vulnerable when access to a physical USB is obtained. This will be done by a mission scenario with a high value target who is carrying it. You will need the following tools.
- IP address of starcom2: [insert IP here]
- John the Ripper
- ssh
- Machine with Kali

Do you have any questions?

```

### Virtual Machine Setup

All the virtual machine setup for this particular CTX are required to be Linux based. Linux Mint 20 is recommended, but various other forms of Linux will work as well. This guide will assume you are using Linux Mint 20 and VirtualBox. Both of these were chosen as they are easy to use, free, open source, and work well on multiple host operating systems.

### Onsite Setup

1. Turn on Kali machine, Python machine, and UI/VM Machine
2. Run the Starcom UI
3. Run the Status Server with `./run_status_server.sh`
4. Run the starcom0 VM
    - See the Virtual Machine Setup Guide for help on how to do this.
5. Edit the IP and run `python3 satellite.py` on the VM
6. Run the webserver with `sudo ./run_server.sh` on the VM
7. Run the starcom1 FTP server
8. Run the starcom2 VM
9. Edit the IP and run `python3 satellite.py` on the VM
10. Gather IP addresses and add to mission statements
11. Load the onto zip file `zip -e secret_files.zip file1.txt`
12. Put zip file on the USB
13. Verify code is on Python machine with Python/VSCode
14. Verify UI is working
15. Verify Kali machine is working
16. Verify FTP client can connect
17. Give brief