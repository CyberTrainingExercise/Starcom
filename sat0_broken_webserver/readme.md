To move files to server, use scp:
`scp -r sat0_broken_webserver/ satcom0@192.168.1.25:/home/satcom0/Desktop`

To solve this box:

1. Give cadets ip address of server
2. Run nmap on server: `nmap [ip address]`
3. Visit website
4. Use directory bruteforce: `dirb http://[ip address]`
5. Open `output.log`
6. Find password: `ctrl+F password` pick the most recent password
7. Find username: see log file directory path `/home/satcom0/logs`
8. SSH into server: `ssh satcom0@[ip address]` using password from previous step