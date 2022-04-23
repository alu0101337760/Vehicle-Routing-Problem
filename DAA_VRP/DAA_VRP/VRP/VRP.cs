namespace DAA_VRP
{
    /// <summary>
    /// Class That offers methods to solve the VRP problem.
    /// </summary>
    public class VRP
    {

        enum GraspTypes
        {
            GRASP_REINSERTION,
            GRASP_SINGLE_ROUTE_SWAP,
            GRASP_MULTI_ROUTE_SWAP,
            GRASP_2_OPT
        };


        string sourceFilename = "";
        int numberOfClients = -1;
        int numberOfVehicles = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        public VRP(Problem problem)
        {
            this.sourceFilename = problem.sourceFilename;
            this.numberOfClients = problem.numberOfClients;
            this.numberOfVehicles = problem.numberOfVehicles;
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

        private GraspSolution GraspReinsertion(GraspSolution solution)
        {
            List<List<int>> paths = solution.paths;
            int bestDistance = solution.totalDistance;

           while (true)
            {
                
            }

            return solution;
        }

        private GraspSolution LocalSearch(GraspSolution solution, GraspTypes type = GraspTypes.GRASP_SINGLE_ROUTE_SWAP)
        {
            switch (type)
            {
                case GraspTypes.GRASP_2_OPT:
                    break;

                case GraspTypes.GRASP_MULTI_ROUTE_SWAP:
                    break;

                case GraspTypes.GRASP_REINSERTION:
                    break;

                case GraspTypes.GRASP_SINGLE_ROUTE_SWAP:
                    break;

            }
            return solution;
        }

        public GraspSolution SolveGrasp(int rclSize)
        {
            GraspSolution solution = new GraspSolution(sourceFilename, numberOfClients, rclSize);
            for (int i = 0; i < 5000; i++)
            {
                GraspSolution newSolution = GraspConstructivePhase(rclSize);

                if (newSolution.totalDistance < solution.totalDistance)
                {
                    solution = newSolution;
                }
            }

            return solution;
        }

    }
}
