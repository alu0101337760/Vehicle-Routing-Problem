namespace DAA_VRP
{
    public abstract class Solution
    {
        public string problemId = "unspecified";
        public int numberOfClients = -1;
        public int totalDistance = -1;
        public long elapsedMilliseconds = -1;
        public List<List<int>>paths = new List<List<int>>();

        public abstract string GetInfoString();
        public abstract string GetPathsString();

        public void SetPaths(List<List<int>> paths)
        {
            this.paths = paths;
        }

    }
}
