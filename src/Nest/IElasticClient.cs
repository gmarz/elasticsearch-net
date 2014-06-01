﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Nest.Domain;

namespace Nest
{
	public interface IElasticClient
	{
		IConnection Connection { get; }
		INestSerializer Serializer { get; }
		IElasticsearchClient Raw { get; }
		ElasticInferrer Infer { get; }

		/// <summary>
		/// Helper method that allows you to reindex from one index into another using SCAN and SCROLL.
		/// </summary>
		/// <returns>An IObservable you can subscribe to to listen to the progress of the reindexation process</returns>
		IObservable<IReindexResponse<T>> Reindex<T>(Func<ReindexDescriptor<T>, ReindexDescriptor<T>> reindexSelector)
			where T : class;
		
		/// <summary>
		/// A search request can be scrolled by specifying the scroll parameter. 
		/// <para>The scroll parameter is a time value parameter (for example: scroll=5m), 
		/// indicating for how long the nodes that participate in the search will maintain relevant resources in
		/// order to continue and support it.</para><para> 
		/// This is very similar in its idea to opening a cursor against a database.</para>
		/// <para> </para><para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-request-scroll.html</para>
		/// </summary>
		/// <typeparam name="T">The type that represents the result hits</typeparam>
		/// <param name="scrollSelector">A descriptor that describes the scroll operation</param>
		/// <returns>A query response holding T hits as well as the ScrollId for the next scroll operation</returns>
		ISearchResponse<T> Scroll<T>(Func<ScrollDescriptor<T>, ScrollDescriptor<T>> scrollSelector)
			where T : class;
		
		/// <summary>
		/// A search request can be scrolled by specifying the scroll parameter. 
		/// <para>The scroll parameter is a time value parameter (for example: scroll=5m), 
		/// indicating for how long the nodes that participate in the search will maintain relevant resources in
		/// order to continue and support it.</para><para> 
		/// This is very similar in its idea to opening a cursor against a database.</para>
		/// <para> </para><para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-request-scroll.html</para>
		/// </summary>
		/// <typeparam name="T">The type that represents the result hits</typeparam>
		/// <param name="scrollSelector">A descriptor that describes the scroll operation</param>
		/// <returns>A query response holding T hits as well as the ScrollId for the next scroll operation</returns>
		Task<ISearchResponse<T>> ScrollAsync<T>(Func<ScrollDescriptor<T>, ScrollDescriptor<T>> scrollSelector)
			where T : class;

		/// <summary>
		/// The update API allows to update a document based on a script provided. 
		/// <para>The operation gets the document (collocated with the shard) from the index, runs the script 
		/// (with optional script language and parameters), and index back the result 
		/// (also allows to delete, or ignore the operation). </para>
		/// <para>It uses versioning to make sure no updates have happened during the "get" and "reindex".</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-update.html
		/// </summary>
		/// <typeparam name="T">The type to describe the document to be updated</typeparam>
		/// <param name="updateSelector">a descriptor that describes the update operation</param>
		IUpdateResponse Update<T>(Func<UpdateDescriptor<T, T>, UpdateDescriptor<T, T>> updateSelector) where T : class;
		
		/// <summary>
		/// The update API allows to update a document based on a script provided. 
		/// <para>The operation gets the document (collocated with the shard) from the index, runs the script 
		/// (with optional script language and parameters), and index back the result 
		/// (also allows to delete, or ignore the operation). </para>
		/// <para>It uses versioning to make sure no updates have happened during the "get" and "reindex".</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-update.html
		/// </summary>
		/// <typeparam name="T">The type to describe the document to be updated</typeparam>
		/// <typeparam name="K">The type of the partial update document</typeparam>
		/// <param name="updateSelector">a descriptor that describes the update operation</param>
		IUpdateResponse Update<T, K>(Func<UpdateDescriptor<T, K>, UpdateDescriptor<T, K>> updateSelector)
			where T : class
			where K : class;
		
		/// <summary>
		/// The update API allows to update a document based on a script provided. 
		/// <para>The operation gets the document (collocated with the shard) from the index, runs the script 
		/// (with optional script language and parameters), and index back the result 
		/// (also allows to delete, or ignore the operation). </para>
		/// <para>It uses versioning to make sure no updates have happened during the "get" and "reindex".</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-update.html
		/// </summary>
		/// <typeparam name="T">The type to describe the document to be updated</typeparam>
		/// <param name="updateSelector">a descriptor that describes the update operation</param>
		Task<IUpdateResponse> UpdateAsync<T>(Func<UpdateDescriptor<T, T>, UpdateDescriptor<T, T>> updateSelector)
			where T : class;
		
		/// <summary>
		/// The update API allows to update a document based on a script provided. 
		/// <para>The operation gets the document (collocated with the shard) from the index, runs the script 
		/// (with optional script language and parameters), and index back the result 
		/// (also allows to delete, or ignore the operation). </para>
		/// <para>It uses versioning to make sure no updates have happened during the "get" and "reindex".</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-update.html
		/// </summary>
		/// <typeparam name="T">The type to describe the document to be updated</typeparam>
		/// <typeparam name="K">The type of the partial update document</typeparam>
		/// <param name="updateSelector">a descriptor that describes the update operation</param>
		Task<IUpdateResponse> UpdateAsync<T, K>(Func<UpdateDescriptor<T, K>, UpdateDescriptor<T, K>> updateSelector)
			where T : class
			where K : class;

		/// <summary>
		/// Change specific index level settings in real time. Note not all index settings CAN be updated.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-update-settings.html
		/// </summary>
		/// <param name="updateSettingsSelector">A descriptor that strongly types all the updateable settings</param>
		IAcknowledgedResponse UpdateSettings( Func<UpdateSettingsDescriptor, UpdateSettingsDescriptor> updateSettingsSelector); 
		
		/// <summary>
		/// Change specific index level settings in real time. Note not all index settings CAN be updated.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-update-settings.html
		/// </summary>
		/// <param name="updateSettingsSelector">A descriptor that strongly types all the updateable settings</param>
		Task<IAcknowledgedResponse> UpdateSettingsAsync(Func<UpdateSettingsDescriptor, UpdateSettingsDescriptor> updateSettingsSelector);

		/// <summary>
		/// The validate API allows a user to validate a potentially expensive query without executing it.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-validate.html
		/// </summary>
		/// <typeparam name="T">The type used to describe the query</typeparam>
		/// <param name="querySelector">A descriptor that describes the query operation</param>
		IValidateResponse Validate<T>(Func<ValidateQueryDescriptor<T>, ValidateQueryDescriptor<T>> querySelector) 
			where T : class;
		
		/// <summary>
		/// The validate API allows a user to validate a potentially expensive query without executing it.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-validate.html
		/// </summary>
		/// <typeparam name="T">The type used to describe the query</typeparam>
		/// <param name="querySelector">A descriptor that describes the query operation</param>
		Task<IValidateResponse> ValidateAsync<T>(Func<ValidateQueryDescriptor<T>, ValidateQueryDescriptor<T>> querySelector) 
			where T : class;
		/// <summary>
		/// The open and close index APIs allow to close an index, and later on opening it. 
		/// A closed index has almost no overhead on the cluster (except for maintaining its metadata), and is blocked 
		/// for read/write operations. 
		/// A closed index can be opened which will then go through the normal recovery process.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-open-close.html
		/// </summary>
		/// <param name="openIndexSelector">A descriptor thata describes the open index operation</param>
		IIndicesOperationResponse OpenIndex(Func<OpenIndexDescriptor, OpenIndexDescriptor> openIndexSelector);
		
		/// <summary>
		/// The open and close index APIs allow to close an index, and later on opening it. 
		/// A closed index has almost no overhead on the cluster (except for maintaining its metadata), and is blocked 
		/// for read/write operations. 
		/// A closed index can be opened which will then go through the normal recovery process.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-open-close.html
		/// </summary>
		/// <param name="openIndexSelector">A descriptor thata describes the open index operation</param>
		Task<IIndicesOperationResponse> OpenIndexAsync(Func<OpenIndexDescriptor, OpenIndexDescriptor> openIndexSelector);
		
		/// <summary>
		/// The open and close index APIs allow to close an index, and later on opening it. 
		/// A closed index has almost no overhead on the cluster (except for maintaining its metadata), and is blocked 
		/// for read/write operations. 
		/// A closed index can be opened which will then go through the normal recovery process.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-open-close.html
		/// </summary>
		/// <param name="closeIndexSelector">A descriptor thata describes the close index operation</param>
		IIndicesOperationResponse CloseIndex(Func<CloseIndexDescriptor, CloseIndexDescriptor> closeIndexSelector);
		
		/// <summary>
		/// The open and close index APIs allow to close an index, and later on opening it. 
		/// A closed index has almost no overhead on the cluster (except for maintaining its metadata), and is blocked 
		/// for read/write operations. 
		/// A closed index can be opened which will then go through the normal recovery process.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-open-close.html
		/// </summary>
		/// <param name="closeIndexSelector">A descriptor thata describes the close index operation</param>
		Task<IIndicesOperationResponse> CloseIndexAsync(Func<CloseIndexDescriptor, CloseIndexDescriptor> closeIndexSelector);
		
		/// <summary>
		/// The gateway snapshot API allows to explicitly perform a snapshot through the gateway of one or more indices (backup them). 
		/// By default, each index gateway periodically snapshot changes, though it can be disabled and be
		/// controlled completely through this API.
		/// <para>Note, this API only applies when using shared storage gateway implementation, 
		/// and does not apply when using the (default) local gateway.</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-gateway-snapshot.html
		/// </summary>
		/// <param name="snapShotSelector">A descriptor that describes the parameters for the gateway snapshot operation</param>
		IShardsOperationResponse GatewaySnapshot(Func<GatewaySnapshotDescriptor, GatewaySnapshotDescriptor> snapShotSelector = null);
		
		/// <summary>
		/// The gateway snapshot API allows to explicitly perform a snapshot through the gateway of one or more indices (backup them). 
		/// By default, each index gateway periodically snapshot changes, though it can be disabled and be
		/// controlled completely through this API.
		/// <para>Note, this API only applies when using shared storage gateway implementation, 
		/// and does not apply when using the (default) local gateway.</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-gateway-snapshot.html
		/// </summary>
		/// <param name="snapShotSelector">A descriptor that describes the parameters for the gateway snapshot operation</param>
		Task<IShardsOperationResponse> GatewaySnapshotAsync(Func<GatewaySnapshotDescriptor, GatewaySnapshotDescriptor> snapShotSelector = null);
		
		/// <summary>
		/// The refresh API allows to explicitly refresh one or more index, making all operations performed since the last refresh 
		/// available for search. The (near) real-time capabilities depend on the index engine used. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-refresh.html
		/// </summary>
		/// <param name="refreshSelector">A descriptor that describes the parameters for the refresh operation</param>
		IShardsOperationResponse Refresh(Func<RefreshDescriptor, RefreshDescriptor> refreshSelector = null);
		
		/// <summary>
		/// The refresh API allows to explicitly refresh one or more index, making all operations performed since the last refresh 
		/// available for search. The (near) real-time capabilities depend on the index engine used. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-refresh.html
		/// </summary>
		/// <param name="refreshSelector">A descriptor that describes the parameters for the refresh operation</param>
		Task<IShardsOperationResponse> RefreshAsync(Func<RefreshDescriptor, RefreshDescriptor> refreshSelector = null);
		
		/// <summary>
		/// Provide low level segments information that a Lucene index (shard level) is built with. 
		/// Allows to be used to provide more information on the state of a shard and an index, possibly optimization information,
		/// data "wasted" on deletes, and so on.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-segments.html
		/// </summary>
		/// <param name="segmentsSelector">A descriptor that describes the parameters for the segments operation</param>
		ISegmentsResponse Segments(Func<SegmentsDescriptor, SegmentsDescriptor> segmentsSelector = null);
		
		/// <summary>
		/// Provide low level segments information that a Lucene index (shard level) is built with. 
		/// Allows to be used to provide more information on the state of a shard and an index, possibly optimization information,
		/// data "wasted" on deletes, and so on.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-segments.html
		/// </summary>
		/// <param name="segmentsSelector">A descriptor that describes the parameters for the segments operation</param>
		Task<ISegmentsResponse> SegmentsAsync(Func<SegmentsDescriptor, SegmentsDescriptor> segmentsSelector = null);
		
		/// <summary>
		/// The cluster state API allows to get a comprehensive state information of the whole cluster.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-state.html
		/// </summary>
		/// <param name="clusterStateSelector">A descriptor that describes the parameters for the cluster state operation</param>
		IClusterStateResponse ClusterState(Func<ClusterStateDescriptor, ClusterStateDescriptor> clusterStateSelector = null);

		/// <summary>
		/// The cluster state API allows to get a comprehensive state information of the whole cluster.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-state.html
		/// </summary>
		/// <param name="clusterStateSelector">A descriptor that describes the parameters for the cluster state operation</param>
		Task<IClusterStateResponse> ClusterStateAsync(Func<ClusterStateDescriptor, ClusterStateDescriptor> clusterStateSelector = null);

		/// <summary>
		/// Allows to put a warmup search request on a specific index (or indices), with the body composing of a regular 
		/// search request. Types can be provided as part of the URI if the search request is designed to be run only 
		/// against the specific types.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-warmers.html#warmer-adding
		/// </summary>
		/// <param name="name">The name for the warmer that you want to register</param>
		/// <param name="selector">A descriptor that further describes what the warmer should look like</param>
		IIndicesOperationResponse PutWarmer(string name, Func<PutWarmerDescriptor, PutWarmerDescriptor> selector);
		
		/// <summary>
		/// Allows to put a warmup search request on a specific index (or indices), with the body composing of a regular 
		/// search request. Types can be provided as part of the URI if the search request is designed to be run only 
		/// against the specific types.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-warmers.html#warmer-adding
		/// </summary>
		/// <param name="name">The name for the warmer that you want to register</param>
		/// <param name="selector">A descriptor that further describes what the warmer should look like</param>
		Task<IIndicesOperationResponse> PutWarmerAsync(string name, Func<PutWarmerDescriptor, PutWarmerDescriptor> selector);
		
		/// <summary>
		/// Getting a warmer for specific index (or alias, or several indices) based on its name. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-warmers.html#warmer-retrieving
		/// </summary>
		/// <param name="name">The name of the warmer to get</param>
		/// <param name="selector">An optional selector specifying additional parameters for the get warmer operation</param>
		IWarmerResponse GetWarmer(string name, Func<GetWarmerDescriptor, GetWarmerDescriptor> selector = null);
		
		/// <summary>
		/// Getting a warmer for specific index (or alias, or several indices) based on its name. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-warmers.html#warmer-retrieving
		/// </summary>
		/// <param name="name">The name of the warmer to get</param>
		/// <param name="selector">An optional selector specifying additional parameters for the get warmer operation</param>
		Task<IWarmerResponse> GetWarmerAsync(string name, Func<GetWarmerDescriptor, GetWarmerDescriptor> selector = null);

		/// <summary>
		/// Deletes a warmer
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-warmers.html#removing
		/// </summary>
		/// <param name="name">The name of the warmer to delete</param>
		/// <param name="selector">An optional selector specifying additional parameters for the delete warmer operation</param>
		IIndicesOperationResponse DeleteWarmer(string name, Func<DeleteWarmerDescriptor, DeleteWarmerDescriptor> selector = null);

		/// <summary>
		/// Deletes a warmer
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-warmers.html#removing
		/// </summary>
		/// <param name="name">The name of the warmer to delete</param>
		/// <param name="selector">An optional selector specifying additional parameters for the delete warmer operation</param>
		Task<IIndicesOperationResponse> DeleteWarmerAsync(string name, Func<DeleteWarmerDescriptor, DeleteWarmerDescriptor> selector = null);

		/// <summary>
		/// Gets an index template
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-templates.html#getting
		/// </summary>
		/// <param name="name">The name of the template to get</param>
		/// <param name="getTemplateSelector">An optional selector specifying additional parameters for the get template operation</param>
		ITemplateResponse GetTemplate(string name, Func<GetTemplateDescriptor, GetTemplateDescriptor> getTemplateSelector = null);
		
		/// <summary>
		/// Gets an index template
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-templates.html#getting
		/// </summary>
		/// <param name="name">The name of the template to get</param>
		/// <param name="getTemplateSelector">An optional selector specifying additional parameters for the get template operation</param>
		Task<ITemplateResponse> GetTemplateAsync(string name, Func<GetTemplateDescriptor, GetTemplateDescriptor> getTemplateSelector = null);

		/// <summary>
		/// Index templates allow to define templates that will automatically be applied to new indices created. 
		/// <para>The templates include both settings and mappings, and a simple pattern template that controls if 
		/// the template will be applied to the index created. </para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-templates.html
		/// </summary>
		/// <param name="name">The name of the template to register</param>
		/// <param name="putTemplateSelector">An optional selector specifying additional parameters for the put template operation</param>
		IIndicesOperationResponse PutTemplate(string name, Func<PutTemplateDescriptor, PutTemplateDescriptor> putTemplateSelector);

		/// <summary>
		/// Index templates allow to define templates that will automatically be applied to new indices created. 
		/// <para>The templates include both settings and mappings, and a simple pattern template that controls if 
		/// the template will be applied to the index created. </para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-templates.html
		/// </summary>
		/// <param name="name">The name of the template to register</param>
		/// <param name="putTemplateSelector">An optional selector specifying additional parameters for the put template operation</param>
		Task<IIndicesOperationResponse> PutTemplateAsync(string name, Func<PutTemplateDescriptor, PutTemplateDescriptor> putTemplateSelector);

		/// <summary>
		/// Deletes an index template
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-templates.html#delete
		/// </summary>
		/// <param name="name">The name of the template to delete</param>
		/// <param name="deleteTemplateSelector">An optional selector specifying additional parameters for the delete template operation</param>
		IIndicesOperationResponse DeleteTemplate(string name, Func<DeleteTemplateDescriptor, DeleteTemplateDescriptor> deleteTemplateSelector = null);
		
		/// <summary>
		/// Deletes an index template
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-templates.html#delete
		/// </summary>
		/// <param name="name">The name of the template to delete</param>
		/// <param name="deleteTemplateSelector">An optional selector specifying additional parameters for the delete template operation</param>
		Task<IIndicesOperationResponse> DeleteTemplateAync(string name, Func<DeleteTemplateDescriptor, DeleteTemplateDescriptor> deleteTemplateSelector = null);

		/// <summary>
		/// Unregister a percolator
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <param name="name">The name for the percolator</param>
		/// <param name="selector">An optional descriptor describing the unregister percolator operation further</param>
		IUnregisterPercolateResponse UnregisterPercolator(string name, Func<UnregisterPercolatorDescriptor, UnregisterPercolatorDescriptor> selector = null);

		/// <summary>
		/// Unregister a percolator
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <param name="name">The name for the percolator</param>
		/// <param name="selector">An optional descriptor describing the unregister percolator operation further</param>
		Task<IUnregisterPercolateResponse> UnregisterPercolatorAsync(string name, Func<UnregisterPercolatorDescriptor, UnregisterPercolatorDescriptor> selector = null);

		/// <summary>
		/// Register a percolator
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from, will also be used to strongly type the query</typeparam>
		/// <param name="name">The name for the percolator</param>
		/// <param name="percolatorSelector">An optional descriptor describing the register percolator operation further</param>
		IRegisterPercolateResponse RegisterPercolator<T>(string name, Func<RegisterPercolatorDescriptor<T>, RegisterPercolatorDescriptor<T>> percolatorSelector)
			where T : class;

		/// <summary>
		/// Register a percolator
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from, will also be used to strongly type the query</typeparam>
		/// <param name="name">The name for the percolator</param>
		/// <param name="percolatorSelector">An optional descriptor describing the register percolator operation further</param>
		Task<IRegisterPercolateResponse> RegisterPercolatorAsync<T>(string name, Func<RegisterPercolatorDescriptor<T>, RegisterPercolatorDescriptor<T>> percolatorSelector)
			where T : class;

		/// <summary>
		/// Percolate a document
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from, and of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		IPercolateResponse Percolate<T>(T @object, Func<PercolateDescriptor<T, T>, PercolateDescriptor<T, T>> percolateSelector = null)
			where T : class;

		/// <summary>
		/// Percolate a document
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from, and of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		Task<IPercolateResponse> PercolateAsync<T>(T @object, Func<PercolateDescriptor<T, T>, PercolateDescriptor<T, T>> percolateSelector = null) 
			where T : class;
		
		/// <summary>
		/// Percolate a document
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from</typeparam>
		/// <typeparam name="K">The type of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		IPercolateResponse Percolate<T, K>(K @object, Func<PercolateDescriptor<T, K>, PercolateDescriptor<T, K>> percolateSelector = null) 
			where T : class 
			where K : class;
		
		/// <summary>
		/// Percolate a document
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from</typeparam>
		/// <typeparam name="K">The type of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		Task<IPercolateResponse> PercolateAsync<T, K>(K @object, Func<PercolateDescriptor<T, K>, PercolateDescriptor<T, K>> percolateSelector = null) 
			where T : class 
			where K : class;

		/// <summary>
		/// Percolate a document but only return the number of matches not the matches itself
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from, and of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		IPercolateCountResponse PercolateCount<T>(T @object, Func<PercolateCountDescriptor<T, T>, PercolateCountDescriptor<T, T>> percolateSelector = null)
			where T : class;

		/// <summary>
		/// Percolate a document but only return the number of matches not the matches itself
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from, and of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		Task<IPercolateCountResponse> PercolateCountAsync<T>(T @object, Func<PercolateCountDescriptor<T, T>, PercolateCountDescriptor<T, T>> percolateSelector = null)
			where T : class;

		/// <summary>
		/// Percolate a document but only return the number of matches not the matches itself
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from</typeparam>
		/// <typeparam name="K">The type of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		IPercolateCountResponse PercolateCount<T, K>(K @object, Func<PercolateCountDescriptor<T, K>, PercolateCountDescriptor<T, K>> percolateSelector = null)
			where T : class
			where K : class;

		/// <summary>
		/// Percolate a document but only return the number of matches not the matches itself
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-percolate.html
		/// </summary>
		/// <typeparam name="T">The type to infer the index/type from</typeparam>
		/// <typeparam name="K">The type of the object that is being percolated</typeparam>
		/// <param name="object">The object to percolator</param>
		/// <param name="percolateSelector">An optional descriptor describing the percolate operation further</param>
		Task<IPercolateCountResponse> PercolateCountAsync<T, K>(K @object, Func<PercolateCountDescriptor<T, K>, PercolateCountDescriptor<T, K>> percolateSelector = null)
			where T : class
			where K : class;

		/// <summary>
		/// The put mapping API allows to register specific mapping definition for a specific type.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-put-mapping.html
		/// </summary>
		/// <typeparam name="T">The type we want to map in elasticsearch</typeparam>
		/// <param name="mappingSelector">A descriptor to describe the mapping of our type</param>
		IIndicesResponse Map<T>(Func<PutMappingDescriptor<T>, PutMappingDescriptor<T>> mappingSelector) 
			where T : class;
		
		/// <summary>
		/// The put mapping API allows to register specific mapping definition for a specific type.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-put-mapping.html
		/// </summary>
		/// <typeparam name="T">The type we want to map in elasticsearch</typeparam>
		/// <param name="mappingSelector">A descriptor to describe the mapping of our type</param>
		Task<IIndicesResponse> MapAsync<T>(Func<PutMappingDescriptor<T>, PutMappingDescriptor<T>> mappingSelector)
			where T : class;

		/// <summary>
		/// The get mapping API allows to retrieve mapping definitions for an index or index/type.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-get-mapping.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the get mapping operation</param>
		IGetMappingResponse GetMapping(Func<GetMappingDescriptor, GetMappingDescriptor> selector);
		
		/// <summary>
		/// The get mapping API allows to retrieve mapping definitions for an index or index/type.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-get-mapping.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the get mapping operation</param>
		Task<IGetMappingResponse> GetMappingAsync(Func<GetMappingDescriptor, GetMappingDescriptor> selector);
		
		/// <summary>
		/// Allow to delete a mapping (type) along with its data. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-delete-mapping.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the delete mapping operation</param>
		IIndicesResponse DeleteMapping(Func<DeleteMappingDescriptor, DeleteMappingDescriptor> selector);
		
		/// <summary>
		/// Allow to delete a mapping (type) along with its data. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-delete-mapping.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the delete mapping operation</param>
		Task<IIndicesResponse> DeleteMappingAsync(Func<DeleteMappingDescriptor, DeleteMappingDescriptor> selector);
		
		/// <summary>
		/// The flush API allows to flush one or more indices through an API. The flush process of an index basically 
		/// frees memory from the index by flushing data to the index storage and clearing the internal transaction log. 
		/// By default, Elasticsearch uses memory heuristics in order to automatically trigger 
		/// flush operations as required in order to clear memory.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-flush.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the flush operation</param>
		IShardsOperationResponse Flush(Func<FlushDescriptor, FlushDescriptor> selector);
		
		/// <summary>
		/// The flush API allows to flush one or more indices through an API. The flush process of an index basically 
		/// frees memory from the index by flushing data to the index storage and clearing the internal transaction log. 
		/// By default, Elasticsearch uses memory heuristics in order to automatically trigger 
		/// flush operations as required in order to clear memory.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-flush.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the flush operation</param>
		Task<IShardsOperationResponse> FlushAsync(Func<FlushDescriptor, FlushDescriptor> selector);
		
		/// <summary>
		/// The get settings API allows to retrieve settings of index/indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-get-settings.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the get index settings operation</param>
		IIndexSettingsResponse GetIndexSettings(Func<GetIndexSettingsDescriptor, GetIndexSettingsDescriptor> selector);

		/// <summary>
		/// The get settings API allows to retrieve settings of index/indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-get-settings.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the get index settings operation</param>
		Task<IIndexSettingsResponse> GetIndexSettingsAsync(Func<GetIndexSettingsDescriptor, GetIndexSettingsDescriptor> selector);

		/// <summary>
		/// The delete index API allows to delete an existing index.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-delete-index.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the delete index operation</param>
		IIndicesResponse DeleteIndex(Func<DeleteIndexDescriptor, DeleteIndexDescriptor> selector);
		
		/// <summary>
		/// The delete index API allows to delete an existing index.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-delete-index.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the delete index operation</param>
		Task<IIndicesResponse> DeleteIndexAsync(Func<DeleteIndexDescriptor, DeleteIndexDescriptor> selector);
		
		/// <summary>
		/// The clear cache API allows to clear either all caches or specific cached associated with one ore more indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-clearcache.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the clear cache operation</param>
		IShardsOperationResponse ClearCache(Func<ClearCacheDescriptor, ClearCacheDescriptor> selector = null);
		
		/// <summary>
		/// The clear cache API allows to clear either all caches or specific cached associated with one ore more indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-clearcache.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the parameters for the clear cache operation</param>
		Task<IShardsOperationResponse> ClearCacheAsync(Func<ClearCacheDescriptor, ClearCacheDescriptor> selector = null);

		/// <summary>
		/// The create index API allows to instantiate an index. Elasticsearch provides support for multiple indices, 
		/// including executing operations across several indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-create-index.html
		/// </summary>
		/// <param name="createIndexSelector">A descriptor that describes the parameters for the create index operation</param>
		IIndicesOperationResponse CreateIndex(Func<CreateIndexDescriptor, CreateIndexDescriptor> createIndexSelector);

		/// <summary>
		/// The create index API allows to instantiate an index. Elasticsearch provides support for multiple indices, 
		/// including executing operations across several indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-create-index.html
		/// </summary>
		/// <param name="createIndexSelector">A descriptor that describes the parameters for the create index operation</param>
		Task<IIndicesOperationResponse> CreateIndexAsync(Func<CreateIndexDescriptor, CreateIndexDescriptor> createIndexSelector);
		
		/// <summary>
		/// Does a request to the root of an elasticsearch node
		/// </summary>
		/// <param name="selector">A descriptor to further describe the root operation</param>
		IRootInfoResponse RootNodeInfo(Func<InfoDescriptor, InfoDescriptor> selector = null);
		
		/// <summary>
		/// Does a request to the root of an elasticsearch node
		/// </summary>
		/// <param name="selector">A descriptor to further describe the root operation</param>
		Task<IRootInfoResponse> RootNodeInfoAsync(Func<InfoDescriptor, InfoDescriptor> selector = null);

		/// <summary>
		/// Indices level stats provide statistics on different operations happening on an index. The API provides statistics on 
		/// the index level scope (though most stats can also be retrieved using node level scope).
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-stats.html
		/// </summary>
		/// <param name="selector">Optionaly further describe the indices stats operation</param>
		IGlobalStatsResponse IndicesStats(Func<IndicesStatsDescriptor, IndicesStatsDescriptor> selector = null);
		
		/// <summary>
		/// Indices level stats provide statistics on different operations happening on an index. The API provides statistics on 
		/// the index level scope (though most stats can also be retrieved using node level scope).
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-stats.html
		/// </summary>
		/// <param name="selector">Optionaly further describe the indices stats operation</param>
		Task<IGlobalStatsResponse> IndicesStatsAsync(Func<IndicesStatsDescriptor, IndicesStatsDescriptor> selector = null);
		
		/// <summary>
		/// The cluster nodes info API allows to retrieve one or more (or all) of the cluster nodes information.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-nodes-info.html
		/// </summary>
		/// <param name="selector">An optional descriptor to further describe the nodes info operation</param>
		INodeInfoResponse NodesInfo(Func<NodesInfoDescriptor, NodesInfoDescriptor> selector = null);
		
		/// <summary>
		/// The cluster nodes info API allows to retrieve one or more (or all) of the cluster nodes information.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-nodes-info.html
		/// </summary>
		/// <param name="selector">An optional descriptor to further describe the nodes info operation</param>
		Task<INodeInfoResponse> NodesInfoAsync(Func<NodesInfoDescriptor, NodesInfoDescriptor> selector = null);
		
		/// <summary>
		/// The cluster nodes stats API allows to retrieve one or more (or all) of the cluster nodes statistics.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-nodes-stats.html
		/// </summary>
		/// <param name="selector">An optional descriptor to further describe the nodes stats operation</param>
		INodeStatsResponse NodesStats(Func<NodesStatsDescriptor, NodesStatsDescriptor> selector = null);
		
		/// <summary>
		/// The cluster nodes stats API allows to retrieve one or more (or all) of the cluster nodes statistics.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-nodes-stats.html
		/// </summary>
		/// <param name="selector">An optional descriptor to further describe the nodes stats operation</param>
		Task<INodeStatsResponse> NodesStatsAsync(Func<NodesStatsDescriptor, NodesStatsDescriptor> selector = null);
		
		/// <summary>
		/// Used to check if the index (indices) exists or not. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-exists.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the index exist operation</param>
		IExistsResponse IndexExists(Func<IndexExistsDescriptor, IndexExistsDescriptor> selector);
		
		/// <summary>
		/// Used to check if the index (indices) exists or not. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-exists.html
		/// </summary>
		/// <param name="selector">A descriptor that describes the index exist operation</param>
		Task<IExistsResponse> IndexExistsAsync(Func<IndexExistsDescriptor, IndexExistsDescriptor> selector);

		/// <summary>
		/// The more like this (mlt) API allows to get documents that are "like" a specified document. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-more-like-this.html
		/// </summary>
		/// <typeparam name="T">Type used to infer the default index and typename and used to describe the search</typeparam>
		/// <param name="mltSelector">A descriptor that describes the more like this operation</param>
		ISearchResponse<T> MoreLikeThis<T>(Func<MoreLikeThisDescriptor<T>, MoreLikeThisDescriptor<T>> mltSelector)
			where T : class;

		/// <summary>
		/// The more like this (mlt) API allows to get documents that are "like" a specified document. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-more-like-this.html
		/// </summary>
		/// <typeparam name="T">Type used to infer the default index and typename and used to describe the search</typeparam>
		/// <param name="mltSelector">A descriptor that describes the more like this operation</param>
		Task<ISearchResponse<T>> MoreLikeThisAsync<T>(Func<MoreLikeThisDescriptor<T>, MoreLikeThisDescriptor<T>> mltSelector)
			where T : class;

		/// <summary>
		/// The cluster health API allows to get a very simple status on the health of the cluster.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-health.html
		/// </summary>
		/// <param name="clusterHealthSelector">An optional descriptor to further describe the cluster health operation</param>
		IHealthResponse ClusterHealth(Func<ClusterHealthDescriptor, ClusterHealthDescriptor> clusterHealthSelector = null);
		
		/// <summary>
		/// The cluster health API allows to get a very simple status on the health of the cluster.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-health.html
		/// </summary>
		/// <param name="clusterHealthSelector">An optional descriptor to further describe the cluster health operation</param>
		Task<IHealthResponse> ClusterHealthAsync(Func<ClusterHealthDescriptor, ClusterHealthDescriptor> clusterHealthSelector = null);
		
		/// <summary>
		/// Performs the analysis process on a text and return the tokens breakdown of the text.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-analyze.html
		/// </summary>
		/// <param name="analyzeSelector">A descriptor that describes the analyze operation</param>
		IAnalyzeResponse Analyze(Func<AnalyzeDescriptor, AnalyzeDescriptor> analyzeSelector);
		
		/// <summary>
		/// Performs the analysis process on a text and return the tokens breakdown of the text.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-analyze.html
		/// </summary>
		/// <param name="analyzeSelector">A descriptor that describes the analyze operation</param>
		Task<IAnalyzeResponse> AnalyzeAsync(Func<AnalyzeDescriptor, AnalyzeDescriptor> analyzeSelector);

		/// <summary>
		/// The search API allows to execute a search query and get back search hits that match the query.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-search.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the index and typename as well describe the query strongly typed</typeparam>
		/// <param name="searchSelector">A descriptor that describes the parameters for the search operation</param>
		ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector) 
			where T : class;
		ISearchResponse<T> Search<T>(ISearchRequest request) 
			where T : class;

		/// <summary>
		/// The search API allows to execute a search query and get back search hits that match the query.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-search.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the index and typename as well describe the query strongly typed</typeparam>
		/// <typeparam name="TResult">The type used to describe the strongly typed query</typeparam>
		/// <param name="searchSelector">A descriptor that describes the parameters for the search operation</param>
		ISearchResponse<TResult> Search<T, TResult>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector)
			where T : class
			where TResult : class;
		ISearchResponse<TResult> Search<T, TResult>(ISearchRequest request)
			where T : class
			where TResult : class;

		/// <summary>
		/// The search API allows to execute a search query and get back search hits that match the query.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-search.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the index and typename as well describe the query strongly typed</typeparam>
		/// <param name="searchSelector">A descriptor that describes the parameters for the search operation</param>
		Task<ISearchResponse<T>> SearchAsync<T>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector) 
			where T : class;

		/// <summary>
		/// The search API allows to execute a search query and get back search hits that match the query.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-search.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the index and typename</typeparam>
		/// <typeparam name="TResult">The type used to describe the strongly typed query</typeparam>
		/// <param name="searchSelector">A descriptor that describes the parameters for the search operation</param>
		Task<ISearchResponse<TResult>> SearchAsync<T, TResult>(Func<SearchDescriptor<T>, SearchDescriptor<T>> searchSelector)
			where T : class
			where TResult : class;

		/// <summary>
		/// The multi search API allows to execute several search requests within the same API.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-multi-search.html
		/// </summary>
		/// <param name="multiSearchSelector">A descriptor that describes the search operations on the multi search api</param>
		IMultiSearchResponse MultiSearch(Func<MultiSearchDescriptor, MultiSearchDescriptor> multiSearchSelector);
		
		/// <summary>
		/// The multi search API allows to execute several search requests within the same API.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-multi-search.html
		/// </summary>
		/// <param name="multiSearchSelector">A descriptor that describes the search operations on the multi search api</param>
		Task<IMultiSearchResponse> MultiSearchAsync(Func<MultiSearchDescriptor, MultiSearchDescriptor> multiSearchSelector);
		
		/// <summary>
		/// The count API allows to easily execute a query and get the number of matches for that query. 
		/// It can be executed across one or more indices and across one or more types. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-count.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename as well as describe the strongly
		///  typed parts of the query</typeparam>
		/// <param name="countSelector">An optional descriptor to further describe the count operation</param>
		ICountResponse Count<T>(Func<CountDescriptor<T>, CountDescriptor<T>> countSelector = null)
			where T : class;
		
		/// <summary>
		/// The count API allows to easily execute a query and get the number of matches for that query. 
		/// It can be executed across one or more indices and across one or more types. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-count.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename as well as describe the strongly
		///  typed parts of the query</typeparam>
		/// <param name="countSelector">An optional descriptor to further describe the count operation</param>
		Task<ICountResponse> CountAsync<T>(Func<CountDescriptor<T>, CountDescriptor<T>> countSelector = null) 
			where T : class;

		/// <summary>
		/// The delete by query API allows to delete documents from one or more indices and one or more types based on a query.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-delete-by-query.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename as well as describe the strongly
		///  typed parts of the query</typeparam>
		/// <param name="deleteByQuerySelector">An optional descriptor to further describe the delete by query operation</param>
		IDeleteResponse DeleteByQuery<T>(Func<DeleteByQueryDescriptor<T>, DeleteByQueryDescriptor<T>> deleteByQuerySelector)
			where T : class;

		/// <summary>
		/// The delete by query API allows to delete documents from one or more indices and one or more types based on a query.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-delete-by-query.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename as well as describe the strongly
		///  typed parts of the query</typeparam>
		/// <param name="deleteByQuerySelector">An optional descriptor to further describe the delete by query operation</param>
		Task<IDeleteResponse> DeleteByQueryAsync<T>(
			Func<DeleteByQueryDescriptor<T>, DeleteByQueryDescriptor<T>> deleteByQuerySelector) 
			where T : class;

		/// <summary>
		/// The bulk API makes it possible to perform many index/delete operations in a single API call. 
		/// This can greatly increase the indexing speed.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <param name="bulkSelector">A descriptor the describe the index/create/delete operation for this bulk operation</param>
		IBulkResponse Bulk(Func<BulkDescriptor, BulkDescriptor> bulkSelector);
		
		/// <summary>
		/// The bulk API makes it possible to perform many index/delete operations in a single API call. 
		/// This can greatly increase the indexing speed.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <param name="bulkSelector">A descriptor the describe the index/create/delete operation for this bulk operation</param>
		Task<IBulkResponse> BulkAsync(Func<BulkDescriptor, BulkDescriptor> bulkSelector);
		
		/// <summary>
		/// Shortcut into the <see cref="Bulk"/> call that deletes the specified objects
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="objects">List of objects to delete</param>
		/// <param name="index">Override the inferred indexname for T</param>
		/// <param name="type">Override the inferred typename for T</param>
		IBulkResponse DeleteMany<T>(IEnumerable<T> @objects, string index = null, string type = null) 
			where T : class;

		/// <summary>
		/// Shortcut into the <see cref="Bulk"/> call that deletes the specified objects
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="objects">List of objects to delete</param>
		/// <param name="index">Override the inferred indexname for T</param>
		/// <param name="type">Override the inferred typename for T</param>
		Task<IBulkResponse> DeleteManyAsync<T>(IEnumerable<T> objects, string index = null, string type = null)
			where T : class;
		
		/// <summary>
		/// The index API adds or updates a typed JSON document in a specific index, making it searchable. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-index_.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="object">The object to be indexed, Id will be inferred (Id property or IdProperty attribute on type)</param>
		/// <param name="indexSelector">Optionally furter describe the index operation i.e override type/index/id</param>
		IIndexResponse Index<T>(T @object, Func<IndexDescriptor<T>, IndexDescriptor<T>> indexSelector = null)
			where T : class;

		/// <summary>
		/// The index API adds or updates a typed JSON document in a specific index, making it searchable. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-index_.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="object">The object to be indexed, Id will be inferred (Id property or IdProperty attribute on type)</param>
		/// <param name="indexSelector">Optionally furter describe the index operation i.e override type/index/id</param>
		Task<IIndexResponse> IndexAsync<T>(T @object, Func<IndexDescriptor<T>, IndexDescriptor<T>> indexSelector = null)
			where T : class;

		/// <summary>
		///The delete API allows to delete a typed JSON document from a specific index based on its id. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-delete.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="deleteSelector">Describe the delete operation, i.e type/index/id</param>
		IDeleteResponse Delete<T>(Func<DeleteDescriptor<T>, DeleteDescriptor<T>> deleteSelector) 
			where T : class;
		
		/// <summary>
		///The delete API allows to delete a typed JSON document from a specific index based on its id. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-delete.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="deleteSelector">Describe the delete operation, i.e type/index/id</param>
		Task<IDeleteResponse> DeleteAsync<T>(Func<DeleteDescriptor<T>, DeleteDescriptor<T>> deleteSelector) 
			where T : class;
		
		/// <summary>
		/// Multi GET API allows to get multiple documents based on an index, type (optional) and id (and possibly routing). 
		/// The response includes a docs array with all the fetched documents, each element similar in structure to a document 
		/// provided by the get API.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-multi-get.html
		/// </summary>
		/// <param name="multiGetSelector">A descriptor describing which documents should be fetched</param>
		IMultiGetResponse MultiGet(Func<MultiGetDescriptor, MultiGetDescriptor> multiGetSelector);
		
		/// <summary>
		/// Multi GET API allows to get multiple documents based on an index, type (optional) and id (and possibly routing). 
		/// The response includes a docs array with all the fetched documents, each element similar in structure to a document 
		/// provided by the get API.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-multi-get.html
		/// </summary>
		/// <param name="multiGetSelector">A descriptor describing which documents should be fetched</param>
		Task<IMultiGetResponse> MultiGetAsync(Func<MultiGetDescriptor, MultiGetDescriptor> multiGetSelector);
		
		/// <summary>
		/// Use the /{index}/{type}/{id}/_source endpoint to get just the _source field of the document, 
		/// without any additional content around it. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-get.html#_source
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="getSelector">A descriptor that describes which document's source to fetch</param>
		T Source<T>(Func<SourceDescriptor<T>, SourceDescriptor<T>> getSelector) 
			where T : class;
		
		/// <summary>
		/// Use the /{index}/{type}/{id}/_source endpoint to get just the _source field of the document, 
		/// without any additional content around it. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-get.html#_source
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="getSelector">A descriptor that describes which document's source to fetch</param>
		Task<T> SourceAsync<T>(Func<SourceDescriptor<T>, SourceDescriptor<T>> getSelector) 
			where T : class;
		
		/// <summary>
		/// The get API allows to get a typed JSON document from the index based on its id.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-get.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="getSelector">A descriptor that describes which document to fetch and how</param>
		IGetResponse<T> Get<T>(Func<GetDescriptor<T>, GetDescriptor<T>> getSelector) 
			where T : class;
		
		/// <summary>
		/// The get API allows to get a typed JSON document from the index based on its id.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-get.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="getSelector">A descriptor that describes which document to fetch and how</param>
		Task<IGetResponse<T>> GetAsync<T>(Func<GetDescriptor<T>, GetDescriptor<T>> getSelector) 
			where T : class;
		
		/// <summary>
		/// APIs in elasticsearch accept an index name when working against a specific index, and several indices when applicable. 
		/// <para>The index aliases API allow to alias an index with a name, with all APIs automatically converting the alias name to the 
		/// actual index name.</para><para> An alias can also be mapped to more than one index, and when specifying it, the alias 
		/// will automatically expand to the aliases indices.i</para><para> An alias can also be associated with a filter that will 
		/// automatically be applied when searching, and routing values.</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-aliases.html
		/// </summary>
		/// <param name="aliasSelector">A desriptor that describes the parameters for the alias operation</param>
		IIndicesOperationResponse Alias(Func<AliasDescriptor, AliasDescriptor> aliasSelector);
		
		/// <summary>
		/// APIs in elasticsearch accept an index name when working against a specific index, and several indices when applicable. 
		/// <para>The index aliases API allow to alias an index with a name, with all APIs automatically converting the alias name to the 
		/// actual index name.</para><para> An alias can also be mapped to more than one index, and when specifying it, the alias 
		/// will automatically expand to the aliases indices.i</para><para> An alias can also be associated with a filter that will 
		/// automatically be applied when searching, and routing values.</para>
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-aliases.html
		/// </summary>
		/// <param name="aliasSelector">A desriptor that describes the parameters for the alias operation</param>
		Task<IIndicesOperationResponse> AliasAsync(Func<AliasDescriptor, AliasDescriptor> aliasSelector);
		
		/// <summary>
		/// The get index alias api allows to filter by alias name and index name. This api redirects to the master and fetches 
		/// the requested index aliases, if available. This api only serialises the found index aliases.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-aliases.html#alias-retrieving
		/// </summary>
		/// <param name="getAliasesDescriptor">A descriptor that describes which aliases/indexes we are interested int</param>
		IGetAliasesResponse GetAliases(Func<GetAliasesDescriptor, GetAliasesDescriptor> getAliasesDescriptor);
		
		/// <summary>
		/// The get index alias api allows to filter by alias name and index name. This api redirects to the master and fetches 
		/// the requested index aliases, if available. This api only serialises the found index aliases.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-aliases.html#alias-retrieving
		/// </summary>
		/// <param name="getAliasesDescriptor">A descriptor that describes which aliases/indexes we are interested int</param>
		Task<IGetAliasesResponse> GetAliasesAsync(Func<GetAliasesDescriptor, GetAliasesDescriptor> getAliasesDescriptor);
		
		/// <summary>
		/// Shortcut into the <see cref="Bulk"/> call that indexes the specified objects
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="objects">List of objects to index, Id will be inferred (Id property or IdProperty attribute on type)</param>
		/// <param name="index">Override the inferred indexname for T</param>
		/// <param name="type">Override the inferred typename for T</param>
		IBulkResponse IndexMany<T>(IEnumerable<T> @objects, string index = null, string type = null) 
			where T : class;
		
		/// <summary>
		/// Shortcut into the <see cref="Bulk"/> call that indexes the specified objects
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-bulk.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="objects">List of objects to index, Id will be inferred (Id property or IdProperty attribute on type)</param>
		/// <param name="index">Override the inferred indexname for T</param>
		/// <param name="type">Override the inferred typename for T</param>
		Task<IBulkResponse> IndexManyAsync<T>(IEnumerable<T> objects, string index = null, string type = null)
			where T : class;
		
		/// <summary>
		/// The optimize API allows to optimize one or more indices through an API. The optimize process basically optimizes 
		/// the index for faster search operations (and relates to the number of segments a Lucene index holds within each shard).
		///  The optimize operation allows to reduce the number of segments by merging them.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-optimize.html
		/// </summary>
		/// <param name="optimizeSelector">An optional descriptor that further describes the optimize operation, i.e limit it to one index</param>
		IShardsOperationResponse Optimize(Func<OptimizeDescriptor, OptimizeDescriptor> optimizeSelector = null);
		
		/// <summary>
		/// The optimize API allows to optimize one or more indices through an API. The optimize process basically optimizes 
		/// the index for faster search operations (and relates to the number of segments a Lucene index holds within each shard).
		///  The optimize operation allows to reduce the number of segments by merging them.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-optimize.html
		/// </summary>
		/// <param name="optimizeSelector">An optional descriptor that further describes the optimize operation, i.e limit it to one index</param>
		Task<IShardsOperationResponse> OptimizeAsync(Func<OptimizeDescriptor, OptimizeDescriptor> optimizeSelector = null);
		
		/// <summary>
		/// The indices status API allows to get a comprehensive status information of one or more indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-status.html
		/// </summary>
		/// <param name="selector">An optional descriptor that further describes the status operation, i.e limiting it to certain indices</param>
		IStatusResponse Status(Func<IndicesStatusDescriptor, IndicesStatusDescriptor> selector = null);
		
		/// <summary>
		/// The indices status API allows to get a comprehensive status information of one or more indices.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-status.html
		/// </summary>
		/// <param name="selector">An optional descriptor that further describes the status operation, i.e limiting it to certain indices</param>
		Task<IStatusResponse> StatusAsync(Func<IndicesStatusDescriptor, IndicesStatusDescriptor> selector = null);

        /// <summary>
        /// Returns information and statistics on terms in the fields of a particular document as stored in the index.
        /// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-termvectors.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="termVectorSelector"></param>
        /// <returns></returns>
		ITermVectorResponse TermVector<T>(Func<TermvectorDescriptor<T>, TermvectorDescriptor<T>> termVectorSelector) 
            where T : class;

        /// <summary>
        /// Returns information and statistics on terms in the fields of a particular document as stored in the index.
        /// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-termvectors.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="termVectorSelector"></param>
        /// <returns></returns>
        Task<ITermVectorResponse> TermVectorAsync<T>(Func<TermvectorDescriptor<T>, TermvectorDescriptor<T>> termVectorSelector) 
            where T : class;

        /// <summary>
        /// Multi termvectors API allows to get multiple termvectors based on an index, type and id.
        /// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-multi-termvectors.html
        /// </summary>
        /// <typeparam name="T">The type used to infer the default index and typename</typeparam>
        /// <param name="multiTermVectorsSelector">The descriptor describing the multi termvectors operation</param>
        IMultiTermVectorResponse MultiTermVectors<T>(Func<MultiTermVectorsDescriptor<T>, MultiTermVectorsDescriptor<T>> multiTermVectorsSelector)
            where T : class;

        /// <summary>
        /// Multi termvectors API allows to get multiple termvectors based on an index, type and id.
        /// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-multi-termvectors.html
        /// </summary>
        /// <typeparam name="T">The type used to infer the default index and typename</typeparam>
        /// <param name="multiTermVectorsSelector">The descriptor describing the multi termvectors operation</param>
        Task<IMultiTermVectorResponse> MultiTermVectorsAsync<T>(Func<MultiTermVectorsDescriptor<T>, MultiTermVectorsDescriptor<T>> multiTermVectorsSelector)
            where T : class;
		
		/// <summary>
		/// The suggest feature suggests similar looking terms based on a provided text by using a suggester. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-suggesters.html
		/// </summary>
		/// <typeparam name="T">The type used to strongly type parts of the suggest operation</typeparam>
		/// <param name="selector">The suggesters to use this operation (can be multiple)</param>
		ISuggestResponse Suggest<T>(Func<SuggestDescriptor<T>, SuggestDescriptor<T>> selector)
			where T : class;

		/// <summary>
		/// The suggest feature suggests similar looking terms based on a provided text by using a suggester. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-suggesters.html
		/// </summary>
		/// <typeparam name="T">The type used to strongly type parts of the suggest operation</typeparam>
		/// <param name="selector">The suggesters to use this operation (can be multiple)</param>
		Task<ISuggestResponse> SuggestAsync<T>(Func<SuggestDescriptor<T>, SuggestDescriptor<T>> selector)
			where T : class;

		/// <summary>
		/// Deletes a registered scroll request on the cluster 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-request-scroll.html
		/// </summary>
		/// <param name="selector">Specify the scroll id as well as request specific configuration</param>
		IEmptyResponse ClearScroll(Func<ClearScrollDescriptor, ClearScrollDescriptor> clearScrollSelector);

		/// <summary>
		/// Deletes a registered scroll request on the cluster 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-request-scroll.html
		/// </summary>
		/// <param name="selector">Specify the scroll id as well as request specific configuration</param>
		Task<IEmptyResponse> ClearScrollAsync(Func<ClearScrollDescriptor, ClearScrollDescriptor> clearScrollSelector);

		/// <summary>
		/// Check if a document exists without returning its contents
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-get.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="existsSelector">Describe what document we are looking for</param>
		IExistsResponse DocumentExists<T>(Func<DocumentExistsDescriptor<T>, DocumentExistsDescriptor<T>> existsSelector)
			where T : class;

		/// <summary>
		/// Check if a document exists without returning its contents
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-get.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="existsSelector">Describe what document we are looking for</param>
		Task<IExistsResponse> DocumentExistsAsync<T>(Func<DocumentExistsDescriptor<T>, DocumentExistsDescriptor<T>> existsSelector)
			where T : class;

		/// <summary>
		/// Before any snapshot or restore operation can be performed a snapshot repository should be registered in Elasticsearch. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_repositories
		/// </summary>
		/// <param name="repository">The name for the repository</param>
		/// <param name="selector">describe what the repository looks like</param>
		IAcknowledgedResponse CreateRepository(string repository, Func<CreateRepositoryDescriptor, CreateRepositoryDescriptor> selector);

		/// <summary>
		/// Before any snapshot or restore operation can be performed a snapshot repository should be registered in Elasticsearch. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_repositories
		/// </summary>
		/// <param name="repository">The name for the repository</param>
		/// <param name="selector">describe what the repository looks like</param>
		Task<IAcknowledgedResponse> CreateRepositoryAsync(string repository, Func<CreateRepositoryDescriptor, CreateRepositoryDescriptor> selector);

		/// <summary>
		/// Delete a repository, if you have ongoing restore operations be sure to delete the indices being restored into first.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_repositories
		/// </summary>
		/// <param name="repository">The name of the repository</param>
		/// <param name="selector">Optionaly provide the delete operation with more details</param>>
		IAcknowledgedResponse DeleteRepository(string repository, Func<DeleteRepositoryDescriptor, DeleteRepositoryDescriptor> selector = null);

		/// <summary>
		/// Delete a repository, if you have ongoing restore operations be sure to delete the indices being restored into first.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_repositories
		/// </summary>
		/// <param name="repository">The name of the repository</param>
		/// <param name="selector">Optionaly provide the delete operation with more details</param>>
		Task<IAcknowledgedResponse> DeleteRepositoryAsync(string repository, Func<DeleteRepositoryDescriptor, DeleteRepositoryDescriptor> selector = null);
		
		/// <summary>
		/// A repository can contain multiple snapshots of the same cluster. Snapshot are identified by unique names within the cluster.
		/// /// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_snapshot
		/// </summary>
		/// <param name="repository">The name of the repository we want to create a snapshot in</param>
		/// <param name="snapshotName">The name of the snapshot</param>
		/// <param name="selector">Optionally provide more details about the snapshot operation</param>
		ISnapshotResponse Snapshot(string repository, string snapshotName, Func<SnapshotDescriptor, SnapshotDescriptor> selector = null);

		/// <summary>
		/// A repository can contain multiple snapshots of the same cluster. Snapshot are identified by unique names within the cluster.
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_snapshot
		/// </summary>
		/// <param name="repository">The name of the repository we want to create a snapshot in</param>
		/// <param name="snapshotName">The name of the snapshot</param>
		/// <param name="selector">Optionally provide more details about the snapshot operation</param>
		Task<ISnapshotResponse> SnapshotAsync(string repository, string snapshotName, Func<SnapshotDescriptor, SnapshotDescriptor> selector = null);
		
		/// <summary>
		/// Delete a snapshot
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_snapshot
		/// </summary>
		/// <param name="repository">The repository name under which the snapshot we want to delete lives</param>
		/// <param name="snapshotName">The name of the snapshot that we want to delete</param>
		/// <param name="selector">Optionally further describe the delete snapshot operation</param>
		IAcknowledgedResponse DeleteSnapshot(string repository, string snapshotName, Func<DeleteSnapshotDescriptor, DeleteSnapshotDescriptor> selector = null);

		/// <summary>
		/// Delete a snapshot
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_snapshot
		/// </summary>
		/// <param name="repository">The repository name under which the snapshot we want to delete lives</param>
		/// <param name="snapshotName">The name of the snapshot that we want to delete</param>
		/// <param name="selector">Optionally further describe the delete snapshot operation</param>
		Task<IAcknowledgedResponse> DeleteSnapshotAsync(string repository, string snapshotName, Func<DeleteSnapshotDescriptor, DeleteSnapshotDescriptor> selector = null);
		
		/// <summary>
		/// Gets information about one or more snapshots
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_snapshot
		/// </summary>
		/// <param name="repository">The repository name under which the snapshots live</param>
		/// <param name="snapshotName">The names of the snapshots we want information from (can be _all or wildcards)</param>
		/// <param name="selector">Optionally further describe the get snapshot operation</param>
		IGetSnapshotResponse GetSnapshot(string repository, string snapshotName, Func<GetSnapshotDescriptor, GetSnapshotDescriptor> selector = null);

		/// <summary>
		/// Gets information about one or more snapshots
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_snapshot
		/// </summary>
		/// <param name="repository">The repository name under which the snapshots live</param>
		/// <param name="snapshotName">The names of the snapshots we want information from (can be _all or wildcards)</param>
		/// <param name="selector">Optionally further describe the get snapshot operation</param>
		Task<IGetSnapshotResponse> GetSnapshotAsync(string repository, string snapshotName, Func<GetSnapshotDescriptor, GetSnapshotDescriptor> selector = null);
		
		/// <summary>
		/// Restore a snapshot
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_restore
		/// </summary>
		/// <param name="repository">The repository name that holds our snapshot</param>
		/// <param name="snapshotName">The name of the snapshot that we want to restore</param>
		/// <param name="selector">Optionally further describe the restore operation</param>
		IRestoreResponse Restore(string repository, string snapshotName, Func<RestoreDescriptor, RestoreDescriptor> selector = null);

		/// <summary>
		/// Restore a snapshot
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/modules-snapshots.html#_restore
		/// </summary>
		/// <param name="repository">The repository name that holds our snapshot</param>
		/// <param name="snapshotName">The name of the snapshot that we want to restore</param>
		/// <param name="selector">Optionally further describe the restore operation</param>
		Task<IRestoreResponse> RestoreAsync(string repository, string snapshotName, Func<RestoreDescriptor, RestoreDescriptor> selector = null);
	}
}
