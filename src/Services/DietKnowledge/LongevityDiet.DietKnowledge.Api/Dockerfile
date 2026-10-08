FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Api/LongevityDiet.DietKnowledge.Api.csproj"
RUN dotnet publish "src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Api/LongevityDiet.DietKnowledge.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LongevityDiet.DietKnowledge.Api.dll"]
