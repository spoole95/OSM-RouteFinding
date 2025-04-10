using OverpassNet.Entities;

namespace RouteFinding.Tests;
public class ElementCollectionTests
{
    [Fact]
    public void Neighbours_ShouldReturnCorrectNeighbours()
    {
        // Arrange
        var node1 = new Node(1);
        var node2 = new Node(2);
        var node3 = new Node(3);
        var way1 = new Way(1) { Nodes = [ 1, 2 ] };
        var way2 = new Way(2) { Nodes = [ 2, 3 ] };

        ElementCollection map = new ElementCollection 
        { 
            Elements = new List<Element>
            {
                node1,
                node2,
                node3,
                way1,
                way2
            }
        };

        // Act
        var neighbours = map.Neighbours(node2);

        // Assert
        Assert.Equal(2, neighbours.Count);
        Assert.Contains(node1, neighbours);
        Assert.Contains(node3, neighbours);
    }
}
