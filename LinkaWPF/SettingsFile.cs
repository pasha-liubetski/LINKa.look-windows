﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace LinkaWPF
{
    public class SettingsFile
    {
        [JsonProperty("keys")]
        public Dictionary<string, string> Keys { get; set; }

        [JsonProperty("isHasGazeEnabled")]
        public bool? IsHasGazeEnabled { get; set; }

        [JsonProperty("isAnimatedClickEnabled")]
        public bool? IsAnimatedClickEnabled { get; set; }

        [JsonProperty("clickDelay")]
        public double? ClickDelay { get; set; }

        [JsonProperty("mousePointReactionFilter")]
        public double? MousePointReactionFilter { get; set; }

        [JsonProperty("isPlayAudioFromCard")]
        public bool? IsPlayAudioFromCard { get; set; }

        [JsonProperty("isPageButtonVisible")]
        public bool? IsPageButtonVisible { get; set; }

        [JsonProperty("isKeyboardEnabled")]
        public bool? IsKeyboardEnabled { get; set; }

        [JsonProperty("isMouseEnabled")]
        public bool? IsMouseEnabled { get; set; }
        [JsonProperty("isOutputType")]
        public bool? IsOutputType { get; set; }

        [JsonProperty("voiceId")]
        public string VoiceId { get; set; }
    }
}
