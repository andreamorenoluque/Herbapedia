using Herbapedia.Client.Servicios;
using Herbapedia.Model;
using System.Threading.Tasks;
namespace Herbapedia.Client;
[QueryProperty(nameof(Post), "Post")]
public partial class PostViewPage : ContentPage, IQueryAttributable
{
    private readonly APIClient _api;
    private readonly Auth _auth;
    private PostModel post;
    public PostModel Post
    {
        get => post;
        set
        {
            post = value;
            OnPropertyChanged();

        }
    }
    public PostViewPage(IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = this;
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            Post = query["Post"] as PostModel;
            Contenido.Text = Post.PostContent;
            Creador.Text = Post.PostCreator.UserName;
            Fecha.Date = Post.PostDate;
            List<CommentModel> comentarios = await _api.GetObject<List<CommentModel>>($"Comment/Filter?filter=POST&value={Post.PostId}");
            this.post.Comments = comentarios;
            cv_Comments.ItemsSource = comentarios.OrderByDescending(e => e.CommentDate);

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
                CommentPost = Post
            };

            CommentModel? comment = await _api.PostObject<CommentModel>("Comment", newComment);

            if (comment != null)
            {
                this.post.Comments.Add(newComment);
                cv_Comments.ItemsSource = this.post.Comments.OrderByDescending(e => e.CommentDate);
                
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
