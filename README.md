# 🩸 BloodDonationSystem

**BloodDonationSystem** é uma API RESTful desenvolvida em **.NET 8** como parte de um estudo prático de **Clean Architecture**, **DDD** e **CQRS**.  
O objetivo é fornecer um sistema robusto para gerenciar **todo o ciclo de doação de sangue** — desde o cadastro de doadores, passando pelo registro de doações, até o controle de estoque por tipo sanguíneo e fator Rh.  

Mais do que código, este projeto serviu como **laboratório de boas práticas de arquitetura** e aplicação de padrões avançados no ecossistema .NET.  

---

## ✨ Funcionalidades Principais

### 👤 Gestão de Doadores
- CRUD completo para doadores.  
- Validação de dados com **FluentValidation**.  
- Integração com a **API ViaCEP** para preenchimento automático de endereço.  

### 🩸 Registro de Doações
- Registro de novas doações associadas ao doador.  
- **Regras de negócio implementadas**:
  - Idade mínima do doador.  
  - Intervalo entre doações (60 dias para homens, 90 dias para mulheres).  
  - Peso mínimo de 50 kg.  
- Atualização automática do estoque ao registrar doação.  

### 🏪 Controle de Estoque de Sangue
- Gestão de estoque por tipo sanguíneo + fator Rh.  
- Operação de **saída de bolsas de sangue** (`Draw`), validada por:
  - Quantidade positiva (1–500ml).  
  - Estoque suficiente.  
- Disparo de **Domain Events** (`BloodStockBecameLowEvent`) quando o estoque cruza o limite mínimo seguro.  

### 🔧 Arquitetura Robusta
- **Mediator customizado** para aplicar CQRS sem dependências externas.  
- **Pipeline Behavior + FluentValidation** → validação automática e centralizada.  
- **Unit of Work** para garantir transações atômicas.  
- **Domain Events** para desacoplar notificações de baixo estoque.  
- Middleware global para tratamento de exceções com respostas padronizadas.  

---

## 🏛️ Arquitetura e Padrões de Design

O projeto segue **Clean Architecture**, dividido em quatro camadas:  

- **Core** → Entidades de domínio, enums, eventos e contratos de repositórios.  
- **Application** → Casos de uso (Commands, Queries, Handlers), DTOs, validações e orquestração da lógica de negócio.  
- **Infrastructure** → Implementação de repositórios (EF Core), serviços externos (ViaCEP) e persistência.  
- **API** → Exposição dos endpoints REST usando Controllers e integração com o Mediator.  

### Padrões implementados
- **CQRS**: separação clara entre leitura e escrita.  
- **Mediator Pattern**: roteamento de comandos e queries sem acoplamento.  
- **Repository Pattern**: abstração de acesso a dados.  
- **Unit of Work**: coordenação de transações atômicas.  
- **Domain Events**: notificação de regras críticas de negócio (estoque baixo).  
- **Dependency Injection**: usada em toda a aplicação.  
- **Middleware**: tratamento global de exceções.  

---

## 🧪 Testes Automatizados

O projeto conta com uma **suíte robusta de testes unitários**, implementados com:  

- **xUnit** → framework de testes.  
- **FluentAssertions** → asserts legíveis e expressivos.  
- **Moq** → mocks para repositórios e serviços externos.  
- **Bogus** → geração de dados consistentes e determinísticos.  

🔍 Cobertura principal:
- **Domínio**: `Donor.Update`, `BloodStock.Draw` (invariantes e Domain Events).  
- **Validators**: `CreateDonorValidator`, `CreateDonationValidator`, `BloodStockDrawValidator`.  
- **Handlers**: `CreateDonorHandler`, `DonorPutCommandHandler`, `CreateDonationCommandHandler`, `OutputBloodStockCommandHandler`.  
- **Infra**: integração com **ViaCEP** mockada em testes.  

---

## 🛠️ Tecnologias Utilizadas

**Backend**  
- .NET 8  
- ASP.NET Core  
- Entity Framework Core 8  

**Banco de Dados**  
- SQL Server (configuração local ou Docker)  

**Validação**  
- FluentValidation  

**Testes**  
- xUnit  
- Moq  
- FluentAssertions  
- Bogus  

---

## 🚀 Como Executar o Projeto Localmente

### Pré-requisitos
- .NET 8 SDK  
- SQL Server (local ou via Docker)  
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

## ✅ Status

📌 **Projeto finalizado como estudo prático**:  
- [x] Aplicação de arquitetura limpa, DDD e CQRS  
- [x] Regras de negócio encapsuladas no domínio  
- [x] Integração com serviço externo (ViaCEP)  
- [x] Testes cobrindo cenários principais  

👨‍💻 Aberto para feedbacks, sugestões e contribuições!
