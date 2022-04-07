import random
import datetime
import os

PASSWORD = "d12j*Lo&1" # password for satcom0 server

def generate_logs():
    file = open("output.log", "w")
    log_level = ["info"] * 50 + ["warning", "error"]
    action = ["login", "request", "update", "view", "request"]
    time = datetime.datetime.now()
    for i in range(0, 1000):
        time = time + datetime.timedelta(seconds=random.randint(1, 100), milliseconds=random.randint(1, 1000))
        file.write(str(time) + ":logger:/home/satcom0/logs/:" + log_level[random.randint(0, len(log_level) - 1)] + \
            ":" + action[random.randint(0, len(action) - 1)] + ":" + str(os.urandom(random.randint(0, 50))) + "\n")
        if (i == 110):
            file.write(str(time) + ":logger:/home/satcom0/logs/:error:passwordchanged:b'" + "password123" + "'\n")
        if (i == 352):
            file.write(str(time) + ":logger:/home/satcom0/logs/:error:passwordchanged:b'" + "firehouse" + "'\n")
        if (i == 730):
            file.write(str(time) + ":logger:/home/satcom0/logs/:error:passwordchanged:b'" + PASSWORD + "'\n")

generate_logs()