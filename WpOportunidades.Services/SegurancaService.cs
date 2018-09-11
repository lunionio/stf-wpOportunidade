using RestSharp;
using System;
using System.Threading.Tasks;
using WpOportunidades.Infrastructure.Exceptions;

namespace WpOportunidades.Services
{
    public class SegurancaService
    {
        private const string BASE_URL = "http://localhost:5300/";
        private const string URL = "api/token/ValidaToken/";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public async Task ValidateTokenAsync(string token)
        {
            try
            {
                var client = new RestClient(BASE_URL);
                var url = $"{ URL }{ token }";
                var request = new RestRequest(url, Method.GET);

                var response = await client.ExecuteTaskAsync(request);

                if (!response.IsSuccessful)
                {
                    throw response.ErrorException;
                }

                if (!Convert.ToBoolean(response.Content))
                {
                    throw new InvalidTokenException("Token recebido na requisição é inválido.", 
                        new ArgumentException("Verifique o token informado.", "token"));
                }
            }
            catch(InvalidTokenException e)
            {
                throw e;
            }
            catch(Exception e)
            {
                throw new ServiceException("Não foi possível verificar o token informado.", e);
            }
        }
    }
}
