

namespace DAA_VRP
{
    public class GRASP
    {
        Problem problem;
        int numberOfClients = -1;
        int numberOfVehicles = -1;
        int totalDistance = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();
        
        enum GraspTypes
        {
            GRASP_REINSERTION_INTRA,
            GRASP_REINSERTION_INTER,
            GRASP_SINGLE_ROUTE_SWAP,
            GRASP_MULTI_ROUTE_SWAP,
            GRASP_2_OPT
        };
        
        public GRASP(Problem problem)
        {
            this.numberOfClients = problem.numberOfClients;
            this.numberOfVehicles = problem.numberOfVehicles;
            this.distanceMatrix = problem.distanceMatrix;
            this.problem = problem;
        }

        private GraspSolution ReinsertPath(List<int> path)
        {

            for (int i = 0; i < path.Count; i++)
            {
                int currentNode = path[i];
                int nextNode = path[i + 1];
                for (int cantidate = nextNode; cantidate < numberOfClients; cantidate++)
                {

                }
            }
            throw new NotImplementedException();
        }

        public GraspSolution GraspReinsertionIntra(GraspSolution solution)
        {
            List<List<int>> paths = solution.paths;
            for (int i = 0; i < paths.Count; i++)
            {
                List<int> currentPath = paths[i];
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
                    break;

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
        
        public GraspSolution Solve(Problem problem, int rclSize)
        {
            GraspSolution solution = new GraspSolution(problem.sourceFilename, problem.numberOfClients, rclSize);
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
