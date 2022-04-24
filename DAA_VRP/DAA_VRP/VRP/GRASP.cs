

namespace DAA_VRP
{
    public enum GraspTypes
    {
        GRASP_REINSERTION_INTRA,
        GRASP_REINSERTION_INTER,
        GRASP_SINGLE_ROUTE_SWAP,
        GRASP_MULTI_ROUTE_SWAP,
        GRASP_2_OPT
    };

    public class GRASP
    {
        Problem problem;
        int numberOfClients = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        public GRASP(Problem problem)
        {
            this.numberOfClients = problem.numberOfClients;
            this.distanceMatrix = problem.distanceMatrix;
            this.problem = problem;
        }

        private GraspSolution ReinsertPath(GraspSolution solution, int index)
        {
            List<int> path = solution.paths[index];
            int nodeToInsert = -1;
            int positionToInsert = -1;
            int minDistance = solution.totalDistance;

            for (int i = 1; i < path.Count - 1; i++)
            {
                int currentNode = path[i];
                int currentDistance = solution.totalDistance -
                    distanceMatrix[currentNode][currentNode+1] -
                    distanceMatrix[currentNode-1][currentNode];

                for (int candidate = currentNode+1; candidate < problem.; candidate++)
                {
                    int nextCandidate = candidate + 1;
                    int candidateDistance = currentDistance +
                        distanceMatrix[candidate][currentNode] +
                        distanceMatrix[currentNode][nextCandidate];

                    if (candidateDistance < minDistance)
                    {
                        minDistance = candidateDistance;
                        nodeToInsert = currentNode;
                        positionToInsert = candidate;
                    }
                }
            }
            
            path.Remove(nodeToInsert);
            path.Insert(positionToInsert, nodeToInsert);
            solution.totalDistance = minDistance;
            return solution;
        }
        public GraspSolution GraspReinsertionIntra(GraspSolution solution)
        {
            int MAX_ITERATIONS = 1000;
            List<int> pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
            int i = 0;
            while (pathsToCheck.Count > 0 || i > MAX_ITERATIONS)
            {
                GraspSolution currentSolution = ReinsertPath(solution, pathsToCheck[i]);
                if (currentSolution.totalDistance == solution.totalDistance)
                {
                    pathsToCheck.RemoveAt(i);
                }
                if (currentSolution.totalDistance < solution.totalDistance)
                {
                    solution = currentSolution;
                    pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
                }
                i++;
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

                case GraspTypes.GRASP_REINSERTION_INTER:
                    break;

                case GraspTypes.GRASP_REINSERTION_INTRA:
                    return GraspReinsertionIntra(solution);

                case GraspTypes.GRASP_SINGLE_ROUTE_SWAP:
                    break;

            }
            return solution;
        }

        /// <summary>
        /// Utility function, for the constructive part of the 
        /// GRASP algorithm
        /// </summary>
        /// <param name="rclSize">size of the RCL of the algorithm</param>
        private GraspSolution GraspConstructivePhase(int rclSize)
        {
            GreedyRCL greedy = new GreedyRCL(problem);

            List<List<int>> paths = greedy.GreedyWithRCL(rclSize);

            GraspSolution solution = new GraspSolution(problem.sourceFilename, numberOfClients, rclSize);
            solution.SetPaths(paths);
            solution.totalDistance = totalDistance;
            return solution;
        }

        public GraspSolution Solve(int rclSize, GraspTypes type)
        {
            GraspSolution bestSolution = new GraspSolution(problem.sourceFilename, numberOfClients, rclSize);
            bestSolution.totalDistance = int.MaxValue;
            for (int i = 0; i < 5000; i++)
            {
                GraspSolution candidate = GraspConstructivePhase(rclSize);
                GraspSolution processed = LocalSearch(candidate, type);
                if (processed.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = processed;
                }
            }

            return bestSolution;
        }
    }
}
