import requests
import time

# satellite apiserver configuration
# this will need to be updated with a new ip address
url = "http://status:5001/status"
status = {
    "status0":"ok",
}

while (True):
    x = requests.post(url, json = status)
    time.sleep(2)