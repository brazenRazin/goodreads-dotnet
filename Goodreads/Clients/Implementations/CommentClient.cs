﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Goodreads.Helpers;
using Goodreads.Http;
using Goodreads.Models.Request;
using Goodreads.Models.Response;
using RestSharp;

namespace Goodreads.Clients
{
    internal sealed class CommentClient : EndpointClient, ICommentClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentClient"/> class.
        /// </summary>
        /// <param name="connection">A RestClient connection to the Goodreads API.</param>
        public CommentClient(IConnection connection)
            : base(connection)
        {
        }

        /// <summary>
        /// Create a new comment.
        /// </summary>
        /// <param name="resourceId">Id of resource given as resourceType param.</param>
        /// <param name="type">A resource type.</param>
        /// <param name="comment">A comment value.</param>
        /// <returns>True if creation is successed. otherwise false.</returns>
        async Task<bool> ICommentClient.Create(int resourceId, ResourceType type, string comment)
        {
            var endpoint = @"comment";

            var parameters = new List<Parameter>
            {
                new Parameter { Name = "type", Value = EnumHelpers.QueryParameterValue(type), Type = ParameterType.QueryString },
                new Parameter { Name = "id", Value = resourceId, Type = ParameterType.QueryString },
                new Parameter { Name = "comment[body]", Value = comment, Type = ParameterType.QueryString }
            };

            var result = await Connection.ExecuteRaw(endpoint, parameters, Method.POST);

            return result.StatusCode == HttpStatusCode.Created;
        }

        /// <summary>
        /// Get lists comments.
        /// </summary>
        /// <param name="resourceId">Id of resource given as resourceType param.</param>
        /// <param name="type">A resource type.</param>
        /// <param name="page">The desired page from the paginated list of friend requests.</param>
        /// <returns>List of comments.</returns>
        async Task<PaginatedList<Comment>> ICommentClient.GetAll(int resourceId, ResourceType type, int page)
        {
            var endpoint = @"comment";

            var parameters = new List<Parameter>
            {
                new Parameter { Name = "type", Value = EnumHelpers.QueryParameterValue(type), Type = ParameterType.QueryString },
                new Parameter { Name = "id", Value = resourceId, Type = ParameterType.QueryString },
                new Parameter { Name = "page", Value = page, Type = ParameterType.QueryString }
            };

            var result = await Connection.ExecuteRaw(endpoint, parameters);

            return await Connection.ExecuteRequest<PaginatedList<Comment>>(endpoint, parameters, null, "comments");
        }
    }
}