namespace DAA_VRP
{
    public class GraspSolution : Solution
    {
        public int rclSize = -1;
        
        public GraspSolution(string problemId, int numberOfClients, int rclSize)
        {
            string[] splittedProblemId = problemId.Split('\\');
            this.problemId = splittedProblemId[splittedProblemId.Length - 1];
            this.rclSize = rclSize;
            this.numberOfClients = numberOfClients;
        }   
    }
}
