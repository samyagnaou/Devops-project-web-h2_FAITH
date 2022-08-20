using Faith.Core.Models;
using Faith.Shared.Constants;
using Faith.Shared.Extensions;
using Faith.Shared.Models;
using Faith.Shared.Models.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Faith.Client.Pages.Timeline
{
    public class TimelineBase : ComponentBase
    {
        protected PostMessageRequest PostMessageRequest { get; set; } = new();
        protected AddCommentRequest AddCommentRequest { get; set; } = new();

        protected Message? SelectedMessage { get; set; }
        protected IEnumerable<Message>? Messages { get; set; } = Enumerable.Empty<Message>();


        [Inject]
        private AuthenticationStateProvider _authStateProvider { get; set; } = null!;

        [Inject]
        private HttpClient _httpClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            try
            {

                await LoadMessages();
            }
            catch (Exception e)
            {

            }
        }

        private async Task LoadMessages()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var role = user.GetClaimValue(ClaimTypes.Role);
            if (role!.Equals(Roles.Mentor))
                await LoadMessagesInMentorGroup();
            else
                await LoadMessagesForAStudent();

            if (SelectedMessage != null)
                SelectedMessage = Messages?.FirstOrDefault(m => m.Id == SelectedMessage.Id);
        }

        protected async Task LoadMessagesInMentorGroup()
        {
            Messages = await _httpClient
                .GetFromJsonAsync<IEnumerable<Message>>("/messages/group");

        }

        protected async Task LoadMessagesForAStudent()
        {
            Messages = await _httpClient
                .GetFromJsonAsync<IEnumerable<Message>>("/messages");
        }

        protected async Task PostMessage()
        {
            using var response = await _httpClient
                .PostAsJsonAsync("/messages", PostMessageRequest);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                await LoadMessagesForAStudent();
                PostMessageRequest = new();
            }
        }

        protected void ShowMenu(Message message)
        {
            SelectedMessage = message;
            StateHasChanged();
        }

        protected async Task OnKeyDown(KeyboardEventArgs e, int messageId)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await AddComment(messageId);
            }
        }

        protected async Task AddComment(int messageId)
        {
            AddCommentRequest.MessageId = messageId;
            using var response = await _httpClient
                .PostAsJsonAsync("/comments", AddCommentRequest);
            if (response.IsSuccessStatusCode)
            {
                await LoadMessages();
                AddCommentRequest = new();
                StateHasChanged();
            }
        }
    }
}