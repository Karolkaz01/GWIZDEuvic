using Gwizd.Clients;
using Org.Apache.Http.Client;
using System.Net.Http.Json;

namespace Gwizd;

public partial class FormPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly MainPage _mainPage;
    private readonly string id;

    public FormPage(HttpClient httpClient,MainPage mainPage,string id,string species, string race)
    {
        _httpClient = httpClient;
        _mainPage = mainPage;
        this.id = id;
        ((Entry)FindByName("SpeciesEntry")).Text = species;
        ((Entry)FindByName("RaceEntry")).Text = race;
        ((Entry)FindByName("TypeEntry")).Text = race;
        InitializeComponent();
    }

    private async Task Button_ClickedAsync(object sender, EventArgs e)
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();
            var species = ((Entry)FindByName("SpeciesEntry")).Text;
            var race = ((Entry)FindByName("RaceEntry")).Text;
            var type = ((Entry)FindByName("TypeEntry")).Text;

            var response = await _httpClient.PostAsJsonAsync("https://mezvs42ny3.execute-api.eu-central-1.amazonaws.com/default/events",
                new AddEventDTO
                {
                    id = id,
                    animal_type = type,
                    label = species,
                    breed = race,
                    event_type = "reported",
                    lat = location.Latitude,
                    lng = location.Longitude
                });
            Navigation.PushAsync(_mainPage);
        }
        catch(Exception ex)
        {
        }
    }
}