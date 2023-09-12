FROM mcr.microsoft.com/dotnet/aspnet:7.0

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS "http://*:5000"

EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0

ARG Configuration=Release

WORKDIR /src

COPY "Dotnet.Homeworks.MainProject/Dotnet.Homeworks.MainProject.csproj" "Dotnet.Homeworks.MainProject/"
RUN dotnet restore "Dotnet.Homeworks.MainProject/Dotnet.Homeworks.MainProject.csproj"
COPY . .

RUN dotnet build "Dotnet.Homeworks.MainProject/Dotnet.Homeworks.MainProject.csproj" -c $Configuration -o /app
RUN dotnet publish "Dotnet.Homeworks.MainProject/Dotnet.Homeworks.MainProject.csproj" -c $Configuration -o /app

WORKDIR /app

ENTRYPOINT ["dotnet", "Dotnet.Homeworks.MainProject.dll"] 