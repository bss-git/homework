#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Dialogs/Dialogs.csproj", "Dialogs/"]
RUN dotnet restore "Dialogs/Dialogs.csproj"
COPY . .
WORKDIR "/src/Dialogs"
RUN dotnet build "Dialogs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dialogs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dialogs.dll"]