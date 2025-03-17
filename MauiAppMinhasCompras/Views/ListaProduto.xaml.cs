using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista; //lst_proutos é o x:name da linha 28 do xaml.
	}

    protected async override void OnAppearing()
    {
		List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => lista.Add(i)); 
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{	
			Navigation.PushAsync(new Views.NovoProduto());

		}	catch (Exception ex)
		{
			DisplayAlert("Ops Algo deu errado!", ex.Message, "Ok");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		string q = e.NewTextValue;

		lista.Clear();

		List<Produto> tmp = await App.Db.Search(q);

		tmp.ForEach (i => lista.Add(i)) ;
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos produtos", msg, "OK");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
		//TO-DO:
		//Adicionar aqui o comando para remover um produto da lista

		
    }
}