#!/bin/bash

echo "Starting SSH service on port 8022"
/etc/init.d/ssh restart
echo "Starting status service"
python3 satellite.py & # Run the status requests in the background
echo "Starting webserver on port 8000"
python3 -m http.server 8000 # Run the webserver in the foreground so as to keep the docker container alive
