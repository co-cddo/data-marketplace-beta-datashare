# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj files and restore dependencies
COPY ./Agrimetrics.DataShare.Api.Core/*.csproj ./Agrimetrics.DataShare.Api.Core/
COPY ./Agrimetrics.DataShare.Api.Dto/*.csproj ./Agrimetrics.DataShare.Api.Dto/
COPY ./Agrimetrics.DataShare.Api.Db/*.csproj ./Agrimetrics.DataShare.Api.Db/
COPY ./Agrimetrics.DataShare.Api.Logic/*.csproj ./Agrimetrics.DataShare.Api.Logic/
COPY ./Agrimetrics.DataShare.Api/*.csproj ./Agrimetrics.DataShare.Api/

RUN dotnet restore ./Agrimetrics.DataShare.Api.Core/*.csproj
RUN dotnet restore ./Agrimetrics.DataShare.Api.Dto/*.csproj
RUN dotnet restore ./Agrimetrics.DataShare.Api.Db/*.csproj
RUN dotnet restore ./Agrimetrics.DataShare.Api.Logic/*.csproj
RUN dotnet restore ./Agrimetrics.DataShare.Api/*.csproj

# Copy the source code for all projects
COPY ./Agrimetrics.DataShare.Api.Core/. ./Agrimetrics.DataShare.Api.Core/
COPY ./Agrimetrics.DataShare.Api.Dto/. ./Agrimetrics.DataShare.Api.Dto/
COPY ./Agrimetrics.DataShare.Api.Db/. ./Agrimetrics.DataShare.Api.Db/
COPY ./Agrimetrics.DataShare.Api.Logic/. ./Agrimetrics.DataShare.Api.Logic/
COPY ./Agrimetrics.DataShare.Api/. ./Agrimetrics.DataShare.Api/

# Build and publish the projects
RUN dotnet publish ./Agrimetrics.DataShare.Api.Core/*.csproj -c Release -o /app/publish/Agrimetrics.DataShare.Api.Core
RUN dotnet publish ./Agrimetrics.DataShare.Api.Dto/*.csproj -c Release -o /app/publish/Agrimetrics.DataShare.Api.Dto
RUN dotnet publish ./Agrimetrics.DataShare.Api.Db/*.csproj -c Release -o /app/publish/Agrimetrics.DataShare.Api.Db
RUN dotnet publish ./Agrimetrics.DataShare.Api.Logic/*.csproj -c Release -o /app/publish/Agrimetrics.DataShare.Api.Logic
RUN dotnet publish ./Agrimetrics.DataShare.Api/*.csproj -c Release -o /app/publish/Agrimetrics.DataShare.Api

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy published output from the build stage
COPY --from=build /app/publish/Agrimetrics.DataShare.Api.Core ./
COPY --from=build /app/publish/Agrimetrics.DataShare.Api.Dto ./
COPY --from=build /app/publish/Agrimetrics.DataShare.Api.Db ./
COPY --from=build /app/publish/Agrimetrics.DataShare.Api.Logic ./
COPY --from=build /app/publish/Agrimetrics.DataShare.Api ./

# Expose the port
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "Agrimetrics.DataShare.Api.dll"]
