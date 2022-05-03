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

        public GvnsSolution SolveGvns(int rclSize, GvnsTypes type = GvnsTypes.VND)
        {
            GVNS gvns = new GVNS(problem);
            return gvns.Solve(rclSize, type);
        }

    }
}
