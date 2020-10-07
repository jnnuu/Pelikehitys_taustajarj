# Yatzy game api

This project's aim was to implement an api to help yatzy players to manage scores using http requests and input the game data to a MongoDB database. Throwing the dice is not done server-side so in order to play the game you need to have your own set of 5 dice.

## Http commands

View help message               = `GET   /yatzy/help`  
View all games                  = `GET   /yatzy/`  
Creating a game                 = `POST  /yatzy/StartNewGame/amount of players:int`  
Adding players                  = `POST  /yatzy/CreateNewPlayer/Game_id:guid 	// FromBody {"name":"Your Name"}`  
Adding score                    = `POST  /yatzy/AddScore/player_id:guid/field_id:int/score:int`  
Deleting score                  = `POST  /yatzy/DeleteScore/player_id:guid/field_id:int`  
Get player                      = `GET   /yatzy/GetPlayer/player_id:guid`  
Get player scoreboard           = `GET   /yatzy/GetScoreBoard/player_id:guid`  
Get free fields of player       = `GET   /yatzy/GetFreeFields/player_id:guid`  
Get winner of a finished game   = `GET   /yatzy/GetWinner/game_id:guid`  

Generate values for player      = `POST  /yatzy/other/GenerateAll/player_id:guid`  
Reset values of player          = `POST  /yatzy/other/ResetAll/player_id:guid`  
Delete Game with id             = `DELETE  /yatzy/game_id:guid`  
Delete all games                = `DELETE  /yatzy/All/`  

## Score sheet and field ids
```
Title       Field id  
____________________  
ones            = 0  
twos            = 1  
threes          = 2  
fours           = 3  
fives           = 4  
sixes           = 5  
____________________  
total_up        = 6  
bonus           = 7  
____________________  
one_pair        = 8  
two_pairs       = 9  
three_same      = 10  
four_same       = 11  
full_house      = 12  
low_straight    = 13  
high_straight   = 14  
chance          = 15  
yatzy           = 16  
____________________  
total           = 17  
```
## Contributors

- Ville Vahla
- Jere Tienhaara
- Tommi Tahvanainen
- Jonni Uusi-Hakimo