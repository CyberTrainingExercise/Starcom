import socket
import signal
import sys
import time
import threading
import os
import datetime

SERVER_PORT = 7777
BUF_LEN = 1024
PACKET_HEADER_DATA_LEN = 4
PACKET_TYPE_CONNECTION = 1
PACKET_TYPE_GET = 2
PACKET_TYPE_PUT = 3
PACKET_TYPE_EXIT = 4
PACKET_TYPE_TRANSFER = 5
PACKET_TYPE_FINISH_TRANSFER = 6
PACKET_TYPE_MESSAGE = 7
PACKET_TYPE_ABORT = 8

def parse_header(data):
    data = data.decode("UTF-8")
    packet_type = int(data[0])
    header_length = int(data[1:PACKET_HEADER_DATA_LEN])
    header_data = data[PACKET_HEADER_DATA_LEN:PACKET_HEADER_DATA_LEN + header_length]
    next_packet = data[PACKET_HEADER_DATA_LEN + header_length:]
    return packet_type, header_length, header_data, str.encode(next_packet)

def decode_data(data):
    return data.decode("UTF-8")

def is_valid_input(user_input):
    return True

def is_valid_packet(packet):
    return True

def create_connection_packet(username, password):
    packet_length = str(len(username) + len(password) + 1)
    packet_length = packet_length + " " * (PACKET_HEADER_DATA_LEN - len(packet_length) - 1)
    packet = str(PACKET_TYPE_CONNECTION) + packet_length + username + " " + password
    return str.encode(packet)

def create_get_packet(filename):
    packet_length = str(len(filename))
    packet_length = packet_length + " " * (PACKET_HEADER_DATA_LEN - len(packet_length) - 1)
    packet = str(PACKET_TYPE_GET) + packet_length + filename
    return str.encode(packet)

def create_put_packet(filename):
    packet_length = str(len(filename))
    packet_length = packet_length + " " * (PACKET_HEADER_DATA_LEN - len(packet_length) - 1)
    packet = str(PACKET_TYPE_PUT) + packet_length + filename
    return str.encode(packet)

def create_exit_packet():
    return str.encode(str(PACKET_TYPE_EXIT) + "0" * (PACKET_HEADER_DATA_LEN - 1))

def create_transfer_packet(data):
    packet_length = str(len(data))
    packet_length = packet_length + " " * (PACKET_HEADER_DATA_LEN - len(packet_length) - 1)
    packet = str(PACKET_TYPE_TRANSFER) + packet_length + data
    return str.encode(packet)

def create_finish_transfer_packet():
    return str.encode(str(PACKET_TYPE_FINISH_TRANSFER) + "0" * (PACKET_HEADER_DATA_LEN - 1))

def create_message_packet(message):
    packet_length = str(len(message))
    packet_length = packet_length + " " * (PACKET_HEADER_DATA_LEN - len(packet_length) - 1)
    packet = str(PACKET_TYPE_MESSAGE) + packet_length + message
    return str.encode(packet)