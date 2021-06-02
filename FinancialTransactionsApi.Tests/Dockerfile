FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./FinancialTransactionsApi.sln ./
COPY ./FinancialTransactionsApi/FinancialTransactionsApi.csproj ./FinancialTransactionsApi/
COPY ./FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj ./FinancialTransactionsApi.Tests/

RUN dotnet restore ./FinancialTransactionsApi/FinancialTransactionsApi.csproj
RUN dotnet restore ./FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj

CMD dotnet test
