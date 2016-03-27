﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AwfulRedux.Core.Interfaces;
using AwfulRedux.Core.Models.Search;
using AwfulRedux.Core.Tools;
using HtmlAgilityPack;

namespace AwfulRedux.Core.Managers
{
    public class SearchManager
    {
        private readonly IWebManager _webManager;

        public SearchManager(IWebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<SearchEntityObject> GetSearchQueryResults(List<int> forumIds, string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new Exception("Need search query!");
            }

            if (!forumIds.Any())
            {
                throw new Exception("Need to select a forum!");
            }

            var form = new MultipartFormDataContent
            {
                {new StringContent("query"), "action"},
                {new StringContent(query), "q"},
            };
            foreach (var id in forumIds)
            {
                form.Add(new StringContent(id.ToString()), "forums[]");
            }
            try
            {
                var response = await _webManager.PostFormData(EndPoints.SearchUrl, form);
                var doc = new HtmlDocument();
                doc.LoadHtml(response.ResultHtml);
                var result = ParseSearchHtml(doc);
                return new SearchEntityObject()
                {
                    SearchEntities = result,
                    LinkUrl = response.RequestUri
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get search results", ex);
            }
        }

        public async Task<SearchEntityObject> GetSearchQueryResultsViaRedirect(string redirect)
        {
            try
            {
                var response = await _webManager.GetData(redirect);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(response.ResultHtml);
                var result = ParseSearchHtml(doc);
                return new SearchEntityObject()
                {
                    SearchEntities = result
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get search results", ex);
            }
        }

        private List<SearchEntity> ParseSearchHtml(HtmlDocument doc)
        {
            HtmlNode forumNode =
                doc.DocumentNode.Descendants("ul")
                    .FirstOrDefault(node => node.GetAttributeValue("id", string.Empty).Equals("search_results"));
            if (forumNode == null)
            {
                // No search results!
                return new List<SearchEntity>();
            }
            IEnumerable<HtmlNode> searchNodes = forumNode.Descendants("li").Where(node => node.GetAttributeValue("class", string.Empty).Equals("search_result"));
            return searchNodes.Select(ParseSearchNode).ToList();
        }

        private SearchEntity ParseSearchNode(HtmlNode searchNode)
        {
            var searchEntity = new SearchEntity();
            var resultNode =
                searchNode.Descendants("div")
                    .First(node => node.GetAttributeValue("class", string.Empty).Equals("result_number"));
            searchEntity.ResultNumber = resultNode.InnerText;

            resultNode = searchNode.Descendants("a")
                    .First(node => node.GetAttributeValue("class", string.Empty).Equals("threadtitle"));
            searchEntity.ThreadLink = resultNode.GetAttributeValue("href", string.Empty);
            searchEntity.ThreadTitle = WebUtility.HtmlDecode(resultNode.InnerText);
            resultNode = searchNode.Descendants("a")
                    .First(node => node.GetAttributeValue("class", string.Empty).Equals("username"));

            searchEntity.Username = WebUtility.HtmlDecode(resultNode.InnerText);

            resultNode = searchNode.Descendants("a")
                    .First(node => node.GetAttributeValue("class", string.Empty).Equals("forumtitle"));

            searchEntity.ForumName = WebUtility.HtmlDecode(resultNode.InnerText);

            resultNode = searchNode.Descendants("div")
                    .First(node => node.GetAttributeValue("class", string.Empty).Equals("blurb"));

            searchEntity.Blurb = Regex.Replace(resultNode.InnerText, @"\t|\n|\r", "");
            return searchEntity;
        }
    }
}
