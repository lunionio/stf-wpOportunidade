using RestSharp;
using RestSharpEx;
using System;
using System.Threading.Tasks;

namespace WpOportunidades.Helper
{
    public class Seguranca
    {
        public static async Task<bool> validaTokenAsync(string token)
        {

            RestClient client = new RestClient("http://seguranca.mundowebpix.com.br:5300/");
            var url = "/api/token/ValidaToken/" + token;
            RestRequest request = null;
            request = new RestRequest(url, Method.GET);
            var response = await client.ExecuteTaskAsync(request);

            return Convert.ToBoolean(response.Content);
        }
    }
}
