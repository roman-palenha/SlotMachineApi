using SlotMachineApi.Entities;

namespace SlotMachineApi.Services
{
    public interface IMachineService
    {
        Task RefreshArray(string id, int newSize);
        int[] ReturnSlotsArray(Machine game);
        Task<Machine> GetById(string id);
        Task Create(Machine game);
    }
}
