/// <summary>
/// Database class (code-first)
/// Implemented players resources for game
/// </summary>
public class GameResources
{
    public int Id { get; set; } // the current player ID
    public int Money { get; set; }
    public int Gems { get; set; }
    public int Tickets { get; set; }
    public int BestScore_Classic { get; set; }
    public int BestScore_Challenge { get; set; }
}
