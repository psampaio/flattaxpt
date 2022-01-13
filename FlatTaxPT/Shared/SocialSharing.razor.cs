using System.Net;
using Microsoft.AspNetCore.Components;

namespace FlatTaxPT.Shared;

public partial class SocialSharing
{
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    public string Message { get; set; } = "Calcula aqui quanto mais vais ganhar por mês com a flat tax da Iniciativa Liberal do que com os atuais escalões de IRS";

    private string EncodedMessage => WebUtility.UrlEncode(Message);
    private string EncodedUrl => WebUtility.UrlEncode(NavigationManager.Uri);
}