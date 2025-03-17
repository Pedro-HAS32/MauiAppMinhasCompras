using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    private CancellationTokenSource _cancellationTokenSource;

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista; // lst_produtos � o x:name da linha 28 do XAML.
    }

    protected async override void OnAppearing()
    {
        await CarregarProdutos();
    }

    // M�todo para carregar produtos (com ou sem busca)
    private async Task CarregarProdutos(string searchText = null)
    {
        lista.Clear(); // Limpa a lista antes de carregar novos dados

        List<Produto> tmp;
        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Carrega todos os produtos se n�o houver texto de busca
            tmp = await App.Db.GetAll();
        }
        else
        {
            // Realiza a busca com o texto fornecido
            tmp = await App.Db.Search(searchText);
        }

        // Adiciona os produtos � lista
        foreach (var item in tmp)
        {
            lista.Add(item);
        }
    }

    // Evento de busca din�mica
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            // Cancela a opera��o anterior, se houver
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            // Adiciona um atraso (debounce) de 300ms antes de executar a busca
            await Task.Delay(300, _cancellationTokenSource.Token);

            string q = e.NewTextValue;

            // Atualiza a lista com os resultados da busca
            await CarregarProdutos(q);
        }
        catch (TaskCanceledException)
        {
            // Ignora a exce��o se a tarefa foi cancelada (debounce)
        }
        catch (Exception ex)
        {
            // Trata outros erros (ex: falha na conex�o com o banco de dados)
            await DisplayAlert("Erro", "Ocorreu um erro durante a busca: " + ex.Message, "OK");
        }
    }

    // Evento do bot�o "Adicionar Produto"
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops, algo deu errado!", ex.Message, "OK");
        }
    }

    // Evento do bot�o "Somar"
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"O total � {soma:C}";
        DisplayAlert("Total dos produtos", msg, "OK");
    }

    // Evento do menu de contexto (remover produto)
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        // Obt�m o produto selecionado
        var menuItem = sender as MenuItem;
        var produto = menuItem?.BindingContext as Produto; // Usa BindingContext para obter o item

        if (produto != null)
        {
            // Remove o produto da lista e do banco de dados
            bool confirmacao = await DisplayAlert("Remover", $"Deseja remover {produto.Descricao}?", "Sim", "N�o");
            if (confirmacao)
            {
                await App.Db.Delete(produto.id);
                lista.Remove(produto);
            }
        }
    }
}