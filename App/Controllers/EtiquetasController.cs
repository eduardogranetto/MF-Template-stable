using System.Net;
using App.Models;
using App.Repository;
using App.VendaERP.Core.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using VendaERP.Core;

namespace App.Controllers
{
    public class EtiquetasController : Controller
    {
        private readonly IEtiquetasPadroesRepository _etiquetasPadroesRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly Autocompletar _autocompletar;
        private readonly ILogger<EtiquetasController> _logger;

        public EtiquetasController(
            IEtiquetasPadroesRepository etiquetasPadroesRepository,
            IEmpresaRepository empresaRepository,
            IProdutoRepository produtoRepository,
            DBAccess db, ILogger<EtiquetasController> logger)
        {
            _etiquetasPadroesRepository = etiquetasPadroesRepository;
            _empresaRepository = empresaRepository;
            _produtoRepository = produtoRepository;
            _autocompletar = new Autocompletar(db);
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
                ViewBag.message = TempData["message"];
            return View(_autocompletar);
        }

        public IActionResult NewModel() => View();

        public IActionResult SaveNewModel(string nome, string papel, string larguraPapel, string alturaPapel, string? larguraEtiqueta, string? alturaEtiqueta, string? espacamentoHorizontal, string? espacamentoVertical, string? margemEsquerda, string? margemSuperior, string? zoomImpressao, int? tamanhoFonte, int? tamanhoPreco, string? alturaBarras)
        {
            var padrao = new DtoEtiquetasPadroes
            {
                Nome = nome,
                Papel = Enum.Parse<TipoPapel>(papel),
                LarguraPapel = double.Parse(larguraPapel),
                AlturaPapel = double.Parse(alturaPapel),
                Largura = ParseNullableDouble(larguraEtiqueta),
                Altura = ParseNullableDouble(alturaEtiqueta),
                EspacamentoHorizontal = ParseNullableDouble(espacamentoHorizontal),
                EspacamentoVertical = ParseNullableDouble(espacamentoVertical),
                MargemEsquerda = ParseNullableDouble(margemEsquerda),
                MargemSuperior = ParseNullableDouble(margemSuperior),
                ZoomImpressao = ParseNullableDouble(zoomImpressao),
                TamanhoFonte = tamanhoFonte,
                TamanhoPreco = tamanhoPreco,
                AlturaEAN = ParseNullableDouble(alturaBarras)
            };
            _etiquetasPadroesRepository.Insert(padrao);
            _logger.LogInformation($"SaveNewModel {padrao.ZoomImpressao}");
            return Redirect("/Etiquetas/ListModels");
        }

        public IActionResult ListModels(int pageNumber)
        {
            var listModels = _etiquetasPadroesRepository.GetAll();
            var pager = new Pager(listModels.Count, pageNumber, 15);
            var model = listModels.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
            ViewBag.pager = pager;
            return View(model);
        }

        public HttpResponseMessage RemoveModel(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _etiquetasPadroesRepository.Delete(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [Route("Etiquetas/EditModel/{id}")]
        public IActionResult EditModel(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.modelo = _etiquetasPadroesRepository.GetById(id);
                if (ViewBag.modelo != null && ViewBag.ZoomImpressao != null)
                {
                    _logger.LogInformation($"EditModel {ViewBag.modelo.ZoomImpressao}");
                }
            }
            return View();
        }

        public IActionResult UpdateModel(string id, string nome, string papel, string larguraPapel, string alturaPapel, string? larguraEtiqueta, string? alturaEtiqueta, string? espacamentoHorizontal, string? espacamentoVertical, string? margemEsquerda, string? margemSuperior, string? zoomImpressao, int? tamanhoFonte, int? tamanhoPreco, string? alturaBarras)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _logger.LogInformation($"UpdateModel {zoomImpressao}");
                var modelo = new DtoEtiquetasPadroes
                {
                    Id = id,
                    Nome = nome,
                    Papel = Enum.Parse<TipoPapel>(papel),
                    LarguraPapel = double.Parse(larguraPapel),
                    AlturaPapel = double.Parse(alturaPapel),
                    Largura = ParseNullableDouble(larguraEtiqueta),
                    Altura = ParseNullableDouble(alturaEtiqueta),
                    EspacamentoHorizontal = ParseNullableDouble(espacamentoHorizontal),
                    EspacamentoVertical = ParseNullableDouble(espacamentoVertical),
                    MargemEsquerda = ParseNullableDouble(margemEsquerda),
                    MargemSuperior = ParseNullableDouble(margemSuperior),
                    ZoomImpressao = ParseNullableDouble(zoomImpressao),
                    TamanhoFonte = tamanhoFonte,
                    TamanhoPreco = tamanhoPreco,
                    AlturaEAN = ParseNullableDouble(alturaBarras)
                };
                _etiquetasPadroesRepository.Update(id, modelo);
            }
            return Redirect($"/Etiquetas/EditModel/{id}");
        }

        [HttpPost]
        public IActionResult Baixar(string? empresa, string? etiqueta, string? clienteFornecedor, string? tabelaDePreco, bool dadoLadoCodigoBarras, bool imprimirCodigoBarras, bool imprimirNumeroCodigoBarras, bool imprimirCodigo, bool imprimirNome, bool imprimirPreco, bool imprimirMarca, bool imprimirBorda, bool imprimirLote, bool imprimirNumeroSerie, List<string> itens)
        {
            if (itens == null || itens.Count == 0)
            {
                TempData["message"] = "Ao menos um item deve ser inserido";
                return Redirect("/Etiquetas/Index");
            }
            if (string.IsNullOrEmpty(empresa))
            {
                TempData["message"] = "Erro: ID da empresa vazia ou nula.";
                return Redirect("/Etiquetas/Index");
            }
            var empresaObj = _empresaRepository.GetById(empresa);
            if (empresaObj == null || string.IsNullOrEmpty(empresaObj.NomeFantasia))
            {
                TempData["message"] = "Erro: Empresa não encontrada";
                return Redirect("/Etiquetas/Index");
            }
            if (string.IsNullOrEmpty(etiqueta))
            {
                TempData["message"] = "Erro: ID do modelo de etiqueta vazia ou nula.";
                return Redirect("/Etiquetas/Index");
            }
            var modelEtiqueta = _etiquetasPadroesRepository.GetById(etiqueta);
            if (modelEtiqueta == null)
            {
                TempData["message"] = "Erro: Modelo de etiqueta não encontrado";
                return Redirect("/Etiquetas/Index");
            }
            var listaItens = new List<ProdutoEscolhido>();
            foreach (var item in itens)
            {
                var prod = item.Split(", ");
                if (prod.Length < 2) continue;
                var produdo = _produtoRepository.GetById(prod[0]);
                if (produdo != null)
                {
                    produdo.Quantidade = int.TryParse(prod[1], out var qtd) ? qtd : 0;
                    if (prod.Length > 2 && !string.IsNullOrEmpty(prod[2]))
                        produdo.Lote = prod[2];
                    if (prod.Length > 3 && !string.IsNullOrEmpty(prod[3]))
                        produdo.NumeroSerie = prod[3];
                    listaItens.Add(produdo);
                }
            }
            ViewBag.opcoesSelecionadas = new OpcoesSelecionadas
            {
                DadoLadoCodigoBarras = dadoLadoCodigoBarras,
                ImprimirBorda = imprimirBorda,
                ImprimirCodigo = imprimirCodigo,
                ImprimirCodigoBarras = imprimirCodigoBarras,
                ImprimirLote = imprimirLote,
                ImprimirMarca = imprimirMarca,
                ImprimirNome = imprimirNome,
                ImprimirNumeroCodigoBarras = imprimirNumeroCodigoBarras,
                ImprimirNumeroSerie = imprimirNumeroSerie,
                ImprimirPreco = imprimirPreco,
            };
            ViewBag.nomeEmpresa = empresaObj.NomeFantasia;
            ViewBag.modelEtiqueta = modelEtiqueta;
            ViewBag.listaItens = listaItens;
            return View();
        }

        [HttpPost]
        public string GetProduto(string deposito, string produto)
        {
            var produtoEscolhido = _produtoRepository.GetById(produto);
            if (produtoEscolhido != null)
            {
                return $"{{\"Id\":\"{produtoEscolhido.Id}\",\"Codigo\":\"{produtoEscolhido.Codigo}\",\"Nome\":\"{produtoEscolhido.Nome.Replace("\"", "\\\"")}\",\"PrecoVenda\":\"{produtoEscolhido.Preco}\",\"Marca\":\"{produtoEscolhido.Marca}\",\"NumeroSerie\":\"{produtoEscolhido.NumeroSerie}\"}}";
            }
            return "Erro";
        }

        private static double? ParseNullableDouble(string? value)
        {
            return double.TryParse(value, out var result) ? result : null;
        }
    }

    public class ProdutoEscolhido
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("Id")]
        public string Id { get; set; }
        [BsonElement("CodigoNFe")]
        public string Codigo { get; set; }
        [BsonElement("Nome")]
        public string Nome { get; set; }
        [BsonElement("PrecoVenda")]
        public double Preco { get; set; }
        [BsonElement("Marca")]
        public string Marca { get; set; }
        [BsonElement("NumeroSerie")]
        public string NumeroSerie { get; set; }
        [BsonElement("EAN_NFe")]
        public string CodigoBarras { get; set; }
        [BsonIgnore]
        public string Lote { get; set; }
        [BsonIgnore]
        public int Quantidade { get; set; }
    }

    class OpcoesSelecionadas
    {
        public bool DadoLadoCodigoBarras { get; set; }
        public bool ImprimirCodigoBarras { get; set; }
        public bool ImprimirNumeroCodigoBarras { get; set; }
        public bool ImprimirCodigo { get; set; }
        public bool ImprimirNome { get; set; }
        public bool ImprimirPreco { get; set; }
        public bool ImprimirMarca { get; set; }
        public bool ImprimirBorda { get; set; }
        public bool ImprimirLote { get; set; }
        public bool ImprimirNumeroSerie { get; set; }
    }
}
