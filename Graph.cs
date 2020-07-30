using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace dealeron_trains
{
    public class Graph
    {
        private Dictionary<string, Dictionary<string, float>> _adjacencyLists { get; set; } = new Dictionary<string, Dictionary<string, float>>();

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
    }
}
