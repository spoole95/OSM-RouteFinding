using OverpassNet.Entities;
using System.Net.Http.Headers;

namespace OverpassNet.Query;

//Similar examples https://github.com/OsmSharp/io-api (OSM read/write api, not overpass specific read only)
public class OverpassClient
{
    private HttpClient _client;

    public OverpassClient(HttpClient client)
    {
        _client = client;
    }

    //TODO
    //Extend to other objects (named area, bounds, etc)
    //Add local cache / use local cache + extend (sql, json file, etc) with refresh on age


    private const string BaseQuery = "[out:json];(way[\"highway\"]({0}););out body;>;out skel qt;";

    public async Task<ElementCollection> GetArea(int areaId = 849358753)
    {
        var query = $"[out:json];area({areaId})->.searchArea;(way[\"highway\"](area.searchArea););out body;>;out skel qt;";

        return await Get(query);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minLat">Sothern-most latitude</param>
    /// <param name="minLon">Western-most longitude</param>
    /// <param name="maxLat">Northern-most latitude</param>
    /// <param name="maxLon">Eastern-most longitude</param>
    /// <returns></returns>
    public async Task<ElementCollection> GetBox(double minLat, double minLon, double maxLat, double maxLon)
    {
        var query = $"[out:json];(way[\"highway\"]({minLat}, {minLon}, {maxLat}, {maxLon}););out body;>;out skel qt;";

        return await Get(query);
    }


    private async Task<ElementCollection> Get(string query)
    {
        var result = await _client.PostAsync("https://www.overpass-api.de/api/interpreter", new StringContent(query));

        if (result.IsSuccessStatusCode)
        {
            return OsmSerializer.Deserialize(await result.Content.ReadAsStringAsync()) ?? new ElementCollection();
        }
        throw new IOException($"Error {result.StatusCode}: {await result.Content.ReadAsStringAsync()}");
    }
}
