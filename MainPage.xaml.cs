namespace Gwizd;

public partial class MainPage : ContentPage
{
    private readonly MenuPage _menuPage;

    public MainPage(MenuPage menuPage)
    {
        InitializeComponent();
        _menuPage = menuPage;
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(_menuPage, true);
    }
}