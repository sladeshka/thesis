FROM ubuntu:latest
WORKDIR /app
RUN apt-get update && apt-get install -y \
    g++ \
    libcpprest-dev \
    libboost-all-dev \
    libssl-dev \
    cmake
COPY . .
RUN g++ -o app main.cpp -lboost_system -lboost_thread -lboost_random -lssl -lcrypto
EXPOSE 8080
CMD ["./app"]