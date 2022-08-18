using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using Faith.Shared.Extensions;
using Faith.Shared.Models;
using Faith.Shared.Models.Requests;

namespace Faith.Client.Pages
{
    public class TimelineBase : ComponentBase
    {
        protected PostMessageRequest PostMessageRequest { get; set; } = new();
        protected IEnumerable<MessageDTO> Messages { get; set; } = Enumerable.Empty<MessageDTO>();

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [Inject]
        protected HttpClient HttpClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var role = user.GetClaimValue(ClaimTypes.Role);
            await LoadMessages();
        }

        protected async Task LoadMessages()
        {
            var messages = await HttpClient
                .GetFromJsonAsync<IEnumerable<MessageDTO>>("/messages");
            Messages = messages!;
        }

        protected async Task PostMessage()
        {
            using var response = await HttpClient
                .PostAsJsonAsync("/messages", PostMessageRequest);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                await LoadMessages();
                PostMessageRequest = new();
            }
        }
    }
}