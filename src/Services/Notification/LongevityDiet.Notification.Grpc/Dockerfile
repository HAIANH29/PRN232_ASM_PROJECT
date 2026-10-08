FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Services/Notification/LongevityDiet.Notification.Grpc/LongevityDiet.Notification.Grpc.csproj"
RUN dotnet publish "src/Services/Notification/LongevityDiet.Notification.Grpc/LongevityDiet.Notification.Grpc.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LongevityDiet.Notification.Grpc.dll"]
