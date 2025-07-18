using System.Windows;
using System.Windows.Media;

namespace RouteFinding.WPF.UiElements;

public class VisualHost : FrameworkElement
{

    private VisualCollection _children;

    public VisualHost()
    {
        _children = new VisualCollection(this);
    }

    public void AddVisual(DrawingVisual visual)
    {
        _children.Add(visual);
    }

    protected override int VisualChildrenCount => _children.Count;

    protected override Visual GetVisualChild(int index) => _children[index];

    public void RedrawVisual(DrawingVisual visual) 
    {
        _children.Clear();
        AddVisual(visual);
    }
}
