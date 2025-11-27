FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY ../AuthenticationServices/AuthenticationServices.csproj ./AuthenticationServices/
RUN dotnet restore ./AuthenticationServices
COPY ./AuthenticationServices/ ./AuthenticationServices/
RUN dotnet publish ./AuthenticationServices -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 5051
ENTRYPOINT ["dotnet", "AuthenticationServices.dll"]
