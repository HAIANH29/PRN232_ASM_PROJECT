FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Workers/LongevityDiet.NotificationWorker/LongevityDiet.NotificationWorker.csproj"
RUN dotnet publish "src/Workers/LongevityDiet.NotificationWorker/LongevityDiet.NotificationWorker.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LongevityDiet.NotificationWorker.dll"]
