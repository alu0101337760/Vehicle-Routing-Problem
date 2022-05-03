using System.Diagnostics;

namespace DAA_VRP
{


    /////////// MODIFICACIÓN ////////////

    public enum GvnsTypes
    {
        VND,
        SEQUENTIAL_VND_1,
        SEQUENTIAL_VND_2,
    }

    /////////////////////////////////////
    public class GVNS
    {
        Problem problem;
        int numberOfNodes = -1;

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
        }

        private GvnsSolution Shaking(GvnsSolution solution, int currentNeighborIndex)
        {
            return (GvnsSolution)neighborhoodStructures[currentNeighborIndex].Shake(problem, (Solution)solution);
        }


        ////////////// MODIFICACIÓN ///////////////

        private GvnsSolution SequentialVndAlternativeOrder(GvnsSolution solution)
        {
            this.neighborhoodStructures = new List<ILocalSearch>{
            new MultiInsertion(),
            new MultiSwap(),
            new SingleInsertion(),
            new SingleSwap(),
            };

            int currentNeighborIndex = 0;
            GvnsSolution bestSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            bestSolution.SetPaths(solution.paths);
            bestSolution.rclSize = solution.GetRclSize();
            bestSolution.totalDistance = solution.totalDistance;


            GvnsSolution currentSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            currentSolution.SetPaths(solution.paths);
            currentSolution.rclSize = solution.GetRclSize();
            currentSolution.totalDistance = solution.totalDistance;

            for (int i = 0; i <= currentNeighborIndex; i++)
            {
                currentSolution = (GvnsSolution)neighborhoodStructures[i].Search(problem, (Solution)currentSolution);
            }

            return bestSolution;
        }

        private GvnsSolution SequentialVND(GvnsSolution solution)
        {
            int currentNeighborIndex = 0;
            GvnsSolution bestSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            bestSolution.SetPaths(solution.paths);
            bestSolution.rclSize = solution.GetRclSize();
            bestSolution.totalDistance = solution.totalDistance;


            GvnsSolution currentSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            currentSolution.SetPaths(solution.paths);
            currentSolution.rclSize = solution.GetRclSize();
            currentSolution.totalDistance = solution.totalDistance;

            for (int i = 0; i <= currentNeighborIndex; i++)
            {
                currentSolution = (GvnsSolution)neighborhoodStructures[i].Search(problem, (Solution)currentSolution);
            }

            return bestSolution;

        }
        ///////////////////////////////////////////
        private GvnsSolution VND(GvnsSolution solution)
        {
            int currentNeighborIndex = 0;
            GvnsSolution bestSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            bestSolution.SetPaths(solution.paths);
            bestSolution.rclSize = solution.GetRclSize();
            bestSolution.totalDistance = solution.totalDistance;


            GvnsSolution currentSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            currentSolution.SetPaths(solution.paths);
            currentSolution.rclSize = solution.GetRclSize();
            currentSolution.totalDistance = solution.totalDistance;


            for (int i = 0; i <= currentNeighborIndex; i++)
            {
                currentSolution = (GvnsSolution)neighborhoodStructures[i].Search(problem, (Solution)currentSolution);
                if (currentSolution.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = currentSolution;
                    currentNeighborIndex = 0;
                }
            }

            return bestSolution;
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

        public GvnsSolution Solve(int rclSize, GvnsTypes type = GvnsTypes.VND)
        {
            GvnsSolution bestSolution = GvnsConstructivePhase(rclSize);
            int k = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 2000; i++)
            {
                GvnsSolution candidate = Shaking(bestSolution, k);

                ///////// MODIFICACIÓN //////////
                candidate = VND(candidate);

                switch (type)
                {
                    case GvnsTypes.VND:
                        candidate = VND(candidate);
                        break;

                    case GvnsTypes.SEQUENTIAL_VND_1:
                        candidate = SequentialVND(candidate);
                        break;

                    case GvnsTypes.SEQUENTIAL_VND_2:
                        candidate = SequentialVndAlternativeOrder(candidate);
                        break;

                    default:
                        candidate = VND(candidate);
                        break;

                }

                /////////////////////////////////

                if (candidate.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = candidate;
                    k = 0;
                    i = 0;
                }
                else
                {
                    k++;
                    if (k >= neighborhoodStructures.Count)
                    {
                        k = 0;
                    }
                }
            }
            sw.Stop();
            bestSolution.elapsedMilliseconds = sw.ElapsedMilliseconds;
            return bestSolution;
        }
    }
}
