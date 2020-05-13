rmdir /S /Q webapi
dotnet publish -c Release --runtime ubuntu.18.04-x64 --self-contained -o webapi
docker build -t webapi .
docker rm -f elasticsearch
docker run -p 8080:8080 --rm --name elasticsearch -d webapi