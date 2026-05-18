using Herbapedia.Client.Servicios;
using Herbapedia.Model;
using System.Threading.Tasks;
namespace Herbapedia.Client;
public partial class MostrarPlantaPage : ContentPage
{
    private readonly APIClient _api;
    private int plantId;

    public MostrarPlantaPage(APIClient api)
    {
        InitializeComponent();
        _api = api;
        CargarPlanta();
    }
    protected  async void CargarPlanta()
    {
        cv_Plantas.ItemsSource = new List<PlantModel>();
        base.OnAppearing(); 
        var plants = await _api.GetObjects<PlantModel>($"Plant/Filter?filter=all");
       
        if (plants != null && plants.Any()) 
        {
            cv_Plantas.ItemsSource = plants;
        }
    }


    public async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is PlantModel selectedPlant)
        {
            var navigationParameter = new Dictionary<string, object>
        {
            { "PlantId", selectedPlant.PlantId } 
        };

            await Shell.Current.GoToAsync(nameof(PlantPage), navigationParameter);
        }
    }
    public void OnCreateClicked(object sender, EventArgs e)
    {
         Shell.Current.GoToAsync("PlantPage");
    }

    private async void cv_Plantas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        PlantModel selectedPlant = e.CurrentSelection[0] as PlantModel;
        var navigationParameter =
            new Dictionary<string, object>
        {
            { "Plant", selectedPlant }
        };

        await Shell.Current.GoToAsync("PlantView", navigationParameter);

    }
}
