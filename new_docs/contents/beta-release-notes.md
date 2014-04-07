---
template: layout.jade
title: Beta Release Notes
menusection: concepts
menuitem: beta-release-notes
---

# 1.0 beta 1

It's been 4 months since [0.12.0](https://github.com/elasticsearch/elasticsearch-net/releases/tag/0.12.0.0) was released and a lot has happened in those months, elasticsearch released 1.0 and even a 1.1. With this release announcement I'm pleased to say NEST finally caught up and brings (almost) all the awesome that 1.0 brings to the .NET world. 

Before diving in all the technical details of the 1.0 release I would first like to express a sincere thank you to all of you who've reached out on twitter, github issues and mail. Your kind words made all the difference these past 4 months switching jobs, refactoring the client, implementing 1.0 features. 

What’s the ‘beta’ mean? All tests are passing, but we are still waiting for at least one new feature to land. Additionally, beta means that some new features may change before general release. In addition this beta period is intended to solicit as much feedback from everyone on breaking changes/oversights/bugs.

# Breaking Changes

This is a 1.0 release and will have many breaking changes. The `0.*` NEST releases have been around since 2010 and it's internals were showing its age. This 1.0 release represents an almost completely refactored `NEST`.

A separate section that lists the breaking changes [can be found here](http://nest.azurewebsites.net/breaking-changes.html). If you find any breaking changes that are not documented we'd love to hear about them on our [github issues](http://www.github.com/elasticsearch/elasticsearch-net/issues).

# Split in to two. 
NEST 0.12.0 introduced a seperate, completely generated, client interface called `IRawElasticClient` which allowed you to build your own requests and responses without having to worry about building endpoints. Much of the work for 1.0 has been around refactoring this out of the NEST assembly and make NEST's strongly typed `IElasticClient` use `IRawElasticClient` internally. 


## Elasticsearch.Net

 Elasticsearch.Net's client is now called `IElasticsearchClient` and is almost completely generated from the client API spec and exposes all the Elasticsearch 1.0 endpoints. It brings a client that's inline in spirit and architecture with the other official elasticsearch client libraries.

`Elasticsearch.Net` leaves all the request and response mapping up to you although it comes with support for mapping responses into a dynamic container (a slightly modifed DynamicDictionary from [NancyFx](http://www.nancyfx.org)) out of the box. Elasticsearch.Net is very much intended to be used by other high level clients.

Another big new feature is [built-in clusterfailover/connectionpooling support](/elasticearch-net/cluster-failover.html) support.

Please read [the new documentation for Elasticsearch.Net](/elasticsearch-net/quick-start.html) to find out more

## NEST 

`NEST`'s client classes have completely been rewritten and internally uses and still exposes the `Elasticsearch.Net` client. It benefits from it's cluster failover support and it no longer has complicated path builders.

`NEST` is highly opinionated how you should build and consume elasticsearch responses and comes with a strongly typed query dsl. NEST has a really high coverage of mapped endpoints and [the unmapped API's are all identified and on the roadmap](https://github.com/elasticsearch/elasticsearch-net/issues?direction=desc&milestone=5&page=1&sort=updated&state=open)

#Changelog

This release consists of 300+ commits over a 4 month timespan so will just list the highlights

### Aggregations
All 1.0 and 1.1! aggregations have been mapped in NEST 1.0.

     var results = this._client.Search<ElasticsearchProject>(s=>s
        .Size(0)
        .Aggregations(a=>a
            .Nested("contributors", n=>n
                .Path(p=>p.Contributors)
                .Aggregations(t=>t
                    .Average("avg_age", m=>m
                        .Field(p=>p.Contributors.First().Age)
                    )
                )
            )
        )
    );

    var bucket = results.Aggs.Nested("contributors");
    bucket.DocCount.Should().BeGreaterThan(1);
    var averageAge = bucket.Average("avg_age");
    averageAge.Value.Should().HaveValue().And.BeGreaterOrEqualTo(18);

### Conditionless() query construct
NEST comes with a powerful feature called conditionless queries which greatly simplifies writing queries with optional parts.

Consider this example:

    .Query(q=>q.Term("this_term_is_conditionless", ""))

NEST will remove the term query and default to not sending a query at all in the body (equivalant to `match_all`)

But wat if you want a different query to be run in instead of `match_all`? This is where the `Conditionless` construct comes into play:

    .Query(q=>q
        .Conditionless(qs=>qs
            .Query(qcq=>qcq.Term("this_term_is_conditionless", ""))
            .Fallback(qcf=>qcf.Term("name", "do_me_instead")
        )
    )

Now we'll fallback into a `term` query on `name` instead. Note that fallbacks themselves can be conditionless and `Conditionless()` constructs can be nested inside fallbacks as well.

Also if you want to disable conditionless queries you can specify `.Strict()` on individual parts in the query or globally on the search descriptor. 

    .Query(q=>q.Strict().Term("this_term_is_conditionless", ""))

The previous example will throw. To enable conditionless only for certain parts in your query you can also do:

    .Strict()
    .Query(q=>
        q.Term("must", "exist") 
        && q.Strict(false).Term("this_term_is_conditionless", "")
    )

The previous example will just send a `term` query on the field `must`, NEST is smart enough to infer the bool is no longer needed.

Finally if you really intend to send a conditionless query to Elasticsearch you can use `.Verbatim()`

    .Query(q=>q.Verbatim().Term("this_term_is_conditionless", ""))

The `Strict()` and `Verbatim()` constructs are not new but worth repeating in this context.

### Pull Requests
As with every release the level of community engangement never ceases to amaze me. Whoever proclaimed OSS in .net is dead?

A huge thank you goes out to the folks who submitted the following pull requests.

* [#412](https://github.com/elasticsearch/elasticsearch-net/pull/412) Add `random_score` support for FunctionScore(). ty @vovikdrg !
* [#419](https://github.com/elasticsearch/elasticsearch-net/pull/419) Add script score support for FunctionScore().  ty @V1tOr !
* [#425](https://github.com/elasticsearch/elasticsearch-net/pull/419) Added support for index status API. ty @richclement !
* [#430](https://github.com/elasticsearch/elasticsearch-net/pull/430) Allow any object to be used for a partial update. ty @Plasma !
* [#431](https://github.com/elasticsearch/elasticsearch-net/pull/431) Add Type as constructor parameter to CustomAnalyzer. ty @sschlesier !
* [#440](https://github.com/elasticsearch/elasticsearch-net/pull/440) Support dictionary key evaluation in expressions. ty @azubanov !
* [#440](https://github.com/elasticsearch/elasticsearch-net/pull/440) Support dictionary key evaluation in expressions. ty @azubanov !
* [#444](https://github.com/elasticsearch/elasticsearch-net/pull/444) Renamed QueryString() to Query() MatchQueryDescriptor. ty @gmarz !
* [#445](https://github.com/elasticsearch/elasticsearch-net/pull/445) Renamed QueryString() to Query() MultiMatchQueryDescriptor. ty @gmarz !
* [#446](https://github.com/elasticsearch/elasticsearch-net/pull/446) Renamed QueryString() to Query() TextQueryDescriptor. ty @gmarz !
* [#447](https://github.com/elasticsearch/elasticsearch-net/pull/447) Added Params() support to script on RangeFacetDescriptor. ty @angielamb !
* [#448](https://github.com/elasticsearch/elasticsearch-net/pull/448) Added filter option in `function_score` queries. ty @gmarz !
* [#479](https://github.com/elasticsearch/elasticsearch-net/pull/479) Added detail response mapped to `_explain`. ty @andreabalducci !
* [#448](https://github.com/elasticsearch/elasticsearch-net/pull/488) Added nested field attribute mapped back in. ty @lukapor !
* [#490](https://github.com/elasticsearch/elasticsearch-net/pull/490) Added size parameter to CompletionSuggestDescriptor. ty @bgiromini !
* [#530](https://github.com/elasticsearch/elasticsearch-net/pull/540) Added missing sort options. ty @Grastveit !
* [#536](https://github.com/elasticsearch/elasticsearch-net/pull/536) Worlds smallest pull request (typo). ty @ChrisMcKee !
* [#541](https://github.com/elasticsearch/elasticsearch-net/pull/541) FunctionScoreQueryDescriptor boost mode. ty @azubanov !
* [#542](https://github.com/elasticsearch/elasticsearch-net/pull/542) Allow fields boosting when using expressions in multimatch query. ty @azubanov !
* [#545](https://github.com/elasticsearch/elasticsearch-net/pull/545) Show requests/response as json string in debug mode when available. ty @azubanov !
* [#548](https://github.com/elasticsearch/elasticsearch-net/pull/548) Create cache directory for code generation if it does not exist. ty @gmarz !
* [#550](https://github.com/elasticsearch/elasticsearch-net/pull/550) Fixed typos, formatting, verbiage, naming conventions in the documentation. ty @paigecook !
* [#563](https://github.com/elasticsearch/elasticsearch-net/pull/563) Added support for new text query types on the `multi_match` query. ty @azubanov !
* [#565](https://github.com/elasticsearch/elasticsearch-net/pull/565) Thrift connection does not clean up properly after going into faulted state. ty @danp60 !

In addition a huge thank you goes out to all the folks who gave continuous feedback on the state of `master` while it underwent major refactoring. 
A special shout out to @synhershko and @icanhasjonas for opening [#565](https://github.com/elasticsearch/elasticsearch-net/pull/565) and [#537](https://github.com/elasticsearch/elasticsearch-net/pull/537)

Seeing [tweets](https://twitter.com/luigiberrettini/status/448413008604438528) [like](https://twitter.com/luigiberrettini/status/448413008604438528) [this](https://twitter.com/tonariman/status/451354457133969408) is awesome, ty @nariman-haghighi & @luigiberrettini !





