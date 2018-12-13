using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpOportunidades.Entities;
using WpOportunidades.Services;

namespace WpOportunidades.Helper
{
    public class EmailHandler
    {
        private readonly IConfiguration _config;
        private readonly ConfiguracaoService _configService;
        private readonly EmailService _emailService;
        private readonly SegurancaService _segService;

        public EmailHandler(IConfiguration config, ConfiguracaoService configService, EmailService emailService, SegurancaService segService)
        {
            _config = config;
            _configService = configService;
            _emailService = emailService;
            _segService = segService;
        }

        public async Task EnviarEmailAsync(string token, Oportunidade oportunidade)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var (emailConfigs, emailConstants) = GetConfiguration();
                var content = await System.IO.File.ReadAllTextAsync("wwwroot/Criar-Opt.html");

                foreach (var item in emailConstants)
                {
                    var text = string.Empty;

                    if ("endereco".Equals(item.Key.ToLower()))
                    {
                        text = oportunidade.Endereco.GetType().GetProperty("Descricao").GetValue(oportunidade.Endereco, null).ToString();
                    }
                    else
                    {
                        text = oportunidade.GetType().GetProperty(item.Key).GetValue(oportunidade, null).ToString();
                    }

                    if (!string.IsNullOrEmpty(text))
                        content = content.Replace(item.Value, text);
                }

                var configuracoes = await _configService.GetConfiguracoesAsync(oportunidade.IdCliente, oportunidade.UsuarioCriacao);
                var sender = emailConfigs.GetValue<string>("Sender");

                var configuracao = configuracoes.Where(c => c.Chave.Equals(sender)).SingleOrDefault();

                if (!string.IsNullOrEmpty(oportunidade.EmailEmpresa))
                {
                    var emailToClient = new Email(content, $"Oportunidade: { oportunidade.Nome }", configuracao.Valor, oportunidade.EmailEmpresa, oportunidade.IdCliente);
                    await _emailService.EnviarEmailAsync(emailToClient, oportunidade.IdCliente, oportunidade.UsuarioCriacao);
                }
            }
            catch(Exception e)
            {
                //Erro ao enviar e-mail não impacta no processo
            }
        }

        private (IConfigurationSection emailConfigs,
            IEnumerable<IConfigurationSection> emailConstants) GetConfiguration()
        {
            var emailConfigs = _config.GetSection("EmailSettings");
            var emailConstants = emailConfigs.GetSection("Constants").GetChildren();
            //var destinatario = emailConfigs.GetValue<string>("Recipient");

            return (emailConfigs, emailConstants);
        }
    }
}
