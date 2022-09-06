FROM ubuntu:latest 

RUN apt-get update && apt-get install --yes python3
RUN apt-get install python3-pip --yes
RUN pip3 install requests

CMD mkdir /satellite
COPY satellite.py /satellite/satellite.py

# add login .env creds here
# RUN adduser admin
# USER admin
WORKDIR /satellite
ENTRYPOINT ["python3", "satellite.py"]
