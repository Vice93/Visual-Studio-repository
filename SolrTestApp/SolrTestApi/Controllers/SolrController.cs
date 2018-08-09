using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolrTestApi.Models;

namespace SolrTestApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Solr")]
    public class SolrController : Controller
    {
        private const string _baseUri = "http://localhost:8983/solr/techproducts/";

        // GET: api/Solr
        [HttpGet]
        public async Task<JObject> Get()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _baseUri + "select?q=*:*&wt=json");
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                var jObject = JObject.Parse(content);
                return jObject;
            }
        }

        // GET: api/Solr/5
        [HttpGet("{searchInput}", Name = "Get")]
        public async Task<JToken> Get(string searchInput)
        {
            using (var client = new HttpClient())
            {
                searchInput = Regex.Replace(searchInput, @"(&|#|\?)", "");
                var request = new HttpRequestMessage(HttpMethod.Get, _baseUri + "select?q=" + searchInput + "~&wt=json");
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                var jObject = JObject.Parse(content);
                var jToken = jObject["response"]["docs"];
                return jToken;
            }
        }
        
        // POST: api/Solr
        [HttpPost]
        public async Task<HttpStatusCode> Post([FromBody]Product prod)
        {
            using (var client = new HttpClient())
            {
                var json = new StringContent(JsonConvert.SerializeObject(prod));
                json.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(_baseUri + "update/json?wt=json&commit=true",json);
                return response.StatusCode;
            }
        }
        
        // PUT: api/Solr/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
