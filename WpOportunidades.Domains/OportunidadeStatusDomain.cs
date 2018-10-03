using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpOportunidades.Domains.Generics;
using WpOportunidades.Entities;
using WpOportunidades.Infrastructure;
using WpOportunidades.Infrastructure.Exceptions;
using WpOportunidades.Services;

namespace WpOportunidades.Domains
{
    public class OportunidadeStatusDomain : IDomain<OportunidadeStatus>
    {
        private readonly SegurancaService _segService;
        private readonly OportunidadeStatusRepository _osRepository;

        public OportunidadeStatusDomain(SegurancaService segService, OportunidadeStatusRepository osRepository)
        {
            _segService = segService;
            _osRepository = osRepository;
        }

        public Task DeleteAsync(OportunidadeStatus entity, string token)
        {
            throw new NotImplementedException("Não é possível excluir um status de oportunidade.");
        }

        public async Task<IEnumerable<OportunidadeStatus>> GetAllAsync(int idCliente, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                var result = _osRepository.GetList(s => s.IdCliente.Equals(idCliente));
                return result;
            }
            catch (InvalidTokenException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new OportunidadeException("Não foi possível recuperar a lista de status das oportunidades.", e);
            }
        }

        public Task<OportunidadeStatus> GetByIdAsync(int entityId, string token)
        {
            throw new NotImplementedException("Não implementado.");
        }

        public Task<OportunidadeStatus> SaveAsync(OportunidadeStatus entity, string token)
        {
            throw new NotImplementedException("Não é possível incluir um novo status de oportunidade.");
        }

        public Task<OportunidadeStatus> UpdateAsync(OportunidadeStatus entity, string token)
        {
            throw new NotImplementedException("Não é possível atualizar um status de oportunidade.");
        }
    }
}
