#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Benefit.API/Benefit.API.csproj", "Benefit.API/"]
COPY ["Benefit.Domain/Benefit.Domain.csproj", "Benefit.Domain/"]
COPY ["Toolkit/Toolkit.csproj", "Toolkit/"]
COPY ["Benefit.Service/Benefit.Service.csproj", "Benefit.Service/"]
RUN dotnet restore "Benefit.API/Benefit.API.csproj"
COPY . .
WORKDIR "/src/Benefit.API"
RUN dotnet build "Benefit.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Benefit.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Benefit.API.dll"]