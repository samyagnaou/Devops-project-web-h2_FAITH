using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Security.Claims;
using Faith.Client.Interfaces;
using Faith.Shared.Extensions;
using PowerArgs.Games;

namespace Faith.Client.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        protected MudThemeProvider MudThemeProvider { get; set; } = null!;
        protected DefaultTheme DefaultTheme { get; set; } = new();
        protected bool IsDrawerOpened { get; set; }
        protected bool IsDarkMode { get; set; }
        protected string? FullName { get; set; }
        protected string? Role { get; set; }
        protected char? FirstLetterOfFirstName { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; } = null!;

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; } = null!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        protected void DrawerToggle()
         => IsDrawerOpened = !IsDrawerOpened;

        protected void DarkModeToggle()
         => IsDarkMode = !IsDarkMode;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsDarkMode = await MudThemeProvider.GetSystemPreference();
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var name = authState?.User?.Identity?.Name;
            FirstLetterOfFirstName = name?[0];
            FullName = name;
            Role = authState?.User.GetClaimValue(ClaimTypes.Role);
        }

        protected void Login(MouseEventArgs args)
        {
            Navigation.NavigateTo("authentication/login");
        }

        protected async Task Logout(MouseEventArgs args)
        {
            await AuthenticationService.Logout();
            Navigation.NavigateTo("/");
        }
    }
}