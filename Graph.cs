using System.Collections.Generic;
using System.Linq;

namespace dealeron_trains
{
    public class Graph
    {
        private readonly Dictionary<string, Dictionary<string, float>> _adjacencyLists = new Dictionary<string, Dictionary<string, float>>();

        /// <summary>
        /// Saves the immediate route between nodes to the graph
        /// </summary>
        public void AddRoute(string from, string to, float distance)
        {
            if (!_adjacencyLists.ContainsKey(from))
            {
                _adjacencyLists[from] = new Dictionary<string, float>();
            }

            _adjacencyLists[from][to] = distance;
        }

        /// <summary>
        /// Given a starting node and a list of stops, returns the distance following the exact route or null if not found
        /// </summary>
        public float? GetRouteDistance(string from, params string[] stops)
        {
            var distance = 0f;
            foreach (var stop in stops)
            {
                if (GetPairDisance(from, stop) is float pairDistance)
                {
                    distance += pairDistance;
                    from = stop;
                }
                else
                {
                    return null;
                }
            }
            return distance;
        }

        /// <summary>
        /// Gets the immedate distance between two nodes if possible or null if not found
        /// </summary>
        private float? GetPairDisance(string from, string to)
        {
            if (!_adjacencyLists.TryGetValue(from, out var adjacencyList)) return null;
            if (!adjacencyList.TryGetValue(to, out var distance)) return null;
            return distance;
        }

        /// <summary>
        /// Finds all routes between nodes with a maximum number of stops
        /// </summary>
        public IEnumerable<IList<string>> FindAllRoutesWithMaxStops(string from, string to, int maxStops)
        {
            return FindAllRoutes(from, to, maxStops: maxStops);
        }

        /// <summary>
        /// Finds all routes between nodes with an exact number of stops
        /// </summary>
        public IEnumerable<IList<string>> FindAllRoutesWithExactStops(string from, string to, int stops)
        {
            return FindAllRoutes(from, to, maxStops: stops, minStops: stops);
        }

        /// <summary>
        /// Finds all routes between nodes with a maximum distance
        /// </summary>
        public IEnumerable<IList<string>> FindAllRoutesWithMaxDistance(string from, string to, float maxDistance)
        {
            return FindAllRoutes(from, to, maxDistance: maxDistance);
        }

        /// <summary>
        /// Finds all routes from one node to another with options for minimum number of stops, maximum number of stops, and distance
        /// </summary>
        private IEnumerable<IList<string>> FindAllRoutes(string from, string to, int? maxStops=null, int? minStops=null, float? maxDistance=null)
        {
            //breadth-first search

            //queue of previously visited paths
            var open = new Queue<(List<string> path, float distance)>();
            open.Enqueue((new List<string> { from }, 0));

            while (open.TryDequeue(out var pathDistance))
            {
                var (path, distance) = pathDistance;
                // we're interested in path.Count + 1 as we're about to look at the children,
                // but we're also interested in maxStops + 1 because the source doesn't count as a stop
                var node = path.Last();
                var children = GetChildren(node);
                foreach (var child in children)
                {
                    var childPath = path.Append(child).ToList();
                    // we know the child exists, so the distance must also exist
                    var childDistance = GetPairDisance(node, child).Value + distance;

                    if (maxStops.HasValue && childPath.Count > maxStops.Value + 1) continue;
                    if (maxDistance.HasValue && childDistance > maxDistance) continue;

                    open.Enqueue((childPath, childDistance));
                    if (child == to && (!minStops.HasValue || childPath.Count >= minStops.Value))
                    {
                        yield return childPath;
                    }
                }
            }
        }

        private IList<string> GetChildren(string node)
        {
            if (!_adjacencyLists.TryGetValue(node, out var adjacencyList)) return new List<string>();
            return adjacencyList.Keys.ToList();
        }
    }
}
