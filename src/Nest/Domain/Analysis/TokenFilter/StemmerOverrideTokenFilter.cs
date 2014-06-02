﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nest
{
    /// <summary>
    /// Overrides stemming algorithms, by applying a custom mapping, then protecting these terms from being modified by stemmers. Must be placed before any stemming filters.
    /// </summary>
    public class StemmerOverrideTokenFilter : TokenFilterBase
    {
        public StemmerOverrideTokenFilter()
            : base("stemmer_override")
        {

        }

        /// <summary>
        /// A list of mapping rules to use.
        /// </summary>
        [JsonProperty("rules")]
        public IEnumerable<string> Rules { get; set; }

        /// <summary>
        /// A path (either relative to config location, or absolute) to a list of mappings.
        /// </summary>
        [JsonProperty("rules_path")]
		public string RulesPath { get; set; }

    }
}