using SlotMachineApi.Entities;

namespace SlotMachineApi.DTO
{
    public class SpinResult
    {
        public int[] Slots { get; set; } 
        public double Balance { get; set; }
        public double Win { get; set; }
    }
}
