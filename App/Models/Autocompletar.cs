namespace App.Models;

public class Autocompletar
{
    public List<CentroDeCusto> centrosDeCusto { get; set; }
    public List<Cliente> clientes { get; set; }
    public List<ContaBancaria> contasBancarias { get; set; }
    public List<Empresa> empresas { get; set; }
    public List<FormaPagamento> formasPagamento { get; set; }
    public List<Grupo> grupos { get; set; }
    public List<PlanoDeConta> planosDeConta { get; set; }
    public List<TabelaDePreco> tabelaDePreco { get; set; }
    public List<Deposito> depositos { get; set; }
    public List<Produto> produtos { get; set; }
    public List<ModeloEtiqueta> modelosEtiquetas { get; set; }
}