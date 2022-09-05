FROM ubuntu:latest 

RUN apt-get update && apt-get install --yes python3

CMD mkdir /webserver
COPY logs/ /webserver/logs/
COPY index.html /webserver
COPY satellite.py /webserver

# RUN adduser admin
# USER admin
WORKDIR /webserver
ENTRYPOINT ["cd /webserver"]
