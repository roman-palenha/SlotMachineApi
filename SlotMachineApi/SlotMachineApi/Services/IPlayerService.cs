using SlotMachineApi.Entities;

namespace SlotMachineApi.Services
{
    public interface IPlayerService
    {
        Task<Player> GetByNameAsync(string username);
        Task CreateAsync(Player player);
        Task UpdateBalanceAsync(string username, double balance);
        Task DeleteAsync(Player player);
    }
}
