using ImobiliariaViviane.Data;
using ImobiliariaViviane.Models;
using ImobiliariaViviane.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ImobiliariaViviane.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;
        private readonly RecoveryCodeService _recoveryCodeService;

        public HomeController(
            AppDbContext context,
            JwtTokenService jwtTokenService,
            IConfiguration configuration,
            RecoveryCodeService recoveryCodeService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _recoveryCodeService = recoveryCodeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Imoveis()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult CadastroImoveis()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult AlterarImoveis()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> CadastrarImovel([FromBody] CadastroImovelRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados inválidos para cadastro do imóvel." });
            }

            if (!Enum.TryParse<TipoImovel>(request.Tipo.Trim(), true, out var tipoImovel))
            {
                return BadRequest(new { mensagem = "Tipo de imóvel inválido. Use CASA ou APARTAMENTO." });
            }

            if (!Enum.TryParse<TipoNegocio>(request.Negocio.Trim(), true, out var tipoNegocio))
            {
                return BadRequest(new { mensagem = "Tipo de negócio inválido. Use COMPRA ou ALUGUEL." });
            }

            var imovel = new Imovel
            {
                Titulo = request.Titulo.Trim(),
                Tipo = tipoImovel,
                Negocio = tipoNegocio,
                Bairro = string.IsNullOrWhiteSpace(request.Bairro) ? null : request.Bairro.Trim(),
                Cidade = request.Cidade.Trim(),
                Estado = request.Estado.Trim().ToUpperInvariant(),
                Cep = string.IsNullOrWhiteSpace(request.Cep) ? null : request.Cep.Trim(),
                Dormitorios = request.Dormitorios,
                Garagem = request.Garagem,
                AreaM2 = request.AreaM2,
                Preco = request.Preco,
                Descricao = string.IsNullOrWhiteSpace(request.Descricao) ? null : request.Descricao.Trim(),
                AtualizadoEm = null
            };

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Imoveis.Add(imovel);
                await _context.SaveChangesAsync();

                if (request.Imagens is { Count: > 0 })
                {
                    var imagens = request.Imagens
                        .Where(url => !string.IsNullOrWhiteSpace(url))
                        .Select((url, index) => new Imagem
                        {
                            ImovelId = imovel.Id,
                            Url = url.Trim(),
                            Ordem = (byte)Math.Min(index + 1, byte.MaxValue)
                        })
                        .ToList();

                    if (imagens.Count > 0)
                    {
                        _context.Imagens.AddRange(imagens);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (IsPacketTooLargeError(ex))
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status413PayloadTooLarge, new
                {
                    mensagem = "As imagens enviadas estão muito grandes. Reduza o tamanho/quantidade e tente novamente."
                });
            }

            return Ok(new
            {
                mensagem = "Imóvel cadastrado com sucesso.",
                id = imovel.Id
            });
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpGet]
        public async Task<IActionResult> ListarImoveisAdmin()
        {
            var imoveis = await _context.Imoveis
                .AsNoTracking()
                .OrderByDescending(i => i.CriadoEm)
                .Select(i => new
                {
                    id = i.Id,
                    titulo = i.Titulo,
                    tipo = i.Tipo.ToString().ToLower(),
                    negocio = i.Negocio.ToString().ToLower(),
                    bairro = i.Bairro,
                    cidade = i.Cidade,
                    estado = i.Estado,
                    cep = i.Cep,
                    dormitorios = i.Dormitorios,
                    garagem = i.Garagem,
                    areaM2 = i.AreaM2,
                    preco = i.Preco,
                    descricao = i.Descricao,
                    imagens = i.Imagens
                        .OrderBy(img => img.Ordem)
                        .Select(img => img.Url)
                        .ToList(),
                    criadoEm = i.CriadoEm,
                    atualizadoEm = i.AtualizadoEm
                })
                .ToListAsync();

            return Ok(imoveis);
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPut]
        public async Task<IActionResult> AtualizarImovel(long id, [FromBody] CadastroImovelRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados inválidos para atualização do imóvel." });
            }

            if (!Enum.TryParse<TipoImovel>(request.Tipo.Trim(), true, out var tipoImovel))
            {
                return BadRequest(new { mensagem = "Tipo de imóvel inválido. Use CASA ou APARTAMENTO." });
            }

            if (!Enum.TryParse<TipoNegocio>(request.Negocio.Trim(), true, out var tipoNegocio))
            {
                return BadRequest(new { mensagem = "Tipo de negócio inválido. Use COMPRA ou ALUGUEL." });
            }

            var imovel = await _context.Imoveis.FirstOrDefaultAsync(i => i.Id == id);
            if (imovel is null)
            {
                return NotFound(new { mensagem = "Imóvel não encontrado." });
            }

            imovel.Titulo = request.Titulo.Trim();
            imovel.Tipo = tipoImovel;
            imovel.Negocio = tipoNegocio;
            imovel.Bairro = string.IsNullOrWhiteSpace(request.Bairro) ? null : request.Bairro.Trim();
            imovel.Cidade = request.Cidade.Trim();
            imovel.Estado = request.Estado.Trim().ToUpperInvariant();
            imovel.Cep = string.IsNullOrWhiteSpace(request.Cep) ? null : request.Cep.Trim();
            imovel.Dormitorios = request.Dormitorios;
            imovel.Garagem = request.Garagem;
            imovel.AreaM2 = request.AreaM2;
            imovel.Preco = request.Preco;
            imovel.Descricao = string.IsNullOrWhiteSpace(request.Descricao) ? null : request.Descricao.Trim();
            imovel.AtualizadoEm = DateTime.UtcNow;

            if (request.Imagens is { Count: > 0 })
            {
                var imagensAtuais = await _context.Imagens
                    .Where(img => img.ImovelId == imovel.Id)
                    .ToListAsync();

                if (imagensAtuais.Count > 0)
                {
                    _context.Imagens.RemoveRange(imagensAtuais);
                }

                var novasImagens = request.Imagens
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .Select((url, index) => new Imagem
                    {
                        ImovelId = imovel.Id,
                        Url = url.Trim(),
                        Ordem = (byte)Math.Min(index + 1, byte.MaxValue)
                    })
                    .ToList();

                if (novasImagens.Count > 0)
                {
                    _context.Imagens.AddRange(novasImagens);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsPacketTooLargeError(ex))
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, new
                {
                    mensagem = "As imagens enviadas estão muito grandes. Reduza o tamanho/quantidade e tente novamente."
                });
            }

            return Ok(new { mensagem = "Imóvel atualizado com sucesso." });
        }

        private static bool IsPacketTooLargeError(DbUpdateException ex)
        {
            var mensagem = ex.GetBaseException().Message;
            return mensagem.Contains("max_allowed_packet", StringComparison.OrdinalIgnoreCase)
                || mensagem.Contains("packet bigger", StringComparison.OrdinalIgnoreCase);
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpDelete]
        public async Task<IActionResult> ExcluirImovel(long id)
        {
            var imovel = await _context.Imoveis.FirstOrDefaultAsync(i => i.Id == id);
            if (imovel is null)
            {
                return NotFound(new { mensagem = "Imóvel não encontrado." });
            }

            _context.Imoveis.Remove(imovel);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Imóvel excluído com sucesso." });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> BuscarImoveis(
            [FromQuery] string? tipo,
            [FromQuery] bool? garagem,
            [FromQuery] int? dormitorios,
            [FromQuery] string? espaco)
        {
            var query = _context.Imoveis
                .AsNoTracking()
                .OrderByDescending(i => i.CriadoEm)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo)
                && Enum.TryParse<TipoImovel>(tipo.Trim(), true, out var tipoImovel))
            {
                query = query.Where(i => i.Tipo == tipoImovel);
            }

            if (garagem.HasValue)
            {
                query = query.Where(i => i.Garagem == garagem.Value);
            }

            if (dormitorios.HasValue && dormitorios.Value > 0)
            {
                query = dormitorios.Value >= 5
                    ? query.Where(i => i.Dormitorios >= 5)
                    : query.Where(i => i.Dormitorios == dormitorios.Value);
            }

            if (!string.IsNullOrWhiteSpace(espaco))
            {
                query = espaco.Trim() switch
                {
                    "0-50" => query.Where(i => i.AreaM2 <= 50),
                    "51-100" => query.Where(i => i.AreaM2 > 50 && i.AreaM2 <= 100),
                    "101-150" => query.Where(i => i.AreaM2 > 100 && i.AreaM2 <= 150),
                    "150+" => query.Where(i => i.AreaM2 > 150),
                    _ => query
                };
            }

            var imoveisRaw = await query
                .Select(i => new
                {
                    id = i.Id,
                    titulo = i.Titulo,
                    tipo = i.Tipo.ToString().ToLower(),
                    negocio = i.Negocio.ToString().ToLower(),
                    dormitorios = i.Dormitorios,
                    garagem = i.Garagem,
                    area = i.AreaM2,
                    preco = i.Preco,
                    bairro = i.Bairro,
                    cidade = i.Cidade,
                    estado = i.Estado,
                    descricao = i.Descricao,
                    imagens = i.Imagens
                        .OrderBy(img => img.Ordem)
                        .Select(img => img.Url)
                        .ToList()
                })
                .ToListAsync();

            var imoveis = imoveisRaw.Select(i => new
            {
                i.id,
                i.titulo,
                i.tipo,
                i.negocio,
                i.dormitorios,
                i.garagem,
                i.area,
                i.preco,
                bairro = i.bairro,
                cidade = i.cidade,
                estado = i.estado,
                endereco = string.Join(" - ", new[] { i.bairro, $"{i.cidade}/{i.estado}" }.Where(v => !string.IsNullOrWhiteSpace(v))),
                descricao = string.IsNullOrWhiteSpace(i.descricao) ? "Sem descrição disponível." : i.descricao,
                imagens = i.imagens,
                imagem = i.imagens.Count == 0 || string.IsNullOrWhiteSpace(i.imagens[0])
                    ? "/assets/placeholder01.jpg"
                    : i.imagens[0]
            });

            return Ok(imoveis);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastroUsuarioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados inválidos para cadastro." });
            }

            var email = request.Email.Trim().ToLowerInvariant();

            var emailJaCadastrado = await _context.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);

            if (emailJaCadastrado)
            {
                return BadRequest(new { mensagem = "Este e-mail já está cadastrado." });
            }

            var usuario = new Usuario
            {
                Name = request.Name.Trim(),
                Email = email,
                Senha = PasswordHasherService.HashPassword(request.Senha.Trim()),
                Tipo = TipoUsuario.USER,
                CriadoEm = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var codigosRecuperacao = _recoveryCodeService.GenerateCodes();
            _context.CodigosRecuperacao.AddRange(_recoveryCodeService.BuildEntities(usuario.Id, codigosRecuperacao));
            await _context.SaveChangesAsync();

            var exposeRecoveryCodes = _configuration.GetValue<bool>("Security:ExposeRecoveryCodesOnRegister");

            if (exposeRecoveryCodes)
            {
                return Ok(new
                {
                    mensagem = "Cadastro realizado com sucesso!",
                    id = usuario.Id,
                    tipo = usuario.Tipo.ToString(),
                    criadoEm = usuario.CriadoEm,
                    codigosRecuperacao
                });
            }

            return Ok(new
            {
                mensagem = "Cadastro realizado com sucesso!",
                id = usuario.Id,
                tipo = usuario.Tipo.ToString(),
                criadoEm = usuario.CriadoEm
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RedefinirSenhaComCodigo([FromBody] RedefinirSenhaComCodigoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados inválidos para redefinir senha." });
            }

            if (request.NovaSenha != request.ConfirmarSenha)
            {
                return BadRequest(new { mensagem = "A confirmação de senha não confere." });
            }

            var email = request.Email.Trim().ToLowerInvariant();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario is null)
            {
                return BadRequest(new { mensagem = "Dados de recuperação inválidos." });
            }

            var consumiuCodigo = await _recoveryCodeService.TryConsumeCodeAsync(_context, usuario.Id, request.CodigoRecuperacao);
            if (!consumiuCodigo)
            {
                return BadRequest(new { mensagem = "Dados de recuperação inválidos." });
            }

            usuario.Senha = PasswordHasherService.HashPassword(request.NovaSenha.Trim());
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Senha redefinida com sucesso. Faça login novamente." });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ValidarLogin([FromBody] LoginUsuarioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Informe e-mail e senha válidos." });
            }

            var email = request.Email.Trim().ToLowerInvariant();
            var senha = request.Senha.Trim();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario is null || !PasswordHasherService.VerifyPassword(senha, usuario.Senha))
            {
                return Unauthorized(new { mensagem = "E-mail ou senha incorretos." });
            }

            if (PasswordHasherService.IsLegacyHash(usuario.Senha))
            {
                usuario.Senha = PasswordHasherService.HashPassword(senha);
                await _context.SaveChangesAsync();
            }

            var token = _jwtTokenService.GenerateToken(usuario);
            var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var parsedMinutes)
                ? parsedMinutes
                : 120;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            };

            if (request.RememberMe)
            {
                cookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes);
            }

            Response.Cookies.Append("auth_token", token, cookieOptions);

            return Ok(new
            {
                mensagem = $"Bem-vindo(a), {usuario.Name}!",
                id = usuario.Id,
                nome = usuario.Name,
                tipo = usuario.Tipo.ToString()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult StatusAutenticacao()
        {
            return Ok(new
            {
                autenticado = User.Identity?.IsAuthenticated == true,
                nome = User.Identity?.Name
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
