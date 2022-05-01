namespace DAA_VRP
{
    public class GVNS
    {
        Problem problem;
        int numberOfNodes = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        List<ILocalSearch> neighborhoodStructures = new List<ILocalSearch>{            
            new MultiInsertion(),
            new SingleInsertion(),           
            new MultiSwap(),
            new SingleSwap(),
            };

        public GVNS(Problem problem)
        {
            this.problem = problem;
            this.numberOfNodes = problem.numberOfClients;
            this.distanceMatrix = problem.distanceMatrix;
        }

        private GvnsSolution GvnsConstructivePhase(int rclSize)
        {
            GreedyRCL greedy = new GreedyRCL(problem);

            GreedySolution greedySolution = greedy.Solve(rclSize);

            GvnsSolution solution = new GvnsSolution(problem.sourceFilename, numberOfNodes, rclSize);
            solution.SetPaths(greedySolution.paths);
            solution.totalDistance = greedySolution.totalDistance;
            return solution;
        }

        public GvnsSolution Solve(int rclSize)
        {
            GvnsSolution bestSolution = new GvnsSolution(problem.sourceFilename, numberOfNodes, rclSize);
            bestSolution.totalDistance = int.MaxValue;

            for(int i = 0; i < 1000; i++)
            {
                GvnsSolution candidate = GvnsConstructivePhase(rclSize);
                
                if (candidate.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = candidate;                    
                }
            }

            return bestSolution;
        }

    }
}
