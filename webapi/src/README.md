# Documentação para Projeto .NET 8

## 1. Instalação do SDK .NET 8

### Windows
1. Acesse o site oficial do .NET: [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
2. Baixe e instale o SDK do .NET 8 para Windows.
3. Verifique a instalação executando no terminal:
   ```sh
   dotnet --version
   ```

### Linux (Ubuntu/Debian)
```sh
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
export PATH=$HOME/.dotnet:$PATH
```
Verifique a instalação:
```sh
dotnet --version
```

### macOS
```sh
brew install dotnet-sdk@8
```
Verifique a instalação:
```sh
dotnet --version
```

## 2. Criando um novo projeto
Para criar um novo projeto ASP.NET Core Web API:
```sh
dotnet new webapi -n webapi
cd webapi
```

### Estrutura do Projeto
```
.
├── Controllers/*
├── Database/*
├── Extensions/*
├── Helpers/*
├── Models/*
├── Repositories/*
├── appsettings.Development.json
├── appsettings.json
├── application.db
├── Program.cs
├── README.md
├── webapi.csproj
└── webapi.sln
```

## 3. Executando o Servidor
### Com `dotnet run`
```sh
dotnet run
```
A aplicação será iniciada e você verá uma saída como:
```
Now listening on: http://localhost:5000
Now listening on: https://localhost:5001
```

### Com `dotnet watch`
Para recarregar automaticamente ao detectar alterações:
```sh
dotnet watch run
```

## 4. Testando a API
Para testar a API, use o **cURL** ou o **Postman**:
```sh
curl -X GET https://localhost:5000/api/v1/
```

## 5. Problemas com banco de dados
Para resolver problemas com banco de dados, recrie o arquivo `application.db`.
Remova totalmente os arquivos `*.db` na raiz do projeto.
Execute os comandos do Entity Framework para recriar o banco.
```sh
dotnet ef migrations remove
dotnet ef migrations add M01-InitialMigration
dotnet ef database update
```