FROM ubuntu:latest 

RUN apt-get update && apt-get install --yes python3
RUN apt-get install python3-pip --yes
RUN pip3 install requests

CMD mkdir /webserver
COPY logs/ /webserver/logs/
COPY index.html /webserver/index.html
COPY satellite.py /webserver/satellite.py
COPY run.sh /webserver/run.sh

# add login .env creds here
# RUN adduser admin
# USER admin
WORKDIR /webserver
ENTRYPOINT ["./run.sh"]
