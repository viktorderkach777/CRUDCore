FROM microsoft/dotnet:2.2-sdk-alpine AS builder
WORKDIR /source
EXPOSE 80
#EXPOSE 443

RUN apk --no-cache add nodejs nodejs-npm

COPY *.csproj .
RUN dotnet restore
COPY ./ ./

RUN dotnet publish "./CRUDCore.csproj" --output "./dist" --configuration Release --no-restore
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine

WORKDIR /app
COPY --from=builder /source/dist .
ENTRYPOINT ["dotnet", "CRUDCore.dll"]