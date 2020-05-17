FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Easynvest.Desafio.Investimentos.Api/Easynvest.Desafio.Investimentos.Api.csproj", "Easynvest.Desafio.Investimentos.Api/"]
RUN dotnet restore "Easynvest.Desafio.Investimentos.Api/Easynvest.Desafio.Investimentos.Api.csproj"
COPY . .
WORKDIR "/src/Easynvest.Desafio.Investimentos.Api"
RUN dotnet build "Easynvest.Desafio.Investimentos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Easynvest.Desafio.Investimentos.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Easynvest.Desafio.Investimentos.Api.dll"]