using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Produto p = new Produto
			{
				
				Descricao = txt_Descricao.Text,
				
				Quantidade = Convert.ToDouble(txt_Quantidade.Text),
				Preco = Convert.ToDouble(txt_Preco.Text)
			};

			await App.Db.Insert(p);
			await DisplayAlert("Sucesso", "Registro inserido com sucesso", "OK");
            await Navigation.PopAsync();

        } catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "Ok");
		}
    }
}