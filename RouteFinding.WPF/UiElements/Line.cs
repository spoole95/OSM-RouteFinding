using System.Windows.Media;

namespace RouteFinding.WPF.UiElements;

public class Line
{
    //
    // Summary:
    //     Gets or sets the x-coordinate of the System.Windows.Shapes.Line start point.
    //
    //
    // Returns:
    //     The x-coordinate for the start point of the line. The default is 0.
    public double X1 { get; set; }
    //
    // Summary:
    //     Gets or sets the x-coordinate of the System.Windows.Shapes.Line end point.
    //
    // Returns:
    //     The x-coordinate for the end point of the line. The default is 0.
    public double X2 { get; set; }
    //
    // Summary:
    //     Gets or sets the y-coordinate of the System.Windows.Shapes.Line start point.
    //
    //
    // Returns:
    //     The y-coordinate for the start point of the line. The default is 0.
    public double Y1 { get; set; }
    //
    // Summary:
    //     Gets or sets the y-coordinate of the System.Windows.Shapes.Line end point.
    //
    // Returns:
    //     The y-coordinate for the end point of the line. The default is 0.
    public double Y2 { get; set; }

    //
    // Summary:
    //     Gets or sets the System.Windows.Media.Brush that specifies how the System.Windows.Shapes.Shape
    //     outline is painted.
    //
    // Returns:
    //     A System.Windows.Media.Brush that specifies how the System.Windows.Shapes.Shape
    //     outline is painted. The default is null.
    public Brush Stroke { get; set; }
    //
    // Summary:
    //     Gets or sets the width of the System.Windows.Shapes.Shape outline.
    //
    // Returns:
    //     The width of the System.Windows.Shapes.Shape outline.
    public double StrokeThickness { get; set; }
}
