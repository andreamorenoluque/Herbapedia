using Herbapedia.Client.Servicios;
using Herbapedia.Model;

namespace Herbapedia.Client;

public partial class ForoPage : ContentPage
{
    private readonly APIClient _api;
    private readonly Auth _auth;
    private int postId;


    public ForoPage(IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = this;
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();
        CargarPost();
    }

    protected async void CargarPost()
    {
        cv_Posts.ItemsSource = new List<PostModel>();
        base.OnAppearing();
        var posts = await _api.GetObjects<PostModel>($"Post/Filter?filter=all");

        if (posts != null && posts.Any())
        {
            cv_Posts.ItemsSource = posts;
        }
    }
    public async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is PostModel selectedPost)
        {
           
                var navigationParameter = new Dictionary<string, object>
                {
                    { "PostId", selectedPost.PostId }
                };

                await Shell.Current.GoToAsync(nameof(NewPostPage), navigationParameter);
            
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(NewPostPage));
        }
    }
    private async void cv_Posts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        PostModel selectedPost = e.CurrentSelection[0] as PostModel;
        var navigationParameter =
            new Dictionary<string, object>
        {
            { "Post", selectedPost }
                
        };

        await Shell.Current.GoToAsync("PostViewPage", navigationParameter);

    }
 
    public async void OnCreatePostClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("NewPostPage");

    }


}
