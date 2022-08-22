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
using Faith.Shared.Models.Responses;
using MudBlazor;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
using Microsoft.AspNetCore.Components.Forms;

namespace Faith.Client.Pages.Timeline
{
    public class TimelineBase : ComponentBase
    {

        protected PostMessageRequest PostMessageRequest { get; set; } = new();
        protected AddCommentRequest AddCommentRequest { get; set; } = new();
        private IBrowserFile? _imageFile { get; set; } = null!;

        protected Message? SelectedMessage { get; set; }
        protected IEnumerable<Message>? Messages { get; set; } = Enumerable.Empty<Message>();
        protected IEnumerable<Message>? ArchivedMessages { get; set; } = Enumerable.Empty<Message>();

        public bool ShowArchivedMessages { get; set; }
        protected string UserId { get; set; } = null!;
        protected string Role { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider _authStateProvider { get; set; } = null!;

        [Inject]
        private HttpClient _httpClient { get; set; } = null!;

        [Inject]
        private IDialogService _dialogService { get; set; } = null!;

        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;

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
            UserId = user.GetClaimValue(ClaimTypes.Name)!;
            Role = user.GetClaimValue(ClaimTypes.Role)!;
            if (Role!.Equals(Roles.Mentor))
                await LoadMessagesInMentorGroup();
            else
                await LoadMessagesForAStudent();

            if (SelectedMessage != null)
            {
                var message = Messages?.FirstOrDefault(m => m.Id == SelectedMessage.Id);
                if (message == null)
                    message = ArchivedMessages?.FirstOrDefault(m => m.Id == SelectedMessage.Id);
                SelectedMessage = message;
            }
        }

        protected async Task LoadMessagesInMentorGroup()
        {
            var response = await _httpClient
                .GetFromJsonAsync<MentorMessagesResponse>("/messages/group");
            ArchivedMessages = response!.ArchivedMessages;
            Messages = response!.Messages;
        }

        protected async Task LoadMessagesForAStudent()
        {
            Messages = await _httpClient
                .GetFromJsonAsync<IEnumerable<Message>>("/messages");
        }

        protected async Task PostAMessage()
        {
            using var content = new MultipartFormDataContent();
            if (_imageFile != null)
            {
                var maxAllowedSize = 1 * 1024 * 1024 * 1024;
                var fileContent = new StreamContent(_imageFile.OpenReadStream(maxAllowedSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(_imageFile.ContentType);
                content.Add(fileContent, "imageFile", _imageFile.Name);
            }

            var textBytes = Encoding.UTF8.GetBytes(PostMessageRequest.Text);
            var textContent = new StreamContent(new MemoryStream(textBytes));
            content.Add(textContent, "text");

            using var response = await _httpClient.PostAsync("/messages", content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                await LoadMessagesForAStudent();
                PostMessageRequest = new();
                _imageFile = null;
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

        protected async Task DeleteCommment(int commentId)
        {
            using var response = await _httpClient
                .DeleteAsync($"/comments/{commentId}");
            if (response.IsSuccessStatusCode)
            {
                await LoadMessages();
                StateHasChanged();
            }
        }

        protected async Task EditAComment(Comment comment)
        {
            var parameters = new DialogParameters();
            parameters.Add("Text", comment.Text);
            var result = await _dialogService.Show<EditComment>("Edit comment", parameters).Result;

            if (!result.Cancelled)
            {
                var text = (string)result.Data;
                using var response = await _httpClient
                    .PutAsJsonAsync($"/comments/{comment.Id}", text);

                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add("The comment has been successfully updated!");
                    await LoadMessages();
                    StateHasChanged();
                }
                else
                {
                    _snackbar.Add(ResultMessages.Error, Severity.Error);
                }
            }
        }

        protected async Task ArchiveAMessage(int messageId)
        {
            using var response = await _httpClient
                .PostAsJsonAsync($"/messages/archive", messageId);

            if (response.IsSuccessStatusCode)
            {
                _snackbar.Add("The message has been archived!");
                await LoadMessages();
                StateHasChanged();
            }
            else
            {
                _snackbar.Add(ResultMessages.Error, Severity.Error);
            }
        }

        protected void ToggleArchivedMessages()
        {
            ShowArchivedMessages = !ShowArchivedMessages;
            SelectedMessage = null;
        }

        protected void SaveImage(InputFileChangeEventArgs e)
        {
            _imageFile = e.File;
        }
    }
}