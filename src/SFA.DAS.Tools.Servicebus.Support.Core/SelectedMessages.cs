using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Tools.Servicebus.Support.Domain;

public class SelectedMessages
{
    [JsonProperty("ids")]
    public IEnumerable<string> Ids { get; set; }
    [JsonProperty("queue")]
    public string Queue { get; set; }
}