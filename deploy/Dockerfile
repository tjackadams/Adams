FROM ubuntu:20.04

ENV DEBIAN_FRONTEND noninteractive
ENV DEBCONF_NOWARNINGS="yes"

RUN apt-get update && \
    apt-get install -y --no-install-recommends apt-utils && \
    apt-get upgrade -y && \   
    apt-get install -y --no-install-recommends tzdata && \    
    apt-get update && \
    ln -fs /usr/share/zoneinfo/Europe/London /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
    apt-get update && \
    apt-get install -y --no-install-recommends curl \
    gnupg \
    ca-certificates \
    git \
    iputils-ping \
    jq \
    lsb-release \
    software-properties-common \
    openssh-client \
    unzip \
    apt-transport-https && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list | tee /etc/apt/sources.list.d/msprod.list && \
    apt-get update && \
    apt-get install -y --no-install-recommends mssql-tools \
    unixodbc-dev \
    wget && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists

RUN curl -fsSL https://get.docker.com -o get-docker.sh

RUN sh get-docker.sh

RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# Can be 'linux-x64', 'linux-arm64', 'linux-arm', 'rhel.6-x64'.
ENV TARGETARCH=linux-x64

WORKDIR /azp

COPY ./start.sh .
RUN chmod +x start.sh

ENTRYPOINT [ "./start.sh" ]