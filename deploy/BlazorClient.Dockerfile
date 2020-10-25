FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build-env
WORKDIR /app

COPY . .
RUN dotnet restore "src/IdentityService.BlazorClient/IdentityService.BlazorClient.csproj"

COPY . .
RUN dotnet build "src/IdentityService.BlazorClient/IdentityService.BlazorClient.csproj" -c Release -o /build

FROM build-env AS publish
RUN dotnet publish "src/IdentityService.BlazorClient/IdentityService.BlazorClient.csproj" -c Release -o /publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
COPY src/IdentityService.BlazorClient/nginx.conf /etc/nginx/nginx.conf