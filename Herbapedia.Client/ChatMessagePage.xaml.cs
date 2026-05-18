using Herbapedia.Client.Servicios;
using Herbapedia.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;
namespace Herbapedia.Client;
[QueryProperty(nameof(TransmitterUser), "Transmitter")]
public partial class ChatMessagePage : ContentPage, IQueryAttributable
{
    private readonly APIClient _api;
    private readonly Auth _auth;
    public UserModel TransmitterUser { get; set; }
    public ObservableCollection<MessageModel> Messages { get; set; } = new();
    public List<UserModel> Users { get; set; } = new();


    public ChatMessagePage(IServiceProvider services)
	{
		InitializeComponent();
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();
        BindingContext = this;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            try
            {
                TransmitterUser = query["Transmitter"] as UserModel;
                List<MessageModel>? messages = await _api.GetObjects<MessageModel>($"Message/Filter?filter=CHAT&value={_auth.Usuario.UserId};{TransmitterUser.UserId}");
                foreach (MessageModel message in messages)
                {
                    if (message.MessageReceiverUserId == _auth.Usuario.UserId)
                    {
                        message.IsTransmitter = true;
                    }
                    else
                    {
                        message.IsReceiver = true;
                    }
                }
                Messages =  new ObservableCollection<MessageModel>(messages);
                cv_Lista_Mensajes.ItemsSource = messages;
                this.Title = TransmitterUser.UserName;
            }
            catch (Exception ex)
            {

            }
        }
    }

    private async void SendMessageCommand(object sender, EventArgs e)
    {


        MessageModel message = new MessageModel
        {
            MessageTransmitter = _auth.Usuario,
            MessageTransmitterUserId = _auth.Usuario.UserId,
            MessageReceiver = TransmitterUser,
            MessageReceiverUserId = TransmitterUser.UserId,
            MessageContent = entry_Mensaje.Text,
            MessageDate = DateTime.UtcNow,
            IsReceiver = true
        };

        MessageModel response = await _api.PostObject<MessageModel>("Message", message);
        Messages.Add(message);
        cv_Lista_Mensajes.ItemsSource = Messages;

        entry_Mensaje.Text = string.Empty;
    }
    
}