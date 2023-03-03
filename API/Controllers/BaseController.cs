namespace API.Controllers;

public abstract class BaseController : Controller
{
    private readonly Modelo c_Modelo;

    public BaseController(Modelo p_modelo)
    {
        c_Modelo = p_modelo;
    }

    public virtual (IQueryable<IEntidade>, IEntidade) CM_DeserializaJsonObtemClasseERegistraSolicitacao(string? p_json, out Solicitacao p_solicitacao)
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

            p_solicitacao = JsonSerializer.Deserialize<Solicitacao>(p_json) ?? throw new SolicitacaoNulaException();
            p_solicitacao.ds_metodo = Request.Method;
            p_solicitacao.dt_inicio = DateTime.Now;
            c_Modelo.Solicitacoes.Add(p_solicitacao);

            var m_entidadeFactory = new EntidadeFactory();
            var m_queryable = m_entidadeFactory.CM_ObtemRegistrosDaEntidadeNaBase(c_Modelo, p_solicitacao);
            return cm_ObtemResultadosDeAcordoComCadaMetodo(p_solicitacao, m_queryable);
        }
        catch (Exception ex)
        {
            p_solicitacao.CMX_EncerrarSolicitacao(ex.Message);
            throw;
        }
    }

    private (IQueryable<IEntidade>, IEntidade) cm_ObtemResultadosDeAcordoComCadaMetodo(Solicitacao p_solicitacao, IQueryable<IEntidade> p_fonteDeDados)
    {
        var m_entidadeFactory = new EntidadeFactory();

        object? m_entidadeEncontrada;
        var m_entidade = m_entidadeFactory.CM_RetornaEntidadeAPartirDaSolicitacao(p_solicitacao);
        bool m_verificaSeEhMetotoGetOuDelete = p_solicitacao.ds_metodo == "GET" || p_solicitacao.ds_metodo == "DELETE";
        if (m_verificaSeEhMetotoGetOuDelete)
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
            var m_retorno = m_entidadeFactory.CM_RetornaEntidadeDesserializadaAPartirDaSolicitacao(p_solicitacao);
            return (new List<IEntidade> { m_retorno }.AsQueryable(), m_retorno);
        }
    }

    [HttpPost]
    public abstract IActionResult CM_Salvar(string json);
    [HttpGet]
    public abstract IActionResult CM_Ler(string json);
    [HttpPut]
    public abstract IActionResult CM_Editar(string json);
    [HttpDelete]
    public abstract IActionResult CM_Deletar(string json);
}
