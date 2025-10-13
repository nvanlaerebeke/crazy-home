FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ./src ./
RUN dotnet publish -c Release -o /build -r linux-x64  /p:DebugSymbols=false /p:DebugType=None "Home/Home.csproj"

# Build runtime image
FROM  mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080

COPY --from=build-env /build .
ENTRYPOINT ["dotnet", "/app/Home.dll"]
