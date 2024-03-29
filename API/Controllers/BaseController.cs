﻿namespace API.Controllers;

public abstract class BaseController : Controller
{
    private readonly Modelo c_Modelo;
    protected readonly JsonSerializerOptions? c_JsonSerializarOptions;

    public BaseController(Modelo p_modelo)
    {
        c_Modelo = p_modelo;
    }

    [NonAction]
    public virtual (IQueryable<IEntidade>, IEntidade) CM_DeserializaJsonObtemClasseERegistraSolicitacao(Modelo p_modelo, string? p_json, out Solicitacao p_solicitacao)
    {
        p_solicitacao = new Solicitacao();

        try
        {
            if (string.IsNullOrWhiteSpace(p_json))
            {
                ReadResult requestBodyInBytes = Request.BodyReader.ReadAsync().Result;
                Request.BodyReader.AdvanceTo(requestBodyInBytes.Buffer.Start, requestBodyInBytes.Buffer.End);
                p_json = Encoding.UTF8.GetString(requestBodyInBytes.Buffer.FirstSpan);
            }

            p_solicitacao = JsonSerializer.Deserialize<Solicitacao>(p_json) ?? throw new JsonException(MensagensExceptions.C_DesserializarSolicitacao);
            p_solicitacao.ds_metodo = Request.Method;
            p_solicitacao.dt_inicio = DateTime.Now;
            p_modelo.Solicitacoes.Add(p_solicitacao);

            var m_entidadeFactory = new EntidadeHelper();
            var m_queryable = m_entidadeFactory.CM_ObtemRegistrosDaEntidadeNaBase(p_modelo, p_solicitacao);
            return CM_ObtemResultadosDeAcordoComCadaMetodo(p_solicitacao, m_queryable);
        }
        catch (Exception ex)
        {
            p_solicitacao.CMX_EncerrarSolicitacao(ex.Message);
            throw;
        }
    }

    [NonAction]
    public (IQueryable<IEntidade>, IEntidade) CM_ObtemResultadosDeAcordoComCadaMetodo(Solicitacao p_solicitacao, IQueryable<IEntidade> p_fonteDeDados)
    {
        var m_entidadeFactory = new EntidadeHelper();

        object? m_entidadeEncontrada;
        var m_entidade = m_entidadeFactory.CM_RetornaEntidadeAPartirDaSolicitacao(p_solicitacao);
        if (p_solicitacao.CMX_VerificaSeMetodoEhGetOuDelete())
        {
            if (p_solicitacao.cd_codigo_interno == 0)
                return (p_fonteDeDados, m_entidade);
            var m_entidadeDaFonte = p_fonteDeDados.FirstOrDefault(a => a.cd_codigo == p_solicitacao.cd_codigo_interno);
            m_entidadeEncontrada = m_entidadeDaFonte == null ? new NenhumRegistroEncontradoException() : m_entidadeDaFonte;
            var m_entidadeDepoisDoCast = m_entidadeEncontrada as IEntidade ?? throw new Exception("Impossível fazer o cast.");
            return (new List<IEntidade> { m_entidadeDepoisDoCast }.AsQueryable(), m_entidadeDepoisDoCast);
        }
        else
        {
            if (p_solicitacao.ds_metodo == "PUT")
                m_entidadeEncontrada = p_fonteDeDados.FirstOrDefault(a => a.cd_codigo == p_solicitacao.cd_codigo_interno) == null ? new NenhumRegistroEncontradoException() : null;
            return (new List<IEntidade> { m_entidade }.AsQueryable(), m_entidade);
        }
    }

    [HttpPost]
    public abstract ActionResult CM_Salvar(string json);
    [HttpGet]
    public abstract ActionResult CM_Ler(string json);
    [HttpPut]
    public abstract ActionResult CM_Editar(string json);
    [HttpDelete]
    public abstract ActionResult CM_Deletar(string json);
}
