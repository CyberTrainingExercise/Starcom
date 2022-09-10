#!/bin/bash

###
### Runs the flask app that acts as a middleman for all API requests
###

export FLASK_APP=status_server.py
export FLASK_ENV=development
flask run -h 0.0.0.0 -p 5001
