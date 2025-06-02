using System.Collections.Generic;
using App.Controllers;

namespace App.Repository
{
    public interface IProdutoRepository
    {
        ProdutoEscolhido? GetById(string id);
    }
}
