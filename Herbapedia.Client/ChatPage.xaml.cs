using Herbapedia.Client.Servicios;
using Herbapedia.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;


namespace Herbapedia.Client;

public partial class ChatPage : ContentPage
{
    private readonly APIClient _api;
    private readonly Auth _auth;


    public ChatPage(IServiceProvider services)
    {
        InitializeComponent();
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();

        LoadMessagesAsync();

    }

    private async void LoadMessagesAsync()
    {
        try
        {

            var messages = await _api.GetObjects<MessageModel>($"Message/Filter?filter=RECEIVER&value={_auth.Usuario.UserId}");
            if (messages != null)
            {
                cv_Mensajes.ItemsSource = messages.Select(u => u.MessageTransmitter).DistinctBy(u => u.UserId);
            }

        }
        catch (Exception e) { }
    }
    private async void cv_Mensajes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UserModel selectedMessage = e.CurrentSelection[0] as UserModel;
        var navigationParameter = new Dictionary<string, object>
        {
            {"Transmitter", selectedMessage }
        };
        await Shell.Current.GoToAsync("ChatMessage", navigationParameter);
    }
    
}
