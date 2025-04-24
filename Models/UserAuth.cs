// This file defines the UserAuth class and its related classes, which represent the user's authentication data.
// It includes information about the user, their credentials, and additional metadata.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    // Represents the user's authentication data, including user info and credentials.
    public class UserAuth
    {
        // Information about the user, such as UID, email, and display name.
        public Info Info { get; set; } = new Info();

        // The authentication token for the user.
        public string Token { get; set; } = string.Empty;

        // The user's credentials, such as ID token and refresh token.
        public Credential Credential { get; set; }
    }

    // Represents detailed information about the user.
    public class Info
    {
        // The unique identifier for the user.
        public string Uid { get; set; }

        // The display name of the user.
        public string DisplayName { get; set; }

        // The email address of the user.
        public string Email { get; set; }

        // Indicates whether the user's email is verified.
        public bool IsEmailVerified { get; set; }

        // The URL of the user's profile photo.
        public object PhotoUrl { get; set; }
    }

    // Represents the user's credentials for authentication.
    public class Credential
    {
        // The ID token for the user's session.
        public string IdToken { get; set; }

        // The refresh token for renewing the user's session.
        public string Refreshtoken { get; set; }

        // The date and time when the credentials were created.
        public DateTime Created { get; set; }

        // The duration (in seconds) for which the credentials are valid.
        public int ExpiresIn { get; set; }

        // The type of provider used for authentication (e.g., Google, Facebook).
        public int ProviderType { get; set; }
    }
}