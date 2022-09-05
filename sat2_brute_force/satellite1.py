import requests
import time

# satellite apiserver configuration
url = "http://localhost:5001/status"
status = {
    "status1":"ok",
}

while (True):
    x = requests.post(url, json = status)
    time.sleep(2)
