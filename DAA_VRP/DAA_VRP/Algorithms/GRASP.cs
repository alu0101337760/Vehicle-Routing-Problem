using System.Diagnostics;

namespace DAA_VRP
{
    /// <summary>
    /// GraspTypes represent different local searches of the grasp algorithm.
    /// </summary>
    public enum GraspTypes
    {
        GRASP_MULTI_ROUTE_INSERTION,
        GRASP_SINGLE_ROUTE_INSERTION,
        GRASP_SINGLE_ROUTE_SWAP,
        GRASP_MULTI_ROUTE_SWAP,
        GRASP_2_OPT
    };

    /// <summary>
    /// GRASP class implements the GRASP algorithm.
    /// </summary>
    public class GRASP
    {
        Problem problem;
        int numberOfNodes = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="problem">problem to solve with the current instance</param>
        public GRASP(Problem problem)
        {
            this.numberOfNodes = problem.numberOfClients;
            this.distanceMatrix = problem.distanceMatrix;
            this.problem = problem;
        }     


        /// <summary>
        /// Local search implements different local search algorithms for the problem.
        /// </summary>
        /// <param name="solution">Initial solution</param>
        /// <param name="type">The type of local search to use, its default to GRASP_SINGLE_ROUTE_SWAP</param>
        private GraspSolution LocalSearch(GraspSolution solution, GraspTypes type = GraspTypes.GRASP_SINGLE_ROUTE_SWAP)
        {
            switch (type)
            {
                case GraspTypes.GRASP_2_OPT:
                    break;

                case GraspTypes.GRASP_SINGLE_ROUTE_SWAP:
                    return (GraspSolution)new SingleSwap().Search(problem, solution);

                case GraspTypes.GRASP_MULTI_ROUTE_SWAP:
                    return (GraspSolution)new MultiSwap().Search(problem, solution);

                case GraspTypes.GRASP_SINGLE_ROUTE_INSERTION:
                    return (GraspSolution)new SingleInsertion().Search(problem, solution);

                case GraspTypes.GRASP_MULTI_ROUTE_INSERTION:
                    return (GraspSolution)new MultiInsertion().Search(problem, solution);
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

        /// <summary>
        /// Solves the problem with the GRASP algorithm
        /// </summary>
        /// <param name="rclSize">specifies the size of the rcl for the constructive part of the GRASP</param>
        /// <param name="type">A GraspType value for selecting which GRASP local search to use</param>
        public GraspSolution Solve(int rclSize, GraspTypes type)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
            sw.Stop();
            bestSolution.elapsedMilliseconds = sw.ElapsedMilliseconds;
            return bestSolution;
        }
    }
}
