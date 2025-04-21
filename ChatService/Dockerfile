FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["./ChatService/ChatService.csproj", "ChatService/"]
COPY ["./Shared/Shared.csproj", "Shared/"]

# TODO: find safer approach
COPY ./dockerized.secrets.json /app/secrets.json

RUN dotnet restore "ChatService/ChatService.csproj"
COPY . .
WORKDIR "/src/ChatService"
RUN dotnet publish "ChatService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM build AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /app/secrets.json /app/secrets.json

ENTRYPOINT ["dotnet", "ChatService.dll"]
