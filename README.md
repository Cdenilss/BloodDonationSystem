

# 🩸 BloodDonationSystem - Sistema de Gestão de Doação de Sangue

![.NET](https://img.shields.io/badge/.NET-8-blueviolet)
![C#](https://img.shields.io/badge/C%23-11-green)
![Arquitetura](https://img.shields.io/badge/Arquitetura-Limpa-orange)
![Padrão](https://img.shields.io/badge/Padrão-CQRS%20%7C%20Mediator-blue)

## 🎯 Sobre o Projeto

**BloodDonationSystem** é uma API RESTful desenvolvida em .NET 8 como parte de um treinamento prático, com o objetivo de criar um sistema robusto para gerenciar todo o ciclo de doação de sangue. A aplicação facilita o cadastro e a gestão de doadores, o controle de estoque de bolsas de sangue e o registro de doações, aplicando regras de negócio essenciais para garantir a segurança e a conformidade do processo.

Este projeto é um estudo de caso prático da aplicação de **Clean Architecture**, **Domain-Driven Design (DDD)** e **CQRS** no ecossistema .NET.

## ✨ Funcionalidades Principais

* **Gestão de Doadores:**
    * CRUD completo para doadores.
    * Validação de dados de entrada na criação e atualização de doadores.
    * Integração com a API [ViaCEP](https://viacep.com.br/) para preenchimento automático de endereço.
* **Registro de Doações:**
    * Endpoint para registrar novas doações.
    * Validações de regras de negócio complexas (idade mínima, intervalo entre doações baseado no gênero).
* **Controle de Estoque de Sangue:**
    * (Em desenvolvimento) Gestão de bolsas de sangue por tipo sanguíneo e fator Rh.
* **Arquitetura Robusta:**
    * Pipeline de validação customizado com FluentValidation e Mediator.
    * Middleware global para tratamento de exceções, retornando respostas de erro padronizadas.

## 🏛️ Arquitetura e Padrões de Design

O projeto foi estruturado seguindo os princípios da **Clean Architecture**, dividindo as responsabilidades em quatro projetos principais:

* **`Core`**: Contém as entidades de domínio, enums, e as interfaces dos repositórios. É o coração da aplicação, sem dependências externas.
* **`Application`**: Orquestra os casos de uso. Contém os Comandos, Queries (CQRS), Handlers, DTOs e a lógica de validação.
* **`Infrastructure`**: Implementa as interfaces definidas no Core. É responsável pela persistência de dados (Entity Framework Core), consumo de serviços externos e outros detalhes de infraestrutura.
* **`API`**: Expõe a aplicação para o mundo exterior através de uma API REST, utilizando Controllers para receber as requisições e o Mediator para encaminhá-las.

### Padrões Implementados:

* **CQRS (Command Query Responsibility Segregation):** Usamos um Mediator customizado para separar claramente os comandos (operações de escrita) das queries (operações de leitura).
* **Mediator Pattern:** Para desacoplar o envio de requisições da sua execução, evitando a dependência direta entre Controllers e Handlers.
* **Repository Pattern:** Para abstrair a camada de acesso a dados.
* **Dependency Injection (DI):** Utilizado extensivamente em todo o projeto para gerenciar as dependências de forma limpa.
* **Middleware:** Para tratar de responsabilidades transversais, como o tratamento global de exceções.

## 🛠️ Tecnologias Utilizadas

* **Backend:**
    * .NET 8
    * ASP.NET Core
    * Entity Framework Core 8
* **Banco de Dados:**
    * SQL Server (configurado para uso local)
* **Validação:**
    * FluentValidation
* **Testes (em breve):**
    * xUnit
    * Moq

## 🚀 Como Executar o Projeto Localmente

Siga os passos abaixo para configurar e rodar o projeto na sua máquina.

### Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
* Um servidor SQL Server (pode ser via Docker ou instalado localmente).

### Passos

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/SEU-USUARIO/BloodDonationSystem.git](https://github.com/SEU-USUARIO/BloodDonationSystem.git)
    cd BloodDonationSystem
    ```

2.  **Configure a Connection String:**
    Abra o arquivo `BloodDonationSystem.API/appsettings.Development.json` e altere a `DefaultConnection` para apontar para o seu servidor SQL Server.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=SEU_SERVIDOR;Database=BloodDonationDb;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True"
    }
    ```

3.  **Aplique as Migrations:**
    Navegue até a pasta da API e execute o comando abaixo para criar o banco de dados e aplicar as tabelas.
    ```bash
    cd BloodDonationSystem.API
    dotnet ef database update
    ```

4.  **Execute a Aplicação:**
    Ainda no diretório `BloodDonationSystem.API`, execute o comando:
    ```bash
    dotnet run
    ```

5.  **Acesse a Documentação da API:**
    Com a aplicação rodando, acesse `http://localhost:PORTA/swagger` no seu navegador para ver e testar os endpoints disponíveis.

## 🗺️ Roadmap Futuro

Este projeto está em constante evolução. Os próximos passos planejados são:

* [ ] Implementar o padrão **Unit of Work** para garantir transações atômicas.
* [ ] Criar **Hosted Services** para executar tarefas em background (ex: alertas de estoque baixo).
* [ ] Utilizar **Domain Events** para desacoplar os módulos do sistema.
* [ ] Desenvolver uma suíte completa de **Testes Unitários e de Integração**.
* [ ] Implementar **autenticação e autorização** com JWT
      
