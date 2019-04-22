using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpOportunidades.Domains.Generics;
using WpOportunidades.Entities;
using WpOportunidades.Infrastructure;
using WpOportunidades.Infrastructure.Exceptions;
using WpOportunidades.Services;

namespace WpOportunidades.Domains
{
    public class OportunidadeDomain : IDomain<Oportunidade>
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
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<Oportunidade> SaveAsync(Oportunidade entity, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                if (entity.ID == 0)
                {
                    entity.DataCriacao = DateTime.UtcNow;
                    entity.DataEdicao = DateTime.UtcNow;
                    entity.Ativo = true;

                    var result = _opRepository.Add(entity);

                    entity.Endereco.OportunidadeId = result;
                }
                else
                {
                    entity = await UpdateAsync(entity, token);
                }

                return entity;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception> 
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<Oportunidade> UpdateAsync(Oportunidade entity, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                entity.DataEdicao = DateTime.UtcNow;
                _opRepository.Update(entity);

                return entity;
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
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<IEnumerable<Oportunidade>> GetAllAsync(int idCliente, string token)
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
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<Oportunidade> GetByIdAsync(int entityId, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var op = _opRepository.GetList(o => o.ID.Equals(entityId) && o.Status != 9).SingleOrDefault();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public async Task DeleteAsync(Oportunidade entity, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                entity.Status = 9;
                entity.Ativo = false;
                _opRepository.Update(entity);
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
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<IEnumerable<Oportunidade>> GetByUsuarioCriacaoIdAsync(int idUsuarioCriacao, int idCliente, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _opRepository.GetList(o => o.UsuarioCriacao.Equals(idUsuarioCriacao) 
                    && o.IdCliente.Equals(idCliente) && o.Status != 9).OrderBy(o => o.DataOportunidade);

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

        public async Task<IEnumerable<Oportunidade>> GetAllAppAsync(int idCliente, int idUsuario, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var optsUser = _repository.GetList(x => x.UserId.Equals(idUsuario)).Select(x => x.OportunidadeId);

                var result = _opRepository.GetList(o => o.Status != 9 && o.IdCliente.Equals(idCliente) && !optsUser.Contains(o.ID)
                    && o.DataOportunidade >= DateTime.Today && o.HoraInicio.Subtract(DateTime.Now.TimeOfDay) >= new TimeSpan(0, 0, 0));

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
        /// <param name="token"></param>
        /// <param name="userXOportunidade"></param>
        /// <returns></returns>
        /// <exception cref="OportunidadeException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<UserXOportunidade> SaveUserXOportunidadeAsync(string token, UserXOportunidade userXOportunidade)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                if (userXOportunidade.ID == 0)
                {
                    userXOportunidade.ID = _repository.Add(userXOportunidade);
                    return userXOportunidade;
                }
                else
                {
                    _repository.Update(userXOportunidade);
                    return userXOportunidade;
                }
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
        /// <exception cref="OportunidadeException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<IEnumerable<Oportunidade>> GetUserOportunidadesAsync(string token, int idCliente, int userId)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _repository.GetList(x => x.UserId.Equals(userId));

                var ids = result.Select(r => r.OportunidadeId);
                var opts = _opRepository.GetList(o => ids.Contains(o.ID) && o.Status != 9 && o.IdCliente.Equals(idCliente));
                
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
        /// <exception cref="OportunidadeException"></exception>
        /// <exception cref="InvalidTokenException"></exception>
        public async Task<IEnumerable<Oportunidade>> GetOportunidadesByDateAsync(DateTime date, string token, int idCliente)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _opRepository.GetList(o => o.IdCliente.Equals(idCliente)
                    && DateTime.Compare(o.DataOportunidade.Date, date.Date) == 0 && o.Status != 9);

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

        public async Task<IEnumerable<Oportunidade>> GetOportunidadesByEmpresaAsync(int idEmpresa, int idCliente, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _opRepository.GetList(o => o.IdEmpresa.Equals(idEmpresa) 
                        && o.IdCliente.Equals(idCliente) && o.Status != 9);

                return result;
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível listar as oportunidades solicitadas. Entre em contato com o suporte.", e);
            }
        }

        public async Task<IEnumerable<UserXOportunidade>> GetUsersAsync(int idOpt, /*int idCliente,*/ string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _repository.GetList(x => x.OportunidadeId.Equals(idOpt));

                //var ids = result.Select(r => r.OportunidadeId);
                //var opts = _opRepository.GetList(o => ids.Contains(o.ID) && o.Status != 9 && o.IdCliente.Equals(idCliente));

                var statusIds = result.Select(r => r.StatusID);
                var allStatus = new StatusRepository().GetList(s => statusIds.Contains(s.ID));

                foreach (var item in result)
                {
                    item.Status = allStatus.FirstOrDefault(s => s.ID.Equals(item.StatusID));
                    //item.Oportunidade = opts.FirstOrDefault(o => o.ID.Equals(item.OportunidadeId));
                }

                return result;

            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch(ServiceException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível listar os usuários da oportunidade. Entre em contato com suporte.", e);
            }
        }

        public async Task<IEnumerable<UserXOportunidade>> GetByStatusAndUserAsync(int userId, int statusId, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _repository.GetList(x => x.UserId.Equals(userId) && x.StatusID.Equals(statusId));

                var opIds = result.Select(x => x.OportunidadeId);
                var oportunidades = _opRepository.GetList(o => opIds.Contains(o.ID));

                var statusIds = result.Select(r => r.StatusID);
                var allStatus = new StatusRepository().GetList(s => statusIds.Contains(s.ID));

                foreach (var item in result)
                {
                    item.Oportunidade = oportunidades.FirstOrDefault(o => o.ID.Equals(item.OportunidadeId));
                    item.Status = allStatus.FirstOrDefault(s => s.ID.Equals(item.StatusID));
                }

                return result;
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (ServiceException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível listar as oportunidades. Entre em contato com suporte.", e);
            }
        }
    }
}