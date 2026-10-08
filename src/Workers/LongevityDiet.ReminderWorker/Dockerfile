FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Workers/LongevityDiet.ReminderWorker/LongevityDiet.ReminderWorker.csproj"
RUN dotnet publish "src/Workers/LongevityDiet.ReminderWorker/LongevityDiet.ReminderWorker.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LongevityDiet.ReminderWorker.dll"]
