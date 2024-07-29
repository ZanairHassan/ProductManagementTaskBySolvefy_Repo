using BLL.Interfaces;
using DAL.DBContext;
using DAL.ViewModels;
using BLL.Interfaces;
using DAL.DBContext;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace ProductManagement.Controllers
{
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public TokenController(ITokenService tokenService, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _tokenService = tokenService;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        [Route("CreateToken")]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] AddTokenVM tokenVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoggingUtility.ExcLog(ModelState.ToString(), _serviceProvider);
                    return BadRequest(ModelState);
                }
                await _tokenService.CreateToken(tokenVM);
                LoggingUtility.LogTxt("CreateToken: Token Created", _configuration);
                return Ok("Token Created");
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
                throw;
            }
        }

        [Route("DeleteToken/{tokenID}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteToken(int tokenID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoggingUtility.ExcLog(ModelState.ToString(), _serviceProvider);
                    return BadRequest(ModelState);
                }
                var deleteToken = await _tokenService.DeleteToken(tokenID);
                LoggingUtility.LogTxt($"DeleteToken: Token with ID {tokenID} deleted", _configuration);
                return Ok(deleteToken);
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
                throw;
            }
        }

        [Route("GetAllToken")]
        [HttpGet]
        public async Task<IActionResult> GetAllToken()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoggingUtility.ExcLog(ModelState.ToString(), _serviceProvider);
                    return BadRequest(ModelState);
                }
                var allToken = await _tokenService.GetAllToken();
                LoggingUtility.LogTxt("GetAllToken: Retrieved all tokens", _configuration);
                return Ok(allToken);
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
                throw;
            }
        }

        [Route("GetToken/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetToken(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoggingUtility.ExcLog(ModelState.ToString(), _serviceProvider);
                    return BadRequest(ModelState);
                }
                var token = await _tokenService.GetToken(id);
                LoggingUtility.LogTxt($"GetToken: Retrieved token with ID {id}", _configuration);
                return Ok(token);
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
                throw;
            }
        }

        [Route("UpdateToken/{tokenID}")]
        [HttpPost]
        public async Task<IActionResult> UpdateToken(int tokenID, AddTokenVM tokenVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoggingUtility.ExcLog(ModelState.ToString(), _serviceProvider);
                    return BadRequest(ModelState);
                }
                var updateToken = await _tokenService.UpdateToken(tokenID, tokenVM);
                LoggingUtility.LogTxt($"UpdateToken: Token with ID {tokenID} updated", _configuration);
                return Ok(updateToken);
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
                throw;
            }
        }
    }
}
