using FluentAssertions;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Unit.QueryParsers.Filter
{
	[TestFixture]
	public class HasParentFilterTests : ParseFilterTestsBase 
	{
		[Test]
		[TestCase("cacheName", "cacheKey", true)]
		public void HasParent_Deserializes(string cacheName, string cacheKey, bool cache)
		{
			var hasParentFilter = this.SerializeThenDeserialize(cacheName, cacheKey, cache, 
				f=>f.HasParent,
				f=>f.HasParent<ElasticsearchProject>(d=>d
					.Scope("my_scope")
					.Query(q=>q.Term(p=>p.Country, "value"))
					)
				);
			hasParentFilter.Scope.Should().Be("my_scope");
			var query = hasParentFilter.Query;
			query.Should().NotBeNull();
			query.Term.Field.Should().Be("country");
		}
		
	}
}