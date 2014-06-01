﻿using Elasticsearch.Net;
using NUnit.Framework;
using Nest.Tests.MockData.Domain;

namespace Nest.Tests.Unit.Search.Suggest
{
	[TestFixture]
	public class PhraseSuggestTests : BaseJsonTests
	{
		[Test]
        public void PhraseSuggestDescriptorTest()
		{
		    var phraseSuggestDescriptor = new PhraseSuggestDescriptor<ElasticsearchProject>()
		        .Analyzer("body")
		        .OnField("bigram")
		        .Size(1)
		        .MaxErrors(0.5m)
		        .GramSize(2);

            var json = TestElasticClient.Serialize(phraseSuggestDescriptor);

            var expected = @"{
                              ""field"": ""bigram"",
                              ""analyzer"": ""body"",
                              ""size"": 1,
                              ""gram_size"": 2,
                              ""max_errors"": 0.5,
                            }";

            Assert.True(json.JsonEquals(expected), json);
        }

        [Test]
        public void PhraseSuggestDescriptorDirectGeneratorTest()
        {
            var phraseSuggestDescriptor = new PhraseSuggestDescriptor<ElasticsearchProject>()
                .Analyzer("body")
                .DirectGenerator(m => m.OnField("body").SuggestMode(SuggestModeOptions.Always).MinWordLength(3));

            var json = TestElasticClient.Serialize(phraseSuggestDescriptor);

            var expected = @"{
                              ""analyzer"": ""body"",
                              ""direct_generator"": [
                                {
                                  ""field"": ""body"",
                                  ""suggest_mode"": ""always"",
                                  ""min_word_len"": 3
                                }
                              ],
                            }";

            Assert.True(json.JsonEquals(expected), json);
        }

		[Test]
		public void PhraseSuggestOnSearchTest()
		{
			var search = this._client.Search<ElasticsearchProject>(s => s
				.SuggestPhrase("myphrasesuggest", ts => ts
					.Text("n")
					.Analyzer("body")
					.OnField("bigram")
					.Size(1)
					.MaxErrors(0.5m)
					.GramSize(2)
				)
			);

			var expected = @"{
				suggest: {
					myphrasesuggest: {
						text: ""n"",
						phrase: {
                              ""field"": ""bigram"",
                              ""analyzer"": ""body"",
                              ""size"": 1,
							  ""gram_size"": 2,
                              ""max_errors"": 0.5
						}
					}
				}
			}";
			var json = search.ConnectionStatus.Request.Utf8String();
			Assert.True(json.JsonEquals(expected), json);
		}
	} 
}
