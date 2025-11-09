using System;
using System.Collections.Generic;
using System.Text;

namespace Client.JwtToken
{
    internal class JwtTokenCache
    {
        public string? Token { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
    }
}
