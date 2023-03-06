namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class MauiController : BaseController
{
    private readonly Modelo c_Modelo;

    public MauiController(Modelo p_modelo) : base(p_modelo)
    {
        c_Modelo = p_modelo;
    }

    [HttpPost]
    public override ActionResult CM_Salvar(string? json)
    {
        try
        {
            var m_retorno = CM_DeserializaJsonObtemClasseERegistraSolicitacao(c_Modelo, json, out Solicitacao p_solicitacao);
            var m_entidade = m_retorno.Item2;
#pragma warning disable CS8634
            c_Modelo.Add(m_entidade);
#pragma warning restore CS8634

            p_solicitacao.CMX_EncerrarSolicitacao(MensagensSolicitacoes.C_CriarEntidade, p_solicitacao.ds_entidade, p_solicitacao.ds_parametros);

            return new OkObjectResult(m_entidade);
        }
        catch (Exception ex)
        {
            return StatusCode(400, ex.Message);
        }
        finally
        {
            c_Modelo.SaveChanges();
        }
    }

    [HttpGet]
    public override ActionResult CM_Ler(string? json)
    {
        try
        {
            var m_retorno = CM_DeserializaJsonObtemClasseERegistraSolicitacao(c_Modelo, json, out Solicitacao p_solicitacao);
            var m_listaDaEntidade = m_retorno.Item1;
            var m_options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var m_json = JsonSerializer.SerializeToElement<IQueryable<object?>>(m_listaDaEntidade, m_options);

            p_solicitacao.CMX_EncerrarSolicitacao(m_json.ToString());

            return new OkObjectResult(m_listaDaEntidade);
        }
        catch (Exception ex)
        {
            return StatusCode(400, ex.Message);
        }
        finally
        {
            c_Modelo.SaveChanges();
        }
    }

    [HttpPut]
    public override ActionResult CM_Editar(string? json)
    {
        try
        {
            var m_retorno = CM_DeserializaJsonObtemClasseERegistraSolicitacao(c_Modelo, json, out Solicitacao p_solicitacao);
            var m_entidade = m_retorno.Item2;
#pragma warning disable CS8634
            c_Modelo.Update(m_entidade);
#pragma warning restore CS8634

            p_solicitacao.CMX_EncerrarSolicitacao(MensagensSolicitacoes.C_EditarEntidade, p_solicitacao.ds_entidade, p_solicitacao.ds_parametros);

            return new OkObjectResult(m_entidade);
        }
        catch (Exception ex)
        {
            return StatusCode(400, ex.Message);
        }
        finally
        {
            c_Modelo.SaveChanges();
        }
    }

    [HttpDelete]
    public override ActionResult CM_Deletar(string? json)
    {
        try
        {
            var m_retorno = CM_DeserializaJsonObtemClasseERegistraSolicitacao(c_Modelo, json, out Solicitacao p_solicitacao);
            var m_entidade = m_retorno.Item2;
#pragma warning disable CS8634
            c_Modelo.Remove(m_entidade);
#pragma warning restore CS8634

            p_solicitacao.CMX_EncerrarSolicitacao(string.Format(MensagensSolicitacoes.C_RemoverEntidade, p_solicitacao.ds_entidade));

            return new OkObjectResult(m_entidade);
        }
        catch (Exception ex)
        {
            return StatusCode(400, ex.Message);
        }
        finally
        {
            c_Modelo.SaveChanges();
        }
    }
}
