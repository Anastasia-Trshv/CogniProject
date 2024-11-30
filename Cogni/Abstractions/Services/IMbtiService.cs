namespace Cogni.Abstractions.Services
{
    public interface IMbtiService
    {
        Task<int> GetMbtiTypeIdByName(string nameOfType);
    }
}
