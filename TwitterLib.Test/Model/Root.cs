using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterLib.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public record Data(IReadOnlyList<string> edit_history_tweet_ids, string id, string text);

    public record Root(Data data);
}
