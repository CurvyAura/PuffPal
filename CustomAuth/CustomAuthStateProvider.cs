using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Firebase.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using PuffPal.Models;

namespace PuffPal.CustomAuth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider, IAccountManagement
    {
        private bool _authenticated = false;
        private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());
        private readonly FirebaseAuthClient _firebaseAuthClient;
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthStateProvider(FirebaseAuthClient firebaseAuthClient, ILocalStorageService localStorageService)
        {
            _firebaseAuthClient = firebaseAuthClient;
            _localStorageService = localStorageService;
        }
        public async Task<bool> CheckAuthAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }

        // This method retrieves the current user's UID from the Firebase authentication client.
        public string? GetCurrentUserUid()
        {
            return _firebaseAuthClient.User?.Uid; // Return the UID of the logged-in user, or null if not authenticated
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;
            var user = Unauthenticated;

            try
            {
                var userInfo = await _localStorageService.GetItemAsync<UserAuth>("userAuth");
                if (userInfo != null) {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, userInfo.Info.DisplayName),
                        new(ClaimTypes.Email, userInfo.Info.Email)
                    };

                    var id = new ClaimsIdentity(claims, nameof(CustomAuthStateProvider));
                    user = new ClaimsPrincipal(id);
                    _authenticated = true;
                }
            }
            catch (Exception ex)
            {

            }
            return new AuthenticationState(user);
        }

        public async Task<FormResult> LoginAsync(string email, string password)
        {
            try
            {
                var result = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
                if (!string.IsNullOrWhiteSpace(result.User.Uid))
                {
                    await _localStorageService.SetItemAsync("userAuth", result.User);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new FormResult { Succeeded = true };
                }
            }
            catch (Exception ex)
            {

            }
            return new FormResult
            {
                Succeeded = false,
                ErrorList = ["Invalid email and/or password."]
            };
        }

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("userAuth");
            if (_firebaseAuthClient.User != null) {
                _firebaseAuthClient.SignOut();
            }
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<FormResult> RegisterAsync(string email, string username, string password)
        {
            string[] defaultError = ["An unknown error prevented registration from succeding."];
            try
            {
                var result = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, username);
                    return new FormResult
                    {
                        Succeeded = true,
                    };
            }
            catch (Exception ex)
            {

            }
            return new FormResult
            {
                Succeeded = false,
                ErrorList = defaultError
            };
        }
    }
}
