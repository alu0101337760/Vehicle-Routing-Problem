namespace DAA_VRP
{
    public class GvnsSolution : Solution
    {
        public int  rclSize = -1;

        public GvnsSolution(string problemId, int numberOfClients, int rclSize)
        {
            string[] splittedProblemId = problemId.Split('\\');
            this.problemId = splittedProblemId[splittedProblemId.Length - 1];
            this.rclSize = rclSize;
            this.numberOfClients = numberOfClients;
        }

        public override int GetRclSize()
        {
            return this.rclSize;
        }
    }
}
