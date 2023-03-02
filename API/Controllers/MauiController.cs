namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class MauiController : BaseController
{
    private readonly Modelo c_Modelo;

    public Solicitacao C_Solicitacao { get; set; }

    public MauiController(Modelo p_modelo) : base(p_modelo)
    {
        c_Modelo = p_modelo;
    }

    [HttpPost]
    public override async Task<IActionResult> CM_Salvar(string? json)
    {
        try
        {
            var m_classe = await CM_DeserializaJsonObtemClasseERegistraSolicitacao(json ?? string.Empty, C_Solicitacao);
#pragma warning disable CS8634
            c_Modelo.Add(m_classe);
#pragma warning restore CS8634

            C_Solicitacao.CMX_EncerrarSolicitacao($"A entidade da classe \"{m_classe?.GetType().Name}\" foi salva corretamente.");

            c_Modelo.SaveChanges();
            return StatusCode(200, C_Solicitacao.ds_resolucao);
        }
        catch (Exception ex)
        {
            c_Modelo.SaveChanges();
            return StatusCode(400, ex.Message);
        }
    }

    [HttpGet]
    public override async Task<IActionResult> CM_Ler(string? json)
    {
        try
        {
            var m_classe = await CM_DeserializaJsonObtemClasseERegistraSolicitacao(json ?? string.Empty, C_Solicitacao)
                ?? throw new NullReferenceException("Não foi possível ler a entidade.");
            var m_json = JsonSerializer.SerializeToElement<object>(m_classe);

            C_Solicitacao.CMX_EncerrarSolicitacao(m_json.ToString());

            c_Modelo.SaveChanges();
            return StatusCode(200, C_Solicitacao.ds_resolucao);
        }
        catch (Exception ex)
        {
            c_Modelo.SaveChanges();
            return StatusCode(400, ex.Message);
        }
    }

    [HttpPut]
    public override async Task<IActionResult> CM_Editar(string? json)
    {
        try
        {
            var m_classe = await CM_DeserializaJsonObtemClasseERegistraSolicitacao(json ?? string.Empty, C_Solicitacao);
#pragma warning disable CS8634
            c_Modelo.Update(m_classe);
#pragma warning restore CS8634

            C_Solicitacao.CMX_EncerrarSolicitacao($"A entidade da classe \"{m_classe?.GetType().Name}\" foi editada corretamente.");

            c_Modelo.SaveChanges();
            return StatusCode(200, C_Solicitacao.ds_resolucao);
        }
        catch (Exception ex)
        {
            c_Modelo.SaveChanges();
            return StatusCode(400, ex.Message);
        }
    }

    [HttpDelete]
    public override async Task<IActionResult> CM_Deletar(string? json)
    {
        try
        {
            var m_classe = await CM_DeserializaJsonObtemClasseERegistraSolicitacao(json ?? string.Empty, C_Solicitacao);
#pragma warning disable CS8634
            c_Modelo.Remove(m_classe);
#pragma warning restore CS8634

            C_Solicitacao.CMX_EncerrarSolicitacao($"A entidade da classe \"{m_classe?.GetType().Name}\" foi removida corretamente.");

            c_Modelo.SaveChanges();
            return StatusCode(200, C_Solicitacao.ds_resolucao);
        }
        catch (Exception ex)
        {
            c_Modelo.SaveChanges();
            return StatusCode(400, ex.Message);
        }
    }
}
