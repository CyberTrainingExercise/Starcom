#!/bin/sh
export FLASK_APP=apiserver.py
export FLASK_ENV=development
flask run -h 0.0.0.0 -p 5001
