FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MultiSnake.csproj", "./"]
RUN dotnet restore "MultiSnake.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "MultiSnake.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MultiSnake.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MultiSnake.dll"]
