using LinkaWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LinkaWPF
{
    public class CardSetFile
    {
        public CardSetFile(int columns, int rows, bool withoutSpace, IList<Card> cards, String description, bool directSet)
        {
            Version = "1.2";
            Columns = columns;
            Rows = rows;
            WithoutSpace = withoutSpace;
            Cards = cards;
            Description = description;
            DirectSet = directSet;
        }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("directSet")]
        public bool DirectSet { get; set; }
        
        [JsonProperty("columns")]
        public int Columns { get; set; }

        [JsonProperty("rows")]
        public int Rows { get; set; }

        [JsonProperty("withoutSpace")]
        public bool WithoutSpace { get; set; }

        [JsonProperty("cards")]
        public IList<Card> Cards { get; set; }
    }
}
