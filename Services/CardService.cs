using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;
using PremierLeagueAPI.Core.Services;

namespace PremierLeagueAPI.Services
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMatchRepository _matchRepository;
        private readonly ICardRepository _cardRepository;

        public CardService(IUnitOfWork unitOfWork,
            IMatchRepository matchRepository,
            ICardRepository cardRepository)
        {
            _unitOfWork = unitOfWork;
            _matchRepository = matchRepository;
            _cardRepository = cardRepository;
        }

        public async Task<IEnumerable<Card>> GetByMatchIdAsync(int matchId)
        {
            return await _cardRepository.GetByMatchIdAsync(matchId);
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            return await _cardRepository.GetAsync(id);
        }

        public async Task<Card> GetDetailByIdAsync(int id)
        {
            return await _cardRepository.GetDetailByIdAsync(id);
        }

        public async Task CreateAsync(Card card)
        {
            _cardRepository.Add(card);

            var match = await _matchRepository.GetAsync(card.MatchId);
            match.IsPlayed = true;

            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(Card card)
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Card card)
        {
            _cardRepository.Remove(card);
            await _unitOfWork.CompleteAsync();
        }
    }
}