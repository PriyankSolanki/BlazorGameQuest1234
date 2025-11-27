FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY SharedModels/SharedModels.csproj ./SharedModels/
COPY ../GameServices/GameServices.csproj ./GameServices/
RUN dotnet restore ./GameServices
COPY SharedModels/ ./SharedModels/
COPY ./GameServices/ ./GameServices/
RUN dotnet publish ./GameServices -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 5051
ENTRYPOINT ["dotnet", "GameServices.dll"]
