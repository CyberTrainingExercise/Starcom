FROM ubuntu:latest 

RUN apt-get update && apt-get install --yes python3
RUN apt-get install python3-pip --yes
RUN pip3 install requests flask zipp

CMD mkdir /server
COPY status_server.py /server/status_server.py
COPY run_status_server.sh /server/run_status_server.sh
CMD chmod +x /server/run_status_server.sh

WORKDIR /server
ENTRYPOINT ["./run_status_server.sh"]
