using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using WebBlazor.Client.Models.Smoking;

namespace WebBlazor.Client.HttpClients
{
    public interface ISmokingHttpClient
    {
        Task<RecipeSummary[]> GetRecipes(string search);

    }
    public class SmokingHttpClient : ISmokingHttpClient
    {
        private readonly HttpClient _http;

        public SmokingHttpClient(HttpClient http)
        {
            _http = http;
        }

        public Task<RecipeSummary[]> GetRecipes(string search)
        {
            var url = string.IsNullOrEmpty(search) ? "recipes" : QueryHelpers.AddQueryString("recipes", "search", search);

            try
            {
                return _http.GetFromJsonAsync<RecipeSummary[]>(url);
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
                return default;
            }
        }
    }
}
