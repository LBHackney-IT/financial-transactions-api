FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# Install dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        zlib1g \
		libgdiplus \
        libc6-dev \
		ca-certificates \
		fontconfig \
		fontconfig-config \
		fonts-dejavu-core \
		libbsd0 \
		libexpat1 \
		libfontconfig1 \
		libfontenc1 \
		libfreetype6 \
		libjpeg62-turbo \
		libpng16-16 \
		libssl1.1 \
		libx11-6 \
		libx11-data \
		libxau6 \
		libxcb1 \
		libxdmcp6 \
		libxext6 \
		libxrender1 \
		ucf \
		x11-common \
		xfonts-75dpi \
		xfonts-base \
		xfonts-encodings \
		xfonts-utils \
     && rm -rf /var/lib/apt/lists/*
# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ENV DynamoDb_LocalMode='true'
ENV Sns_LocalMode='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./FinancialTransactionsApi.sln ./
COPY ./FinancialTransactionsApi/FinancialTransactionsApi.csproj ./FinancialTransactionsApi/
COPY ./FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj ./FinancialTransactionsApi.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./FinancialTransactionsApi/FinancialTransactionsApi.csproj
RUN dotnet restore ./FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out FinancialTransactionsApi.Tests/FinancialTransactionsApi.Tests.csproj

CMD dotnet test
