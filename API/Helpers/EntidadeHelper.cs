namespace API.Helpers;

public class EntidadeHelper
{
    public IEntidade CM_RetornaEntidadeAPartirDaSolicitacao(Solicitacao? p_solicitacao)
    {
        if (p_solicitacao == null)
            throw new SolicitacaoNulaException();

        if(p_solicitacao.CMX_VerificaSeMetodoEhGetOuDelete())
        {
            return p_solicitacao.ds_entidade switch
            {
                nameof(Produto) => new Produto(),
                nameof(Setor) => new Setor(),
                nameof(Etiqueta) => new Etiqueta(),
                _ => throw new EntidadeNaoEncontradaException(p_solicitacao.ds_entidade)
            };
        }
        else
        {
            return p_solicitacao.ds_entidade switch
            {
                nameof(Produto) => JsonHelper.CM_DeserializaOuLancaException<Produto>(p_solicitacao.ds_parametros),
                nameof(Setor) => JsonHelper.CM_DeserializaOuLancaException<Setor>(p_solicitacao.ds_parametros),
                nameof(Etiqueta) => JsonHelper.CM_DeserializaOuLancaException<Etiqueta>(p_solicitacao.ds_parametros),
                _ => throw new EntidadeNaoEncontradaException(p_solicitacao.ds_entidade)
            };
        }
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
