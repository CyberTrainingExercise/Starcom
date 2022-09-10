#!/bin/bash

python3 satellite.py & # Run the status requests in the background
python3 -m http.server 8000 # Run the webserver in the foreground so as to keep the docker container alive
