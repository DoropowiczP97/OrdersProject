FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrdersProject.WebUI/OrdersProject.WebUI.csproj", "OrdersProject.WebUI/"]
RUN dotnet restore "OrdersProject.WebUI/OrdersProject.WebUI.csproj"
COPY . .
WORKDIR "/src/OrdersProject.WebUI"
RUN dotnet build "OrdersProject.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrdersProject.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrdersProject.WebUI.dll"]
