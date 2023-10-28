import requests
import time

# satellite apiserver configuration
url = "http://status:5001/status"
status = {
    "status2":"OPERATIONAL",
}

while (True):
    x = requests.post(url, json = status)
    time.sleep(2)
