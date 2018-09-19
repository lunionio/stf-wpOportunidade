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
    public class EnderecoDomain : IDomain<Endereco>
    {
        private readonly SegurancaService _segService;
        private readonly EnderecoRepository _edRepository;

        public EnderecoDomain(SegurancaService service,
            EnderecoRepository repository)
        {
            _segService = service;
            _edRepository = repository;
        }

        public async Task<Endereco> SaveAsync(Endereco entity, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);

                if (entity.ID == 0)
                {
                    entity.DataCriacao = DateTime.UtcNow;
                    entity.DataEdicao = DateTime.UtcNow;
                    entity.ID = _edRepository.Add(entity);
                }
                else
                {
                    entity = await UpdateAsync(entity, token);
                }

                return entity;
            }
            catch(Exception e)
            {
                throw new EnderecoException("Não foi possível salvar o endereço da oportunidade. Entre em contato com o suporte.", e);
            }
        }

        public async Task<Endereco> UpdateAsync(Endereco entity, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                entity.DataEdicao = DateTime.UtcNow;
                _edRepository.Update(entity);

                return entity;
            }
            catch (Exception e)
            {
                throw new EnderecoException("Não foi possível atualizar o endereço da oportunidade. Entre em contato com o suporte.", e);
            }
        }

        public async Task DeleteAsync(Endereco entity, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                entity.Status = 9;
                entity.Ativo = false;
                _edRepository.Update(entity);
            }
            catch (Exception e)
            {
                throw new EnderecoException("Não foi possível remover o endereço da oportunidade.", e);
            }
        }

        public async Task<IEnumerable<Endereco>> GetAllAsync(List<int> oportunidadesIds, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                var enderecos = _edRepository.GetList(e => oportunidadesIds.Contains(e.OportunidadeId));
                return enderecos;
            }
            catch(Exception e)
            {
                throw new EnderecoException("Não foi possível listar os endereços.", e);
            }
        }

        public async Task<Endereco> GetByIdAsync(int entityId, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                var endereco = _edRepository.GetList(e => e.OportunidadeId.Equals(entityId)).SingleOrDefault();
                return endereco;
            }
            catch (Exception e)
            {
                throw new EnderecoException("Não foi possível recuperar o endereço.", e);
            }
        }

        public async Task<IEnumerable<Endereco>> GetAllAsync(int idCliente, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                var enderecos = _edRepository.GetList(e => e.IdCliente.Equals(idCliente));

                return enderecos;
            }
            catch(Exception e)
            {
                throw new EnderecoException("Não foi possível recuperar os endereços.", e);
            }
        }
    }
}