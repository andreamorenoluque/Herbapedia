
using Herbapedia.Client.Servicios;
using Herbapedia.Model;
using System.Threading.Tasks;
namespace Herbapedia.Client;
[QueryProperty(nameof(Plant), "Plant")]
public partial class PlantViewPage : ContentPage, IQueryAttributable
{
    private PlantModel plant;
	public PlantModel  Plant
    {
        get => plant;
        set
        {
            plant = value;
            OnPropertyChanged();
        }
    }
    private readonly APIClient _api;
    private readonly Auth _auth;
    public PlantViewPage(IServiceProvider services)
	{
		InitializeComponent();
        BindingContext = this;
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            if (query.Count > 0)
            {
                Plant = query["Plant"] as PlantModel;
                DescriptionPlant.Text = Plant.PlantDescription;
                TipsPlant.Text = Plant.PlantTips;
                PlantTypeName.Text = Plant.PlantType.PlantTypeName;
                List<CommentModel> comentarios = await _api.GetObject<List<CommentModel>>($"Comment/Filter?filter=PLANT&value={Plant.PlantId}");
                if (comentarios.Count > 0)
                {
                    this.plant.Comments = comentarios;
                    cv_Comments.ItemsSource = comentarios.OrderByDescending(e => e.CommentDate);
                }
                this.Title = Plant.PlantName;

            }
        }
        catch (Exception ex)
        {

        }
            
    }
    public async void OnCommentClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(commentEntry.Text))
        {
            CommentModel newComment = new CommentModel
            {
                CommentCreatorUserId = _auth.Usuario.UserId,
                CommentCreator = _auth.Usuario,
                CommentContent = commentEntry.Text,
                CommentDate = DateTime.Now,
                CommentPlant = Plant
            };

            CommentModel? comment = await _api.PostObject<CommentModel>("Comment", newComment);

            if (comment != null)
            {
                this.plant.Comments.Add(newComment);
                cv_Comments.ItemsSource = this.plant.Comments.OrderByDescending(e => e.CommentDate);

            }

            await DisplayAlert("Comentario enviado", "Tu comentario ha sido añadido.", "OK");
            commentEntry.Text = string.Empty;

        }
        else
        {
            await DisplayAlert("Error", "El comentario no puede estar vacío.", "OK");
        }
    }
}