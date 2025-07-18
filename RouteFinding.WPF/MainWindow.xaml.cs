using OverpassNet;
using OverpassNet.Entities;
using OverpassNet.Query;
using OverpassNet.Tags;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
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
        elements = await new OverpassQueryBuilder()
            .Way(52.463465, -0.962827, 52.499166, -0.887756)
            .WithTag("highway")
            .Output()
            .RecurseDown()
            .Output()
            .GetAsync();
        //elements = await new OverpassQueryBuilder()
        //        .Relation(8485220)
        //        .ToArea(".lei")
        //        .BeginUnion()
        //        .WayByTag("area.lei")
        //        .WithTag("highway")
        //        .RecurseDown()
        //        .EndUnion()
        //        .Output()
        //        .GetAsync();

        timer.Tick += new EventHandler(timer_Tick);
        timer.Interval = TimeSpan.FromSeconds(1);

        timer.Start();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
        if (elements == null) return;

        var nodes = elements.Elements.Where(x => x.Type == ElementType.Node).Select(x => (Node)x);

        var minLon = nodes.Min(x => x.Lon);
        var maxLon = nodes.Max(x => x.Lon);
        var minLat = nodes.Min(x => x.Lat);
        var maxLat = nodes.Max(x => x.Lat);

        if (FirstTick)
        {
            //TODO - slow at huge sizes (query 2)
            DrawMap(minLat, maxLat, minLon, maxLon);
            FirstTick = false;
        }

        RouteCanvas.Children.Clear(); //Clear for redraw


        //var start = (Node)elements.Elements.First(x => x.Id == 7414203190); //Havest Road
        var start = (Node)elements.Elements.Where(x => x.Type == ElementType.Node).PickRandom();
        var end = (Node)elements.Elements.Where(x => x.Type == ElementType.Node).PickRandom();

        var route = RouteFinder.AStar(start, end, elements);

        if (route != null)
        {
            for (var i = 0; i < route.Count - 1; i++)
            {
                var n1 = route.Single(x => x.Id == route[i].Id);
                var n2 = route.Single(x => x.Id == route[i + 1].Id);

                (double x1, double y1) = ConvertLatLonToXY(n1.Lat, n1.Lon, minLat, maxLat, minLon, maxLon, Width, Height);
                (double x2, double y2) = ConvertLatLonToXY(n2.Lat, n2.Lon, minLat, maxLat, minLon, maxLon, Width, Height);

                Line line = new Line
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                };
                RouteCanvas.Children.Add(line);
            }
        }
    }

    private void DrawMap(double minLat,
        double maxLat,
        double minLon,
        double maxLon)
    {
        MainCanvas.Height = Height;
        MainCanvas.Width = Width;

        var height = Height;
        var width = Width;

        var ways = elements.Elements.Where(x => x.Type == ElementType.Way
             && (x.Tags == null ||
                !x.Tags.ContainsKey("landuse"))).Cast<Way>().ToList();

        foreach (var way in ways)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            var wayNodes = elements.Elements.Where(x => way.Nodes.Contains((ulong)x.Id)).DistinctBy(x => x.Id).Select(x => (Node)x).ToHashSet();

            var wayNodeIds = way.Nodes.ToList();

            for (var i = 0; i < wayNodeIds.Count - 1; i++)
            {
                var n1 = wayNodes.Single(x => (ulong)x.Id == wayNodeIds[i]);
                var n2 = wayNodes.Single(x => (ulong)x.Id == wayNodeIds[i + 1]);

                (double x1, double y1) = ConvertLatLonToXY(n1.Lat, n1.Lon, minLat, maxLat, minLon, maxLon, width, height);
                (double x2, double y2) = ConvertLatLonToXY(n2.Lat, n2.Lon, minLat, maxLat, minLon, maxLon, width, height);

                Line line = new Line
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Stroke = GetWayColor(way),
                    StrokeThickness = 2
                };
                MapCanvas.Children.Add(line);
            }
            //});
        }
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

        return (x, y);
    }
}