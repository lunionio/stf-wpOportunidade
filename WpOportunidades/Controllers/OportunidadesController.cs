using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WpOportunidades.Domains;
using WpOportunidades.Entities;
using WpOportunidades.Helper;
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
        private readonly OportunidadeStatusDomain _sDomain;
        private readonly EmailHandler _emailHandler;

        public OportunidadesController([FromServices]OportunidadeDomain opDomain, 
            [FromServices]EnderecoDomain edDomain, [FromServices]OportunidadeStatusDomain sDomain, [FromServices]EmailHandler emailHandler)
        {
            _opDomain = opDomain;
            _edDomain = edDomain;
            _sDomain = sDomain;
            _emailHandler = emailHandler;
        }

        [HttpPost("Save/{token}")]
        public async Task<IActionResult> SaveAsync([FromRoute]string token, [FromBody]Oportunidade oportunidade)
        {
            try
            {
                var op = await _opDomain.SaveAsync(oportunidade, token);
                op.Endereco = await _edDomain.SaveAsync(op.Endereco, token);

                if(oportunidade.ID == 0)
                    await _emailHandler.EnviarEmailAsync(token, op);

                return Ok(op);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar salvar a oportunidade recebida. Entre em contato com o suporte.");
            }
        }

        //[HttpPut("Update/{token}")]
        //public async Task<IActionResult> UpdateAsync([FromRoute]string token, [FromBody]Oportunidade oportunidade)
        //{
        //    try
        //    {
        //        var op = await _opDomain.UpdateAsync(oportunidade, token);
        //        await _edDomain.UpdateAsync(op.Endereco, token);
        //        return Ok("Oportunidade atualizada com sucesso.");
        //    }
        //    catch (InvalidTokenException e)
        //    {
        //        return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
        //    }
        //    catch (OportunidadeException e)
        //    {
        //        return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
        //    }
        //    catch (EnderecoException e)
        //    {
        //        return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, "Ocorreu um erro ao tentar atualizar a oportunidade recebida. Entre em contato com o suporte.");
        //    }
        //}

        [HttpPost("Delete/{token}")]
        public async Task<IActionResult> RemoveOportunidadeAsync([FromRoute]string token, [FromBody]Oportunidade oportunidade)
        {
            try
            {
                await _opDomain.DeleteAsync(oportunidade, token);
                await _edDomain.DeleteAsync(oportunidade.Endereco, token);

                return Ok(true);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
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
                var oportunidades = await _opDomain.GetAllAsync(idCliente, token);

                var enderecos = await _edDomain.GetAllAsync(oportunidades.Select(o => o.ID).ToList(), token);
                foreach (var opt in oportunidades)
                {
                    opt.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(opt.ID));
                }

                var statuses = await _sDomain.GetAllAsync(idCliente, token);
                foreach (var o in oportunidades)
                {
                    o.OportunidadeStatus = statuses.FirstOrDefault(s => s.ID.Equals(o.OportunidadeStatusID));
                }

                return Ok(oportunidades);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
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
                var oportunidades = await _opDomain.GetByUsuarioCriacaoIdAsync(idUsuarioCriacao, idCliente, token);

                var enderecos = await _edDomain.GetAllAsync(oportunidades.Select(o => o.ID).ToList(), token);
                foreach (var opt in oportunidades)
                {
                    opt.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(opt.ID));
                }

                var statuses = await _sDomain.GetAllAsync(idCliente, token);
                foreach (var o in oportunidades)
                {
                    o.OportunidadeStatus = statuses.FirstOrDefault(s => s.ID.Equals(o.OportunidadeStatusID));
                }

                return Ok(oportunidades);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
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
                var oportunidade = await _opDomain.GetByIdAsync(id, token);
                oportunidade.Endereco = await _edDomain.GetByIdAsync(oportunidade.ID, token);
                oportunidade.OportunidadeStatus = (await _sDomain.GetAllAsync(oportunidade.IdCliente, token))
                            .FirstOrDefault(s => s.ID.Equals(oportunidade.OportunidadeStatusID));

                return Ok(oportunidade);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar a oportunidade solicitada. Entre em contato com o suporte.");
            }
        }

        [HttpGet("GetByUser/{idCliente:int}/{idUser:int}/{token}")]
        public async Task<IActionResult> GetOportunidadeByUserIdAsync([FromRoute]string token, [FromRoute]int idCliente, [FromRoute]int idUser)
        {
            try
            {
                var result = await _opDomain.GetUserOportunidadesAsync(token, idCliente, idUser);

                var enderecos = await _edDomain.GetAllAsync(result.Select(x => x.ID).ToList(), token);
                foreach (var r in result)
                {
                    r.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(r.ID));
                }

                var statuses = await _sDomain.GetAllAsync(idCliente, token);
                foreach (var o in result)
                {
                    o.OportunidadeStatus = statuses.FirstOrDefault(s => s.ID.Equals(o.OportunidadeStatusID));
                }

                return Ok(result);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar a oportunidade solicitada. Entre em contato com o suporte.");
            }
        }

        [HttpPost("ApplyUserFor/{token}")]
        public async Task<IActionResult> RelatesUserOportunidadeAsync([FromRoute]string token, [FromBody]UserXOportunidade userXOportunidade)
        {
            try
            {
                await _opDomain.SaveUserXOportunidadeAsync(token, userXOportunidade);

                if (userXOportunidade.StatusID == 1) //Aprovado
                {
                    var op = await _opDomain.GetByIdAsync(userXOportunidade.OportunidadeId, token);
                    if (op != null)
                    {
                        await _emailHandler.EnviarEmailAsync(token, op, userXOportunidade);
                    }
                }

                return Ok("Usuário foi relacionado a oportunidade com sucesso.");
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar relacionar o usuário à oportunidade. Entre em contato com o suporte.");
            }
        }
        
        [HttpGet("GetByDate/{idCliente:int}/{date:datetime}/{token}")]
        public async Task<IActionResult> GetByDate([FromRoute]DateTime date, [FromRoute]string token, [FromRoute]int idCliente)
        {
            try
            {
                var oportunidades = await _opDomain.GetOportunidadesByDateAsync(date, token, idCliente);

                var enderecos = await _edDomain.GetAllAsync(oportunidades.Select(o => o.ID).ToList(), token);
                foreach (var o in oportunidades)
                {
                    o.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(o.ID));
                }

                var statuses = await _sDomain.GetAllAsync(idCliente, token);
                foreach (var o in oportunidades)
                {
                    o.OportunidadeStatus = statuses.FirstOrDefault(s => s.ID.Equals(o.OportunidadeStatusID));
                }

                return Ok(oportunidades);
            }
            catch(InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar as oportunidades solicitadas. Entre em contato com o suporte.");
            }
        }

        [HttpGet("GetByEmpresa/{idEmpresa:int}/{idCliente:int}/{token}")]
        public async Task<IActionResult> GetByEmpresaId([FromRoute]int idEmpresa, [FromRoute]int idCliente, [FromRoute]string token)
        {
            try
            {
                var oportunidades = await _opDomain.GetOportunidadesByEmpresaAsync(idEmpresa, idCliente, token);

                var enderecos = await _edDomain.GetAllAsync(oportunidades.Select(o => o.ID).ToList(), token);
                foreach (var o in oportunidades)
                {
                    o.Endereco = enderecos.FirstOrDefault(e => e.OportunidadeId.Equals(o.ID));
                }

                var statuses = await _sDomain.GetAllAsync(idCliente, token);
                foreach (var o in oportunidades)
                {
                    o.OportunidadeStatus = statuses.FirstOrDefault(s => s.ID.Equals(o.OportunidadeStatusID));
                }

                return Ok(oportunidades);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (EnderecoException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar as oportunidades solicitadas. Entre em contato com o suporte.");
            }
        }

        [HttpGet("GetByOpt/{idOpt:int}/{token}")]
        public async Task<IActionResult> GetUsersByOportunidadeAsync([FromRoute]int idOpt, [FromRoute]string token)
        {
            try
            {
                var users = await _opDomain.GetUsersAsync(idOpt, token);
                return Ok(users);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (ServiceException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar os usuários solicitados. Entre em contato com o suporte.");
            }
        }

        [HttpGet("GetByStatus/{userId:int}/{statusId:int}/{token}")]
        public async Task<IActionResult> GetByUserAndStatusIdAsync([FromRoute]int userId, [FromRoute]int statusId, [FromRoute]string token)
        {
            try
            {
                var oprtunidades = await _opDomain.GetByStatusAndUserAsync(userId, statusId, token);
                return Ok(oprtunidades);
            }
            catch (InvalidTokenException e)
            {
                return StatusCode(401, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (OportunidadeException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (ServiceException e)
            {
                return StatusCode(400, $"{ e.Message } { e.InnerException.Message }");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro ao tentar recuperar os usuários solicitados. Entre em contato com o suporte.");
            }
        }
    }
}