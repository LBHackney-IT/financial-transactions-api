FROM mcr.microsoft.com/dotnet/sdk:6.0


# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG SONAR_TOKEN
ENV SONAR_TOKEN=$SONAR_TOKEN

ENV DynamoDb_LocalMode='true'
ENV Sns_LocalMode='true'

WORKDIR /app

# Enable SonarCloud
RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet sonarscanner begin /k:"LBHackney-IT_financial-transactions-api" /o:"lbhackney-it" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login="${SONAR_TOKEN}"

# Copy csproj and restore as distinct layers
COPY ./FinancialTransactionsApi.sln ./
COPY ./FinancialTransactionsApi/FinancialTransactionsApi.csproj ./FinancialTransactionsApi/
COPY ./FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj ./FinancialTransactionsApi.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./FinancialTransactionsApi/FinancialTransactionsApi.csproj
RUN dotnet restore ./FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out FinancialTransactionsApi/FinancialTransactionsApi.csproj
RUN dotnet build -c debug -o out FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj

CMD dotnet test FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj  --collect:"XPlat Code Coverage"

reportgenerator "-reports:*\TestResults\*\coverage.cobertura.xml" "-targetdir:sonarqubecoverage" "-reporttypes:SonarQube"

RUN dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"