### Sat0 - Broken Webserver

This mini-CTX is designed to teach directory brute forcing.

The cadets will be given, at minimum, an ip address. Depending on how hard the mini-CTX is designed to be addtional things can be given.

### Setup

1. Create an Ubuntu or similar VM
2. Verify SSH works
3. Verify python3 works
4. Copy the `sat0_broken_webserver` folder over to it 
5. Change the IP address in `satellite.py` and verify it can connect to the `apiserver.py` and the `satcom_ui`
6. Run the webserver using `sudo ./run_server`
7. Verify the server is running and accessible from the outside
8. Give the cadets instructions and a Kali laptop

### Solution

To solve this box:

1. Give cadets ip address of server
2. Run nmap on server: `nmap [ip address]`
3. Visit website
4. Use directory bruteforce: `dirb http://[ip address]`
5. Open `output.log`
6. Find password: using `ctrl+F password` or `grep password` and pick the most recent password
7. Find username: see log file directory path `/home/satcom0/logs`
8. SSH into server: `ssh satcom0@[ip address]` using password from previous step