#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SavingPhaseService/SavingPhaseService.csproj", "SavingPhaseService/"]
RUN dotnet restore "SavingPhaseService/SavingPhaseService.csproj"
COPY . .
WORKDIR "/src/SavingPhaseService"
RUN dotnet build "SavingPhaseService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SavingPhaseService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SavingPhaseService.dll"]