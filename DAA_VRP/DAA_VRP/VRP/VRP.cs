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


        public GraspSolution SolveGrasp(int rclSize)
        {
            throw new NotImplementedException();
        }

    }
}
