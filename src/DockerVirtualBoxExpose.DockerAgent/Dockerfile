FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY src/DockerVirtualBoxExpose.DockerAgent/DockerVirtualBoxExpose.DockerAgent.csproj src/DockerVirtualBoxExpose.DockerAgent/
COPY src/DockerVirtualBoxExpose.Common/DockerVirtualBoxExpose.Common.csproj src/DockerVirtualBoxExpose.Common/
RUN dotnet restore src/DockerVirtualBoxExpose.DockerAgent/DockerVirtualBoxExpose.DockerAgent.csproj
COPY . .
WORKDIR /src/src/DockerVirtualBoxExpose.DockerAgent
RUN dotnet build DockerVirtualBoxExpose.DockerAgent.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DockerVirtualBoxExpose.DockerAgent.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DockerVirtualBoxExpose.DockerAgent.dll"]
