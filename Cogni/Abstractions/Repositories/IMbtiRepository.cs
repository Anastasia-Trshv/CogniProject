namespace Cogni.Abstractions.Repositories
{
    public interface IMbtiRepository
    {
        Task<int> GetMbtiTypeByName(string nameOfType);
    }
}
