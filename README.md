# Imobiliaria Mathers

Aplicação web desenvolvida em **ASP.NET Core MVC (.NET 10)** para gerenciamento e divulgação de imóveis, com suporte a cadastro de usuários, autenticação JWT, painel administrativo e busca/filtragem de imóveis.

<img width="1919" height="1078" alt="image" src="https://github.com/user-attachments/assets/70317904-0422-4769-9878-837ccffc6594" />

---

## Sumário

- [Apresentação do Sistema](#apresentação-do-sistema)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Modelos de Dados](#modelos-de-dados)
- [Serviços](#serviços)
- [Funcionalidades](#funcionalidades)
- [Configuração](#configuração)
- [Executando o Projeto](#executando-o-projeto)
- [Endpoints da API](#endpoints-da-api)
- [Segurança](#segurança)

---

## Apresentação do Sistema

A **ImobiliariaMathers** é uma plataforma para listagem e gestão de imóveis. Usuários podem navegar pelo catálogo e filtrar imóveis por tipo, número de dormitórios, garagem e área. Administradores têm acesso a um painel exclusivo para cadastrar, editar e excluir imóveis com suporte a múltiplas imagens.

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core 9 com Pomelo (MySQL) |
| Banco de dados | MySQL 8.0+ |
| Autenticação | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Hash de senhas | BCrypt (via `IPasswordHasherService`) |
| Front-end | Razor Views (`.cshtml`), CSS, JavaScript |

---

## Estrutura do Projeto

```
ImobiliariaMathers/
├── Controllers/
│   └── HomeController.cs          # Único controller; gerencia todos os endpoints
├── Data/
│   └── AppDbContext.cs            # Contexto do EF Core; mapeamento das entidades
├── Models/
│   ├── Imovel.cs                  # Entidade de imóvel (enums TipoImovel, TipoNegocio)
│   ├── Imagem.cs                  # Imagens associadas a um imóvel
│   ├── Usuario.cs                 # Usuário (enum TipoUsuario: USER, ADMINISTRATOR)
│   ├── CodigoRecuperacao.cs       # Códigos de recuperação de senha (one-time use)
│   ├── CadastroImovelRequest.cs   # DTO de cadastro/atualização de imóvel
│   ├── CadastroUsuarioRequest.cs  # DTO de registro de usuário
│   ├── LoginUsuarioRequest.cs     # DTO de login
│   └── RedefinirSenhaComCodigoRequest.cs
├── Services/
│   ├── IJwtTokenService.cs / JwtTokenService.cs
│   ├── IPasswordHasherService.cs / PasswordHasherService.cs
│   └── IRecoveryCodeService.cs / RecoveryCodeService.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Página inicial
│   │   ├── Imoveis.cshtml         # Listagem pública de imóveis
│   │   ├── CadastroImoveis.cshtml # Painel admin: cadastro de imóvel
│   │   ├── AlterarImoveis.cshtml  # Painel admin: edição de imóvel
│   │   └── Login.cshtml           # Tela de login
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _Header.cshtml
├── Program.cs                     # Configuração da aplicação (DI, JWT, EF, Antiforgery)
├── appsettings.json               # Configurações base (não incluir dados sensíveis)
└── appsettings.Local.example.json # Exemplo de configuração local
```

---

## Modelos de Dados

### `Imovel`
| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `long` | Chave primária |
| `Titulo` | `string (max 150)` | Título do imóvel |
| `Tipo` | `TipoImovel` | `CASA` ou `APARTAMENTO` |
| `Negocio` | `TipoNegocio` | `COMPRA` ou `ALUGUEL` |
| `Cidade` | `string` | Cidade |
| `Estado` | `char(2)` | UF (ex.: `SP`) |
| `Bairro` | `string?` | Bairro (opcional) |
| `Cep` | `string?` | CEP (opcional) |
| `Preco` | `decimal(12,2)` | Preço |
| `Dormitorios` | `byte` | Número de dormitórios |
| `Garagem` | `bool` | Possui garagem |
| `AreaM2` | `decimal(8,2)` | Área em m² |
| `Imagens` | `ICollection<Imagem>` | Imagens associadas |

### `Usuario`
| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `long` | Chave primária |
| `Name` | `string (max 100)` | Nome completo |
| `Email` | `string (max 150)` | E-mail único |
| `Senha` | `string (max 255)` | Hash BCrypt |
| `Tipo` | `TipoUsuario` | `USER` ou `ADMINISTRATOR` |

### `CodigoRecuperacao`
Códigos de uso único gerados no cadastro para redefinição de senha sem e-mail. Cada usuário recebe 8 códigos; cada código é consumido uma única vez (`UsadoEm` registrado após uso).

---

## Serviços

### `IJwtTokenService`
Gera tokens JWT assinados com `HS256` contendo as claims do usuário (id, nome, tipo/role).

### `IPasswordHasherService`
Realiza hash e verificação de senhas. Suporta detecção de hashes legados (`IsLegacyHash`) para migração transparente na próxima autenticação.

### `IRecoveryCodeService`
Gera, persiste e valida códigos de recuperação de conta. Os códigos são armazenados como hashes no banco de dados.

Todos os serviços são registrados via injeção de dependência por interface (`AddScoped`), seguindo o princípio de inversão de dependência.

---

## Funcionalidades

**Área pública**
- Listagem de imóveis com filtros por tipo, garagem, número de dormitórios e faixa de área
- Cadastro de usuário com geração de códigos de recuperação
- Login com "lembrar de mim" (cookie persistente) e logout
- Redefinição de senha via código de recuperação
- Verificação de status de autenticação

**Área administrativa** (`[Authorize(Roles = "ADMINISTRATOR")]`)
- Cadastro de imóveis com múltiplas imagens (base64)
- Listagem completa de imóveis
- Atualização de imóvel (substitui as imagens por completo)
- Exclusão de imóvel

---

## Configuração

### 1. Clone o repositório

```bash
git clone <url-do-repositório>
cd ImobiliariaMathers
```

### 2. Configure o banco de dados

Crie o banco MySQL:

```sql
CREATE DATABASE imobiliaria_mathers CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. Configure as variáveis locais

Crie o arquivo `appsettings.Local.json` na raiz do projeto baseando-se no exemplo fornecido:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=imobiliaria_mathers;user=root;password=SUA_SENHA"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_FORTE_AQUI_COM_32_OU_MAIS_CARACTERES",
    "Issuer": "ImobiliariaMathers",
    "Audience": "ImobiliariaMathers.Client",
    "ExpiresInMinutes": 120
  },
  "Security": {
    "ExposeRecoveryCodesOnRegister": false
  }
}
```

### 4. Aplique as migrations

```bash
dotnet ef database update
```

## Endpoints da API

Todos os endpoints retornam JSON. O token JWT é transportado via cookie HttpOnly `auth_token`.

| Método | Rota | Autenticação | Descrição |
|---|---|---|---|
| `GET` | `/Home/Index` | — | Página inicial |
| `GET` | `/Home/Imoveis` | — | Página de listagem |
| `GET` | `/Home/Login` | — | Página de login |
| `GET` | `/Home/BuscarImoveis` | — | Busca imóveis com filtros (`tipo`, `garagem`, `dormitorios`, `espaco`) |
| `GET` | `/Home/StatusAutenticacao` | — | Retorna se o usuário está autenticado |
| `POST` | `/Home/CadastrarUsuario` | — | Registra novo usuário |
| `POST` | `/Home/ValidarLogin` | — | Autentica e seta cookie JWT |
| `POST` | `/Home/Logout` | `[Authorize]` | Invalida o cookie |
| `POST` | `/Home/RedefinirSenhaComCodigo` | — | Redefine senha via código de recuperação |
| `GET` | `/Home/CadastroImoveis` | `ADMINISTRATOR` | Página de cadastro (admin) |
| `GET` | `/Home/AlterarImoveis` | `ADMINISTRATOR` | Página de edição (admin) |
| `GET` | `/Home/ListarImoveisAdmin` | `ADMINISTRATOR` | Lista todos os imóveis (admin) |
| `POST` | `/Home/CadastrarImovel` | `ADMINISTRATOR` | Cadastra novo imóvel |
| `PUT` | `/Home/AtualizarImovel?id={id}` | `ADMINISTRATOR` | Atualiza imóvel existente |
| `DELETE` | `/Home/ExcluirImovel?id={id}` | `ADMINISTRATOR` | Remove imóvel |

### Filtros disponíveis em `BuscarImoveis`

| Parâmetro | Valores aceitos |
|---|---|
| `tipo` | `CASA`, `APARTAMENTO` |
| `garagem` | `true`, `false` |
| `dormitorios` | `1`, `2`, `3`, `4`, `5` (≥5 agrupados) |
| `espaco` | `0-50`, `51-100`, `101-150`, `150+` |

---

## Segurança

- **Tokens JWT** armazenados em cookie `HttpOnly`, `Secure`, `SameSite=Strict` — protegidos contra XSS e CSRF.
- **Anti-forgery token** habilitado globalmente via `AutoValidateAntiforgeryTokenAttribute`; o header esperado é `X-CSRF-TOKEN`.
- **Senhas** armazenadas com hash BCrypt; suporte à migração de hashes legados de forma transparente.
- **Códigos de recuperação** armazenados como hashes no banco e invalidados após uso único.
- **Configurações sensíveis** (chave JWT, string de conexão) isoladas em `appsettings.Local.json`, carregado opcionalmente e fora do controle de versão.
- **Proteção contra pacotes MySQL oversized**: requisições com imagens muito grandes retornam HTTP 413 com mensagem amigável.
