public class Scoreboard
{
    int ones { get; set; }
    int twos { get; set; }
    int threes { get; set; }
    int fours { get; set; }
    int fives { get; set; }
    int sixes { get; set; }
    int total_up { get; set; }
    int bonus { get; set; }
    int one_pair { get; set; }
    int two_pairs { get; set; }
    int three_same { get; set; }
    int four_same { get; set; }
    int full_house { get; set; }
    int low_straight { get; set; }
    int high_straight { get; set; }
    int chance { get; set; }
    int yatzy { get; set; }
    int total { get; set; }

    public Scoreboard()
    {
        ones = -1;
        twos = -1;
        threes = -1;
        fours = -1;
        fives = -1;
        sixes = -1;
        total_up = 0;
        bonus = -1;
        one_pair = -1;
        two_pairs = -1;
        three_same = -1;
        four_same = -1;
        full_house = -1;
        low_straight = -1;
        high_straight = -1;
        chance = -1;
        yatzy = -1;
        total = 0;
    }

}