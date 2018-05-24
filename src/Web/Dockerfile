FROM ubuntu:16.04

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Prevents restore from extracting all the XML files
# This reduces the size of the image
ENV NUGET_XMLDOC_MODE skip

ENV ASPNETCORE_URLS http://*:5106

# Get Bluestar dependencies and dependencies to fetch .NET Core
RUN export DEBIAN_FRONTEND='noninteractive' && \
    apt-get update -q && \
    apt-get install -qy \
        apt-utils \
        apt-transport-https \
        libboost-all-dev \
        swi-prolog-nox \
        libaio1 \
        libstdc++6 \
        && \
    apt-get install -qy software-properties-common && \
    add-apt-repository ppa:ubuntu-toolchain-r/test && \
    apt-get update -q && \
    apt-get install gcc-4.9 -qy && \
    apt-get upgrade libstdc++6 -qy

# Install the .NET Core SDK
RUN echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list && \
    apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893 && \
    apt-get update -q && \
    apt-get install -qy dotnet-sdk-2.1.105

# Trigger population of the local package cache
RUN mkdir warmup && \
    cd warmup && \
    dotnet new && \
    cd .. && \
    rm -rf warmup && \
    rm -rf /tmp/NuGetScratch

RUN mkdir app
COPY . ./app

WORKDIR ./app

RUN dotnet publish -c Release -o out

ENV StarcounterBin ./runtimes/ubuntu.16.04-x64/native

WORKDIR ./out
ENTRYPOINT ["dotnet", "Web.dll", "--server.urls", "http://*:5106"]
