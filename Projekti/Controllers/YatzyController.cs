using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Projekti.Controllers
{
    [ApiController]
    [Route("yatzy")]

    public class YatzyController : ControllerBase
    {
        private readonly ILogger<YatzyController> _logger;
        private readonly IRepository _repository;

        public YatzyController(ILogger<YatzyController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        [Route("StartNewGame/{numberOfPlayers:int}")]
        public async Task<Game> StartNewGame(int numberOfPlayers)
        {
            Game newGame = new Game();
            newGame.Id = Guid.NewGuid().ToString();
            Console.WriteLine(newGame.Id);
            newGame._numberOfPlayers = numberOfPlayers;
            return await _repository.StartNewGame(newGame);
        }

        [HttpPost]
        [Route("CreateAPlayer/{id}")]
        public async Task<Player> CreateAPlayer(String id, [FromBody] NewPlayer newplayer)
        {
            Player player = new Player();
            player.Id = Guid.NewGuid().ToString();
            player.name = newplayer.name;
            await _repository.CreateAPlayer(player, id);

            return player;
        }

        [HttpGet]
        [Route("{id}")]

        public async Task<int> GetScore(String id)
        {
            return await _repository.GetScore(id);
        }

        [HttpPost]
        [Route("AddScore/{id}/{combination:int}/{score:int}")]
        public async Task<Player> AddScore(String id, int score, int combination)
        {
            return await _repository.AddScore(id, score, (Combination)combination);

        }

        [HttpGet]
        [Route("GetPlayer/{id}")]
        public async Task<Player> GetPlayer(String id)
        {
            return await _repository.GetPlayer(id);
        }
        [HttpGet]
        [Route("GetWinner/{id_game}")]
        public async Task<Player> GetWinner(String id_game)
        {
            return await _repository.GetWinner(id_game);
        }
        [HttpGet]
        public async Task<Game[]> GetGames()
        {
            return await _repository.GetGames();
        }
        [HttpGet]
        [Route("Help")]
        public async Task<String> Help()
        {
            return await _repository.Help();
        }

    }
}