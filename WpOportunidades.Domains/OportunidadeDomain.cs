using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpOportunidades.Entities;
using WpOportunidades.Infrastructure;
using WpOportunidades.Infrastructure.Exceptions;
using WpOportunidades.Services;

namespace WpOportunidades.Domains
{
    public class OportunidadeDomain
    {
        private readonly SegurancaService _segService;
        private readonly OportunidadeRepository _opRepository;
        private readonly EnderecoService _endService;
        private readonly UserXOportunidadeRepository _repository;
        
        public OportunidadeDomain(SegurancaService service, 
            OportunidadeRepository repository, EnderecoService endService)
        {
            _segService = service;
            _opRepository = repository;
            _endService = endService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oportunidade"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        public async Task SaveAsync(Oportunidade oportunidade, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                if (oportunidade.ID == 0)
                {
                    oportunidade.DataCriacao = DateTime.UtcNow;
                    oportunidade.DataEdicao = DateTime.UtcNow;
                    oportunidade.Ativo = true;

                    var result = _opRepository.Add(oportunidade);

                    oportunidade.Endereco.OportunidadeId = result;

                    //Retornar o oportunidade para salvar o endereço lá por forano domain do endereço
                    await _endService.SaveEnderecoAsync(oportunidade.Endereco, token);
                }
                else
                {
                    await UpdateAsync(oportunidade, token);
                }
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível completar a operação.", e);
            }
        }

        private async Task UpdateAsync(Oportunidade oportunidade, string token)
        {
            try
            {
                oportunidade.DataEdicao = DateTime.UtcNow;
                _opRepository.Update(oportunidade);

                await _endService.RemoveEnderecoAsync(oportunidade.Endereco, token);
                await _endService.SaveEnderecoAsync(oportunidade.Endereco, token);
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível atualizar o endereço.", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        public async Task<IEnumerable<Oportunidade>> GetOportunidadesAsync(int idCliente, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _opRepository.GetAll().Where(o => o.Status != 9 && o.IdCliente.Equals(idCliente)).ToList();

                return result;
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível recuperar a lista de oportunidades.", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        public async Task<Oportunidade> GetOportunidadeAsync(int id, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var op = _opRepository.GetList(o => o.ID.Equals(id)).SingleOrDefault();
                return op;
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível recuperar a oportunidade solicitada.", e);
            }
        }

        public void DeleteAsync(Oportunidade oportunidade, string token)
        {
            try
            {
                _opRepository.Remove(oportunidade);
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível remover a oportunidade.", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuarioCriacao"></param>
        /// <param name="idCliente"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        public async Task<IEnumerable<Oportunidade>> GetOportunidadesAsync(int idUsuarioCriacao, int idCliente, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _opRepository.GetAll()
                    .Where(o => o.UsuarioCriacao.Equals(idUsuarioCriacao) 
                    && o.IdCliente.Equals(idCliente)).ToList().OrderBy(o => o.DataOportunidade);

                return result;
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível recuperar a lista de oportunidades do usuário.", e);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userXOportunidade"></param>
        /// <returns></returns>
        public async Task SaveUserXOportunidadeAsync(string token, UserXOportunidade userXOportunidade)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                var result = _repository.Add(userXOportunidade);
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível relacionar o usuário à oportunidade. Entre em contato com o suporte.", e);
            }
        }

        public async Task<IEnumerable<Oportunidade>> GetUserOportunidades(string token, int userId)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _repository.GetAll()
                    .Where(x => x.IdUser.Equals(userId))
                    .Select(r => r.IdOportunidade).ToList();

                var opts = _opRepository.GetAll().Where(o => result.Contains(o.ID)).ToList();

                return opts;
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível listar as oportunidades do usuário. Entre em contato com suporte.", e);
            }
        }
    }
}
