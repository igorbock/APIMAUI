namespace API.Exceptions;

public class SolicitacaoNulaException : Exception
{
    public SolicitacaoNulaException() => throw new SolicitacaoNulaException("A solicitação é nula", this);

    public SolicitacaoNulaException(string p_mensagem, Exception p_innerException) : base(p_mensagem, p_innerException) { }
}
