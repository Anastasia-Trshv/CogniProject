using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Abstractions.Repositories;

namespace Cogni.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task SetTestResult(UserModel user, int mbtiId)
        {
            await _userRepository.SetTestResult(user, mbtiId);
        }
    }
}
