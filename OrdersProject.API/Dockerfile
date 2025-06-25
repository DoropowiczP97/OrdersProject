FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrdersProject.API/OrdersProject.API.csproj", "OrdersProject.API/"]
COPY ["OrdersProject.Application/OrdersProject.Application.csproj", "OrdersProject.Application/"]
COPY ["OrdersProject.Infrastructure/OrdersProject.Infrastructure.csproj", "OrdersProject.Infrastructure/"]
COPY ["OrdersProject.Domain/OrdersProject.Domain.csproj", "OrdersProject.Domain/"]
RUN dotnet restore "OrdersProject.API/OrdersProject.API.csproj"
COPY . .
WORKDIR "/src/OrdersProject.API"
RUN dotnet build "OrdersProject.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrdersProject.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "OrdersProject.API.dll"]
