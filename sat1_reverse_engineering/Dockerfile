FROM ubuntu:latest 

RUN apt-get update && apt-get install --yes python3
RUN apt-get install python3-pip --yes
RUN pip3 install requests

CMD mkdir /ftpserver/client
COPY tigers.py /ftpserver/tigers.py
COPY client/tigershared.py /ftpserver/client/tigershared.py
COPY passwords.txt /ftpserver/passwords.txt

# add login .env creds here
# RUN adduser admin
# USER admin
WORKDIR /ftpserver
ENTRYPOINT ["python3", "tigers.py"]
