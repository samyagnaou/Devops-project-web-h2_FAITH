using Faith.Client.Pages.Users;
using Faith.Core.Models;
using Faith.Shared.Constants;
using Faith.Shared.Models.Requests;
using Faith.Shared.Models.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace Faith.Client.Pages.Community
{
    public class CommunityBase : ComponentBase
    {
        protected IEnumerable<Student>? StudentGroup { get; set; } = Enumerable.Empty<Student>();
        protected string? SearchQuery { get; set; }
        protected bool IsLoading { get; set; }

        [Inject]
        private HttpClient _httpClient { get; set; } = null!;

        [Inject]
        private IDialogService _dialogService { get; set; } = null!;

        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;

        private async Task LoadStudents()
        {
            IsLoading = true;
            StudentGroup = await _httpClient.GetFromJsonAsync<IEnumerable<Student>>("/students/group");
            IsLoading = false;
        }

        protected override async Task OnInitializedAsync()
            => await LoadStudents();

        protected bool Filter(Student student) => FilterStudents(student, SearchQuery);

        protected bool FilterStudents(Student student, string? searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return true;
            if (student.MemberId.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (student.FirstName != null && student.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;
            if (student.LastName != null && student.LastName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        protected async Task CreateNewStudent()
        {
            var result = await _dialogService.Show<CreateUser>("Add Student").Result;

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
                    await LoadStudents();
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

        protected async Task AddStudentToGroup()
        {
            var parameters = new DialogParameters();
            parameters.Add("CurrentStudents", StudentGroup);

            var result = await _dialogService
            .Show<AddStudentToGroup>("Add student to group", parameters).Result;

            if (!result.Cancelled)
            {
                var studentUserId = (string)result.Data;
                using var response = await _httpClient
                    .PostAsJsonAsync($"/students/add-to-group", studentUserId);

                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add($"{studentUserId} has been added to your group!",
                        Severity.Success);
                    await LoadStudents();
                }
                else
                {
                    _snackbar.Add(ResultMessages.Error, Severity.Error);
                }

            }
        }

        protected async Task RemoveStudentFromGroup(string studentUserId)
        {
            var result = await _dialogService.Show<RemoveStudentFromGroup>("").Result;

            if (!result.Cancelled)
            {
                using var response = await _httpClient
                    .PostAsJsonAsync($"/students/remove-from-group", studentUserId);

                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add($"{studentUserId} has been removed from your group!",
                        Severity.Success);
                    StudentGroup = StudentGroup!.Where(s => s.MemberId != studentUserId);
                }
                else
                {
                    _snackbar.Add(ResultMessages.Error, Severity.Error);
                }
            }
        }
    }
}