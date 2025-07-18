using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using OverpassNet.Entities;
using System.Threading;

namespace OverpassNet;

//Similar examples https://github.com/OsmSharp/io-api (OSM read/write api, not overpass specific read only)
public class OverpassClient
{
    private readonly HttpClient _client;
    private ILogger<OverpassClient> _logger;

    public OverpassClient(HttpClient client)
    {
        _client = client;

        var configureNamedOptions = new ConfigureNamedOptions<ConsoleLoggerOptions>(
            "",
            null);
        var optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(
            new[] { configureNamedOptions },
            Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>()
        );
        var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(
            optionsFactory,
            Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(),
            new OptionsCache<ConsoleLoggerOptions>());

        _logger = new LoggerFactory(
            new[] { new ConsoleLoggerProvider(optionsMonitor) },
            new LoggerFilterOptions { MinLevel = LogLevel.Information })
            .CreateLogger<OverpassClient>();
    }

    public async Task<ElementCollection> Get(string query, CancellationToken cancellationToken = default)
    {
        var retries = 0;
        var maxRetries = 0;
        var retryDelay = 1000;

        while (true)
        {
            try
            {
                var result = await _client.PostAsync("https://www.overpass-api.de/api/interpreter", new StringContent(query), cancellationToken);

                if (result.IsSuccessStatusCode)
                {
                    return OsmSerializer.Deserialize(await result.Content.ReadAsStringAsync()) ?? new ElementCollection();
                }
                throw new IOException($"Error {result.StatusCode}: {await result.Content.ReadAsStringAsync()}");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error fetching osm data");

                retries++;
                if (retries >= maxRetries)
                {
                    throw;
                }
                await Task.Delay(retryDelay, cancellationToken);

                retryDelay = retryDelay * 2;
            }
        }
    }
}
;