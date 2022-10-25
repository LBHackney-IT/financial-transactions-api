# FROM mcr.microsoft.com/dotnet/sdk:6.0
# RUN apt-get update && apt-get -y install xvfb && apt-get -y install fontconfig && apt-get -y install libssl1.0-dev && apt-get -y install libx11-dev libx11-xcb-dev libxcb-icccm4-dev libxcb-image0-dev libxcb-keysyms1-dev libxcb-randr0-dev libxcb-render-util0-dev libxcb-render0-dev libxcb-shm0-dev libxcb-util0-dev libxcb-xfixes0-dev libxcb-xkb-dev libxcb1-dev libxfixes-dev libxrandr-dev libxrender-dev
# ENV DynamoDb_LocalMode='true'
# ENV Sns_LocalMode='true'

# ARG LBHPACKAGESTOKEN
# ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

# WORKDIR /app

# # Install dependencies
# # Copy csproj and restore as distinct layers
# COPY *.csproj ./
# RUN dotnet restore

# # Copy everything else and build
# COPY . ./
# RUN dotnet build -c Release -o out
# RUN dotnet publish -c Release -o out

# EXPOSE ${PORT:-3000}
# CMD ASPNETCORE_URLS=http://+:${PORT:-3000} dotnet ./out/FinancialTransactionsApi.dll




FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE ${PORT:-3000}

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN


WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["FinancialTransactionsApi/FinancialTransactionsApi.csproj", "FinancialTransactionsApi/"]
RUN dotnet restore "FinancialTransactionsApi/FinancialTransactionsApi.csproj"

COPY . .
WORKDIR "/src/FinancialTransactionsApi"
RUN dotnet build "FinancialTransactionsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialTransactionsApi.csproj" -r linux-x64 -c Release -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialTransactionsApi.dll"]
