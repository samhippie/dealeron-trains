using System;
using System.Collections.Generic;
using System.Linq;

namespace dealeron_trains
{
    public class Program
    {
        static void Main(string[] args)
        {
            Graph graph;
            try
            {
                string input;
                if (args.Length > 0)
                    input = args[0];
                else
                    input = ReadAllStdin();

                graph = ReadGraphFromString(input);
            }
            catch
            {
                Console.Error.WriteLine("Failed to parse input");
                throw;
            }

            PrintOutput1(graph);
            PrintOutput2(graph);
            PrintOutput3(graph);
            PrintOutput4(graph);
            PrintOutput5(graph);
            PrintOutput6(graph);
            PrintOutput7(graph);
            PrintOutput8(graph);
            PrintOutput9(graph);
            PrintOutput10(graph);
        }

        private static string ReadAllStdin()
        {
            var lines = new List<string>();
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                lines.Add(line);
            }
            return string.Join("", lines);
        }

        public static Graph ReadGraphFromString(string input)
        {
            var graph = new Graph();

            var routeStrings = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var routeString in routeStrings)
            {
                var (from, to, distance) = GetRouteFromString(routeString);
                graph.AddRoute(from, to, distance);
            }

            return graph;
        }

        private static (string, string, float) GetRouteFromString(string routeString)
        {
            routeString = routeString.Trim();
            var from = routeString.Substring(0, 1);
            var to = routeString.Substring(1, 1);
            var distance = float.Parse(routeString.Substring(2));
            return (from, to, distance);
        }

        private static void PrintOutput1(Graph graph)
        {
            var path = (int?) graph.GetRouteDistance("A", "B", "C");
            Console.WriteLine($"Output #1: {path?.ToString() ?? "NO SUCH ROUTE"}");
        }

        private static void PrintOutput2(Graph graph)
        {
            var path = (int?) graph.GetRouteDistance("A", "D");
            Console.WriteLine($"Output #2: {path?.ToString() ?? "NO SUCH ROUTE"}");
        }

        private static void PrintOutput3(Graph graph)
        {
            var path = (int?) graph.GetRouteDistance("A", "D", "C");
            Console.WriteLine($"Output #3: {path?.ToString() ?? "NO SUCH ROUTE"}");
        }

        private static void PrintOutput4(Graph graph)
        {
            var path = (int?) graph.GetRouteDistance("A", "E", "B", "C", "D");
            Console.WriteLine($"Output #4: {path?.ToString() ?? "NO SUCH ROUTE"}");
        }

        private static void PrintOutput5(Graph graph)
        {
            var path = (int?) graph.GetRouteDistance("A", "E", "D");
            Console.WriteLine($"Output #5: {path?.ToString() ?? "NO SUCH ROUTE"}");
        }

        private static void PrintOutput6(Graph graph)
        {
            var routes = graph.FindAllRoutesWithMaxStops("C", "C", 3);
            Console.WriteLine($"Output #6: {routes.Count()}");
        }

        private static void PrintOutput7(Graph graph)
        {
            var routes = graph.FindAllRoutesWithExactStops("A", "C", 4);
            Console.WriteLine($"Output #7: {routes.Count()}");
        }

        private static void PrintOutput8(Graph graph)
        {
            var distance = (int) graph.FindShortestRoute("A", "C").Value.distance;
            Console.WriteLine($"Output #8: {distance}");
        }

        private static void PrintOutput9(Graph graph)
        {
            var distance = (int) graph.FindShortestRoute("B", "B").Value.distance;
            Console.WriteLine($"Output #9: {distance}");
        }

        private static void PrintOutput10(Graph graph)
        {
            var routes = graph.FindAllRoutesWithMaxDistance("C", "C", 30);
            Console.WriteLine($"Output #10: {routes.Count()}");
        }
    }
}
