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

        private GvnsSolution Shaking(GvnsSolution solution, int currentNeighborIndex)
        {
            ILocalSearch currentSearch = neighborhoodStructures[currentNeighborIndex];
            return solution;
        }

        private GvnsSolution VND(GvnsSolution solution)
        {
            int currentNeighborIndex = 0;
            GvnsSolution bestSolution = solution;
            GvnsSolution currentSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            
            for (int i = 0; i < currentNeighborIndex; i++)
            {
                currentSolution = (GvnsSolution) neighborhoodStructures[i].Search(problem, (Solution)currentSolution);
                if (currentSolution.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = currentSolution;
                    currentNeighborIndex = 0;
                }
            }
            
            return solution;
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

            GvnsSolution candidate = GvnsConstructivePhase(rclSize);
            
            for (int i = 0; i < 1000; i++)
            {
                int currentNeighborIndex = 0;

                candidate = Shaking(candidate, currentNeighborIndex);
                candidate = VND(candidate);
                
                if (candidate.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = candidate;
                    currentNeighborIndex = 0;
                }
            }

            return bestSolution;
        }

    }
}
