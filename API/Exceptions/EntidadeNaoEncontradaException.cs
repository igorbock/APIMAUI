namespace API.Exceptions;

public class EntidadeNaoEncontradaException : Exception
{
    public EntidadeNaoEncontradaException(string p_entidade) => throw new EntidadeNaoEncontradaException(string.Format(MensagensExceptions.C_EntidadeNaoEncontrada, p_entidade), this);

    public EntidadeNaoEncontradaException(string p_mensagem, Exception p_innerException) : base(p_mensagem, p_innerException) { }
}
