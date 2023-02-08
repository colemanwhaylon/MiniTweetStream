namespace TwitterLib.Driver.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public record Data(IReadOnlyList<string> edit_history_tweet_ids, string id, string text);

    public record Root(Data data);
}
