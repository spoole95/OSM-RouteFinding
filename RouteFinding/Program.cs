// See https://aka.ms/new-console-template for more information
using OverpassNet.Query;

Console.WriteLine("Hello, World!");
var elements = await new OverpassClient(new HttpClient()).GetArea(849358753);

Console.WriteLine($"{elements.Elements?.Count(x => x.Type == OverpassNet.Entities.ElementType.Node)} nodes");