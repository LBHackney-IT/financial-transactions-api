FROM mcr.microsoft.com/dotnet/core/sdk:3.1



ENV DynamoDb_LocalMode='true'
ENV Sns_LocalMode='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

WORKDIR /app

# Install dependencies
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
RUN yum update -y
RUN  yum install -y yum-utils rpmdevtools
RUN yum install libxrender
# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet build -c Release -o out
RUN dotnet publish -c Release -o out

EXPOSE ${PORT:-3000}
CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/FinancialTransactionsApi.dll
