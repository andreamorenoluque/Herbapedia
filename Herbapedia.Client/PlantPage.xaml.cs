using Herbapedia.Client.Servicios;
using Herbapedia.Model;
using System.Threading.Tasks;
namespace Herbapedia.Client;
[QueryProperty(nameof(PlantId), "PlantId")]
public partial class PlantPage : ContentPage, IQueryAttributable
{
    private readonly APIClient _api;
    private readonly Auth _auth;
    private PlantTypeModel selectedType;
    private int plantId;
    public int PlantId
    {
        get => plantId;
        set
        {
            plantId = value;
            OnPropertyChanged();
        }
    }
    public PlantPage(IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = this;
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();
    }
    public async void OnSavePlantClicked(object sender, EventArgs e)
    {
        try
        {
            if (selectedType == null)
            {
                await DisplayAlert("Error", "Debes seleccionar un tipo de planta", "OK");
                return;
            }
            if (plantId == null)
            {
                PlantModel newPlant = new PlantModel()
                {
                    PlantName = NamePlantEntry.Text,
                    PlantDescription = DescriptionPlantEntry.Text,
                    PlantTips = TipsEntry.Text,
                    PlantTypeId = selectedType.PlantTypeId,
                    PlantCreationDate = DateTime.Now.ToUniversalTime(),
                    PlantCreator = _auth.Usuario,
                    PlantEditor = _auth.Usuario,
                    Comments = null
                };

                PlantModel? plant = await _api.PostObject<PlantModel>("Plant/Create", newPlant);
            }
            else
            {
                PlantModel existingPlant = new PlantModel()
                {
                    PlantId = plantId,
                    PlantName = NamePlantEntry.Text,
                    PlantDescription = DescriptionPlantEntry.Text,
                    PlantTips = TipsEntry.Text,
                    PlantTypeId = selectedType.PlantTypeId,
                    PlantEditor = _auth.Usuario,
                    PlantModificationDate = DateTime.Now.ToUniversalTime()
                };
                PlantModel? plant = await _api.PutObject<PlantModel>("Plant/Modify", existingPlant);
            }
                await DisplayAlert("Éxito", "La planta ha sido guardada correctamente", "OK");
        }

        catch
        {
            await DisplayAlert("Fallo", "La planta no se pudo guardar", "OK");
        }
            
    }
    private async void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            selectedType = await _api.GetObject<PlantTypeModel>($"PlantType/Filter?filter=name&value={plantTypePicker.SelectedItem}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Debes seleccionar un tipo de planta válido.", "OK");
        }

    }
  


    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            plantId = Convert.ToInt32(query["PlantId"]);
            List<PlantModel> plants = await _api.GetObjects<PlantModel>($"Plant/Filter?filter=id&value={plantId}");

            NamePlantEntry.Text = plants.FirstOrDefault().PlantName;

            DescriptionPlantEntry.Text = plants.FirstOrDefault().PlantDescription;
            TipsEntry.Text = plants.FirstOrDefault().PlantTips;
            plantTypePicker.SelectedIndex = plantTypePicker.Items.IndexOf(plants.FirstOrDefault().PlantType.PlantTypeName);
        }

    }
}