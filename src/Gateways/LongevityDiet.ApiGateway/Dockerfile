FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Gateways/LongevityDiet.ApiGateway/LongevityDiet.ApiGateway.csproj"
RUN dotnet publish "src/Gateways/LongevityDiet.ApiGateway/LongevityDiet.ApiGateway.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LongevityDiet.ApiGateway.dll"]
