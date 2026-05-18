using Herbapedia.Client.Servicios;
using Herbapedia.Model;

using System.Threading.Tasks;
namespace Herbapedia.Client;
[QueryProperty(nameof(PostId), "PostId")]
public partial class NewPostPage : ContentPage, IQueryAttributable
{
    private readonly APIClient _api;
    private readonly Auth _auth;


    private int? postId;
    public int PostId
    {
        get => postId.Value;
        set
        {
            postId = value;
            
        }
    }

    public NewPostPage(IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = this;
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();
    }
    public async void OnEnviarClicked(object sender, EventArgs e)
    {
        try
        {

            if (postId == null)
            {
                PostModel newPost = new PostModel()
                {

                    PostContent = NewPostEntry.Text,

                    PostDate = DateTime.Now.ToUniversalTime(),
                    PostCreator = _auth.Usuario,

                    Comments = null
                };

                PostModel? post = await _api.PostObject<PostModel>("Post", newPost);
                if (post == null)
                {
                    await DisplayAlert("Error", "La publicación no se pudo guardar correctamente", "OK");
                }
                else
                {
                    await DisplayAlert("Éxito", "La publicación ha sido guardada correctamente", "OK");
                }
            }
            else
            {
                PostModel existingPost = new PostModel()
                {
                    PostId = postId.Value,

                    PostContent = NewPostEntry.Text,
                    PostDate = DateTime.Now.ToUniversalTime(),
                    PostCreator = _auth.Usuario,

                    Comments = null


                };
                PostModel? post = await _api.PutObject<PostModel>("Post", existingPost);
            }


        }

        catch
        {
            await DisplayAlert("Fallo", "La publicacion no se pudo guardar", "OK");
        }
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query) {
        if (query.Count > 0)
        {
            postId = Convert.ToInt32(query["PostId"]);
            List<PostModel> posts = await _api.GetObjects<PostModel>($"Post/Filter?filter=id&value={postId}");
            NewPostEntry.Text = posts.FirstOrDefault().PostContent;

        }



    }
}
