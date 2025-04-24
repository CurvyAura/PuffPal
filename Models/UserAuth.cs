using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuffPal.Models
{
    public class UserAuth
    {
        public Info Info { get; set; } = new Info();
        public string Token { get; set; } = string.Empty; // Add this property

        public Credential Credential { get; set; }
    }

    public class Info
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public object PhotoUrl { get; set; }
    }

    public class Credential
    {
        public string IdToken { get; set; }
        public string Refreshtoken { get; set; }
        public DateTime Created { get; set; }
        public int ExpiresIn { get; set; }
        public int ProviderType { get; set; }
    }
}
