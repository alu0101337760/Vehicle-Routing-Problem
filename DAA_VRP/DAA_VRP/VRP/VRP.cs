namespace DAA_VRP
{
    /// <summary>
    /// Class That offers methods to solve the VRP problem.
    /// </summary>
    public class VRP
    {
        Problem problem;

        public VRP(Problem problem)
        {
            this.problem = problem;
        }

        /// <summary>
        /// Function that solves the VRP with a greedy algorithm.
        /// Returs a new GreedySolution object
        /// </summary>
        public GreedySolution SolveGreedy()
        {
            GreedyRCL greedyRCL = new GreedyRCL(problem);
            return greedyRCL.Solve();
        }

        /// <summary>
        /// Function that solves the VRP with a GRASP algorithm
        /// </summary>
        /// <param name="rclSize">The size of the RCL of the constructive phase</param>
        /// <param name="type">The type of local search to be used</param>
        public GraspSolution SolveGrasp(int rclSize, GraspTypes type)
        {
            GRASP grasp = new GRASP(problem);
            return grasp.Solve(rclSize, type);
        }

        /// <summary>
        /// Helper function to calculate the total distance of a given path.
        /// </summary>
        public int CalculateDistance(List<List<int>> paths)
        {
            int distance = 0;
            for (int i = 0; i < paths.Count; i++)
            {
                for (int j = 0; j < paths[i].Count - 1; j++)
                {
                    distance += problem.distanceMatrix[paths[i][j]][paths[i][j + 1]];
                }
            }
            return distance;
        }

    }
}
