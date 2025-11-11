using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game03Client.HttpRequester;

public interface IHttpRequesterProvider
{
    Task<HttpRequesterResult?> GetResponceAsync(string url, string? jsonBody = null, bool useJwtToken = true);
}
