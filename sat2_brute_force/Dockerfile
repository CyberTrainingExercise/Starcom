FROM ubuntu:latest 

RUN apt-get clean && apt-get update && apt-get install --yes python3
RUN apt-get install python3-pip --yes
RUN pip3 install requests

RUN apt-get install openssh-server --yes
# Add the ssh group and admin user
RUN groupadd sshgroup && useradd -ms /bin/bash -g sshgroup admin
# Set user password
RUN usermod -p password admin
# Make SSH directory
RUN mkdir -p /home/admin/.ssh
# If the below line fails, please see the README and generate an authorized key
COPY authorized_keys /home/admin/.ssh/authorized_keys
RUN chown admin:sshgroup /home/admin/.ssh/authorized_keys && chmod 600 /home/admin/.ssh/authorized_keys

CMD mkdir /satellite
COPY satellite.py /satellite/satellite.py
COPY run.sh /satellite/run.sh

EXPOSE 5022
WORKDIR /satellite
ENTRYPOINT ["./run.sh"]