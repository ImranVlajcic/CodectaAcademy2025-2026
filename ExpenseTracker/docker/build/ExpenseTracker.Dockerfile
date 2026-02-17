FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files first for better layer caching
COPY ExpenseTracker/src/ExpenseTracker.WebApi/ExpenseTracker.WebApi.csproj ExpenseTracker/src/ExpenseTracker.WebApi/
COPY ExpenseTracker/src/ExpenseTracker.Application/ExpenseTracker.Application.csproj ExpenseTracker/src/ExpenseTracker.Application/
COPY ExpenseTracker/src/ExpenseTracker.Domain/ExpenseTracker.Domain.csproj ExpenseTracker/src/ExpenseTracker.Domain/
COPY ExpenseTracker/src/ExpenseTracker.Contracts/ExpenseTracker.Contracts.csproj ExpenseTracker/src/ExpenseTracker.Contracts/
COPY ExpenseTracker/src/ExpenseTracker.Infrastructure/ExpenseTracker.Infrastructure.csproj ExpenseTracker/src/ExpenseTracker.Infrastructure/

# Restore dependencies
RUN dotnet restore ExpenseTracker/src/ExpenseTracker.WebApi/ExpenseTracker.WebApi.csproj

# Copy all source code
COPY ExpenseTracker/src/ ExpenseTracker/src/

# Build and publish
WORKDIR /src/ExpenseTracker/src/ExpenseTracker.WebApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ExpenseTracker.WebApi.dll"]

