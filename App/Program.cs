using App.Repository;
using App.Services;
using VendaERP.Core;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("MongoConnection"));

builder.Services.AddSingleton<DBAccess>();

builder.Services.AddSingleton<ProdutoRepository>();
builder.Services.AddSingleton<EtiquetasPadroesRepository>();
builder.Services.AddSingleton<EmpresaRepository>();
builder.Services.AddSingleton<ClienteRepository>();
builder.Services.AddSingleton<FormaPagamentoRepository>();
builder.Services.AddSingleton<PlanoDeContaRepository>();
builder.Services.AddSingleton<ContaBancariaRepository>();
builder.Services.AddSingleton<GrupoRepository>();
builder.Services.AddSingleton<CentroDeCustoRepository>();
builder.Services.AddSingleton<TabelaDePrecoRepository>();
builder.Services.AddSingleton<DepositoRepository>();
builder.Services.AddSingleton<AutocompletarService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
