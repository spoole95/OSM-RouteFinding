using OverpassNet.Entities;
using OverpassNet.Extensions;
using OverpassNet.Query;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace RouteFinding.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static DispatcherTimer timer = new DispatcherTimer();
    private ElementCollection elements;
    private bool FirstTick = true;


    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //elements = await new OverpassQueryBuilder()
        //    .BeginUnion()
        //    .Way(52.463465, -0.962827, 52.499166, -0.887756) //Market Harborough area   
        //    .WithTag("highway")
        //    .RecurseDown()
        //    .EndUnion()
        //    .Output()
        //    .GetAsync();
        elements = await new OverpassQueryBuilder()
                .Relation(8485220)
                .ToArea(".lei")
                .BeginUnion()
                .WayByTag("area.lei")
                .WithTag("highway")
                .RecurseDown()
                .EndUnion()
                .Output()
                .GetAsync();

        timer.Tick += new EventHandler(timer_Tick);
        timer.Interval = TimeSpan.FromSeconds(1);

        timer.Start();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
        if (elements == null) return;

        //Query 1 12079 (map ~ 0.5s) - vs Query 2 - 527137 (map ~ haha)
        //var nodes = elements.Elements.Where(x => x.Type == ElementType.Node).Select(x => (Node)x);
        //var nodes = elements.Nodes;


        var minLon = elements.MinLon;
        var maxLon = elements.MaxLon;
        var maxLat = elements.MaxLat;
        var minLat = elements.MinLat;

        if (FirstTick)
        {
            DrawMap(minLat, maxLat, minLon, maxLon);
            FirstTick = false;
        }
        else
        {

            elements.Nodes.TryGetValue(7414203190, out var start); //Havest Road
                                                                   //var start = elements.Nodes.PickRandom().Value;
            var end = elements.Nodes.PickRandom().Value;

            //if (true) return; //TODO - remove this to run route finding
            var route = RouteFinder.AStar(start, end, elements);

            if (route != null)
            {
                var routeElements = new List<RouteFinding.WPF.UiElements.Line>();
                for (var i = 0; i < route.Count - 1; i++)
                {
                    var n1 = route.Single(x => x.Id == route[i].Id);
                    var n2 = route.Single(x => x.Id == route[i + 1].Id);

                    (double x1, double y1) = ConvertLatLonToXY(n1.Lat, n1.Lon, minLat, maxLat, minLon, maxLon, Width, Height);
                    (double x2, double y2) = ConvertLatLonToXY(n2.Lat, n2.Lon, minLat, maxLat, minLon, maxLon, Width, Height);

                    UiElements.Line line = new UiElements.Line
                    {
                        X1 = x1,
                        Y1 = y1,
                        X2 = x2,
                        Y2 = y2,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2
                    };
                    routeElements.Add(line);
                }

                var visual = new DrawingVisual();
                using (var dc = visual.RenderOpen())
                {
                    foreach (var line in routeElements)
                    {
                        var pen = new Pen(line.Stroke, line.StrokeThickness);
                        dc.DrawLine(pen, new Point(line.X1, line.Y1), new Point(line.X2, line.Y2));
                    }
                }
                RouteCanvas.RedrawVisual(visual);
            }
            timer.Stop();
        }
    }


    private void DrawMap(double minLat,
        double maxLat,
        double minLon,
        double maxLon)
    {
        //MainCanvas.Height = Height;
        //MainCanvas.Width = Width;

        var height = Height;
        var width = Width;

        var sw = new Stopwatch();
        sw.Start();

        //var ways = elements.Elements.Where(x => x.Type == ElementType.Way
        //     && (x.Tags == null ||
        //        !x.Tags.ContainsKey("landuse"))).Cast<Way>().ToList();
        var ways = elements.Ways.Values;

        var lineElements = new ConcurrentBag<UiElements.Line>();

        Parallel.ForEach(ways, new ParallelOptions { MaxDegreeOfParallelism = 5 }, (way) =>
        //foreach (var way in ways)
        {
            elements.Nodes.TryGetValues(way.Nodes, out var wayNodes);

            var wayNodeIds = way.Nodes.Distinct().ToList();

            for (var i = 0; i < wayNodeIds.Count - 1; i++)
            {
                var n1 = wayNodes.First(x => (ulong)x.Id == wayNodeIds[i]);
                var n2 = wayNodes.First(x => (ulong)x.Id == wayNodeIds[i + 1]);

                (double x1, double y1) = ConvertLatLonToXY(n1.Lat, n1.Lon, minLat, maxLat, minLon, maxLon, width, height);
                (double x2, double y2) = ConvertLatLonToXY(n2.Lat, n2.Lon, minLat, maxLat, minLon, maxLon, width, height);

                UiElements.Line line = new UiElements.Line
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Stroke = GetWayColor(way),
                    StrokeThickness = 2
                };
                lineElements.Add(line);
            }
        });

        var visual = new DrawingVisual();
        using (var dc = visual.RenderOpen())
        {
            foreach (var line in lineElements)
            {
                var pen = new Pen(line.Stroke, line.StrokeThickness);
                dc.DrawLine(pen, new Point(line.X1, line.Y1), new Point(line.X2, line.Y2));
            }
        }
        MapCanvas.AddVisual(visual);

        sw.Stop();
        Console.WriteLine(sw.Elapsed.TotalSeconds); // ~1.3s 500k nodes, max degree 5
    }


    private SolidColorBrush GetWayColor(Way way)
    {
        if (way.Tags == null) return Brushes.Aquamarine;

        if (way.Tags.TryGetValue("highway", out var highwayType))
        {
            //TODO - make a property descriptor on Way
            switch (highwayType)
            {
                case "footway":
                case "bridleway":
                case "steps":
                case "corridor":
                case "path":
                case "via_ferrata":
                case "pedestrian":
                    return Brushes.Green; //Path type highways
            }
        }

        return Brushes.Black;
    }

    private static (double, double) ConvertLatLonToXY(double lat,
        double lon,
        double minLat,
        double maxLat,
        double minLon,
        double maxLon,
        double width,
        double height)
    {
        // Normalize longitude (left to right)
        double x = (lon - minLon) / (maxLon - minLon) * width;

        // Normalize latitude using Mercator projection
        double latRad = lat.Radians();
        double minLatRad = minLat.Radians();
        double maxLatRad = maxLat.Radians();

        double normalizedY = (Math.Log(Math.Tan(latRad) + 1 / Math.Cos(latRad)) -
                              Math.Log(Math.Tan(minLatRad) + 1 / Math.Cos(minLatRad))) /
                             (Math.Log(Math.Tan(maxLatRad) + 1 / Math.Cos(maxLatRad)) -
                              Math.Log(Math.Tan(minLatRad) + 1 / Math.Cos(minLatRad)));

        // Flip y-axis since screen coordinates start from the top-left
        double y = height * (1 - normalizedY);

        if (double.IsNaN(x) || double.IsNaN(y))
        {
            //throw new ArgumentException("Invalid latitude or longitude values.");
        }

        return (x, y);
    }
}