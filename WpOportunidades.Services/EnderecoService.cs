using RestSharp;
using System;
using System.Threading.Tasks;
using WpOportunidades.Entities;
using WpOportunidades.Infrastructure.Exceptions;

namespace WpOportunidades.Services
{
    public class EnderecoService
    {
        private const string BASE_URL = "http://localhost:5330/api/endereco/";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endereco"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task SaveEnderecoAsync(Endereco endereco, string token)
        {
            try
            {                
                var client = new RestClient(BASE_URL);
                var url = $"SaveAsync/{ token }";
                var request = new RestRequest(url, Method.POST);

                var json = SimpleJson.SimpleJson.SerializeObject(endereco);

                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = await client.ExecuteTaskAsync(request);

                if (!response.IsSuccessful || !Convert.ToBoolean(response.Content))
                {
                    throw new Exception(response.StatusDescription);
                }
            }
            catch(Exception e)
            {
                throw new ServiceException("Não foi possível salvar o endereço informado.", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endereco"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task RemoveEnderecoAsync(Endereco endereco, string token)
        {
            try
            {
                var client = new RestClient(BASE_URL);
                var url = $"RemoveAsync/{ token }";
                var request = new RestRequest(url, Method.POST);;

                var json = SimpleJson.SimpleJson.SerializeObject(endereco);

                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = await client.ExecuteTaskAsync(request);

                if (!response.IsSuccessful || !Convert.ToBoolean(response.Content))
                {
                    throw new Exception(response.StatusDescription);
                }
            }
            catch(Exception e)
            {
                throw new ServiceException("Não foi possível remover o endereço informado.", e);
            }
        }
    }
}
