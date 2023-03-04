namespace API.Helpers;

public static class JsonHelper
{
    public static Tipo CM_DeserializaOuLancaException<Tipo>(string p_json)
        => JsonSerializer.Deserialize<Tipo>(p_json) ?? throw new Exception("Impossível desserializar.");
}
