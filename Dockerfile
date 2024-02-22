# Use the official Microsoft .NET Core SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# # In practice, built in Azure, this step generally not 
# # part of Dockerfile
# COPY *.csproj .
# RUN dotnet restore

# Copy the project files and build the release
COPY . ./
RUN dotnet publish -c Release -o out

# Generate the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose port 80 for the application
EXPOSE 5246

# Start the application
ENTRYPOINT ["dotnet", "Api.dll"]
