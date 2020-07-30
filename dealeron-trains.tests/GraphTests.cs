using NUnit.Framework;
using System.Collections.Generic;
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
        public void GetRouteDistance_HandlesInvalidPath()
        {
            var distance = _graph.GetRouteDistance("a", "b", "e", "a");
            Assert.IsNull(distance);
        }


        [Test]
        public void FindAllRoutesWithMaxStops_FindsSinglePath()
        {
            var routes = _graph.FindAllRoutesWithMaxStops("a", "q", 8).ToList();
            Assert.AreEqual(1, routes.Count);
            Assert.AreEqual(new List<string> { "a", "q" }, routes[0]);
        }

        [Test]
        public void FindAllRoutesWithMaxStops_FindsMultiplePaths()
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
            var routes = _graph.FindAllRoutesWithExactStops("e", "c", 4).ToList();
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
    }
}
