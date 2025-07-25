﻿using System.Text.Json.Serialization;

namespace OverpassNet.Entities;

public class OSM3SInfo
{
    [JsonPropertyName("timestamp_osm_base")]
    public DateTime? OsmTimestamp { get; set; }
    public string? Copyright { get; set; }
}