using OverpassNet.Entities;

namespace OverpassNet;

//Similar examples https://github.com/OsmSharp/io-api (OSM read/write api, not overpass specific read only)
public class OverpassClient(HttpClient client)
{ 
    public async Task<ElementCollection> Get(string query)
    {
        try
        {
            var result = await client.PostAsync("https://www.overpass-api.de/api/interpreter", new StringContent(query));

            if (result.IsSuccessStatusCode)
            {
                return OsmSerializer.Deserialize(await result.Content.ReadAsStringAsync()) ?? new ElementCollection();
            }
            throw new IOException($"Error {result.StatusCode}: {await result.Content.ReadAsStringAsync()}");
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR: Error calling overpass client, {e.Message}");
            return null;
        }
    }
}
