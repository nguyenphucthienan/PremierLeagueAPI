using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueAPI.Constants;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Queries;
using PremierLeagueAPI.Core.Services;
using PremierLeagueAPI.Dtos.Card;
using PremierLeagueAPI.Helpers;

namespace PremierLeagueAPI.Controllers
{
    [Route("api/matches/{matchId}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlayerService _playerService;
        private readonly ISquadService _squadService;
        private readonly IMatchService _matchService;
        private readonly ICardService _cardService;

        public CardsController(IMapper mapper,
            IPlayerService playerService,
            ISquadService squadService,
            IMatchService matchService,
            ICardService cardService)
        {
            _mapper = mapper;
            _playerService = playerService;
            _squadService = squadService;
            _matchService = matchService;
            _cardService = cardService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCards(int matchId, [FromQuery] CardQuery cardQuery)
        {
            cardQuery.MatchId = matchId;

            var cards = await _cardService.GetAsync(cardQuery);
            var returnCards = _mapper.Map<PaginatedList<CardListDto>>(cards);

            return Ok(returnCards);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCard(int matchId, int id)
        {
            var card = await _cardService.GetDetailByIdAsync(id);

            if (card == null || card.MatchId != matchId)
                return NotFound();

            var returnCard = _mapper.Map<CardDetailDto>(card);
            return Ok(returnCard);
        }

        [HttpPost]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> CreateCard(int matchId, [FromBody] CardCreateDto cardCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var match = await _matchService.GetDetailByIdAsync(matchId);
            var player = await _playerService.GetByIdAsync(cardCreateDto.PlayerId);

            if (match == null || player == null)
                return BadRequest();

            if (cardCreateDto.ClubId != match.HomeClubId && cardCreateDto.ClubId != match.AwayClubId)
                return BadRequest();

            var cardToCreate = _mapper.Map<Card>(cardCreateDto);
            cardToCreate.MatchId = matchId;

            var homeClubSquad = await _squadService
                .GetDetailBySeasonIdAndClubIdAsync(match.SeasonId, match.HomeClubId);
            var awayClubSquad = await _squadService
                .GetDetailBySeasonIdAndClubIdAsync(match.SeasonId, match.AwayClubId);

            bool isHomePlayer;
            if (player.SquadPlayers.Any(sp => sp.SquadId == homeClubSquad.Id))
                isHomePlayer = true;
            else if (player.SquadPlayers.Any(sp => sp.SquadId == awayClubSquad.Id))
                isHomePlayer = false;
            else
                return BadRequest();

            if ((isHomePlayer && cardCreateDto.ClubId != match.HomeClubId)
                || (!isHomePlayer && cardCreateDto.ClubId != match.AwayClubId))
                return BadRequest();

            var existRedCard = match.Cards
                .SingleOrDefault(c => c.CardType == CardType.Red && c.PlayerId == player.Id);

            if (existRedCard != null)
                return BadRequest();

            var existYellowCard = match.Cards
                .SingleOrDefault(c => c.CardType == CardType.Yellow && c.PlayerId == player.Id);

            Card redCardToCreate = null;
            if (existYellowCard != null && cardToCreate.CardType == CardType.Yellow)
            {
                redCardToCreate = new Card
                {
                    MatchId = matchId,
                    ClubId = cardToCreate.ClubId,
                    PlayerId = cardToCreate.PlayerId,
                    CardType = CardType.Red,
                    CardTime = cardToCreate.CardTime
                };
            }

            await _cardService.CreateAsync(cardToCreate);

            if (redCardToCreate != null)
                await _cardService.CreateAsync(redCardToCreate);

            var card = await _cardService.GetDetailByIdAsync(cardToCreate.Id);
            var returnCard = _mapper.Map<CardDetailDto>(card);

            return Ok(returnCard);
        }

        [HttpPut("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> UpdateCard(int matchId, int id, [FromBody] CardUpdateDto cardUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var match = await _matchService.GetByIdAsync(matchId);
            var player = await _playerService.GetByIdAsync(cardUpdateDto.PlayerId);

            if (match == null || player == null)
                return BadRequest();

            var card = await _cardService.GetByIdAsync(id);

            if (card == null)
                return NotFound();

            if (card.MatchId != matchId)
                return BadRequest();

            _mapper.Map(cardUpdateDto, card);

            var homeClubSquad = await _squadService
                .GetDetailBySeasonIdAndClubIdAsync(match.SeasonId, match.HomeClubId);
            var awayClubSquad = await _squadService
                .GetDetailBySeasonIdAndClubIdAsync(match.SeasonId, match.AwayClubId);

            bool isHomePlayer;
            if (player.SquadPlayers.Any(sp => sp.SquadId == homeClubSquad.Id))
                isHomePlayer = true;
            else if (player.SquadPlayers.Any(sp => sp.SquadId == awayClubSquad.Id))
                isHomePlayer = false;
            else
                return BadRequest();

            if ((isHomePlayer && cardUpdateDto.ClubId != match.HomeClubId)
                || (!isHomePlayer && cardUpdateDto.ClubId != match.AwayClubId))
                return BadRequest();

            await _cardService.UpdateAsync(card);

            var updatedCard = await _cardService.GetDetailByIdAsync(id);
            var returnCard = _mapper.Map<CardDetailDto>(updatedCard);

            return Ok(returnCard);
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.RequiredAdminRole)]
        public async Task<IActionResult> DeleteCard(int matchId, int id)
        {
            var card = await _cardService.GetByIdAsync(id);

            if (card == null)
                return NotFound();

            if (card.MatchId != matchId)
                return BadRequest();

            await _cardService.DeleteAsync(card);

            return Ok(id);
        }
    }
}