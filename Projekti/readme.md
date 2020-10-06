# Yatzy game api

This project's aim was to implement an api to help yatzy players to manage scores using http requests and input the data to a MongoDB database. Throwing the dice is not done server-side so in order to play the game you need to have your own set of 5 dice.

## Http commands

View all games                  = GET   /yatzy/ \n
Creating a game                 = POST  /yatzy/StartNewGame/<amount of players:int> \n
Adding players                  = POST  /yatzy/CreateNewPlayer/<Game id:guid> 	// FromBody {"name":"<Your Name>"}
Adding score                    = POST  /yatzy/AddScore/<player id:guid>/<field id:int>/<score:int>
Deleting score                  = POST  /yatzy/DeleteScore/<player id:guid>/<field id:int>
Get player                      = GET   /yatzy/GetPlayer/<player id:guid>
Get player scoreboard           = GET   /yatzy/GetScoreBoard/<player id:guid>
Get free fields of player       = GET   /yatzy/GetFreeFields/<player id: guid>
Get winner of a finished game   = GET   /yatzy/GetWinner/<game id:guid> 

Generate values for player      = POST  /yatzy/other/GenerateAll/<player id:guid>
Reset values of player          = POST  /yatzy/other/ResetAll/<player id:guid>
Delete Game with id             = DELETE  /yatzy/<game id:guid>
Delete all games                = DELETE  /yatzy/All/

## Contributors

- Ville Vahla
- Jere Tienhaara
- Tommi Tahvanainen
- Jonni Uusi-Hakimo