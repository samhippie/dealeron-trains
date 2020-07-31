using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace dealeron_trains.tests
{
    [TestFixture]
    public class GraphTests
    {
        private Graph _graph;

        [SetUp]
        public void SetUp()
        {
            _graph = new Graph();
            _graph.AddRoute("a", "b", 5);
            _graph.AddRoute("b", "c", 8);
            _graph.AddRoute("c", "d", 2);
            _graph.AddRoute("a", "d", 4);
            _graph.AddRoute("a", "h", 1);
            _graph.AddRoute("h", "c", 5);
            _graph.AddRoute("h", "e", 5);
            _graph.AddRoute("b", "e", 1);
            _graph.AddRoute("e", "c", 3);
            _graph.AddRoute("c", "e", 9);
            _graph.AddRoute("a", "q", 10);
            _graph.AddRoute("a", "g", 10);
            _graph.AddRoute("g", "f", 11);
            _graph.AddRoute("a", "f", 12);
            _graph.AddRoute("i", "j", 2);
            _graph.AddRoute("j", "i", 3);
        }

        [Test]
        public void GetRouteDistance_FindsDoubleDistance()
        {
            var distance = _graph.GetRouteDistance("e", "c");
            Assert.AreEqual(3, distance);
        }

        [Test]
        public void GetRouteDistance_FindsTripleDistance()
        {
            var distance = _graph.GetRouteDistance("a", "b", "c");
            Assert.AreEqual(13, distance);
        }

        [Test]
        public void GetRouteDistance_HandlesInvalidRoute()
        {
            var distance = _graph.GetRouteDistance("a", "b", "e", "a");
            Assert.IsNull(distance);
        }


        [Test]
        public void FindAllRoutesWithMaxStops_FindsSingleRoute()
        {
            var routes = _graph.FindAllRoutesWithMaxStops("a", "q", 8).ToList();
            Assert.AreEqual(1, routes.Count);
            Assert.AreEqual(new List<string> { "a", "q" }, routes[0]);
        }

        [Test]
        public void FindAllRoutesWithMaxStops_FindsMultipleRoutes()
        {
            var routes = _graph.FindAllRoutesWithMaxStops("a", "f", 8).ToList();
            Assert.AreEqual(2, routes.Count);

            var shortRoute = routes.Single(r => r.Count == 2);
            Assert.AreEqual(new List<string> { "a", "f" }, shortRoute);

            var longRoute = routes.Single(r => r.Count == 3);
            Assert.AreEqual(new List<string> { "a", "g", "f" }, longRoute);
        }

        [Test]
        public void FindAllRoutesWithMaxStops_CutsOffLongRoutes()
        {
            var routes = _graph.FindAllRoutesWithMaxStops("e", "c", 3).ToList();
            Assert.AreEqual(2, routes.Count);

            var shortRoute = routes.Single(r => r.Count == 2);
            Assert.AreEqual(new List<string> { "e", "c" }, shortRoute);

            var longRoute = routes.Single(r => r.Count == 4);
            Assert.AreEqual(new List<string> { "e", "c", "e", "c" }, longRoute);
        }

        [Test]
        public void FindAllRoutesWithExactStops_FindsSingleRoute()
        {
            var routes = _graph.FindAllRoutesWithExactStops("e", "c", 3).ToList();
            Assert.AreEqual(1, routes.Count);
            Assert.AreEqual(new List<string> { "e", "c", "e", "c" }, routes[0]);
        }

        [Test]
        public void FindAllRoutesWithExactStops_FindsTwoRoutes()
        {
            var routes = _graph.FindAllRoutesWithExactStops("a", "c", 2).ToList();
            Assert.AreEqual(2, routes.Count);
            var route1 = routes.Single(r => r[1] == "b");
            Assert.AreEqual(new List<string> { "a", "b", "c" }, route1);
            var route2 = routes.Single(r => r[1] == "h");
            Assert.AreEqual(new List<string> { "a", "h", "c" }, route2);
        }

        [Test]
        public void FindShortestRoute_FindsNontrivialRouteForTrivialRoute()
        {
            var (route, distance) = _graph.FindShortestRoute("i", "i").Value;
            Assert.AreEqual(new List<string> { "i", "j", "i" }, route);
            Assert.AreEqual(5f, distance);
        }

        [Test]
        public void FindShortestRoute_FindsShortRoute()
        {
            var (route, distance) = _graph.FindShortestRoute("a", "g").Value;
            Assert.AreEqual(new List<string> { "a", "g" }, route);
            Assert.AreEqual(10f, distance);
        }

        [Test]
        public void FindShortestRoute_FindsMediumRoute()
        {
            var (route, distance) = _graph.FindShortestRoute("a", "e").Value;
            Assert.AreEqual(new List<string> { "a", "h", "e" }, route);
            Assert.AreEqual(6f, distance);
        }

        [Test]
        public void FindShortestRoute_FindsLongRoutes()
        {
            //This is going to be a little more complicated, so we'll use a new graph
            //This is copied from the CLRS algorithms book
            var graph = new Graph();
            graph.AddRoute("s", "t", 10);
            graph.AddRoute("s", "y", 5);
            graph.AddRoute("t", "x", 1);
            graph.AddRoute("t", "y", 2);
            graph.AddRoute("y", "t", 3);
            graph.AddRoute("y", "x", 9);
            graph.AddRoute("y", "z", 2);
            graph.AddRoute("x", "z", 4);
            graph.AddRoute("z", "x", 6);
            graph.AddRoute("z", "s", 7);

            var (route, distance) = graph.FindShortestRoute("s", "y").Value;
            Assert.AreEqual(new List<string> { "s", "y" }, route);
            Assert.AreEqual(5f, distance);

            (route, distance) = graph.FindShortestRoute("s", "t").Value;
            Assert.AreEqual(new List<string> { "s", "y", "t" }, route);
            Assert.AreEqual(8f, distance);

            (route, distance) = graph.FindShortestRoute("s", "x").Value;
            Assert.AreEqual(new List<string> { "s", "y", "t", "x" }, route);
            Assert.AreEqual(9f, distance);

            (route, distance) = graph.FindShortestRoute("s", "z").Value;
            Assert.AreEqual(new List<string> { "s", "y", "z" }, route);
            Assert.AreEqual(7f, distance);
        }
    }
}
