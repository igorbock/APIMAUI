namespace API.Extensions;

public static class SolicitacaoExtension
{
    public static void CMX_EncerrarSolicitacao(this Solicitacao p_solicitacao, string p_resolucao)
    {
        p_solicitacao.dt_fim = DateTime.Now;
        p_solicitacao.ds_resolucao = p_resolucao;
    }
}
