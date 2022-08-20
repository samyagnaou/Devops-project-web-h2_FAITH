using Faith.Shared.Constants;
using Faith.Shared.Models;
using Faith.Shared.Models.Requests;
using Faith.Shared.Models.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace Faith.Client.Pages.Users
{
    public class UsersBase : ComponentBase
    {
        protected IEnumerable<UserDTO>? Users { get; set; } = Enumerable.Empty<UserDTO>();
        protected string? SearchQuery { get; set; }
        protected bool IsLoading { get; set; }

        [Inject]
        private HttpClient _httpClient { get; set; } = null!;

        [Inject]
        private IDialogService _dialogService { get; set; } = null!;

        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;

        private async Task LoadUsers()
        {
            IsLoading = true;
            Users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("/accounts");
            IsLoading = false;
        }

        protected override async Task OnInitializedAsync()
            => await LoadUsers();

        protected bool Filter(UserDTO user) => FilterUsers(user, SearchQuery);

        protected bool FilterUsers(UserDTO user, string? searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return true;
            if (user.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (user.Role.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        protected async Task CreateUser()
        {
            var result = await _dialogService.Show<CreateUser>("Add User").Result;

            if (!result.Cancelled)
            {
                var newUser = (CreateUserWithRoleRequest)result.Data;
                using var response = await _httpClient.PostAsJsonAsync("/accounts", newUser);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add(string.Format(ResultMessages.SuccessfulCreationFormat,
                        newUser.Email),
                        Severity.Success);
                    await LoadUsers();
                }
                else
                {
                    var createUserResponse = JsonSerializer.Deserialize<RegisterUserResponse>(content);
                    var hasErrors = createUserResponse != null && createUserResponse.Errors.Any();
                    _snackbar.Add(
                        hasErrors ? string.Join(Environment.NewLine, createUserResponse!.Errors)
                                  : ResultMessages.Error,
                        Severity.Error);
                }
            }
        }

        protected async Task DeleteUser(UserDTO user)
        {
        }
    }
}