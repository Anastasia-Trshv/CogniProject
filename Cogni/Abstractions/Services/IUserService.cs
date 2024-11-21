using Cogni.Models;
using Cogni.Database.Entities;

namespace Cogni.Abstractions.Services
{
    public interface IUserService
    {
        Task SetTestResult(UserModel user, int mbtiId);
    }
}
