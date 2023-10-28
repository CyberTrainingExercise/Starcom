import socket

# Create a TCP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Connect to the server
sock.connect(('localhost', 7777))

# Loop receiving information from the server and printing it
while True:
    # Receive data from the server
    data = sock.recv(1024)

    # If the data is empty, the server has closed the connection
    if not data:
        break

    # Print the data to the console
    print(data.decode())

    # Prompt the user for a response
    response = input('> ')

    # Send the response to the server
    sock.send(response.encode())

# Close the socket
sock.close()
