using App.Models;
using App.Repository;
using VendaERP.Core;

namespace App.Services;

public class AutocompletarService
{
    private readonly ClienteRepository _clienteRepository;
    private readonly EmpresaRepository _empresaRepository;
    private readonly FormaPagamentoRepository _formaPagamentoRepository;
    private readonly PlanoDeContaRepository _planoDeContaRepository;
    private readonly ContaBancariaRepository _contaBancariaRepository;
    private readonly GrupoRepository _grupoRepository;
    private readonly CentroDeCustoRepository _centrosDeCustoRepository;
    private readonly TabelaDePrecoRepository _tabelaDePrecoRepository;
    private readonly DepositoRepository _depositoRepository;
    private readonly ProdutoRepository _produtoRepository;
    private readonly EtiquetasPadroesRepository _etiquetasPadroesRepository;
    private Autocompletar? _autocompletar;

    public AutocompletarService(ClienteRepository clienteRepository, EmpresaRepository empresaRepository,
        FormaPagamentoRepository formaPagamentoRepository, PlanoDeContaRepository planoDeContaRepository,
        ContaBancariaRepository contaBancariaRepository, GrupoRepository grupoRepository,
        CentroDeCustoRepository centrosDeCustoRepository, TabelaDePrecoRepository tabelaDePrecoRepository,
        DepositoRepository depositoRepository, ProdutoRepository produtoRepository,
        EtiquetasPadroesRepository etiquetasPadroesRepository)
    {
        _clienteRepository = clienteRepository;
        _empresaRepository = empresaRepository;
        _formaPagamentoRepository = formaPagamentoRepository;
        _planoDeContaRepository = planoDeContaRepository;
        _contaBancariaRepository = contaBancariaRepository;
        _grupoRepository = grupoRepository;
        _centrosDeCustoRepository = centrosDeCustoRepository;
        _tabelaDePrecoRepository = tabelaDePrecoRepository;
        _depositoRepository = depositoRepository;
        _produtoRepository = produtoRepository;
        _etiquetasPadroesRepository = etiquetasPadroesRepository;
    }

    public Autocompletar getAutocompletar()
    {
        return _autocompletar ??= CreateAutoCompletar();
    }

    private Autocompletar CreateAutoCompletar()
    {
        return new Autocompletar
        {
            clientes = _clienteRepository.GetAllClientes(),
            empresas = _empresaRepository.GetAllEmpresas(),
            formasPagamento = _formaPagamentoRepository.GetAllFormasPagamento(),
            planosDeConta = _planoDeContaRepository.GetAllPlanosDeConta(),
            contasBancarias = _contaBancariaRepository.GetAllContasBancarias(),
            grupos = _grupoRepository.GetAllGrupos(),
            centrosDeCusto = _centrosDeCustoRepository.GetAllCentrosDeCusto(),
            tabelaDePreco = _tabelaDePrecoRepository.GetAllTabelasDePreco(),
            depositos = _depositoRepository.GetAllDepositos(),
            produtos = _produtoRepository.GetAllProdutos(),
            modelosEtiquetas = _etiquetasPadroesRepository.GetAllModelosEtiquetas()
        };
    }
}