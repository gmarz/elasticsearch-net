using FluentAssertions;
using NUnit.Framework;

namespace Nest.Tests.Unit.QueryParsers.Filter
{
	[TestFixture]
	public class MissingFilterTests : ParseFilterTestsBase 
	{
		[Test]
		[TestCase("cacheName", "cacheKey", true)]
		public void Missing_Deserializes(string cacheName, string cacheKey, bool cache)
		{
			var missingFilter = this.SerializeThenDeserialize(cacheName, cacheKey, cache, 
				f=>f.Missing,
				f=>f.Missing(p=>p.Name)
				);

			missingFilter.Field.Should().Be("name");
		}
		
	}
}