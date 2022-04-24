

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
        int numberOfNodes = -1;
        int totalDistance = -1;
        int numberOfPaths = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        public GRASP(Problem problem)
        {
            this.numberOfNodes = problem.numberOfClients;
            this.distanceMatrix = problem.distanceMatrix;
            this.problem = problem;
            this.numberOfPaths = problem.numberOfVehicles;
        }

        private GraspSolution ReinsertPath(GraspSolution solution, int index)
        {
            List<int> path = solution.paths[index];
            int indexToRemove = 0;
            int positionToInsert = 0;
            int minDistance = solution.totalDistance;

            for (int currentIndex = 1; currentIndex < path.Count - 1; currentIndex++)
            {
                int distanceAfterRemoving = solution.totalDistance -
                    distanceMatrix[path[currentIndex]][path[currentIndex + 1]] -
                    distanceMatrix[path[currentIndex - 1]][path[currentIndex]] +
                    distanceMatrix[path[currentIndex - 1]][path[currentIndex + 1]];

                for (int candidatePosition = currentIndex + 1; candidatePosition < path.Count - 1; candidatePosition++)
                {
                    int nextCandidate = candidatePosition + 1;
                    int candidateDistance = distanceAfterRemoving +
                        distanceMatrix[path[candidatePosition]][path[currentIndex]] +
                        distanceMatrix[path[currentIndex]][path[nextCandidate]] -
                        distanceMatrix[path[candidatePosition]][path[nextCandidate]];


                    if (candidateDistance < minDistance)
                    {
                        minDistance = candidateDistance;
                        indexToRemove = currentIndex;
                        positionToInsert = candidatePosition;
                    }
                }
            }

            int nodeToInsert = path[indexToRemove];

            GraspSolution newSolution = new GraspSolution(solution.problemId, numberOfNodes, solution.rclSize );
            newSolution.SetPaths(solution.paths);
            newSolution.totalDistance = minDistance;
            newSolution.paths[index].RemoveAt(indexToRemove);
            
            newSolution.paths[index].Insert(positionToInsert, nodeToInsert);
            return newSolution;
        }
        public GraspSolution GraspReinsertionIntra(GraspSolution solution)
        {
            int MAX_ITERATIONS = 5000;
            List<int> pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
            int it = 0;

            GraspSolution bestSolution = solution;
            
            while (pathsToCheck.Count > 0 && it < MAX_ITERATIONS)
            {
                for (int i = 0; i < pathsToCheck.Count; i++)
                {
                    GraspSolution currentSolution = ReinsertPath(bestSolution, pathsToCheck[i]);
                    if (currentSolution.totalDistance == bestSolution.totalDistance)
                    {
                        pathsToCheck.RemoveAt(i);
                    }
                    if (currentSolution.totalDistance < bestSolution.totalDistance)
                    {
                        bestSolution = currentSolution;
                        // Búsqueda ansiosa:  pathsToCheck = Enumerable.Range(0, solution.paths.Count).ToList();
                    }
                }
                it++;
            }

            return bestSolution;
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

            GreedySolution greedySolution = greedy.Solve(rclSize);

            GraspSolution solution = new GraspSolution(problem.sourceFilename, numberOfNodes, rclSize);
            solution.SetPaths(greedySolution.paths);
            solution.totalDistance = totalDistance;
            solution.totalDistance = greedySolution.totalDistance;
            return solution;
        }

        public GraspSolution Solve(int rclSize, GraspTypes type)
        {
            GraspSolution bestSolution = new GraspSolution(problem.sourceFilename, numberOfNodes, rclSize);
            bestSolution.totalDistance = int.MaxValue;
            for (int i = 0; i < 2000; i++)
            {
                GraspSolution candidate = GraspConstructivePhase(rclSize);
                GraspSolution processed = LocalSearch(candidate, type);
                if (processed.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = processed;
                    i = 0;
                }
            }

            return bestSolution;
        }
    }
}
