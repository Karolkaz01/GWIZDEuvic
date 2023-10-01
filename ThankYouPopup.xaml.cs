using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace Gwizd;

public partial class ThankYouPopup : Popup
{
    public ThankYouPopup()
    {
        InitializeComponent();
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}