using System.Diagnostics;
namespace DAA_VRP
{
    /// <summary>
    /// Implements the Greedy algorithm
    /// </summary>
    public class GreedyRCL
    {

        string sourceFilename = "";
        int numberOfNodes = -1;
        int numberOfPaths = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        public GreedyRCL(Problem problem)
        {
            this.sourceFilename = problem.sourceFilename;
            this.numberOfNodes = problem.numberOfClients;
            this.numberOfPaths = problem.numberOfVehicles;
            this.distanceMatrix = problem.distanceMatrix;
        }


        /// <summary>
        /// Utility function that builds the RCL (restricted candidate list)
        /// given a list with the available nodes, the current node and the 
        /// desired size for the rclSize,
        /// </summary>
        /// <param name="availableNodes">list of available nodes</param>
        /// <param name="currentNode">current node</param>
        /// <param name="rclSize">desired size for the rclSize, set to 1 by default</param>
        private List<int> MakeRCL(HashSet<int> availableNodes, int currentNode, int rclSize = 1)
        {

            List<int> rcl = Enumerable.Repeat(availableNodes.ToList()[0], rclSize).ToList();
            List<int> distance = distanceMatrix[currentNode];

            foreach (int node in availableNodes)
            {
                int candidate = node;
                int currentMinDistance = distance[candidate];

                if (currentMinDistance < distance[rcl[rcl.Count - 1]])
                {
                    for (int i = 0; i < rcl.Count; i++)
                    {
                        if (currentMinDistance < distance[rcl[i]])
                        {
                            currentMinDistance = distance[rcl[i]];

                            int temp = rcl[i];
                            rcl[i] = candidate;
                            candidate = temp;
                        }
                    }
                }
            }

            return rcl;
        }

        /// <summary>
        /// Utility funciton that implements a greedy solution to the VRP
        /// the greedy algorithm naturally selects a random element from a
        /// RCL of the given size.
        /// </summary>
        /// <param name="rclSize">size of the RCL of the algorithm</param>
        public List<List<int>> GreedyWithRCL(int rclSize)
        {
            HashSet<int> availableNodes = new HashSet<int>(Enumerable.Range(1, numberOfNodes - 1).ToList());
            List<List<int>> paths = new List<List<int>>();

            totalDistance = 0;

            for (int i = 0; i < this.numberOfPaths; i++)
            {
                paths.Add(new List<int>());
                paths[i].Add(0);
            }

            int numberOfPaths = this.numberOfPaths;
            while (availableNodes.Count > 0)
            {
                if (availableNodes.Count < numberOfPaths)
                {
                    numberOfPaths = availableNodes.Count;
                }
                for (int i = 0; i < numberOfPaths; i++)
                {
                    int lastNode = paths[i][paths[i].Count - 1];

                    List<int> rcl = MakeRCL(availableNodes, lastNode, rclSize);
                    int randomNode = rcl[new Random().Next(rcl.Count)];

                    paths[i].Add(randomNode);
                    availableNodes.Remove(randomNode);

                    totalDistance += distanceMatrix[lastNode][randomNode];
                }
            }

            for (int i = 0; i < this.numberOfPaths; i++)
            {
                int lastNode = paths[i][paths[i].Count - 1];
                paths[i].Add(0);
                totalDistance += distanceMatrix[lastNode][0];
            }

            return paths;
        }

        /// <summary>
        /// Solves the problem with the given RCL size
        /// </summary>
        /// <param name="rclSize">The RCL size to be used, default to 1</param>
        public GreedySolution Solve(int rclSize = 1)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<List<int>> paths = GreedyWithRCL(rclSize);
            sw.Stop();
            GreedySolution solution = new GreedySolution(sourceFilename, numberOfNodes, totalDistance, sw.ElapsedMilliseconds);
            solution.SetPaths(paths);
            
            return solution;
        }
    }
}
