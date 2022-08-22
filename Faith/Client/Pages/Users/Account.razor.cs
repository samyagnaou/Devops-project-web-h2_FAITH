using Faith.Core.Models;
using Faith.Shared.Constants;
using Faith.Shared.Models.Requests;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace MentoringPlatform.Client.Pages.Users
{
    public class AccountBase : ComponentBase
    {
        public ChangePasswordRequest ChangePasswordRequest { get; set; } = new();
        public MemberProfile? ChangeProfileDetailsRequest { get; set; } = new();

        [Inject]
        private HttpClient _httpClient { get; set; } = null!;

        [Inject]
        private IDialogService _dialogService { get; set; } = null!;

        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            ChangeProfileDetailsRequest = await _httpClient
                .GetFromJsonAsync<MemberProfile>("/accounts/profile");
        }

        public async Task UpdateProfile()
        {
            var result = await _httpClient.
                PostAsJsonAsync("/accounts/profile", ChangeProfileDetailsRequest!);
            if (result.IsSuccessStatusCode)
                _snackbar.Add("Your profile settings has been updated successfully!", Severity.Success);
            else
                _snackbar.Add(ResultMessages.Error, Severity.Error);
            ChangePasswordRequest = new();
        }

        public async Task ChangePassword()
        {
            var result = await _httpClient
                .PostAsJsonAsync("/accounts/change-password", ChangePasswordRequest);

            if (result.IsSuccessStatusCode)
            {
                _snackbar.Add("Your password has been changed successfully!", Severity.Success);
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();
                var errors = JsonSerializer.Deserialize<string[]>(content,
                         new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _snackbar.Add($"{string.Join(Environment.NewLine, errors!)}", Severity.Error);
            }
            ChangePasswordRequest = new();
        }
    }
}