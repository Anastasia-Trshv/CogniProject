using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Database.Context;

namespace Cogni.Services
{
    public class MbtiService : IMbtiService
    {
        private readonly IMbtiRepository _mbtiRepository;

        public MbtiService(IMbtiRepository mbtiRepository) 
        {
            _mbtiRepository = mbtiRepository;
        }

        public async Task<int> GetMbtiTypeIdByName(string nameOfType)
        {
            return await _mbtiRepository.GetMbtiTypeByName(nameOfType);
        }
    }
}
