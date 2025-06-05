using OverpassNet.Entities;
using System.Net.Http.Headers;

namespace OverpassNet;

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
    //out body; >; out body qt; //returns elements and their children
    private const string BaseQuery = "[out:json];{query};out body;>;out skel qt;";

    public async Task<ElementCollection> GetArea(int areaId = 849358753)
    {
        //area({areaId})->.searchArea; //get area id
        //way[\"highway\"](area.searchArea); //get all highways in area
        //out body;>;out body; //get all children of highways
        var query = $"[out:json];area({areaId})->.searchArea;(way[\"highway\"](area.searchArea);;out body;>;out body;";

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
        var query = $"[out:json];way[\"highway\"]({minLat}, {minLon}, {maxLat}, {maxLon});out body;>;out body;";

        return await Get(query);
    }

    public async Task<ElementCollection> GetAllCanalsInRelationArea(int id)
    {
        var query = $"[out:json][timeout:25];" +
            $"// Define the UK boundary" +
            $"relation({id});" +
            $"map_to_area -> .uk;" +
            $"// Find all ways tagged as canals within the UK boundary" +
            $"way[\"waterway\"=\"canal\"](area.uk)->.canals;" +
            $"// Output the canals and their nodes" +
            $"(" +
                $".canals;" +
                $">;" +
            $");" +
            $"out body;";

        return await Get(query);
    }

    public async Task<ElementCollection> GetAllPathsConnectedToBramptonValleyWay()
    {
        var query = "[out:json];way[name=\"Brampton Valley Way\"]; out body;>;out body;<<;out body;";
        return await Get(query);
    }

    public async Task<ElementCollection> Get(string query)
    {
        var result = await _client.PostAsync("https://www.overpass-api.de/api/interpreter", new StringContent(query));

        if (result.IsSuccessStatusCode)
        {
            return OsmSerializer.Deserialize(await result.Content.ReadAsStringAsync()) ?? new ElementCollection();
        }
        throw new IOException($"Error {result.StatusCode}: {await result.Content.ReadAsStringAsync()}");
    }
}
