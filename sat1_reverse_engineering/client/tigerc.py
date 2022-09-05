import socket
import select
import signal
import sys
import time
import threading
import os
import datetime
from tigershared import *

SOCK = None
RECV_SIGNAL = False

class input_thread(threading.Thread):
    def __init__(self, args):
        threading.Thread.__init__(self)
        self.args = args
             
    def run(self):
        global SOCK
        global RECV_SIGNAL
        # target function of the thread class
        try:
            while(not RECV_SIGNAL):
                try:
                    user_input = ""
                    if (self.args != ""):
                        user_input = self.args
                        self.args = ""
                    else:
                        i, o, e = select.select([sys.stdin], [], [], 0.5)
                        if (i):
                            user_input = sys.stdin.readline().strip()
                        else:
                            # timeout
                            continue
                    if (not is_valid_input(user_input)):
                        print("Invalid input, try again")
                        continue
                    command = user_input.split(" ")[0]
                    data = user_input.split(" ")[1:]
                    packet = ""
                    if (command == "connect"):
                        try:
                            SOCK = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                            SOCK.connect((data[0], SERVER_PORT))
                            SOCK.settimeout(1)
                        except Exception as e:
                            print("Error creating socket:", e)
                        packet = create_connection_packet(data[1], data[2])
                        SOCK.sendall(packet)
                        continue
                    if (command == "exit"):
                        RECV_SIGNAL = True
                    if (SOCK is None):
                        print("Please connect to the server first")
                        continue
                    if (command == "get"):
                        packet = create_get_packet(data[0])
                        SOCK.sendall(packet)
                    elif (command == "put"):
                        packet = create_put_packet(data[0])
                        SOCK.sendall(packet)
                        # load a file and send it over
                        print("Sending file:", data[0])
                        file = open(data[0], "r")
                        for line in file:
                            packet = create_transfer_packet(line)
                            SOCK.sendall(packet)
                        SOCK.sendall(create_finish_transfer_packet())
                        print("File sent")
                    elif (command == "exit"):
                        packet = create_exit_packet()
                        SOCK.sendall(packet)
                    elif (command == "abort"):
                        packet = create_abort_packet()
                        SOCK.sendall(packet)
                except Exception as e:
                    print("An error occurred:", e)
        except Exception as e:
            print("An error occurred:", e)

def handler(signum, frame):
    print("Use the `exit` command to exit")

def main():
    global SOCK
    global RECV_SIGNAL
    
    print("Tiger FTP Client")
    
    args = ""
    if (len(sys.argv) > 1):
        for i in range(1, len(sys.argv)):
            args += sys.argv[i] + " "

    thread = input_thread(args[0:len(args) - 1])
    thread.start()

    # create a signal handler to cleanly close
    signal.signal(signal.SIGINT, handler)

    next_packet = str.encode("")
    packet_type = 0
    missing_data = 0

    # reception loop
    try:
        while(not RECV_SIGNAL):
            if (SOCK == None):
                time.sleep(0.5)
                continue
            if (missing_data <= 0):
                if (len(next_packet) >= PACKET_HEADER_DATA_LEN):
                    data = next_packet
                else:
                    try:
                        data = SOCK.recv(BUF_LEN)
                        if (len(next_packet) > 0):
                            data = next_packet + data
                    except socket.timeout as e:
                        continue
                packet_type, header_length, header_data, next_packet = parse_header(data)
                # calculate any missing data
                missing_data = header_length - len(header_data)
            else:
                try:
                    data = SOCK.recv(missing_data) # only recv missing data
                except socket.timeout:
                    continue
                header_data = decode_data(data)
                missing_data -= len(header_data)
                if (missing_data < 0): # in case this next packet contains the end of current packet and a new packet
                    next_packet = data[-missing_data:] # double negative
                else:
                    next_packet = str.encode("")
            if (packet_type == PACKET_TYPE_EXIT):
                print("Connection closed by server")
                SOCK.close()
                RECV_SIGNAL = True
                sys.exit()
            elif (packet_type == PACKET_TYPE_PUT):
                print("Downloading file:", header_data)
                file = open(header_data, "w")
            elif (packet_type == PACKET_TYPE_TRANSFER):
                # response to a get
                file.write(header_data)
            elif (packet_type == PACKET_TYPE_FINISH_TRANSFER):
                print("File had been downloaded")
                file.close()
                file = None
            elif (packet_type == PACKET_TYPE_MESSAGE):
                print(header_data)
    except Exception as e:
        print("Client closing:", e)
    if (SOCK is not None):
        SOCK.close()
    RECV_SIGNAL = True
    sys.exit()

main()