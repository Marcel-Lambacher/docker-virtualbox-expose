FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY DockerVirtualBoxExpose.DockerAgent/DockerVirtualBoxExpose.DockerAgent.csproj DockerVirtualBoxExpose.DockerAgent/
COPY DockerVirtualBoxExpose.Common/DockerVirtualBoxExpose.Common.csproj DockerVirtualBoxExpose.Common/
RUN dotnet restore DockerVirtualBoxExpose.DockerAgent/DockerVirtualBoxExpose.DockerAgent.csproj
COPY . .
WORKDIR /src/DockerVirtualBoxExpose.DockerAgent
RUN dotnet build DockerVirtualBoxExpose.DockerAgent.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DockerVirtualBoxExpose.DockerAgent.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DockerVirtualBoxExpose.DockerAgent.dll"]
