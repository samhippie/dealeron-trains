using System;
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
                _adjacencyLists[from] = new Dictionary<string, float>();

            //go ahead and make this so we can iterate over the dictionary's keys to get all the nodes
            if (!_adjacencyLists.ContainsKey(to))
                _adjacencyLists[to] = new Dictionary<string, float>();

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
                    if (maxDistance.HasValue && childDistance >= maxDistance) continue;

                    open.Enqueue((childPath, childDistance));
                    if (child == to && (!minStops.HasValue || childPath.Count >= minStops.Value + 1))
                        yield return childPath;
                }
            }
        }

        private IList<string> GetChildren(string node)
        {
            if (!_adjacencyLists.TryGetValue(node, out var adjacencyList)) return new List<string>();
            return adjacencyList.Keys.ToList();
        }

        /// <summary>
        /// Finds the stops and distance of the shorted path between two nodes. Returns null if no path is found.
        /// </summary>
        public (IList<string> stops, float distance)? FindShortestRoute(string from, string to)
        {
            //Dijkstra's algorithm, except we'll return early when we find `to`
            //and you can't visit yourself directly, so a path from A to A can't just be "A"

            //best paths so far for each destination
            //if we relax a child, we'll update it's best path using the parent best path
            var bestPaths = new Dictionary<string, IEnumerable<string>>
            {
                { from, new List<string> { from } },
            };

            var queue = new MinHeap<float, string>();
            queue.Insert(0, from);
            foreach (var adjacencyList in _adjacencyLists)
            {
                if (adjacencyList.Key != from)
                    queue.Insert(float.PositiveInfinity, adjacencyList.Key);
            }

            var closed = new HashSet<string>();
            //a couple flags to modify standard dijkstra's algorithm to revisit the root
            var skippedRoot = false;
            var revisitedRoot = false;
            while (queue.Length > 0)
            {
                var node = queue.Extract().Value;
                if (node.value == to && (to != from || revisitedRoot))
                {
                    return (bestPaths[node.value].ToList(), node.key);
                }
                //need to avoid adding the root to closed so we can revisit it
                if (!skippedRoot)
                    skippedRoot = true;
                else
                    closed.Add(node.value);
                foreach (var child in GetChildren(node.value))
                {
                    if (closed.Contains(child))
                        continue;
                    var newDistance = node.key + GetPairDisance(node.value, child).Value;
                    if ((child == from && !revisitedRoot) || newDistance < queue.GetKey(child))
                    {
                        if (child == from)
                        {
                            revisitedRoot = true;
                            //we removed the root when we started the algorithm, so we have to add it again
                            queue.Insert(newDistance, child);
                        }
                        else
                        {
                            queue.DecreaseKey(child, newDistance);
                        }
                        bestPaths[child] = bestPaths[node.value].Append(child);
                    }
                }
            }
            return null;
        }
    }
}
