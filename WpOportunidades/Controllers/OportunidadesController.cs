using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WpOportunidades.Domains;
using WpOportunidades.Entities;
using WpOportunidades.Infrastructure.Exceptions;

namespace WpOportunidades.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class OportunidadesController : ControllerBase
    {
        private readonly OportunidadeDomain _opDomain;
        private readonly EnderecoDomain _edDomain;

        public OportunidadesController(OportunidadeDomain opDomain, EnderecoDomain edDomain)
        {
            _opDomain = opDomain;
            _edDomain = edDomain;
        }

        [HttpPost("Save/{token}")]
        public async Task<IActionResult> SaveAsync([FromRoute]string token, [FromBody]Oportunidade oportunidade)
        {
            try
            {
                var op = await _opDomain.SaveAsync(oportunidade, token);
                await _edDomain.SaveAsync(op.Endereco, token);
                return Ok("Oportunidade salva com sucesso.");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar salvar a oportunidade recebida. Entre em contato com o suporte.");
            }
        }

        [HttpPut("Update/{token}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]string token, [FromBody]Oportunidade oportunidade)
        {
            try
            {
                var op = await _opDomain.UpdateAsync(oportunidade, token);
                await _edDomain.UpdateAsync(op.Endereco, token);
                return Ok("Oportnidade atualizada com sucesso.");
            }
            catch(OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar atualizar a oportunidade recebida. Entre em contato com o suporte.");
            }
        }

        [HttpDelete("Delete/{token}")]
        public async Task<IActionResult> RemoveOportunidadeAsync([FromRoute]string token, [FromBody]Oportunidade oportunidade)
        {
            try
            {
                await _opDomain.DeleteAsync(oportunidade, token);
                await _edDomain.DeleteAsync(oportunidade.Endereco, token);
                return Ok("Oportunidade removida com sucesso.");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocoreu um erro ao tentar remover a oportunidade. Entre em contato com o suporte.");
            }
        }

        [HttpGet("GetAll/{idCliente:int}/{token}")]
        public async Task<IActionResult> GetOportunidadesAsync([FromRoute]string token, [FromRoute]int idCliente)
        {
            try
            {
                var oportunidades = await _opDomain.GetOportunidadesAsync(idCliente, token);
                var enderecos = await _edDomain.GetEnderecosAsync(oportunidades.Select(o => o.ID).ToList(), token);

                foreach (var opt in oportunidades)
                {
                    opt.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(opt.ID));
                }

                return Ok(oportunidades);
            }
            catch (OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar listar as oportunidades disponíveis. Entre em contato com o suporte.");
            }
        }

        [HttpGet("GetByCliente/{idUsuarioCriacao:int}/{idCliente:int}/{token}")]
        public async Task<IActionResult> GetOportunidadesByClienteAsync([FromRoute]int idUsuarioCriacao, [FromRoute]int idCliente, [FromRoute]string token)
        {
            try
            {
                var oportunidades = await _opDomain.GetOportunidadesAsync(idUsuarioCriacao, idCliente, token);

                var enderecos = await _edDomain.GetEnderecosAsync(oportunidades.Select(o => o.ID).ToList(), token);

                foreach (var opt in oportunidades)
                {
                    opt.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(opt.ID));
                }

                return Ok(oportunidades);
            }
            catch (OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar listar as oportunidades disponíveis. Entre em contato com o suporte.");
            }
        }

        [HttpGet("GetById/{id:int}/{token}")]
        public async Task<IActionResult> GetOportunidadeByIdAsync([FromRoute]int id, [FromRoute]string token)
        {
            try
            {
                var oportunidade = await _opDomain.GetOportunidadeAsync(id, token);
                oportunidade.Endereco = await _edDomain.GetEnderecoAsync(oportunidade.ID, token);

                return Ok(oportunidade);
            }
            catch (OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar a oportunidade solicitada. Entre em contato com o suporte.");
            }
        }
        
        //Não testado
        [HttpGet("GetByUser/{idUser:int}/{token}")]
        public async Task<IActionResult> GetOportunidadeByUserIdAsync([FromRoute]string token, [FromRoute]int idUser)
        {
            try
            {
                var oportunidades = await _opDomain.GetUserOportunidadesAsync(token, idUser);

                var enderecos = await _edDomain.GetEnderecosAsync(oportunidades.Select(o => o.ID).ToList(), token);

                foreach (var opt in oportunidades)
                {
                    opt.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(opt.ID));
                }

                return Ok(oportunidades);
            }
            catch (OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar a oportunidade solicitada. Entre em contato com o suporte.");
            }
        }

        [HttpPost("Relates/{token}")]
        public async Task<IActionResult> RelatesUserOportunidadeAsync([FromRoute]string token, [FromBody]UserXOportunidade userXOportunidade)
        {
            try
            {
                await _opDomain.SaveUserXOportunidadeAsync(token, userXOportunidade);

                return Ok("Usuário foi relacionado a oportunidade com sucesso.");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(502, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar relacionar o usuário à oportunidade. Entre em contato com o suporte.");
            }
        }
    }
}