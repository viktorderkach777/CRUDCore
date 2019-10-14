FROM microsoft/dotnet:sdk AS builder
WORKDIR /source
EXPOSE 80
#EXPOSE 443

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs
#RUN apk --no-cache add nodejs nodejs-npm

COPY *.csproj .
RUN dotnet restore

COPY ./ ./
RUN chmod +x ./migration.sh
CMD /bin/bash ./migration.sh
RUN dotnet publish "./CRUDCore.csproj" --output "./dist" --configuration Release --no-restore
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine

WORKDIR /app
COPY --from=builder /source/dist .
ENTRYPOINT ["dotnet", "CRUDCore.dll"]