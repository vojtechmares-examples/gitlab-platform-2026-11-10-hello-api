# ASP.NET Core runtime image from Microsoft's registry. The pipeline publishes first (dotnet publish -> publish/), so the
# code that was tested is the code that ships.
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app
COPY publish/ .

ARG BUILD_ID=local
ENV APP_BUILD=${BUILD_ID} \
    ASPNETCORE_URLS=http://+:8080
LABEL org.opencontainers.image.revision="${BUILD_ID}"

# The image ships a non-root user named "app"
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "App.dll"]
