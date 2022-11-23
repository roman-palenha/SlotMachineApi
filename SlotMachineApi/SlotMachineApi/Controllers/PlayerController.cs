using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlotMachineApi.DTO;
using SlotMachineApi.Entities;
using SlotMachineApi.Services;

namespace SlotMachineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterPlayer registerPlayer)
        {
            try
            {
                var player = new Player
                {
                    UserName = registerPlayer.UserName,
                    Balance = registerPlayer.Balance
                };
                await _playerService.CreateAsync(player);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateBalance(string username, double balance)
        {
            try
            {
                await _playerService.UpdateBalanceAsync(username, balance);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
