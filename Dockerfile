# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files (including .csproj) into the container
COPY *.csproj ./ 

# Restore project dependencies
RUN dotnet restore

# Copy the rest of the files and build the application
COPY . ./ 

# Publish the application to the 'out' directory
RUN dotnet publish -c Release -o /app/out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build container
COPY --from=build /app/out ./ 

# Expose the port that your application will run on
EXPOSE 5000

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Land.dll"]

# Optionally, you can set environment variables here if needed, e.g.:
# ENV GOOGLE_API_KEY=your-api-key-here
