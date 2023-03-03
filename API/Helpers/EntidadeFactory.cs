namespace API.Helpers;

public class EntidadeFactory
{
    public IEntidade CM_RetornaEntidadeDesserializadaAPartirDaSolicitacao(Solicitacao? p_solicitacao)
    {
        if (p_solicitacao == null)
            throw new SolicitacaoNulaException();

        return p_solicitacao.ds_entidade switch
        {
            nameof(Produto) => JsonSerializer.Deserialize<Produto>(p_solicitacao.ds_parametros),
            nameof(Setor) => JsonSerializer.Deserialize<Setor>(p_solicitacao.ds_parametros),
            nameof(Etiqueta) => JsonSerializer.Deserialize<Etiqueta>(p_solicitacao.ds_parametros),
            _ => throw new EntidadeNaoEncontradaException(p_solicitacao.ds_entidade)
        };
    }

    public IEntidade CM_RetornaEntidadeAPartirDaSolicitacao(Solicitacao? p_solicitacao)
    {
        if (p_solicitacao == null)
            throw new SolicitacaoNulaException();

        return p_solicitacao.ds_entidade switch
        {
            nameof(Produto) => new Produto(),
            nameof(Setor) => new Setor(),
            nameof(Etiqueta) => new Etiqueta(),
            _ => throw new EntidadeNaoEncontradaException(p_solicitacao.ds_entidade)
        };
    }

    private IQueryable<Entidade> cm_DeserializaERetornaLista<Entidade>(string p_parametros)
    {
        var m_entidade = JsonSerializer.Deserialize<Entidade>(p_parametros) ?? throw new SerializationException("Não foi possível desserializar a entidade");
        return new List<Entidade> { m_entidade }.AsQueryable();
    }

    public IQueryable<IEntidade> CM_ObtemRegistrosDaEntidadeNaBase(Modelo p_modelo, Solicitacao p_solicitacao)
    {
        IQueryable<IEntidade> m_queryable = p_solicitacao.ds_entidade switch
        {
            nameof(Produto) => p_modelo.Produtos.AsNoTracking(),
            nameof(Setor) => p_modelo.Setores.AsNoTracking(),
            nameof(Etiqueta) => p_modelo.Etiquetas.AsNoTracking(),
            _ => throw new EntidadeNaoEncontradaException(p_solicitacao.ds_entidade)
        };

        bool m_lancarExceptionCountEhIgualAZeroEMetodoEhDiferenteDePost = m_queryable?.ToList().Count == 0 && p_solicitacao.ds_metodo != "POST";
        if (m_lancarExceptionCountEhIgualAZeroEMetodoEhDiferenteDePost || m_queryable == null)
            throw new NenhumRegistroEncontradoException();

        return m_queryable;
    }
}
