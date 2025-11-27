
# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ../ApiGateway/ApiGateway.csproj ./ApiGateway/
RUN dotnet restore ./ApiGateway
COPY ./ApiGateway/ ./ApiGateway/
RUN dotnet publish ./ApiGateway -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose port 5002
EXPOSE 5002

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
