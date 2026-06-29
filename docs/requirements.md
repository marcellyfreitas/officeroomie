# OfficeRoomie — Documento de Requisitos

## 1. Introdução

### 1.1 Propósito
OfficeRoomie é um sistema de gerenciamento de reserva de salas corporativas. Permite que clientes visualizem um catálogo público de salas, façam reservas e acompanhem suas solicitações. Administradores gerenciam salas, clientes, cartões, reservas e outros administradores por meio de um dashboard protegido.

### 1.2 Tecnologias
| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET | 8.0 | Runtime e framework web |
| ASP.NET Core MVC | 8.0 | Arquitetura MVC com Razor Pages |
| Entity Framework Core | 8.0.10 | ORM e acesso a dados |
| BCrypt.Net-Next | 4.0.3 | Hash de senhas |
| Bootstrap | 5 | Interface de usuário responsiva |
| jQuery / jQuery Validation | — | Validação client-side |
| SQLite / MySQL / SQL Server | — | Provedores de banco de dados |
| Docker / Docker Compose | — | Conteinerização |

### 1.3 Público-alvo
- **Administradores corporativos** — gerenciam salas, reservas, clientes e cartões
- **Clientes / Funcionários** — realizam reservas de salas e consultam status

---

## 2. Requisitos Funcionais (RF)

### 2.1 Módulo de Autenticação

**RF01 — Login de Administrador**
O sistema deve permitir que um administrador faça login informando e-mail e senha. A senha deve ser verificada contra o hash BCrypt armazenado no banco. Em caso de sucesso, o sistema deve criar uma sessão via cookie de autenticação e redirecionar para o dashboard. Em caso de falha, deve exibir mensagem de erro na própria tela de login.
- **Entrada:** e-mail (string, max 100, formato email), senha (string, max 100)
- **Saída esperada:** Redirecionamento para `/dashboard` (ou `ReturnUrl`)
- **Saída em erro:** View `Login` com mensagem `"Dados inválidos!"`
- **Validações:** `ModelState.IsValid` para formato de e-mail e campos obrigatórios
- **Observação:** A rota é `/authentication/login`, mas também acessível via `/auth/login` (registro de rota alternativo)

**RF02 — Logout de Administrador**
O sistema deve permitir que o administrador encerre sua sessão, removendo o cookie de autenticação e redirecionando para a tela de login.
- **Entrada:** Nenhuma (requer sessão ativa, mas o controller não usa `[Authorize]`)
- **Saída:** Redirecionamento para `/authentication/login`
- **Ação interna:** `HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)`

**RF03 — Proteção de Rotas Administrativas**
As rotas dos controllers `AdministradoresController`, `ClientesController`, `CartoesController` e `DashboardController` devem ser protegidas por `[Authorize]`, exigindo que o usuário esteja autenticado via cookie para acessá-las. Usuários não autenticados devem ser redirecionados para a tela de login.
- **Controllers protegidos:** Administradores, Clientes, Cartoes, Dashboard
- **Controllers públicos:** Home, Salas, Reservas, Authentication
- **Redirecionamento:** Para `/authentication/login?ReturnUrl=...`

**RF04 — Geração de Token JWT (não implementado / código morto)**
O controller `AuthenticationController` possui um método `CreateToken` que gera um token JWT com claims (id, name, given name, email), assinado com HMAC-SHA256 e chave privada hardcoded. **Este método não é chamado por nenhuma ação do sistema** — documentado como funcionalidade prevista mas não integrada.
- **Chave:** `"bAafd@A7d9#@F4*V!LHZs#ebKQrkE6pad2f3kj34c3dXy@"`
- **Expiração:** 1 hora
- **Issuer:** `"yourIssuer"` / **Audience:** `"yourAudience"`

---

### 2.2 Módulo de Salas (Catálogo)

**RF05 — Listar Catálogo de Salas (Público)**
A página inicial do sistema deve exibir o catálogo de salas disponíveis com paginação de 12 itens por página, ordenadas por ID decrescente.
- **Rota:** `GET /` ou `GET /Home`
- **Parâmetros:** `pageNumber` (int, opcional, default 1)
- **Retorno:** View com lista paginada de `Sala`

**RF06 — Buscar Salas por Nome (Público)**
O catálogo de salas deve permitir busca por nome (case-insensitive).
- **Rota:** `GET /Salas`
- **Parâmetros:** `searchString` (string, opcional), `pageNumber` (int, opcional)
- **Paginação:** 5 itens por página, ordenado por ID decrescente
- **Comportamento:** Se `searchString` for vazio ou nulo, retorna todas as salas

**RF07 — Visualizar Detalhes de Sala**
O sistema deve exibir os detalhes de uma sala específica.
- **Rotas:** `GET /Salas/Details/{id}`, `GET /Home/ClienteReserva/{id}`
- **Parâmetros:** `id` (int, obrigatório)
- **Retorno em erro:** `404` se `id` for nulo ou sala não encontrada

**RF08 — CRUD de Salas (Público)**
O controller `SalasController` não possui `[Authorize]`, tornando as operações de criação, edição e exclusão de salas acessíveis publicamente. Isso é documentado como uma **limitação de segurança** (ver seção 8).
- **Create:** `GET/POST /Salas/Create` — campos: nome, descricao, capacidade, categoria
- **Edit:** `GET/POST /Salas/Edit/{id}` — mesmos campos
- **Delete:** `GET/POST /Salas/Delete/{id}` — confirmação necessária
- **Validação:** Todos os campos são `[Required]`; usa `[Bind]` para whitelist de propriedades

---

### 2.3 Módulo de Reservas — Fluxo Público

**RF09 — Realizar Reserva Pública**
Um cliente não cadastrado pode fazer uma reserva informando nome, e-mail, data, horário inicial e final, e opcionalmente um cartão. O sistema cria automaticamente um novo cliente no banco e gera a reserva com status `"solicitada"`.
- **Rota:** `POST /Home/ClienteReserva/{id}`
- **Parâmetros:** `id` (int, ID da sala), formulário com `ClienteReserva` (nome, email, hora_inicio, hora_fim, data_reserva, cartao_id)
- **Ações internas:**
  1. Criar novo `Cliente` com nome e email informados
  2. Criar `Reserva` vinculada ao novo cliente e à sala
  3. Gerar protocolo via `ProtocoloHelper.GerarProtocolo()` (formato: `RES-YYYYMMDD-XXXXXXXX`)
  4. Status inicial: `"solicitada"`
- **Saída:** Redirecionamento para `/Home/Index` com `TempData["MensagemSucesso"]` contendo o número do protocolo
- **Observação:** Não há validação de conflito de horário com reservas existentes
- **Observação:** Não possui `[ValidateAntiForgeryToken]`

**RF10 — Consultar Reserva por Protocolo**
Um cliente pode consultar sua reserva informando o número do protocolo. A busca é case-insensitive.
- **Rota:** `GET /Home/Reserva`
- **Parâmetros:** `SearchString` (string, opcional), `pageNumber` (int, opcional)
- **Paginação:** 5 itens por página
- **Comportamento:** Se `SearchString` for vazio, retorna lista vazia
- **Inclui:** Dados do cliente e da sala (eager loading)

**RF11 — Cancelar Reserva (Público)**
Um cliente pode cancelar sua reserva pelo protocolo. O sistema altera o status para `"cancelada"`.
- **Rota:** `POST /Home/Reserva`
- **Parâmetros:** `id` (int, ID da reserva)
- **Ação:** Altera `status` da reserva para `"cancelada"` e salva no banco
- **Saída:** Redirecionamento de volta para a página de busca com o protocolo preenchido

---

### 2.4 Módulo de Reservas — Fluxo Administrativo

**RF12 — Listar Reservas (Admin)**
O administrador pode visualizar a lista de reservas com filtro por status (ativas ou canceladas) e busca por protocolo.
- **Rota:** `GET /Reservas`
- **Parâmetros:** `active` (string, "1" = ativas/não canceladas, qualquer outro valor = canceladas), `searchString` (string, opcional), `pageNumber` (int, opcional)
- **Paginação:** 5 itens por página, eager loading de cartão, cliente e sala
- **Observação:** Controller **não possui** `[Authorize]`

**RF13 — Criar Reserva (Admin)**
O administrador pode criar uma reserva selecionando sala, cliente, data e horários existentes.
- **Rota:** `GET/POST /Reservas/Create`
- **GET:** Carrega view com dropdowns de salas, clientes e cartões
- **POST:** Cria reserva com protocolo gerado automaticamente; campo `cartao_id` não é mapeado no POST atual
- **ViewModel:** `ReservaCreate` (contém `Reserva`, `List<Sala>`, `List<Cliente>`, `List<Cartao>`)

**RF14 — Editar Reserva (Admin)**
O administrador pode editar uma reserva existente. Se um cartão for associado, a reserva é automaticamente confirmada.
- **Rota:** `GET/POST /Reservas/Edit/{id}`
- **Regra de negócio crítica:** Se `cartao_id` não for nulo, o status é forçado para `"confirmada"` independentemente do valor enviado no formulário
- **GET:** Carrega dados da reserva + lista de salas, clientes e cartões filtrados pelo cliente da reserva
- **Tratamento de concorrência:** `DbUpdateConcurrencyException` capturado com recarga e verificação de existência

**RF15 — Excluir Reserva (Admin)**
O administrador pode excluir fisicamente uma reserva do banco de dados.
- **Rota:** `POST /Reservas/Delete/{id}`
- **Ação:** Remove o registro do banco (hard delete)
- **Observação:** Controller não possui `[Authorize]`

**RF16 — Geração de Protocolo**
Toda reserva criada deve receber um protocolo único no formato `RES-YYYYMMDD-XXXXXXXX`, onde `YYYYMMDD` é a data atual e `XXXXXXXX` são 8 caracteres hexadecimais derivados de um GUID.
- **Implementação:** `ProtocoloHelper.GerarProtocolo()`
- **Exemplo:** `RES-20241122-A1B2C3D4`

---

### 2.5 Módulo de Administradores

**RF17 — CRUD de Administradores**
O sistema deve permitir o gerenciamento completo de administradores (criar, listar, editar, visualizar, excluir).
- **Rotas:** `GET/POST /Administradores/{action}/{id?}`
- **Autorização:** Requer `[Authorize]`
- **Campos:** nome (obrigatório), email (obrigatório), senha (obrigatório no create), cpf (opcional), permissoes (opcional)

**RF18 — Hash de Senha com BCrypt**
A senha do administrador deve ser armazenada como hash BCrypt. Na criação, a senha é hasheada antes de persistir. Na edição, a senha **não pode ser alterada** (o edit usa `AdministradorEdit` que não inclui o campo senha).
- **Implementação:** `PasswordHelper.HashPassword(password)` → `BCrypt.Net.BCrypt.HashPassword(password)`
- **Verificação:** `PasswordHelper.VerifyPassword(password, hash)` → `BCrypt.Net.BCrypt.Verify(password, hash)`

**RF19 — Paginação e Busca de Administradores**
A listagem de administradores deve ser paginada (5 itens por página) e permitir busca por nome (case-insensitive).
- **Parâmetros:** `searchString`, `pageNumber`, `currentFilter`
- **Ordenação:** ID decrescente

**RF20 — Edição de Administrador sem Alteração de Senha**
A edição de administrador usa o ViewModel `AdministradorEdit`, que exclui o campo `senha`. A senha do administrador não pode ser modificada após a criação.

---

### 2.6 Módulo de Clientes

**RF21 — CRUD de Clientes**
O sistema deve permitir o gerenciamento completo de clientes (criar, listar, editar, visualizar, excluir).
- **Rotas:** `GET/POST /Clientes/{action}/{id?}`
- **Autorização:** Requer `[Authorize]`
- **Campos:** nome (obrigatório), email (obrigatório), cpf (opcional), endereço completo (logradouro, numero, complemento, cep, bairro, cidade, estado, pais — todos opcionais)

**RF22 — Paginação e Busca de Clientes**
A listagem de clientes deve ser paginada (5 itens por página) e permitir busca por nome (case-insensitive).

**RF23 — Endereço Completo do Cliente**
O cliente pode ter um endereço completo armazenado em campos separados (logradouro, número, complemento, CEP, bairro, cidade, estado, país). Todos os campos de endereço são opcionais.

---

### 2.7 Módulo de Cartões

**RF24 — CRUD de Cartões**
O sistema deve permitir o gerenciamento completo de cartões de crédito (criar, listar, editar, visualizar, excluir).
- **Rotas:** `GET/POST /Cartoes/{action}/{numeroDoCartao}`
- **Autorização:** Requer `[Authorize]`
- **Campos:** numeroDoCartao (obrigatório, usado como identificador nas rotas), nomeDoTitular (obrigatório), validade (obrigatório, DateTime), cvv (obrigatório, int), cliente_id (obrigatório)

**RF25 — Associação Cartão-Cliente**
Cada cartão deve pertencer a um cliente. A criação permite selecionar o cliente via dropdown. A edição **não permite** alterar o cliente associado (`cliente_id` não está no `[Bind]` do Edit).
- **Relacionamento:** Cliente 1:N Cartao
- **Chave estrangeira:** `cartoes.cliente_id` → `clientes.id` (ON DELETE CASCADE)

**RF26 — Identificação por Número do Cartão**
As rotas de detalhes, edição e exclusão de cartões usam `numeroDoCartao` (string) como identificador, não o `id` numérico.
- **Tipo no banco:** string (TEXT)

---

### 2.8 Módulo de Dashboard

**RF27 — Página Inicial Administrativa**
O sistema deve exibir uma página de dashboard para administradores autenticados.
- **Rota:** `GET /Dashboard`
- **Autorização:** `[Authorize]`
- **Conteúdo:** View simples (sem lógica adicional no controller atualmente)

---

### 2.9 Módulo de Notificações

**RF28 — Serviço de E-mail Configurado**
O sistema possui um serviço de e-mail configurado via SMTP, injetável por DI. As configurações de servidor SMTP vêm do `appsettings.json`.
- **Implementação:** `EmailService` com `SendEmailAsync(string to, string subject, string body)`
- **Configuração:** `Smtp:Host`, `Smtp:Port`, `Smtp:EnableSsl`, `Smtp:User`, `Smtp:Password`
- **Observação:** Nenhuma chamada ao serviço está ativa — todas estão comentadas no código

**RF29 — Notificação de Reserva (não implementado)**
O sistema deve enviar e-mails de notificação para o cliente quando uma reserva for criada, confirmada ou cancelada. Esta funcionalidade está prevista mas **não implementada** (códigos comentados em `ReservasController`).

---

### 2.10 Módulo de Páginas Institucionais

**RF30 — Páginas Institucionais**
O sistema deve exibir páginas públicas de Sobre, Ajuda e Privacidade.
- **Rotas:** `GET /Home/Sobre`, `GET /Home/Ajuda`, `GET /Home/Privacidade`
- **Autorização:** Público

---

## 3. Requisitos Não Funcionais (RNF)

**RNF01 — Segurança no Armazenamento de Senhas**
Senhas de administradores devem ser armazenadas utilizando BCrypt (custo padrão da biblioteca BCrypt.Net-Next).

**RNF02 — Proteção CSRF**
Todas as requisições POST em operações de criação, edição e exclusão devem ser protegidas por `[ValidateAntiForgeryToken]`, com exceção de:
- `POST /authentication/login` (não possui antifalsificação)
- `POST /Home/ClienteReserva/{id}` (não possui antifalsificação)

**RNF03 — Proteção de Rotas Administrativas**
Rotas de gerenciamento (Administradores, Clientes, Cartões, Dashboard) devem ser acessíveis apenas para usuários autenticados via cookie de sessão.

**RNF04 — Suporte a Múltiplos Bancos de Dados**
O sistema deve suportar SQLite (desenvolvimento), MySQL (produção), SQL Server (produção) e SQL Server (SmarterASP) sem alteração de código, apenas via configuração em `appsettings.json`.

**RNF05 — Paginação em Listagens**
Todas as listagens devem ser paginadas com tamanhos de página definidos por funcionalidade:
- Catálogo público de salas: 12 itens
- Listagens administrativas: 5 itens

**RNF06 — Conteinerização**
O sistema deve ser executável em contêineres Docker com Docker Compose, incluindo:
- Serviço MySQL 8.0 para banco de dados
- Aplicação .NET compilada em imagem Linux
- Proxy reverso Nginx para produção

**RNF07 — Interface Responsiva**
A interface deve utilizar Bootstrap 5 para garantir responsividade em diferentes tamanhos de tela.

**RNF08 — Validação Client-side e Server-side**
As validações de formulário devem ocorrer tanto no cliente (jQuery Validation) quanto no servidor (Data Annotations + `ModelState.IsValid`).

**RNF09 — Disponibilidade de Dados Iniciais**
O sistema deve semear dados iniciais na primeira execução: 1 administrador padrão, 20 clientes e 20 salas.

**RNF10 — Portabilidade**
O sistema deve ser compatível com Windows, Linux e macOS por meio do runtime .NET 8.0.

**RNF11 — Tratamento de Erros**
Erros não tratados devem redirecionar para uma página de erro amigável (`/Home/Error`) em produção, e exibir detalhes em desenvolvimento.

---

## 4. Especificações de Autenticação

### 4.1 Mecanismo Principal: Cookie Authentication

| Aspecto | Detalhe |
|---|---|
| Esquema | `Microsoft.AspNetCore.Authentication.Cookies` |
| Login Path | `/authentication/login` |
| Logout Path | `/authentication/logout` |
| Access Denied Path | Não configurado (usa default) |
| Expiração do Cookie | 60 minutos |
| Persistência | `IsPersistent = true` |
| Claims armazenadas | `ClaimTypes.NameIdentifier` (id do admin), `ClaimTypes.Name` (nome do admin) |

### 4.2 Fluxo de Autenticação

```
[Usuário não autenticado] → GET /authentication/login → View Login
  → Preenche e-mail + senha → POST /authentication/login
    → Busca Administrador por e-mail
    → Verifica senha com BCrypt
    → Se válido:
        → Cria ClaimsIdentity com id e nome
        → HttpContext.SignInAsync (cookie)
        → Redireciona para /dashboard (ou ReturnUrl)
    → Se inválido:
        → Retorna View Login com erro "Dados inválidos!"
```

### 4.3 JWT (não utilizado)

O método `CreateToken` no `AuthenticationController` gera um token JWT mas nunca é invocado. A configuração inclui:

| Parâmetro | Valor |
|---|---|
| Algoritmo | HMAC-SHA256 |
| Chave | `"bAafd@A7d9#@F4*V!LHZs#ebKQrkE6pad2f3kj34c3dXy@"` |
| Issuer | `"yourIssuer"` |
| Audience | `"yourAudience"` |
| Expiração | 1 hora |
| Claims | id, Name, GivenName, Email |

### 4.4 Rotas Protegidas vs. Públicas

| Controller | Autorização | Observação |
|---|---|---|
| `AuthenticationController` | Público | Login/logout |
| `HomeController` | Público | Landing page, reserva pública, páginas institucionais |
| `SalasController` | **Público** | CRUD completo de salas — **sem proteção** |
| `ReservasController` | **Público** | CRUD completo de reservas — **sem proteção** |
| `AdministradoresController` | `[Authorize]` | |
| `ClientesController` | `[Authorize]` | |
| `CartoesController` | `[Authorize]` | |
| `DashboardController` | `[Authorize]` | |

### 4.5 Hash de Senhas

- **Algoritmo:** BCrypt (via biblioteca BCrypt.Net-Next 4.0.3)
- **Hash na criação:** `PasswordHelper.HashPassword(senha)`
- **Verificação no login:** `PasswordHelper.VerifyPassword(enteredPassword, hashedPassword)`
- **Alteração de senha:** Não é possível após a criação (ViewModel de edição não inclui campo senha)

### 4.6 Anti-Forgery Token

A maioria dos formulários POST utiliza `[ValidateAntiForgeryToken]`. Exceções documentadas:
- `AuthenticationController.Authenticate`
- `HomeController.ClienteReserva`

---

## 5. Documentação das Rotas

### 5.1 Convenções de Rota

O sistema registra duas rotas em `RouteConfigExtension.RegisterRoutes`:
1. `auth` — `{controller=Authentication}/{action=Login}/{id?}`
2. `default` — `{controller=Home}/{action=Index}/{id?}`

### 5.2 Tabela Completa de Rotas

| Método | Rota | Controller.Action | Parâmetros | Autorização | Descrição |
|---|---|---|---|---|---|
| GET | `/` | `Home.Index` | `pageNumber?` | Público | Catálogo público de salas (12/página) |
| GET | `/Home` | `Home.Index` | `pageNumber?` | Público | Catálogo público de salas |
| GET | `/Home/ClienteReserva/{id}` | `Home.ClienteReserva` (GET) | `id` (int) | Público | Formulário de reserva pública |
| POST | `/Home/ClienteReserva/{id}` | `Home.ClienteReserva` (POST) | `id` (int), form data | Público | Criar reserva pública + cliente |
| GET | `/Home/Reserva` | `Home.Reserva` (GET) | `SearchString?`, `pageNumber?` | Público | Buscar reserva por protocolo |
| POST | `/Home/Reserva` | `Home.Reserva` (POST) | `id` (int) | Público | Cancelar reserva |
| GET | `/Home/Sobre` | `Home.Sobre` | — | Público | Página "Sobre" |
| GET | `/Home/Ajuda` | `Home.Ajuda` | — | Público | Página "Ajuda" |
| GET | `/Home/Privacidade` | `Home.Privacidade` | — | Público | Página "Privacidade" |
| GET | `/Home/Error` | `Home.Error` | — | Público | Página de erro |
| GET | `/authentication/login` | `Authentication.Index` | — | Público | Formulário de login |
| POST | `/authentication/login` | `Authentication.Authenticate` | `email`, `password` (form) | Público | Efetuar login |
| GET | `/authentication/logout` | `Authentication.Logout` | — | Público | Efetuar logout |
| GET | `/auth/login` | `Authentication.Index` | — | Público | Rota alternativa de login |
| GET | `/Administradores` | `Administradores.Index` | `searchString?`, `pageNumber?`, `currentFilter?` | `[Authorize]` | Listar administradores |
| GET | `/Administradores/Details/{id}` | `Administradores.Details` | `id` (int) | `[Authorize]` | Detalhes do admin |
| GET | `/Administradores/Create` | `Administradores.Create` (GET) | — | `[Authorize]` | Formulário de criação |
| POST | `/Administradores/Create` | `Administradores.Create` (POST) | `Administrador` (form) | `[Authorize]` | Criar administrador |
| GET | `/Administradores/Edit/{id}` | `Administradores.Edit` (GET) | `id` (int) | `[Authorize]` | Formulário de edição |
| POST | `/Administradores/Edit/{id}` | `Administradores.Edit` (POST) | `id` (int), `AdministradorEdit` (form) | `[Authorize]` | Editar administrador |
| GET | `/Administradores/Delete/{id}` | `Administradores.Delete` (GET) | `id` (int) | `[Authorize]` | Página de confirmação |
| POST | `/Administradores/Delete/{id}` | `Administradores.DeleteConfirmed` | `id` (int) | `[Authorize]` | Excluir administrador |
| GET | `/Clientes` | `Clientes.Index` | `searchString?`, `pageNumber?`, `currentFilter?` | `[Authorize]` | Listar clientes |
| GET | `/Clientes/Details/{id}` | `Clientes.Details` | `id` (int) | `[Authorize]` | Detalhes do cliente |
| GET | `/Clientes/Create` | `Clientes.Create` (GET) | — | `[Authorize]` | Formulário de criação |
| POST | `/Clientes/Create` | `Clientes.Create` (POST) | `Cliente` (form, Bind) | `[Authorize]` | Criar cliente |
| GET | `/Clientes/Edit/{id}` | `Clientes.Edit` (GET) | `id` (int) | `[Authorize]` | Formulário de edição |
| POST | `/Clientes/Edit/{id}` | `Clientes.Edit` (POST) | `id` (int), `Cliente` (form, Bind) | `[Authorize]` | Editar cliente |
| GET | `/Clientes/Delete/{id}` | `Clientes.Delete` (GET) | `id` (int) | `[Authorize]` | Página de confirmação |
| POST | `/Clientes/Delete/{id}` | `Clientes.DeleteConfirmed` | `id` (int) | `[Authorize]` | Excluir cliente |
| GET | `/Cartoes` | `Cartoes.Index` | — | `[Authorize]` | Listar cartões |
| GET | `/Cartoes/Details/{numeroDoCartao}` | `Cartoes.Details` | `numeroDoCartao` (string) | `[Authorize]` | Detalhes do cartão |
| GET | `/Cartoes/Create` | `Cartoes.Create` (GET) | — | `[Authorize]` | Formulário de criação |
| POST | `/Cartoes/Create` | `Cartoes.Create` (POST) | `Cartao` (form, Bind) | `[Authorize]` | Criar cartão |
| GET | `/Cartoes/Edit/{numeroDoCartao}` | `Cartoes.Edit` (GET) | `numeroDoCartao` (string) | `[Authorize]` | Formulário de edição |
| POST | `/Cartoes/Edit/{numeroDoCartao}` | `Cartoes.Edit` (POST) | `numeroDoCartao` (string), `Cartao` (form, Bind) | `[Authorize]` | Editar cartão |
| GET | `/Cartoes/Delete/{numeroDoCartao}` | `Cartoes.Delete` (GET) | `numeroDoCartao` (string) | `[Authorize]` | Página de confirmação |
| POST | `/Cartoes/Delete/{numeroDoCartao}` | `Cartoes.DeleteConfirmed` | `numeroDoCartao` (string) | `[Authorize]` | Excluir cartão |
| GET | `/Dashboard` | `Dashboard.Index` | — | `[Authorize]` | Dashboard administrativo |
| GET | `/Salas` | `Salas.Index` | `searchString?`, `pageNumber?`, `currentFilter?` | Público | Listar salas |
| GET | `/Salas/Details/{id}` | `Salas.Details` | `id` (int) | Público | Detalhes da sala |
| GET | `/Salas/Create` | `Salas.Create` (GET) | — | Público | Formulário de criação |
| POST | `/Salas/Create` | `Salas.Create` (POST) | `Sala` (form, Bind) | Público | Criar sala |
| GET | `/Salas/Edit/{id}` | `Salas.Edit` (GET) | `id` (int) | Público | Formulário de edição |
| POST | `/Salas/Edit/{id}` | `Salas.Edit` (POST) | `id` (int), `Sala` (form, Bind) | Público | Editar sala |
| GET | `/Salas/Delete/{id}` | `Salas.Delete` (GET) | `id` (int) | Público | Página de confirmação |
| POST | `/Salas/Delete/{id}` | `Salas.DeleteConfirmed` | `id` (int) | Público | Excluir sala |
| GET | `/Reservas` | `Reservas.Index` | `active?`, `searchString?`, `pageNumber?` | Público | Listar reservas |
| GET | `/Reservas/Details/{id}` | `Reservas.Details` | `id` (int) | Público | Detalhes da reserva |
| GET | `/Reservas/Create` | `Reservas.Create` (GET) | — | Público | Formulário de criação |
| POST | `/Reservas/Create` | `Reservas.Create` (POST) | `ReservaCreate` (form) | Público | Criar reserva |
| GET | `/Reservas/Edit/{id}` | `Reservas.Edit` (GET) | `id` (int) | Público | Formulário de edição |
| POST | `/Reservas/Edit/{id}` | `Reservas.Edit` (POST) | `id` (int), `ReservaCreate` (form) | Público | Editar reserva |
| GET | `/Reservas/Delete/{id}` | `Reservas.Delete` (GET) | `id` (int) | Público | Página de confirmação |
| POST | `/Reservas/Delete/{id}` | `Reservas.DeleteConfirmed` | `id` (int) | Público | Excluir reserva |

---

## 6. Especificações de Testes

### 6.1 Testes Unitários

#### 6.1.1 `PasswordHelper`
| Caso | Entrada | Saída Esperada |
|---|---|---|
| Hash de senha válida | `"minhaSenha123"` | String de hash BCrypt válida (começa com `$2`) |
| Verificar senha correta | `("minhaSenha123", hash)` | `true` |
| Verificar senha incorreta | `("outraSenha", hash)` | `false` |

#### 6.1.2 `ProtocoloHelper.GerarProtocolo()`
| Caso | Entrada | Saída Esperada |
|---|---|---|
| Formato do protocolo | — | Regex `^RES-\d{8}-[A-F0-9]{8}$` |
| Unicidade | Duas chamadas consecutivas | Strings diferentes |
| Componente data | — | Parte `YYYYMMDD` deve ser a data atual |

#### 6.1.3 `ModelPaginado<T>`
| Caso | Entrada | Saída Esperada |
|---|---|---|
| Paginação página 1 | `(source com 12 itens, page=1, size=5)` | 5 itens, `PageIndex=1`, `TotalPages=3`, `HasPreviousPage=false`, `HasNextPage=true` |
| Paginação página 2 | `(source com 12 itens, page=2, size=5)` | 5 itens, `PageIndex=2`, `TotalPages=3`, `HasPreviousPage=true`, `HasNextPage=true` |
| Paginação última página | `(source com 12 itens, page=3, size=5)` | 2 itens, `PageIndex=3`, `TotalPages=3`, `HasPreviousPage=true`, `HasNextPage=false` |
| Fonte vazia | `(source vazio, page=1, size=5)` | 0 itens, `TotalPages=0` |
| PageIndex inválido (negativo) | `(source com 10 itens, page=-1, size=5)` | Deve tratar como página 1 ou lançar exceção |

### 6.2 Testes de Integração

#### 6.2.1 Fluxo de Autenticação
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Login com credenciais válidas | POST `/authentication/login` com email=`email@email.com`, senha=`123` | Redirecionamento 302 para `/dashboard`; cookie de autenticação presente |
| Login com senha inválida | POST `/authentication/login` com email=`email@email.com`, senha=`senha_errada` | View `Login` com mensagem de erro; sem cookie |
| Login com e-mail inexistente | POST `/authentication/login` com email=`nao.existe@teste.com`, senha=`123` | View `Login` com mensagem de erro |
| Acesso a rota protegida sem auth | GET `/Dashboard` | Redirecionamento 302 para `/authentication/login?ReturnUrl=/Dashboard` |
| Logout | GET `/authentication/logout` | Cookie removido; redirecionamento para `/authentication/login` |

#### 6.2.2 Fluxo de Reserva Pública
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Reserva bem-sucedida | POST `/Home/ClienteReserva/1` com nome, email, hora_inicio, hora_fim, data_reserva | Cliente criado no banco; reserva com status `"solicitada"`; redirecionamento com `TempData` contendo protocolo |
| Reserva sem dados obrigatórios | POST `/Home/ClienteReserva/1` sem nome | `ModelState` inválido; view retornada com erros |
| Visualizar formulário de reserva | GET `/Home/ClienteReserva/1` | View com dados da sala |
| Sala inexistente | GET `/Home/ClienteReserva/999` | 404 |

#### 6.2.3 Fluxo de Consulta e Cancelamento de Reserva
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Buscar por protocolo existente | GET `/Home/Reserva?SearchString=RES-...` | Lista com a reserva encontrada |
| Buscar por protocolo inexistente | GET `/Home/Reserva?SearchString=RES-INEXISTENTE` | Lista vazia |
| Buscar sem termo | GET `/Home/Reserva` | Lista vazia |
| Cancelar reserva | POST `/Home/Reserva` com `id` válido | Status da reserva alterado para `"cancelada"`; redirecionamento para busca |

#### 6.2.4 CRUD de Salas
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Criar sala | POST `/Salas/Create` com dados válidos | Sala persistida; redirecionamento para `Index` |
| Listar salas | GET `/Salas` | Lista paginada (5/página) |
| Buscar sala por nome | GET `/Salas?searchString=Nome` | Lista filtrada |
| Editar sala | POST `/Salas/Edit/1` com novos dados | Sala atualizada |
| Excluir sala | POST `/Salas/Delete/1` | Sala removida do banco |

#### 6.2.5 CRUD de Reservas (Admin)
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Criar reserva | POST `/Reservas/Create` com dados válidos | Reserva criada com protocolo |
| Editar reserva sem cartão | POST `/Reservas/Edit/1` sem `cartao_id` | Status mantido conforme enviado |
| Editar reserva com cartão | POST `/Reservas/Edit/1` com `cartao_id` válido | Status forçado para `"confirmada"` |
| Excluir reserva | POST `/Reservas/Delete/1` | Registro removido |
| Listar ativas | GET `/Reservas?active=1` | Apenas reservas não canceladas |
| Listar canceladas | GET `/Reservas?active=0` | Apenas reservas canceladas |

#### 6.2.6 CRUD de Administradores
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Criar admin | POST `/Administradores/Create` com dados válidos | Admin criado com senha hasheada |
| Listar admins | GET `/Administradores` | Lista paginada (5/página) |
| Editar admin | POST `/Administradores/Edit/1` | Campos editados; senha NÃO alterada |

#### 6.2.7 CRUD de Clientes
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Criar cliente | POST `/Clientes/Create` com dados válidos | Cliente persistido |
| Listar clientes | GET `/Clientes` | Lista paginada (5/página) |
| Concorrência na edição | Dois usuários editam simultaneamente | `DbUpdateConcurrencyException` tratado |

#### 6.2.8 CRUD de Cartões
| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Criar cartão | POST `/Cartoes/Create` com dados válidos | Cartão persistido |
| Listar cartões | GET `/Cartoes` | Lista com eager loading do cliente |
| Editar cartão | POST `/Cartoes/Edit/{num}` | `cliente_id` NÃO alterado |

### 6.3 Testes de Segurança

| Cenário | Passos | Resultado Esperado |
|---|---|---|
| Acesso anônimo a CRUD de salas | GET/POST `/Salas/Create` sem autenticação | Acesso permitido (comportamento atual — documentado como limitação) |
| Acesso anônimo a CRUD de reservas | GET/POST `/Reservas/Delete/1` sem autenticação | Acesso permitido (comportamento atual — documentado como limitação) |
| CSRF em login | POST `/authentication/login` sem token antifalsificação | Aceito (comportamento atual — documentado como limitação) |
| Injeção de SQL | Parâmetros de busca com caracteres especiais | Comportamento normal (EF Core parametriza consultas) |
| Força bruta de login | Múltiplas tentativas de login | Sem bloqueio implementado |

### 6.4 Testes de Validação de Modelo

| Cenário | Entrada | Resultado Esperado |
|---|---|---|
| Login sem e-mail | `email=""`, `password="123"` | `ModelState` inválido |
| Login com e-mail inválido | `email="invalido"`, `password="123"` | `ModelState` inválido |
| Sala sem nome | `nome=""` | `ModelState` inválido |
| Reserva sem data | `data_reserva=""` | `ModelState` inválido |

### 6.5 Testes de Paginação

| Cenário | Parâmetros | Resultado Esperado |
|---|---|---|
| Página 1 | `pageNumber=1` | Primeiros N itens |
| Página negativa | `pageNumber=-1` | Tratado como página 1 |
| Página além do limite | `pageNumber=9999` | Última página com zero itens |
| Transição de busca | `searchString="X"` → paginar → `searchString="Y"` | Nova busca reseta para página 1 |

---

## 7. Regras de Negócio

### 7.1 Ciclo de Vida da Reserva

```
solicitada ──→ confirmada (via associação de cartão na edição)
     │                │
     └──→ cancelada   └──→ cancelada
```

- **Status inicial:** `"solicitada"` — criado tanto pelo fluxo público quanto pelo admin
- **Confirmação:** Ao editar uma reserva e associar um `cartao_id`, o status é forçado para `"confirmada"` (regra implementada em `ReservasController.Edit`)
- **Cancelamento:** Pode ocorrer a qualquer momento via `HomeController.Reserva` (POST) — status alterado para `"cancelada"`
- **Exclusão:** Apenas via `ReservasController.Delete` (hard delete)

### 7.2 Geração de Protocolo

- Toda reserva recebe um protocolo único no formato `RES-YYYYMMDD-XXXXXXXX`
- `YYYYMMDD` = data atual no momento da criação
- `XXXXXXXX` = 8 caracteres hexadecimais (maiúsculos) derivados de um GUID
- O protocolo é gerado em C# e não há garantia de unicidade no banco (sem unique index)

### 7.3 Cadastro de Cliente na Reserva Pública

No fluxo público (`HomeController.ClienteReserva`), o cliente é criado automaticamente com apenas `nome` e `email`. Não há verificação se o cliente já existe com o mesmo e-mail — um novo cliente é sempre criado.

### 7.4 Associação Cartão-Cliente

- Um cliente pode ter múltiplos cartões
- O cartão é identificado pelo campo `numeroDoCartao` (string) nas rotas
- Na edição de cartão, o cliente associado não pode ser alterado
- Ao editar uma reserva, apenas cartões do cliente vinculado à reserva são listados

### 7.5 Ausência de Verificação de Conflito de Horário

**O sistema não verifica se já existe uma reserva para a mesma sala no mesmo horário.** É possível criar duas reservas para a mesma sala com data e horários sobrepostos. Isso se aplica tanto ao fluxo público quanto ao administrativo.

### 7.6 Backfill de Timestamps

As colunas `created_at` e `updated_at` são preenchidas no construtor do modelo em C# usando `DateTime.Now`, não via DEFAULT do banco de dados. Isso significa que o timestamp reflete o momento da instanciação do objeto em memória, não o momento da inserção no banco.

---

## 8. Limitações Conhecidas e Melhorias Futuras

### 8.1 Segurança

| # | Limitação | Impacto | Sugestão |
|---|---|---|---|
| L01 | `SalasController` e `ReservasController` sem `[Authorize]` | Qualquer usuário pode criar/editar/excluir salas e reservas | Adicionar `[Authorize]` e definir política de roles |
| L02 | `POST /authentication/login` sem `[ValidateAntiForgeryToken]` | Vulnerável a ataques CSRF no login | Adicionar antifalsificação |
| L03 | `POST /Home/ClienteReserva` sem `[ValidateAntiForgeryToken]` | Vulnerável a CSRF na criação de reserva | Adicionar antifalsificação |
| L04 | Chave JWT hardcoded no código | Exposição de segredo em repositório | Mover para variável de ambiente / User Secrets |
| L05 | JWT não implementado | Funcionalidade prevista mas incompleta | Integrar JWT como alternativa de autenticação para API |
| L06 | Sem bloqueio de força bruta | Múltiplas tentativas de login ilimitadas | Implementar rate limiting ou lockout |

### 8.2 Funcionalidades

| # | Limitação | Impacto | Sugestão |
|---|---|---|---|
| L07 | Sem verificação de conflito de horário | Duas reservas podem ocupar a mesma sala no mesmo horário | Implementar validação antes de criar/editar reserva |
| L08 | Serviço de e-mail desabilitado | Nenhuma notificação é enviada | Ativar chamadas ao `EmailService` |
| L09 | Senha de admin não pode ser alterada | Admin não pode trocar senha | Adicionar campo senha na edição (com hash) |
| L10 | Cliente duplicado na reserva pública | Mesmo e-mail pode criar múltiplos clientes | Verificar existência por e-mail antes de criar |
| L11 | Protocolo sem unique index no banco | Possibilidade de protocolos duplicados | Adicionar índice único |
| L12 | Busca vazia em `HomeController.Reserva` | Usuário vê lista vazia em vez de todas as reservas | Mostrar resultados recentes quando não houver busca |

### 8.3 Técnicas

| # | Limitação | Impacto | Sugestão |
|---|---|---|---|
| L13 | Middleware de auth após MapControllers | Proteção pode não funcionar corretamente | Mover `UseAuthentication()` antes de `MapControllers()` |
| L14 | Duas chamadas a `UseHttpsRedirection` e `UseStaticFiles` | Sem impacto funcional, mas código duplicado | Remover duplicatas |
| L15 | Timestamps em C# vs banco | Possível inconsistência em inserts em lote | Usar `DEFAULT CURRENT_TIMESTAMP` no banco |
| L16 | `Cartao.created_at` com formato de data apenas | Inconsistência com os demais timestamps | Padronizar para datetime completo |

### 8.4 Experiência do Usuário

| # | Limitação | Impacto | Sugestão |
|---|---|---|---|
| L17 | Sem feedback visual para ações (exceto TempData) | Usuário não vê confirmação de ações | Melhorar sistema de notificações |
| L18 | Paginação não mostra total de itens | Usuário não sabe quantos registros existem | Adicionar contagem |

---

## 9. Matriz de Rastreabilidade

| Requisito | Controller | Action(s) | Modelo | View |
|---|---|---|---|---|
| RF01 | AuthenticationController | Index, Authenticate | Administrador | Login |
| RF02 | AuthenticationController | Logout | — | — |
| RF03 | — | — | — | — |
| RF04 | AuthenticationController | CreateToken | Administrador | — |
| RF05 | HomeController | Index | Sala | Index |
| RF06 | SalasController | Index | Sala | Index |
| RF07 | SalasController, HomeController | Details, ClienteReserva | Sala | Details, ClienteReserva |
| RF08 | SalasController | Create, Edit, Delete, DeleteConfirmed | Sala | Create, Edit, Delete |
| RF09 | HomeController | ClienteReserva (POST) | Cliente, Reserva | — |
| RF10 | HomeController | Reserva (GET) | Reserva | Reserva |
| RF11 | HomeController | Reserva (POST) | Reserva | — |
| RF12 | ReservasController | Index | Reserva | Index |
| RF13 | ReservasController | Create | Reserva, ReservaCreate | Create |
| RF14 | ReservasController | Edit | Reserva, ReservaCreate | Edit |
| RF15 | ReservasController | Delete, DeleteConfirmed | Reserva | Delete |
| RF16 | ProtocoloHelper | GerarProtocolo | — | — |
| RF17 | AdministradoresController | CRUD completo | Administrador | CRUD views |
| RF18 | PasswordHelper | HashPassword, VerifyPassword | Administrador | — |
| RF19 | AdministradoresController | Index | Administrador | Index |
| RF20 | AdministradoresController | Edit (POST) | AdministradorEdit | Edit |
| RF21 | ClientesController | CRUD completo | Cliente | CRUD views |
| RF22 | ClientesController | Index | Cliente | Index |
| RF23 | ClientesController | Create, Edit | Cliente | Create, Edit |
| RF24 | CartoesController | CRUD completo | Cartao | CRUD views |
| RF25 | CartoesController | Create, Edit | Cartao, CartoesCreate | Create, Edit |
| RF26 | CartoesController | Details, Edit, Delete | Cartao | Details, Edit, Delete |
| RF27 | DashboardController | Index | — | Index |
| RF28 | EmailService | SendEmailAsync | — | — |
| RF29 | ReservasController | (comentado) | — | — |
| RF30 | HomeController | Sobre, Ajuda, Privacidade | — | Sobre, Ajuda, Privacidade |

---

## 10. Esquema do Banco de Dados

### 10.1 Diagrama de Entidades e Relacionamentos

```
┌──────────────────┐       ┌──────────────────┐
│  administradores │       │     salas        │
├──────────────────┤       ├──────────────────┤
│ id (PK)          │       │ id (PK)          │
│ nome             │       │ nome             │
│ email            │       │ descricao        │
│ senha (bcrypt)   │       │ capacidade       │
│ cpf              │       │ categoria        │
│ permissoes       │       │ created_at       │
│ created_at       │       │ updated_at       │
│ updated_at       │       └────────┬─────────┘
└──────────────────┘                │
                                    │ 1
                                    │
┌──────────────────┐       ┌────────▼─────────┐
│    clientes      │       │    reservas       │
├──────────────────┤       ├──────────────────┤
│ id (PK)          │1      │ id (PK)          │
│ nome             │◄──────│ hora_inicio      │
│ email            │N      │ hora_fim         │
│ cpf              │       │ data_reserva     │
│ endereco_* (9)   │       │ protocolo        │
│ created_at       │       │ status           │
│ updated_at       │       │ sala_id (FK)     │
└────────┬─────────┘       │ cliente_id (FK)  │
         │ 1               │ cartao_id (FK?)  │
         │                 │ created_at       │
         │                 │ updated_at       │
         │ N               └──────────────────┘
┌────────▼─────────┐
│     cartoes      │
├──────────────────┤
│ id (PK)          │
│ numeroDoCartao   │
│ nomeDoTitular    │
│ validade         │
│ cvv              │
│ cliente_id (FK)  │
│ created_at       │
│ updated_at       │
└──────────────────┘
```

### 10.2 Dicionário de Dados

#### `administradores`

| Coluna | Tipo SQL (SQLite) | .NET | Obrigatório | Padrão | Observação |
|---|---|---|---|---|---|
| id | INTEGER | int | Sim | Auto-increment | PK |
| nome | TEXT | string | Sim | — | |
| email | TEXT | string | Sim | — | |
| senha | TEXT | string | Sim | — | Armazenado como hash BCrypt |
| cpf | TEXT | string? | Não | null | |
| permissoes | TEXT | string? | Não | null | |
| created_at | TEXT | string? | Não | `DateTime.Now` | Formato: `yyyy-MM-dd HH:mm:ss` |
| updated_at | TEXT | string? | Não | `DateTime.Now` | Formato: `yyyy-MM-dd HH:mm:ss` |

#### `clientes`

| Coluna | Tipo SQL | .NET | Obrigatório | Padrão |
|---|---|---|---|---|
| id | INTEGER | int | Sim | Auto-increment (PK) |
| nome | TEXT | string | Sim | — |
| email | TEXT | string | Sim | — |
| cpf | TEXT | string? | Não | null |
| endereco_logradouro | TEXT | string? | Não | null |
| endereco_numero | TEXT | string? | Não | null |
| endereco_complemento | TEXT | string? | Não | null |
| endereco_cep | TEXT | string? | Não | null |
| endereco_bairro | TEXT | string? | Não | null |
| endereco_cidade | TEXT | string? | Não | null |
| endereco_estado | TEXT | string? | Não | null |
| endereco_pais | TEXT | string? | Não | null |
| created_at | TEXT | string? | Não | `DateTime.Now` |
| updated_at | TEXT | string? | Não | `DateTime.Now` |

#### `salas`

| Coluna | Tipo SQL | .NET | Obrigatório | Padrão |
|---|---|---|---|---|
| id | INTEGER | int | Sim | Auto-increment (PK) |
| nome | TEXT | string | Sim | — |
| descricao | TEXT | string | Sim | — |
| capacidade | TEXT | string | Sim | — (armazenado como string) |
| categoria | TEXT | string | Sim | — |
| created_at | TEXT | string? | Não | `DateTime.Now` |
| updated_at | TEXT | string? | Não | `DateTime.Now` |

#### `cartoes`

| Coluna | Tipo SQL | .NET | Obrigatório | Padrão | FK |
|---|---|---|---|---|---|
| id | INTEGER | int | Sim | Auto-increment (PK) | |
| numeroDoCartao | TEXT | string | Sim | — | |
| nomeDoTitular | TEXT | string | Sim | — | |
| validade | TEXT | DateTime | Sim | — | |
| cvv | INTEGER | int | Sim | — | |
| cliente_id | INTEGER | int | Sim | — | FK → `clientes(id)` ON DELETE CASCADE |
| created_at | TEXT | string? | Não | `DateTime.Now` (yyyy-MM-dd) | |
| updated_at | TEXT | string? | Não | `DateTime.Now` (yyyy-MM-dd) | |

#### `reservas`

| Coluna | Tipo SQL | .NET | Obrigatório | Padrão | FK |
|---|---|---|---|---|---|
| id | INTEGER | int | Sim | Auto-increment (PK) | |
| hora_inicio | TEXT | string | Sim | — | Formato: `HH:mm` |
| hora_fim | TEXT | string | Sim | — | Formato: `HH:mm` |
| data_reserva | TEXT | string | Sim | — | Formato: `yyyy-MM-dd` |
| protocolo | TEXT | string | Sim | Gerado | Formato: `RES-YYYYMMDD-XXXXXXXX` |
| status | TEXT | string | Sim | — | Valores: `solicitada`, `confirmada`, `cancelada` |
| sala_id | INTEGER | int | Sim | — | FK → `salas(id)` ON DELETE CASCADE |
| cliente_id | INTEGER | int | Sim | — | FK → `clientes(id)` ON DELETE CASCADE |
| cartao_id | INTEGER | int? | Não | null | FK → `cartoes(id)` ON DELETE NO ACTION |
| created_at | TEXT | string? | Não | `DateTime.Now` | |
| updated_at | TEXT | string? | Não | `DateTime.Now` | |

---

## 11. Configuração do Ambiente

### 11.1 Variáveis de Configuração (`appsettings.json`)

```json
{
  "DatabaseProvider": "SQLite | Localdb | MySql | SqlServer | Production",
  "ConnectionStrings": {
    "DefaultConnection": "SQL Server (LocalDB)",
    "SqlServerConnection": "SQL Server (TCP)",
    "MySqlConnection": "MySQL",
    "SQLiteConnection": "SQLite",
    "SmarterAspMSSQL": "SQL Server (hospedagem)"
  },
  "Smtp": {
    "Host": "smtp.seuservidor.com",
    "Port": 587,
    "EnableSsl": true,
    "User": "seu-usuario",
    "Password": "sua-senha"
  }
}
```

### 11.2 Credenciais Padrão (Seeder)

| Papel | E-mail | Senha |
|---|---|---|
| Administrador | email@email.com | 123 |

---

## 12. Glossário

| Termo | Definição |
|---|---|
| Protocolo | Identificador único da reserva no formato `RES-YYYYMMDD-XXXXXXXX` |
| Status `solicitada` | Reserva criada aguardando confirmação |
| Status `confirmada` | Reserva confirmada com cartão |
| Status `cancelada` | Reserva cancelada pelo cliente |
| BCrypt | Algoritmo de hash para armazenamento seguro de senhas |
| Cookie Authentication | Mecanismo de autenticação baseado em cookie do ASP.NET Core |
| ModelPaginado | Classe genérica que estende `List<T>` com propriedades de paginação |
| Anti-Forgery Token | Token de segurança contra ataques CSRF |
