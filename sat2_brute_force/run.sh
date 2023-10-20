#!/bin/bash

echo "Starting SSH service on port 22"
/etc/init.d/ssh restart
echo "Starting status service"
python3 satellite.py