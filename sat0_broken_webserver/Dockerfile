
FROM ubuntu:latest 

RUN apt-get clean && apt-get update && apt-get install --yes python3 sudo
RUN apt-get install python3-pip --yes
RUN pip3 install requests

RUN apt-get install openssh-server --yes
# Add the ssh group and admin user
RUN groupadd sshgroup && useradd -ms /bin/bash -g sshgroup starcom0
# Add the user to sudoers group 
RUN usermod -aG sudo starcom0
# Set user password
RUN echo "starcom0:password" | chpasswd
# Make SSH directory
RUN mkdir -p /home/admin/.ssh
# If the below line fails, please see the README and generate an authorized key
COPY authorized_keys /home/starcom0/.ssh/authorized_keys
RUN chown starcom0:sshgroup /home/starcom0/.ssh/authorized_keys && chmod 600 /home/starcom0/.ssh/authorized_keys

CMD mkdir /webserver
COPY logs/ /webserver/logs/
COPY index.html /webserver/index.html
COPY satellite.py /webserver/satellite.py
COPY run.sh /webserver/run.sh

EXPOSE 5021
EXPOSE 8000
WORKDIR /webserver
ENTRYPOINT ["./run.sh"]
