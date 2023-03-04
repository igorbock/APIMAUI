namespace APITeste.Services;

public class ContextService
{
    private Modelo _Modelo;

    public ContextService(Modelo p_modelo)
    {
        _Modelo = p_modelo;
    }

    public List<Setor> CM_BuscarSetores()
        => (from b in _Modelo.Setores
            orderby b.ds_setor
            select b).ToList();

    public List<Produto> CM_BuscarProdutos()
        => (from p in _Modelo.Produtos
            orderby p.cd_codigo
            select p).ToList();
}
