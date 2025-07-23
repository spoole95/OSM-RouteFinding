using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace OverpassNet.Entities;

public class ElementCollection
{
    public object? Version { get; set; }

    public string? Generator { get; set; }

    [JsonPropertyName("osm3s")]
    public OSM3SInfo? OSM3S { get; set; }

    public string? Copyright { get; set; }

    public string? Attribution { get; set; }

    public string? License { get; set; }

    public IReadOnlyCollection<Element>? Elements { get; set; }

    [JsonIgnore]
    public IDictionary<ulong, Node> Nodes
    {
        get
        {
            if (_nodes == null)
            {
                CalculateIndexes();
            }
            return _nodes;
        }
    }

    [JsonIgnore]
    public double MinLat
    {
        get
        {
            if (_nodes == null)
            {
                CalculateIndexes();
            }
            return _minLat!.Value;
        }
        private set => _minLat = value;
    }
    [JsonIgnore]
    public double MinLon
    {
        get
        {
            if (_minLon == null)
            {
                CalculateIndexes();
            }
            return _minLon!.Value;
        }
        private set => _minLon = value;
    }
    [JsonIgnore]
    public double MaxLat
    {
        get
        {
            if (_maxLat == null)
            {
                CalculateIndexes();
            }
            return _maxLat!.Value;
        }
        private set => _maxLat = value;
    }

    [JsonIgnore]
    public double MaxLon
    {
        get
        {
            if (_maxLon == null)
            {
                CalculateIndexes();
            }
            return _maxLon!.Value;
        }
        private set => _maxLon = value;
    }

    [JsonIgnore]
    public IDictionary<ulong, Way> Ways
    {
        get
        {
            if (_ways == null)
            {
                CalculateIndexes();
            }
            return _ways;
        }
    }

    [JsonIgnore]
    public IDictionary<ulong, Relation> Relations
    {
        get
        {
            if (_relations == null)
            {
                CalculateIndexes();
            }
            return _relations;
        }
    }

    private IDictionary<ulong, Node>? _nodes = null;
    private IDictionary<ulong, Way>? _ways = null;
    private IDictionary<ulong, Relation>? _relations = null;
    private double? _minLat;
    private double? _minLon;
    private double? _maxLat;
    private double? _maxLon;
    private void CalculateIndexes()
    {
        if (Elements == null)
        {
            return;
        }

        var sw = new Stopwatch();
        sw.Start();
        var nodes = new Dictionary<ulong, Node>();
        var ways = new Dictionary<ulong, Way>();
        var relations = new Dictionary<ulong, Relation>();
        var minLat = double.MaxValue;
        var minLon = double.MaxValue;
        var maxLat = double.MinValue;
        var maxLon = double.MinValue;
        var calls = 0;
        object minMaxLock = new();

        //Parallel.ForEach(Elements,
        //() => (minLat: double.MaxValue, minLon: double.MaxValue, maxLat: double.MinValue, maxLon: double.MinValue),
        //(element, state, localMinMax) =>
        //(element)=>
        foreach (var element in Elements)
        {
            switch (element)
            {
                case Node when element is Node node:
                    nodes.TryAdd(element.UId ?? (ulong)element.Id, node);
                    //Determine min/max for this thread, then compute once finished (reduces bottleneck when most elements are nodes)
                    //localMinMax.minLat = Math.Min(localMinMax.minLat, node.Lat);
                    //localMinMax.minLon = Math.Min(localMinMax.minLon, node.Lon);
                    //localMinMax.maxLat = Math.Max(localMinMax.maxLat, node.Lat);
                    //localMinMax.maxLon = Math.Max(localMinMax.maxLon, node.Lon);

                    lock (minMaxLock)
                    {
                        calls++;
                        minLat = Math.Min(minLat, node.Lat);
                        minLon = Math.Min(minLon, node.Lon);
                        maxLat = Math.Max(maxLat, node.Lat);
                        maxLon = Math.Max(maxLon, node.Lon);
                    }
                    break;
                case Way when element is Way way:
                    ways.TryAdd(element.UId ?? (ulong)element.Id, way);
                    break;
                case Relation when element is Relation relation:
                    relations.TryAdd(element.UId ?? (ulong)element.Id, relation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element.Type), $"Unknown element type: {element.Type}");
            }
            //return localMinMax;
            //},
            //localMinMax =>
            //{
            //    lock (minMaxLock)
            //    {
            //        calls++;
            //        MinLat = Math.Min(MinLat, localMinMax.minLat);
            //        MinLon = Math.Min(MinLon, localMinMax.minLon);
            //        MaxLat = Math.Max(MaxLat, localMinMax.maxLat);
            //        MaxLon = Math.Max(MaxLon, localMinMax.maxLon);
        }
        //});

        _nodes = nodes;
        _ways = ways;
        _relations = relations;
        MinLat = minLat;
        MinLon = minLon;
        MaxLat = maxLat;
        MaxLon = maxLon;
        sw.Stop();
        Console.WriteLine(sw.Elapsed.TotalSeconds); // < 0.1s for 624506 elements (~500k nodes, 100k ways) (query 2) - no parraell

    }
}