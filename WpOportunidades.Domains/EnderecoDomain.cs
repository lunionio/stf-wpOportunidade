using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpOportunidades.Entities;
using WpOportunidades.Infrastructure;
using WpOportunidades.Infrastructure.Exceptions;
using WpOportunidades.Services;

namespace WpOportunidades.Domains
{
    public class EnderecoDomain
    {
        private readonly SegurancaService _segService;
        private readonly EnderecoRepository _edRepository;

        public EnderecoDomain(SegurancaService service,
            EnderecoRepository repository)
        {
            _segService = service;
            _edRepository = repository;
        }

        public async Task SaveAsync(Endereco endereco, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                endereco.DataCriacao = DateTime.UtcNow;
                endereco.DataEdicao = DateTime.UtcNow;

                _edRepository.Add(endereco);
            }
            catch(Exception e)
            {
                throw new EnderecoException("Não foi possível salvar o endereço da oportunidade. Entre em contato com o suporte.", e);
            }
        }

        public async Task UpdateAsync(Endereco endereco, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                endereco.DataEdicao = DateTime.UtcNow;
                _edRepository.Update(endereco);
            }
            catch (Exception e)
            {
                throw new EnderecoException("Não foi possível atualizar o endereço da oportunidade. Entre em contato com o suporte.", e);
            }
        }

        public async Task DeleteAsync(Endereco endereco, string token)
        {
            try
            {
                await _segService.ValidateTokenAsync(token);
                endereco.Status = 9;
                endereco.Ativo = false;
                _edRepository.Update(endereco);
            }
            catch (Exception e)
            {
                throw new EnderecoException("Não foi possível remover o endereço da oportunidade.", e);
            }
        }

        public async Task<IEnumerable<Endereco>> GetEnderecosAsync(List<int> oportunidadesIds, string token)
        {
            try
            {
                //await _segService.ValidateTokenAsync(token);
                var enderecos = _edRepository.GetAll().Where(e => oportunidadesIds.Contains(e.OportunidadeId));
                return enderecos;
            }
            catch(Exception e)
            {
                throw new EnderecoException("Não foi possível listar os endereços.", e);
            }
        }

        public async Task<Endereco> GetEnderecoAsync(int oportunidadeId, string token)
        {
            try
            {
                //await _segService.ValidateTokenAsync(token);
                var endereco = _edRepository.GetAll().Where(e => e.OportunidadeId.Equals(oportunidadeId)).SingleOrDefault();
                return endereco;
            }
            catch (Exception e)
            {
                throw new EnderecoException("Não foi possível recuperar o endereço.", e);
            }
        }
    }
}
