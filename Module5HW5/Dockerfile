#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Module5HW5/Module5HW5.csproj", "Module5HW5/"]
RUN dotnet restore "Module5HW5/Module5HW5.csproj"
COPY . .
WORKDIR "/src/Module5HW5"
RUN dotnet build "Module5HW5.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Module5HW5.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Module5HW5.dll"]