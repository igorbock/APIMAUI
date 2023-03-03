namespace API.Exceptions;

public class NenhumRegistroEncontradoException : Exception
{
    public NenhumRegistroEncontradoException() => throw new NenhumRegistroEncontradoException("Nenhum registro foi encontrado na base de dados");

    public NenhumRegistroEncontradoException(string p_mensagem) : base(p_mensagem) { }
}
