using Gwizd.Clients;
using Gwizd.Services;

namespace Gwizd;

public partial class MainPage : ContentPage
{
    //private bool IsBusy = false;

    public MainPage()
	{
        InitializeComponent();
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuPage());
    }
}