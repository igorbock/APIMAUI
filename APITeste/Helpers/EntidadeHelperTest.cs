namespace APITeste.Helpers;

public class EntidadeHelperTest
{
    public EntidadeHelper C_EntidadeHelper { get; set; }
    public Solicitacao C_SolicitacaoGetSetor { get; set; }
    public Solicitacao? C_SolicitacaoNula { get; set; }
    public Solicitacao C_SolicitacaoDeleteProduto { get; set; }
    public Solicitacao C_SolicitacaoPostEtiqueta { get; set; }
    public Solicitacao C_SolicitacaoEntidadeInexsitente { get; set; }
    public Solicitacao C_SolicitacaoPutSetor { get; set; }

    public IQueryable<Setor> C_FonteDeDadosSetores { get; set; }
    public Mock<DbSet<Setor>> C_MockSetores { get; set; }
    public Mock<Modelo> C_MockModelo { get; set; }
    public ContextService C_ModeloService { get; set; }
    public List<Setor>? C_Setores { get; set; }

    public IQueryable<Produto> C_FonteDeDadosProdutos { get; set; }
    public Mock<DbSet<Produto>> C_MockProdutos { get; set; }
    public List<Produto>? C_Produtos { get; set; }

    [SetUp]
    public void Setup()
    {
        C_EntidadeHelper = new EntidadeHelper();
        C_SolicitacaoGetSetor = new Solicitacao()
        {
            cd_codigo = 1,
            cd_codigo_interno = 2,
            ds_entidade = "Setor",
            ds_metodo = "GET",
            ds_parametros = null,
            ds_resolucao = null,
            dt_inicio = new DateTime(2023, 03, 03, 12, 12, 12)
        };
        C_SolicitacaoDeleteProduto = new Solicitacao()
        {
            cd_codigo = 4,
            cd_codigo_interno = 5,
            ds_entidade = "Produto",
            ds_metodo = "DELETE",
            ds_parametros = null,
            ds_resolucao = null,
            dt_inicio = new DateTime(2023, 03, 03, 12, 12, 12)
        };
        C_SolicitacaoPostEtiqueta = new Solicitacao
        {
            cd_codigo = 6,
            cd_codigo_interno = 7,
            ds_entidade = "Etiqueta",
            ds_metodo = "POST",
            ds_parametros = "{\"cd_codigo\": 7, \"cd_etiqueta\": 2, \"cd_produto\": 20, \"vl_m2\": 3.00, \"vl_quantidade\": 4, \"cd_setor\": 5}",
            ds_resolucao = null,
            dt_inicio = new DateTime(2023, 03, 03, 12, 12, 12)
        };
        C_SolicitacaoEntidadeInexsitente = new Solicitacao
        {
            cd_codigo = 19,
            cd_codigo_interno = 82,
            ds_entidade = "Mingau",
            ds_metodo = "DELETE",
            ds_parametros = null,
            ds_resolucao = null,
            dt_inicio = new DateTime(2023, 03, 03, 12, 12, 12)
        };
        C_SolicitacaoPutSetor = new Solicitacao
        {
            cd_codigo = 105,
            cd_codigo_interno = 99,
            ds_entidade = "Setor",
            ds_metodo = "PUT",
            ds_parametros = "{\"cd_codigo\": 99, \"ds_setor\": \"Produção\"}",
            ds_resolucao = null,
            dt_inicio = new DateTime(2023, 03, 03, 12, 12, 12)
        };

        C_FonteDeDadosSetores = new List<Setor>
        {
            new Setor{cd_codigo = 1, ds_setor = "Produção"},
            new Setor{cd_codigo = 2, ds_setor = "Vendas"},
            new Setor{cd_codigo = 3, ds_setor = "Compras"}
        }.AsQueryable();
        C_MockSetores = new Mock<DbSet<Setor>>();
        C_MockSetores.As<IQueryable<Setor>>().Setup(s => s.Provider).Returns(C_FonteDeDadosSetores.Provider);
        C_MockSetores.As<IQueryable<Setor>>().Setup(s => s.Expression).Returns(C_FonteDeDadosSetores.Expression);
        C_MockSetores.As<IQueryable<Setor>>().Setup(s => s.ElementType).Returns(C_FonteDeDadosSetores.ElementType);
        C_MockSetores.As<IQueryable<Setor>>().Setup(s => s.GetEnumerator()).Returns(C_FonteDeDadosSetores.GetEnumerator);

        C_FonteDeDadosProdutos = new List<Produto>
        {
            new Produto{cd_codigo = 1, ds_descricao = "Chapa de vidro temperado", ds_nome = "Chapa Temperada", vl_valor = 310.00M},
            new Produto{cd_codigo = 2, ds_descricao = "Perfil de alumínio", ds_nome = "Perfil Alum.", vl_valor = 120.00M},
            new Produto{cd_codigo = 3, ds_descricao = "Fechadura de inox", ds_nome = "Fechadura", vl_valor = 451.00M},
            new Produto{cd_codigo = 4, ds_descricao = "Chapa de vidro laminado", ds_nome = "Chapa Laminado", vl_valor = 225.67M},
        }.AsQueryable();
        C_MockProdutos = new Mock<DbSet<Produto>>();
        C_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.Provider).Returns(C_FonteDeDadosProdutos.Provider);
        C_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.Expression).Returns(C_FonteDeDadosProdutos.Expression);
        C_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.ElementType).Returns(C_FonteDeDadosProdutos.ElementType);
        C_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.GetEnumerator()).Returns(C_FonteDeDadosProdutos.GetEnumerator);

        C_MockModelo = new Mock<Modelo>();
        C_MockModelo.Setup(c => c.Setores).Returns(C_MockSetores.Object);
        C_MockModelo.Setup(p => p.Produtos).Returns(C_MockProdutos.Object);

        C_ModeloService = new ContextService(C_MockModelo.Object);
    }

    [Test]
    public void SolicitacaoNulaTest()
    {
        try
        {
            var m_erro = C_EntidadeHelper.CM_RetornaEntidadeAPartirDaSolicitacao(C_SolicitacaoNula);
            Assert.Fail();
        }
        catch (SolicitacaoNulaException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("A solicitação é nula"));
            Assert.That(ex.InnerException?.GetType(), Is.EqualTo(typeof(SolicitacaoNulaException)));
            Assert.Pass();
        }
    }

    [Test]
    public void EntidadeSetorComMetodoGetTest()
    {
        var m_entidade = C_EntidadeHelper.CM_RetornaEntidadeAPartirDaSolicitacao(C_SolicitacaoGetSetor);
        Assert.That(m_entidade.GetType(), Is.EqualTo(typeof(Setor)));
        Assert.That(m_entidade.cd_codigo, Is.EqualTo(0));
        Assert.Pass();
    }

    [Test]
    public void EntidadeProdutoComMetodoDeleteTest()
    {
        var m_entidade = C_EntidadeHelper.CM_RetornaEntidadeAPartirDaSolicitacao(C_SolicitacaoDeleteProduto);
        Assert.That(m_entidade.GetType(), Is.EqualTo(typeof(Produto)));
        Assert.That(m_entidade.cd_codigo, Is.EqualTo(0));
        Assert.Pass();
    }

    [Test]
    public void EntidadeEtiquetaComMetodoPostTest()
    {
        var m_entidade = C_EntidadeHelper.CM_RetornaEntidadeAPartirDaSolicitacao(C_SolicitacaoPostEtiqueta);
        Assert.That(m_entidade.GetType(), Is.EqualTo(typeof(Etiqueta)));
        Assert.That(m_entidade.cd_codigo, Is.EqualTo(7));

        var m_etiqueta = m_entidade as Etiqueta;
        Assert.That(m_etiqueta, Is.Not.EqualTo(null));
        Assert.Multiple(() =>
        {
            Assert.That(m_etiqueta?.cd_etiqueta, Is.EqualTo(2));
            Assert.That(m_etiqueta?.cd_produto, Is.EqualTo(20));
            Assert.That(m_etiqueta?.vl_m2, Is.EqualTo(3.00M));
            Assert.That(m_etiqueta?.vl_quantidade, Is.EqualTo(4));
            Assert.That(m_etiqueta?.cd_setor, Is.EqualTo(5));
        });
        Assert.Pass();
    }

    [Test]
    public void EntidadeInexistenteTest()
    {
        try
        {
            var m_entidade = C_EntidadeHelper.CM_RetornaEntidadeAPartirDaSolicitacao(C_SolicitacaoEntidadeInexsitente);
            Assert.Fail();
        }
        catch (EntidadeNaoEncontradaException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("A entidade \"Mingau\" não está na base de dados e não foi possível encontrar registros"));
            Assert.That(ex.InnerException?.GetType(), Is.EqualTo(typeof(EntidadeNaoEncontradaException)));
            Assert.Pass();
        }
    }

    [Test]
    public void EntidadeSetorComMetodoPutTest()
    {
        var m_entidade = C_EntidadeHelper.CM_RetornaEntidadeAPartirDaSolicitacao(C_SolicitacaoPutSetor);
        Assert.That(m_entidade.GetType(), Is.EqualTo(typeof(Setor)));
        Assert.That(m_entidade.cd_codigo, Is.EqualTo(99));

        var m_setor = m_entidade as Setor;
        Assert.That(m_setor, Is.Not.EqualTo(null));
        Assert.That(m_setor?.ds_setor, Is.EqualTo("Produção"));
        Assert.Pass();
    }

    [Test]
    public void ObterRegistroDeSetorNaBaseMockadaTest()
    {
        C_Setores = C_ModeloService.CM_BuscarSetores();
        var m_queryableSetor = C_EntidadeHelper.CM_ObtemRegistrosDaEntidadeNaBase(C_MockModelo.Object, C_SolicitacaoGetSetor);

        Assert.That(C_Setores.Count, Is.EqualTo(3));
        Assert.That(C_Setores[0].ds_setor, Is.EqualTo("Compras"));
        Assert.That(C_Setores[1].ds_setor, Is.EqualTo("Produção"));
        Assert.That(C_Setores[2].ds_setor, Is.EqualTo("Vendas"));

        var m_setor = from setor in m_queryableSetor
                      where setor.cd_codigo == 1
                      select setor as Setor;

        Assert.That(m_queryableSetor.Count(), Is.EqualTo(3));
        Assert.That(m_setor.ToList().Last().ds_setor, Is.EqualTo("Produção"));

        Assert.Pass();
    }

    [Test]
    public void ObterRegistroDeProdutoNaBaseMockadaTest()
    {
        C_Produtos = C_ModeloService.CM_BuscarProdutos();
        var m_queryableProduto = C_EntidadeHelper.CM_ObtemRegistrosDaEntidadeNaBase(C_MockModelo.Object, C_SolicitacaoDeleteProduto);

        Assert.That(C_Produtos.Count, Is.EqualTo(4));
        Assert.That(C_Produtos[0].ds_nome, Is.EqualTo("Chapa Temperada"));
        Assert.That(C_Produtos[1].ds_nome, Is.EqualTo("Perfil Alum."));
        Assert.That(C_Produtos[2].ds_nome, Is.EqualTo("Fechadura"));
        Assert.That(C_Produtos[3].ds_nome, Is.EqualTo("Chapa Laminado"));

        var m_produto = from produto in m_queryableProduto
                      where produto.cd_codigo == 1
                      select produto as Produto;

        Assert.That(m_queryableProduto.Count(), Is.EqualTo(4));
        Assert.That(m_produto.ToList().Last().ds_nome, Is.EqualTo("Chapa Temperada"));

        Assert.Pass();
    }
}