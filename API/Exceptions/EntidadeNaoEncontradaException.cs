namespace API.Exceptions;

public class EntidadeNaoEncontradaException : Exception
{
    public EntidadeNaoEncontradaException(string p_entidade) => throw new EntidadeNaoEncontradaException($"A entidade \"{p_entidade}\" não está na base de dados e não foi possível encontrar registros", this);

    public EntidadeNaoEncontradaException(string p_mensagem, Exception p_innerException) : base(p_mensagem, p_innerException) { }
}
