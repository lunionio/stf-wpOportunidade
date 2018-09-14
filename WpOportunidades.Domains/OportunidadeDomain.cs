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
        private readonly UserXOportunidadeRepository _repository;
        
        public OportunidadeDomain(SegurancaService service, 
            OportunidadeRepository repository, UserXOportunidadeRepository xRepository)
        {
            _segService = service;
            _opRepository = repository;
            _repository = xRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oportunidade"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        public async Task<Oportunidade> SaveAsync(Oportunidade oportunidade, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                oportunidade.DataCriacao = DateTime.UtcNow;
                oportunidade.DataEdicao = DateTime.UtcNow;
                oportunidade.Ativo = true;

                var result = _opRepository.Add(oportunidade);

                oportunidade.Endereco.OportunidadeId = result;

                return oportunidade;
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível completar a operação.", e);
            }
        }

        public async Task<Oportunidade> UpdateAsync(Oportunidade oportunidade, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                oportunidade.DataEdicao = DateTime.UtcNow;
                _opRepository.Update(oportunidade);

                return oportunidade;
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível atualizar a oportunidade.", e);
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

                var result = _opRepository.GetList(o => o.Status != 9 && o.IdCliente.Equals(idCliente));

                return result;
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
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
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível recuperar a oportunidade solicitada.", e);
            }
        }

        public async Task DeleteAsync(Oportunidade oportunidade, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                oportunidade.Status = 9;
                oportunidade.Ativo = false;
                _opRepository.Update(oportunidade);
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
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

                var result = _opRepository.GetList(o => o.UsuarioCriacao.Equals(idUsuarioCriacao) 
                    && o.IdCliente.Equals(idCliente)).OrderBy(o => o.DataOportunidade);

                return result;
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
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
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível relacionar o usuário à oportunidade. Entre em contato com o suporte.", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Oportunidade>> GetUserOportunidadesAsync(string token, int userId)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _repository.GetList(x => x.UserId.Equals(userId));

                var ids = result.Select(r => r.OportunidadeId);
                var opts = _opRepository.GetList(o => ids.Contains(o.ID));
                
                var statusIds = result.Select(r => r.StatusID);
                var allStatus = new StatusRepository().GetList(s => statusIds.Contains(s.ID));

                foreach (var r in result)
                {
                    r.Oportunidade = opts.FirstOrDefault(o => o.ID.Equals(r.OportunidadeId));
                    r.Oportunidade.OptStatus = allStatus.FirstOrDefault(s => s.ID.Equals(r.StatusID));
                }

                return result.Select(r => r.Oportunidade);
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível listar as oportunidades do usuário. Entre em contato com suporte.", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Oportunidade>> GetOportunidadesByDateAsync(DateTime date, string token, int idCliente)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _opRepository.GetList(o => o.IdCliente.Equals(idCliente)
                    && DateTime.Compare(o.DataOportunidade.Date, date.Date) == 0);

                return result;
            }
            catch(InvalidTokenException e)
            {
                throw e;
            }
            catch(Exception e)
            {
                throw new OportunidadeException("Não foi possível listar as oportunidades solicitadas. Entre em contato com o suporte.", e);
            }
        }
    }
}
