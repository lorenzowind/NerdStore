#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./api gateways/NS.Buyer.AGW/NS.Buyer.AGW.csproj", "api gateways/NS.Buyer.AGW/"]
COPY ["./building blocks/NS.Core/NS.Core.csproj", "building blocks/NS.Core/"]
COPY ["./building blocks/NS.MessageBus/NS.MessageBus.csproj", "building blocks/NS.MessageBus/"]
COPY ["./building blocks/NS.WebAPI.Core/NS.WebAPI.Core.csproj", "building blocks/NS.WebAPI.Core/"]

RUN dotnet restore "api gateways/NS.Buyer.AGW/NS.Buyer.AGW.csproj"
COPY . .
WORKDIR "api gateways/NS.Buyer.AGW"
RUN dotnet build "NS.Buyer.AGW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NS.Buyer.AGW.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NS.Buyer.AGW.dll"]