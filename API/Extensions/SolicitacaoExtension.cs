using System.Text.Json.Nodes;

namespace API.Extensions;

public static class SolicitacaoExtension
{
    public static void CMX_EncerrarSolicitacao(this Solicitacao p_solicitacao, string p_resolucao)
    {
        if (p_solicitacao == null)
            throw new SolicitacaoNulaException();

        p_solicitacao.dt_fim = DateTime.Now;
        p_solicitacao.ds_resolucao = p_resolucao;
    }

    public static void CMX_EncerrarSolicitacao(this Solicitacao p_solicitacao, string p_resolucao, string p_entidade, string p_json)
    {
        if (p_solicitacao == null)
            throw new SolicitacaoNulaException();

        var m_jsonNode = JsonNode.Parse(p_json);
        var m_opcoesSerializacao = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };
        var m_jsonFormatado = m_jsonNode?.ToJsonString(m_opcoesSerializacao);

        p_solicitacao.dt_fim = DateTime.Now;
        p_solicitacao.ds_resolucao = string.Format(p_resolucao, p_entidade, m_jsonFormatado);
    }

    public static bool CMX_VerificaSeMetodoEhGetOuDelete(this Solicitacao p_solicitacao)
        => p_solicitacao.ds_metodo == "GET" || p_solicitacao.ds_metodo == "DELETE" ? true : false;
}
