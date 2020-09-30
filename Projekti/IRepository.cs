using System;
using System.Threading.Tasks;

public enum Combination
{
    ones, twos, threes, fours, fives, sixes, total_up, bonus, one_pair, two_pairs, three_same, four_same, full_house, low_straight, high_straight, chance, yatzy, total
}

public interface IRepository
{
    Task<Player> GetPlayer(Guid id);
    Task<Player> AddScoreToOnes(Guid id, int score, Combination combination);


}