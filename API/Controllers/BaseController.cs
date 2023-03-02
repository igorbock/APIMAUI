namespace API.Controllers;

public abstract class BaseController : Controller
{
    private readonly Modelo c_Modelo;

    public BaseController(Modelo p_modelo)
    {
        c_Modelo = p_modelo;
    }

    public virtual async Task<IEntidade?> CM_DeserializaJsonObtemClasseERegistraSolicitacao(string p_json, Solicitacao p_solicitacao)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(p_json))
            {
                ReadResult requestBodyInBytes = await Request.BodyReader.ReadAsync();
                Request.BodyReader.AdvanceTo(requestBodyInBytes.Buffer.Start, requestBodyInBytes.Buffer.End);
                p_json = Encoding.UTF8.GetString(requestBodyInBytes.Buffer.FirstSpan);
            }

            p_solicitacao = JsonSerializer.Deserialize<Solicitacao>(p_json) ?? throw new ArgumentNullException("Solicitação está vazia.");
            p_solicitacao.ds_metodo = Request.Method;
            p_solicitacao.dt_inicio = DateTime.Now;
            c_Modelo.Solicitacoes.Add(p_solicitacao);

            var m_entidadeFactory = new EntidadeFactory();
            IQueryable<IEntidade>? m_queryable = p_solicitacao.ds_entidade switch
            {
                nameof(Produto) => c_Modelo.Produtos.AsNoTracking(),
                nameof(Setor) => c_Modelo.Setores.AsNoTracking(),
                nameof(Etiqueta) => c_Modelo.Etiquetas.AsNoTracking(),
                _ => null
            };
            if((m_queryable?.ToList().Count == 0 && p_solicitacao.ds_metodo != "POST" ) || m_queryable == null )
                throw new KeyNotFoundException("Os dados na base não existem ou não foram encontrados.");

            var m_entidade = m_entidadeFactory.CM_ObtemEntidade(p_solicitacao, m_queryable);
            if (m_entidade == null)
                throw new KeyNotFoundException("A entidade não foi encontrada ou não está implementada.");

            if(string.IsNullOrWhiteSpace(p_solicitacao.ds_parametros))
            {
                var m_classeDaBase = c_Modelo.Find(m_entidade.GetType(), p_solicitacao.cd_codigo_interno);
                return m_classeDaBase as IEntidade;
            }
            return m_entidade;
        }
        catch (Exception ex)
        {
            p_solicitacao.CMX_EncerrarSolicitacao(ex.Message);
            throw;
        }
    }

    public virtual JsonDocument CM_ObtemJsonDaClasse<Classe>(Classe p_entidade)
        => JsonSerializer.SerializeToDocument(p_entidade);

    [HttpPost]
    public abstract Task<IActionResult> CM_Salvar(string json);
    [HttpGet]
    public abstract Task<IActionResult> CM_Ler(string json);
    [HttpPut]
    public abstract Task<IActionResult> CM_Editar(string json);
    [HttpDelete]
    public abstract Task<IActionResult> CM_Deletar(string json);
}
