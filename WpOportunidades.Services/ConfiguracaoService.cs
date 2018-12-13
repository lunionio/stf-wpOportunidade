using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WpOportunidades.Entities;
using WpOportunidades.Infrastructure.Exceptions;

namespace WpOportunidades.Services
{
    public class ConfiguracaoService
    {
        private const string BASE_URL = "http://localhost:5300/api/Seguranca/Principal/";

        public async Task<IEnumerable<Configuracao>> GetConfiguracoesAsync(int idCliente, int idUsuario)
        {
            try
            {
                var client = new RestClient(BASE_URL);
                var url = $"buscarconfiguracoes/{ idCliente }/{ idUsuario }";
                var request = new RestRequest(url, Method.POST);

                var envio = new
                {
                    configuracao = new
                    {
                        idCliente
                    }
                };

                var json = JsonConvert.SerializeObject(envio);

                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = await client.ExecuteTaskAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception(response.StatusDescription);
                }

                //var configuracoes = SimpleJson.SimpleJson.DeserializeObject<IEnumerable<Configuracao>>(response.Content);
                var configuracoes = JsonConvert.DeserializeObject<IEnumerable<Configuracao>>(response.Content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                return configuracoes;
            }
            catch (Exception e)
            {
                throw new ServiceException("Não foi possível recuperar as configurações.", e);
            }
        }
    }
}
