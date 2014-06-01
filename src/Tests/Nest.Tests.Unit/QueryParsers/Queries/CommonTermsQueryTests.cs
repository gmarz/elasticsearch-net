using FluentAssertions;
using NUnit.Framework;

namespace Nest.Tests.Unit.QueryParsers.Queries
{
	[TestFixture]
	public class CommonTermsQueryTests : ParseQueryTestsBase
	{
		[Test]
		public void CommonTerms_Deserializes()
		{
			var q = this.SerializeThenDeserialize(
				f=>f.CommonTerms,
				f=>f.CommonTerms(ct=>ct
					.Analyzer("my_analyzer")
					.Boost(1.1)
					.CutOffFrequency(2.0)
					.DisableCoord()
					.HighFrequencyOperator(Operator.or)
					.LowFrequencyOperator(Operator.and)
					.MinimumShouldMatch(2)
					.OnField(p=>p.Name)
					.Query("query")
					)
				);
			q.Analyzer.Should().Be("my_analyzer");
			q.Boost.Should().Be(1.1);
			q.CutoffFrequency.Should().Be(2.0);
			q.DisableCoord.Should().BeTrue();
			q.HighFrequencyOperator.Should().Be(Operator.or);
			q.LowFrequencyOperator.Should().Be(Operator.and);
			q.MinimumShouldMatch.Should().Be("2");
			q.Field.Should().Be("name");
			q.Query.Should().Be("query");
		}
	}
}