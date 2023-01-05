public class ScoreModel
{
    public string author { get; set; }
    public int score { get; set; }
    public float secondsSurvived { get; set; }
    public string tmpScoreId { get; set; }

    public override string ToString()
    {
        return "From=" + author + ";Score=" + score + ";SecondsSurvived=" + secondsSurvived;
    }
}
