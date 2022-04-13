## CTX2

3 satellites will be running as a set of virtual machines on a master laptop, each of them will have a set of vulnerabilities that will allow remote access from the cadets. A UI display will show the status of all the satellites and their connection with the DLAB (where the cadets are hacking from).

1. One of the satellites (satcom0) will be running a vulnerable web application that contains logs of passwords. Directory brute forcing and log examining will lead to an SSH login being obtained.

2. Another satellite (satcom1) will be taken offline as it is running a vulnerable FTP server. This server is very simple and written in python. The cadets will reverse engineer the code and write a few lines into their client to exploit the vulnerability.

3. Lastly, the final satellite (satcom2) will only be vulnerable when access to a physical USB is obtained. This will be done by a mission scenario with a high value target who is carrying it. After this, the satellite will be taken offline via John the Ripper and SSH.

### Objectives

Estimated Time: 1.5 hours

Technical Objectives:
1. Intro to port scanning
2. Intro to reverse engineering
3. Intro to FTP servers
4. Intro to attacking web services
5. Intro to SSH

Tools:
1. Laptop with VirtualBox
2. USB Stick
3. Laptop with Kali
4. Laptop with Python3 and a code editor
5. Optional: LED lights

Setup:

### Scenario

Team is briefed with the following.

```
Welcome to the DLAB Satellite Command Center.

While you were taking control of the DLAB the Yuktobanian soldiers activated their satellite based missile system. From now on, if you or any of your team step outside of the DLAB and nearby buildings, you will be shot by enemy missiles.

However, allied forces have been gathering intel on this satellite system and believe that it contains a set of vulnerabilities and can be taken offline by a skilled team of hackers. When each satellite is taken offline, there will be a 10 minute window displayed on the screen where you can go outside. The entire satellite system takes 30 minutes to orbit with each satellite providing 10 minutes of coverage.

1. One of the satellites (satcom0) is reported to be running a vulnerable web application that details the status of the missile system. You are tasked with taking this satellite offline. Intel believes the following tools will be all you need to complete the job.
- IP address of satcom0: [insert IP here] 129.21.65.42
- nmap
- dirb
- ssh
- Laptop with Kali

2. Another satellite (satcom1) will be taken offline as it is running a vulnerable FTP server. The exact vulnerability in unknown, but we have managed to gain access to the source code. You will need to reverse engineer the server, add code to the client, and exploit the vulnerability.
- IP address of satcom1: [insert IP here] 129.21.67.157
- Laptop with VSCode, Python, and the FTP server and client code

3. Lastly, the final satellite (satcom2) will only be vulnerable when access to a physical USB is obtained. This will be done by a mission scenario with a high value target who is carrying it. You will need the following tools.
- IP address of satcom2: [insert IP here]
- John the Ripper
- ssh
- Laptop with Kali

Do you have any questions?

```

### Onsite Setup

1. Plug in Kali Laptop, Python Laptop, and UI/VM Laptop
2. Run the SatcomUI
3. Run the apiserver with `./bootstrap.sh`
4. Run the satcom0 VM
5. Edit the IP and run `python3 satellite.py` on the VM
6. Run the webserver with `sudo ./run_server.sh` on the VM
7. Run the satcom1 FTP server
8. Run the satcom2 VM
9. Edit the IP and run `python3 satellite.py` on the VM
10. Gather IP addresses and add to mission statements
11. Load the onto zip file `zip -e secret_files.zip file1.txt`
12. Put zip file on the USB
13. Verify code is on Laptop with Python/VSCode
14. Verify UI is working
15. Verify Kali laptop is working
16. Verify FTP client can connect
17. Give brief