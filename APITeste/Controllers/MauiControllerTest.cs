using BibliotecaMAUI.Interfaces;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace APITeste.Controllers;

public class MauiControllerTest
{
    private Mock<Modelo> c_ModeloMock { get; set; }
    private MauiController? c_Controller { get; set; }
    private Mock<DbSet<Solicitacao>> c_MockSolicitacoes { get; set; }
    private Mock<DbSet<Setor>> c_MockSetores { get; set; }
    private Mock<DbSet<Produto>> c_MockProdutos { get; set; }
    private Mock<DbSet<Etiqueta>> c_MockEtiquetas { get; set; }
    private JsonSerializerOptions c_JsonSerializerOptions { get; set; }
    private OkObjectResult? c_OkObjectResult { get; set; }
    private ControllerContext c_ControllerContextGET { get; set; }
    private ControllerContext c_ControllerContextPOST { get; set; }
    private ControllerContext c_ControllerContextPUT { get; set; }
    private ControllerContext c_ControllerContextDELETE { get; set; }

    [SetUp]
    public void SetUp()
    {
        var m_requestGET = new Mock<HttpRequest>();
        m_requestGET.Setup(x => x.Method).Returns("GET");
        var m_httpContextGET = Mock.Of<HttpContext>(a => a.Request == m_requestGET.Object);
        c_ControllerContextGET = new ControllerContext() { HttpContext = m_httpContextGET };

        var m_requestPOST = new Mock<HttpRequest>();
        m_requestPOST.Setup(x => x.Method).Returns("POST");
        var m_httpContextPOST = Mock.Of<HttpContext>(a => a.Request == m_requestPOST.Object);
        c_ControllerContextPOST = new ControllerContext() { HttpContext = m_httpContextPOST };

        var m_requestPUT = new Mock<HttpRequest>();
        m_requestPUT.Setup(x => x.Method).Returns("PUT");
        var m_httpContextPUT = Mock.Of<HttpContext>(a => a.Request == m_requestPUT.Object);
        c_ControllerContextPUT = new ControllerContext() { HttpContext = m_httpContextPUT };

        var m_requestDELETE = new Mock<HttpRequest>();
        m_requestDELETE.Setup(x => x.Method).Returns("DELETE");
        var m_httpContextDELETE = Mock.Of<HttpContext>(a => a.Request == m_requestDELETE.Object);
        c_ControllerContextDELETE = new ControllerContext() { HttpContext = m_httpContextDELETE };

        var m_fonteDadosSolicitacoes = new List<Solicitacao>().AsQueryable();
        c_MockSolicitacoes = new Mock<DbSet<Solicitacao>>();
        c_MockSolicitacoes.As<IQueryable<Solicitacao>>().Setup(s => s.Provider).Returns(m_fonteDadosSolicitacoes.Provider);
        c_MockSolicitacoes.As<IQueryable<Solicitacao>>().Setup(s => s.Expression).Returns(m_fonteDadosSolicitacoes.Expression);
        c_MockSolicitacoes.As<IQueryable<Solicitacao>>().Setup(s => s.ElementType).Returns(m_fonteDadosSolicitacoes.ElementType);
        c_MockSolicitacoes.As<IQueryable<Solicitacao>>().Setup(s => s.GetEnumerator()).Returns(m_fonteDadosSolicitacoes.GetEnumerator);

        var m_fonteDadosSetores = new List<Setor>
        {
            new Setor { cd_codigo = 1, ds_setor = "Produção" },
            new Setor { cd_codigo = 2, ds_setor = "Vendas" }
        }.AsQueryable();
        c_MockSetores = new Mock<DbSet<Setor>>();
        c_MockSetores.As<IQueryable<Setor>>().Setup(s => s.Provider).Returns(m_fonteDadosSetores.Provider);
        c_MockSetores.As<IQueryable<Setor>>().Setup(s => s.Expression).Returns(m_fonteDadosSetores.Expression);
        c_MockSetores.As<IQueryable<Setor>>().Setup(s => s.ElementType).Returns(m_fonteDadosSetores.ElementType);
        c_MockSetores.As<IQueryable<Setor>>().Setup(s => s.GetEnumerator()).Returns(m_fonteDadosSetores.GetEnumerator);

        var m_fonteDeDadosProdutos = new List<Produto>
        {
            new Produto{cd_codigo = 1, ds_descricao = "Chapa de vidro temperado", ds_nome = "Chapa Temperada", vl_valor = 310.00M},
            new Produto{cd_codigo = 2, ds_descricao = "Perfil de alumínio", ds_nome = "Perfil Alum.", vl_valor = 120.00M},
            new Produto{cd_codigo = 3, ds_descricao = "Fechadura de inox", ds_nome = "Fechadura", vl_valor = 451.00M},
            new Produto{cd_codigo = 4, ds_descricao = "Chapa de vidro laminado", ds_nome = "Chapa Laminado", vl_valor = 225.67M},
        }.AsQueryable();
        c_MockProdutos = new Mock<DbSet<Produto>>();
        c_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.Provider).Returns(m_fonteDeDadosProdutos.Provider);
        c_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.Expression).Returns(m_fonteDeDadosProdutos.Expression);
        c_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.ElementType).Returns(m_fonteDeDadosProdutos.ElementType);
        c_MockProdutos.As<IQueryable<Produto>>().Setup(p => p.GetEnumerator()).Returns(m_fonteDeDadosProdutos.GetEnumerator);

        var m_fonteDeDadosEtiquetas = new List<Etiqueta>
        {
            new Etiqueta { cd_codigo = 1, cd_produto = 23, vl_m2 = 314.00M, vl_quantidade = 114, cd_setor = 8 },
        }.AsQueryable();
        c_MockEtiquetas = new Mock<DbSet<Etiqueta>>();
        c_MockEtiquetas.As<IQueryable<Etiqueta>>().Setup(p => p.Provider).Returns(m_fonteDeDadosEtiquetas.Provider);
        c_MockEtiquetas.As<IQueryable<Etiqueta>>().Setup(p => p.Expression).Returns(m_fonteDeDadosEtiquetas.Expression);
        c_MockEtiquetas.As<IQueryable<Etiqueta>>().Setup(p => p.ElementType).Returns(m_fonteDeDadosEtiquetas.ElementType);
        c_MockEtiquetas.As<IQueryable<Etiqueta>>().Setup(p => p.GetEnumerator()).Returns(m_fonteDeDadosEtiquetas.GetEnumerator);

        c_ModeloMock = new Mock<Modelo>();
        c_ModeloMock.Setup(a => a.Solicitacoes).Returns(c_MockSolicitacoes.Object);
        c_ModeloMock.Setup(a => a.Setores).Returns(c_MockSetores.Object);
        c_ModeloMock.Setup(a => a.Produtos).Returns(c_MockProdutos.Object);
        c_ModeloMock.Setup(a => a.Etiquetas).Returns(c_MockEtiquetas.Object);
        c_ModeloMock.Setup(a => a.SaveChanges()).Returns(null);

        c_JsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = false
        };
    }

    [Test]
    public void LeituraActionComCodigoTest()
    {
        c_Controller = new MauiController(c_ModeloMock.Object)
        {
            ControllerContext = c_ControllerContextGET
        };

        var m_json = @"{
        ""cd_codigo_interno"": 2,
        ""ds_parametros"": null,
        ""ds_entidade"": ""Setor""
}";
        var m_jsonSetor = "[{\"cd_codigo\":2,\"ds_setor\":\"Vendas\"}]";
        var m_result = c_Controller.CM_Ler(m_json);
        var m_okResult = m_result as OkObjectResult;
        var m_setor = (from setor in m_okResult?.Value as IQueryable<IEntidade>
                       select setor as Setor).ToList();
        var m_jsonResultado = JsonSerializer.Serialize(m_setor);
        Assert.That(m_jsonResultado, Is.EqualTo(m_jsonSetor));
        Assert.Pass();
    }

    [Test]
    public void LeituraActionTest()
    {
        c_Controller = new MauiController(c_ModeloMock.Object)
        {
            ControllerContext = c_ControllerContextGET
        };

        var m_json = @"{
        ""cd_codigo_interno"": 0,
        ""ds_parametros"": null,
        ""ds_entidade"": ""Setor""
}";
        var m_jsonSetor = "[{\"cd_codigo\":1,\"ds_setor\":\"Produção\"},{\"cd_codigo\":2,\"ds_setor\":\"Vendas\"}]";
        var m_result = c_Controller.CM_Ler(m_json);
        c_OkObjectResult = m_result as OkObjectResult;
        var m_setores = (from setor in c_OkObjectResult?.Value as IQueryable<IEntidade>
                         select setor as Setor).ToList();
        var m_jsonResultado = JsonSerializer.Serialize(m_setores, c_JsonSerializerOptions);
        Assert.That(m_jsonResultado, Is.EqualTo(m_jsonSetor));
        Assert.That(m_setores.Count, Is.EqualTo(2));
        Assert.Pass();
    }

    [Test]
    public void SalvarActionTest()
    {
        c_Controller = new MauiController(c_ModeloMock.Object)
        {
            ControllerContext = c_ControllerContextPOST
        };

        var m_json = @"{
        ""cd_codigo_interno"": 7,
        ""ds_parametros"": ""{\""cd_codigo\"":7,\""ds_setor\"":\""Carregamento\""}"",
        ""ds_entidade"": ""Setor""
}";
        var m_esperado = "{\"cd_codigo\":7,\"ds_setor\":\"Carregamento\"}";
        var m_response = c_Controller.CM_Salvar(m_json);
        c_OkObjectResult = m_response as OkObjectResult;
        var m_setor = c_OkObjectResult?.Value as Setor;
        var m_jsonResultado = JsonSerializer.Serialize(m_setor, c_JsonSerializerOptions);
        Assert.That(m_jsonResultado, Is.EqualTo(m_esperado));
        Assert.Pass();
    }

    [Test]
    public void EditarActionTest()
    {
        c_Controller = new MauiController(c_ModeloMock.Object)
        {
            ControllerContext = c_ControllerContextPUT
        };

        var m_json = @"{
        ""cd_codigo_interno"": 1,
        ""ds_parametros"": ""{\""cd_codigo\"":1,\""ds_descricao\"":\""Chapa de vidro laminado\"",\""ds_nome\"":\""Chapa Laminada\"",\""vl_valor\"":284.00}"",
        ""ds_entidade"": ""Produto""
}";
        var m_esperado = "{\"cd_codigo\":1,\"ds_nome\":\"Chapa Laminada\",\"ds_descricao\":\"Chapa de vidro laminado\",\"vl_valor\":284.00}";
        var m_response = c_Controller.CM_Editar(m_json);
        c_OkObjectResult = m_response as OkObjectResult;
        var m_produto = c_OkObjectResult?.Value as Produto;
        var m_jsonResultado = JsonSerializer.Serialize(m_produto, c_JsonSerializerOptions);
        Assert.That(m_jsonResultado, Is.EqualTo(m_esperado));
        Assert.Pass();
    }

    [Test]
    public void DeletarActionTest()
    {
        c_Controller = new MauiController(c_ModeloMock.Object)
        {
            ControllerContext = c_ControllerContextDELETE
        };

        var m_json = @"{
        ""cd_codigo_interno"": 1,
        ""ds_parametros"": null,
        ""ds_entidade"": ""Etiqueta""
}";
        var m_esperado = "{\"cd_codigo\":1,\"cd_produto\":23,\"vl_m2\":314.00,\"vl_quantidade\":114,\"cd_setor\":8}";
        var m_response = c_Controller.CM_Deletar(m_json);
        c_OkObjectResult = m_response as OkObjectResult;
        var m_etiqueta = c_OkObjectResult?.Value as Etiqueta;
        var m_jsonResultado = JsonSerializer.Serialize(m_etiqueta, c_JsonSerializerOptions);
        Assert.That(m_jsonResultado, Is.EqualTo(m_esperado));
        Assert.Pass();
    }
}
