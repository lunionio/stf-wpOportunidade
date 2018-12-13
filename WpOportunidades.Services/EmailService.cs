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
    public class EmailService
    {
        private const string BASE_URL = "http://localhost:5300/api/Seguranca/Principal/";

        public async Task EnviarEmailAsync(Email email, int idCliente, int idUsuario)
        {
            try
            {
                var client = new RestClient(BASE_URL);
                var url = $"enviaremail/{ idCliente }/{ idUsuario }";
                var request = new RestRequest(url, Method.POST);

                var data = new
                {
                    envio = email
                };

                var json = JsonConvert.SerializeObject(data);

                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = await client.ExecuteTaskAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception(response.StatusDescription);
                }
            }
            catch (Exception e)
            {
                throw new ServiceException("Não foi possível enviar o e-mail.", e);
            }
        }
    }
}
