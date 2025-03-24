using MauiAppMinhasCompras.Models;
namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_Descricao.Text,
                Quantidade = Convert.ToDouble(txt_Quantidade.Text),
                Preco = Convert.ToDouble(txt_Preco.Text)
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso", "Registro atualizado com sucesso", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

}