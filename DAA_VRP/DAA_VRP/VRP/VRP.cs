namespace DAA_VRP
{
    /// <summary>
    /// Class That offers methods to solve the VRP problem.
    /// </summary>
    public class VRP
    {

        string sourceFilename = "";
        int numberOfClients = -1;
        int numberOfVehicles = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        public VRP(string filename)
        {
            BuildFromFile(filename);
        }

        /// <summary>
        /// Utility function to retrieve the number of clients 
        /// from a string
        /// </summary>
        private int RetrieveNumberOfClients(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]);
        }

        /// <summary>
        /// Utility function to retrieve the number of vehicles
        /// from a string
        /// </summary>
        private int RetrieveNumberOfVehicles(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]);
        }

        /// <summary>
        /// Utility function to build the distance matrix from
        /// a list of strings
        /// </summary>
        private void buildDistanceMatrix(List<string> lines)
        {
            for (int i = 0; i < numberOfClients; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < numberOfClients; j++)
                {
                    row.Add(int.Parse(lines[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[j]));
                }
                distanceMatrix.Add(row);
            }
        }

        /// <summary>
        /// Function that builds the VRP class from the contents of 
        /// the file given, it must follow the correct format.
        /// </summary>
        /// <param name="filename"></param>
        public void BuildFromFile(string filename)
        {
            sourceFilename = filename;
            List<string> lines = new List<string>(File.ReadAllLines(filename));
            this.numberOfClients = RetrieveNumberOfClients(lines[0]);
            this.numberOfVehicles = RetrieveNumberOfVehicles(lines[1]);
            buildDistanceMatrix(lines.GetRange(3, lines.Count - 4));

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
        private List<List<int>> GreedyWithRCL(int rclSize)
        {
            HashSet<int> availableNodes = new HashSet<int>(Enumerable.Range(1, numberOfClients - 1).ToList());
            List<List<int>> paths = new List<List<int>>();

            totalDistance = 0;

            for (int i = 0; i < numberOfVehicles; i++)
            {
                paths.Add(new List<int>());
                paths[i].Add(0);
            }

            int numberOfPaths = numberOfVehicles;
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

            for (int i = 0; i < numberOfVehicles; i++)
            {
                int lastNode = paths[i][paths[i].Count - 1];
                paths[i].Add(0);
                totalDistance += distanceMatrix[lastNode][0];
            }

            return paths;
        }

        /// <summary>
        /// Function that solves the VRP with a greedy algorithm.
        /// Returs a new GreedySolution object
        /// </summary>
        public GreedySolution SolveGreedy()
        {
            List<List<int>> paths = GreedyWithRCL(1);

            return new GreedySolution(sourceFilename, numberOfClients, totalDistance, -1, paths);
        }

        /// <summary>
        /// Utility function, for the constructive part of the 
        /// GRASP algorithm
        /// </summary>
        /// <param name="rclSize">size of the RCL of the algorithm</param>
        private GraspSolution GraspConstructivePhase(int rclSize)
        {
            List<List<int>> paths = GreedyWithRCL(rclSize);

            GraspSolution solution = new GraspSolution(sourceFilename, numberOfClients, rclSize);
            solution.paths = paths;
            solution.totalDistance = totalDistance;
            return solution;

        }

        public GraspSolution SolveGrasp()
        {
            throw new NotImplementedException();
        }

    }
}
