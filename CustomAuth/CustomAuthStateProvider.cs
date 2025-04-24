using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var authState = await GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        // This method retrieves the current user's UID from the Firebase authentication client.
        public string? GetCurrentUserUid()
        {
            if (_firebaseAuthClient.User == null)
            {
                Debug.WriteLine("Firebase client has no authenticated user.");
            }
            else
            {
                Debug.WriteLine($"Firebase client authenticated user UID: {_firebaseAuthClient.User.Uid}");
            }

            return _firebaseAuthClient.User?.Uid;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;
            var user = Unauthenticated;

            try
            {
                var userInfo = await _localStorageService.GetItemAsync<UserAuth>("userAuth");
                if (userInfo != null)
                {
                    Debug.WriteLine($"GetAuthenticationStateAsync: User found in local storage: {userInfo.Info.Email}");
                    Debug.WriteLine($"User Info: Uid = {userInfo.Info.Uid}, Email = {userInfo.Info.Email}, DisplayName = {userInfo.Info.DisplayName}");

                    var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userInfo.Info.DisplayName ?? "Unknown"), // Handle empty DisplayName
                new(ClaimTypes.Email, userInfo.Info.Email ?? "NoEmail@example.com")
            };

                    var id = new ClaimsIdentity(claims, nameof(CustomAuthStateProvider));
                    user = new ClaimsPrincipal(id);
                    _authenticated = true;

                    Debug.WriteLine("GetAuthenticationStateAsync: User is authenticated.");
                }
                else
                {
                    Debug.WriteLine("GetAuthenticationStateAsync: No user info found in local storage.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
            }

            return new AuthenticationState(user);
        }


        public async Task<FormResult> LoginAsync(string email, string password)
        {
            try
            {
                // Authenticate the user with Firebase  
                var result = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);

                // Extract user information  
                var userId = result.User?.Uid ?? "UnknownUid";
                var emailAddress = result.User?.Info?.Email ?? "NoEmail@example.com"; // Updated to use Info.Email  
                var displayName = result.User?.Info?.DisplayName ?? "Unknown"; // Set default value if DisplayName is null or empty

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var userAuth = new UserAuth
                    {
                        Info = new Info
                        {
                            Uid = userId,
                            Email = emailAddress,
                            DisplayName = displayName // Set the DisplayName
                        }
                    };

                    // Store the user information in local storage  
                    await _localStorageService.SetItemAsync("userAuth", userAuth);
                    Debug.WriteLine($"LoginAsync: UserAuth set in local storage...");
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new FormResult { Succeeded = true };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoginAsync Error: {ex.Message}");
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
