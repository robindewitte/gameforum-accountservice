#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["fictivus_accountservice/fictivus_accountservice.csproj", "fictivus_accountservice/"]
RUN dotnet restore "fictivus_accountservice/fictivus_accountservice.csproj"
COPY . .
WORKDIR "/src/fictivus_accountservice"
RUN dotnet build "fictivus_accountservice/fictivus_accountservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "fictivus_accountservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fictivus_accountservice.dll"]
