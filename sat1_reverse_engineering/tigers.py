import socket
import signal
import sys
import time
import ctypes
import threading
import requests
from client.tigershared import *

MAX_CONNECTIONS = 300
RECV_SIGNAL = False
SAT_SIGNAL = True


class sat_thread(threading.Thread):
    def __init__(self, url):
        threading.Thread.__init__(self)
        # satellite apiserver configuration
        self.url = url
        self.status = {
            "status1":"ok",
        }

    def run(self):
        global SAT_SIGNAL
        while (SAT_SIGNAL):
            x = requests.post(self.url, json = self.status)
            time.sleep(2)
        

class client_thread(threading.Thread):
    def __init__(self, conn, addr, passwords):
        threading.Thread.__init__(self)
        self.conn = conn
        self.addr = addr
        self.passwords = passwords
        self.file = None
        self.thread_active = True
        conn.settimeout(1)
        self.conn.sendall(create_message_packet("Connected to server"))

    def get_thread_status(self):
        return self.thread_active
             
    def run(self):
        global RECV_SIGNAL
        global SAT_SIGNAL
        next_packet = str.encode("") # used if multiple packets were concatenated
        missing_data = 0 # used if the last packet was not fully sent
        packet_type = 0
        # target function of the thread class
        try:
            while(not RECV_SIGNAL and self.thread_active):
                if (missing_data <= 0):
                    # if there is no missing data
                    if (len(next_packet) >= PACKET_HEADER_DATA_LEN):
                        # if the next packet is big enough to contain a full header
                        data = next_packet
                    else:
                        try:
                            data = self.conn.recv(BUF_LEN)
                            if (len(next_packet) > 0):
                                # next packet was not big enough to contain a full header, add it to the reception of new data
                                data = next_packet + data
                        except socket.timeout:
                            continue
                    if (len(data) == 0):
                        continue
                    packet_type, header_length, header_data, next_packet = parse_header(data)
                    # calculate any missing data
                    missing_data = header_length - len(header_data)
                else:
                    try:
                        data = self.conn.recv(missing_data) # only recv missing data
                    except socket.timeout:
                        continue
                    header_data = decode_data(data)
                    missing_data -= len(header_data)
                    if (missing_data < 0): # in case this next packet contains the end of current packet and a new packet
                        next_packet = data[-missing_data:] # double negative
                    else:
                        next_packet = str.encode("")
                if (packet_type == PACKET_TYPE_CONNECTION):
                    username, password = self.parse_username_password(header_data)
                    if (username not in self.passwords or self.passwords[username] != password):
                        # invalid username
                        self.conn.sendall(create_message_packet("Invalid username or password"))
                elif (packet_type == PACKET_TYPE_GET):
                    # load a file and send it over
                    self.conn.sendall(create_put_packet(header_data))
                    file = open(header_data, "r")
                    for line in file:
                        self.conn.sendall(create_transfer_packet(line))
                    self.conn.sendall(create_finish_transfer_packet())
                elif (packet_type == PACKET_TYPE_PUT):
                    # download file
                    self.file = open(header_data, "w")
                elif (packet_type == PACKET_TYPE_TRANSFER):
                    self.file.write(header_data)
                elif (packet_type == PACKET_TYPE_FINISH_TRANSFER):
                    self.file.close()
                    self.file = None
                elif (packet_type == PACKET_TYPE_EXIT):
                    # close this connection and stop this thread
                    self.clean()
                elif (packet_type == PACKET_TYPE_ABORT):
                    SAT_SIGNAL = False
                    self.conn.sendall(create_message_packet("Satellite connection aborted"))

        except Exception as e:
            print("An error occurred on client", self.addr, "cleaning up:", e)
            self.conn.sendall(create_message_packet(str(e)))
            self.clean()

    def parse_username_password(self, data):
        username = data.split(" ")[0]
        password = data.split(" ")[1]
        return username, password

    def clean(self):
        try:
            self.conn.sendall(create_exit_packet())
            time.sleep(1)
            self.conn.close()
            self._is_alive = False
        except:
            pass
        try:
            if (self.file is not None):
                self.file.close()
        except:
            pass
        self.thread_active = False

def load_passwords():
    file = open("passwords.txt", "r")
    passwords = dict()
    for line in file:
        username = line.split(":")[0].strip()
        password = line.split(":")[1].strip()
        passwords[username] = password
    file.close()
    return passwords

def serve_client(conn, client_addr):
    while (True):
        data = conn.recv(BUF_LEN)
        if not data:
            break
        print("Recv data:", data)
    conn.sendall(str.encode("End Connection"))

def handler(signum, frame):
    global RECV_SIGNAL
    ret = input("Do you really want to exit (y/n)? ")
    if ret == 'y':
        RECV_SIGNAL = True
        print("Exiting application...")
    else:
        print("Resuming application...")

def main():
    global SAT_SIGNAL
    # main loop, accept incoming connections and receive them on a new thread
    try:
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        sock.settimeout(1)
        sock.bind(('0.0.0.0', SERVER_PORT))
        sock.listen(MAX_CONNECTIONS)
    except Exception as e:
        print("Error creating socket:", e)
        sys.exit()

    print("Tiger FTP Server")

    # create a signal handler to cleanly close
    signal.signal(signal.SIGINT, handler)

    passwords = load_passwords()
    threads = list()

    sat = sat_thread("http://localhost:5001/status")
    sat.start()

    while (not RECV_SIGNAL):
        # create a new thread
        try:
            conn, client_addr = sock.accept()
            new_thread = client_thread(conn, client_addr, passwords)
            new_thread.start()
            threads.append(new_thread)
        except socket.timeout:
            pass

    print("Closing client threads")
    active_threads = 0
    for i in range(0, len(threads)):
        if (threads[i].get_thread_status()):
            active_threads += 1
            threads[i].clean()
        threads[i].join()
    print("Active clients closed:", active_threads)
    SAT_SIGNAL = False
    sat.join()
    print("Closed Sat Thread")
    sock.close()
    sys.exit()

main()