public class Scoreboard
{
    public int[] scores { get; set; }

    // public int ones { get; set; }
    // public int twos { get; set; }
    // public int threes { get; set; }
    // public int fours { get; set; }
    // public int fives { get; set; }
    // public int sixes { get; set; }
    // public int total_up { get; set; }
    // public int bonus { get; set; }
    // public int one_pair { get; set; }
    // public int two_pairs { get; set; }
    // public int three_same { get; set; }
    // public int four_same { get; set; }
    // public int full_house { get; set; }
    // public int low_straight { get; set; }
    // public int high_straight { get; set; }
    // public int chance { get; set; }
    // public int yatzy { get; set; }
    // public int total { get; set; }

    public Scoreboard()
    {
        scores = new int[18];
        scores[(int)Combination.ones] = -1;
        scores[(int)Combination.twos] = -1;
        scores[(int)Combination.threes] = -1;
        scores[(int)Combination.fours] = -1;
        scores[(int)Combination.fives] = -1;
        scores[(int)Combination.sixes] = -1;
        scores[(int)Combination.total_up] = 0;
        scores[(int)Combination.bonus] = 0;
        scores[(int)Combination.one_pair] = -1;
        scores[(int)Combination.two_pairs] = -1;
        scores[(int)Combination.three_same] = -1;
        scores[(int)Combination.four_same] = -1;
        scores[(int)Combination.full_house] = -1;
        scores[(int)Combination.low_straight] = -1;
        scores[(int)Combination.high_straight] = -1;
        scores[(int)Combination.chance] = -1;
        scores[(int)Combination.yatzy] = -1;
        scores[(int)Combination.total] = 0;
    }

}