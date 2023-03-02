namespace API.Helpers;

public class EntidadeFactory
{
    public IEntidade? CM_ObtemEntidade(Solicitacao? p_solicitacao, IQueryable<IEntidade> p_fonteDados)
    {
        if (p_solicitacao == null)
            throw new ArgumentNullException();

        IEntidade? m_classe;
        if(string.IsNullOrWhiteSpace(p_solicitacao.ds_parametros))
        {
            return p_solicitacao.ds_entidade switch
            {
                nameof(Produto) => new Produto(),
                nameof(Setor) => new Setor(),
                nameof(Etiqueta) => new Etiqueta(),
                _ => null
            };
        }
        else
        {
            m_classe = p_solicitacao.ds_entidade switch
            {
                nameof(Produto) => JsonSerializer.Deserialize<Produto>(p_solicitacao.ds_parametros),
                nameof(Setor) => JsonSerializer.Deserialize<Setor>(p_solicitacao.ds_parametros),
                nameof(Etiqueta) => JsonSerializer.Deserialize<Etiqueta>(p_solicitacao.ds_parametros),
                _ => null
            };

            if (m_classe == null)
                throw new KeyNotFoundException();

            return m_classe;
        }
    }
}
