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

        public GraspSolution SolveGrasp(int rclSize, GraspTypes type)
        {
            GRASP grasp = new GRASP(problem);
            return grasp.Solve(rclSize, type);
        }

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
