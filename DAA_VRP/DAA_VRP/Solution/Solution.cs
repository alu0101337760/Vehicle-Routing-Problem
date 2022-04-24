namespace DAA_VRP
{
    public abstract class Solution
    {
        public string problemId = "unspecified";
        public int numberOfClients = -1;
        public int totalDistance = -1;
        public long elapsedMilliseconds = -1;
        public List<int>paths = new List<int>();

        public abstract string GetInfoString();
        public abstract string GetPathsString();

        public void SetPaths(List<List<int>> paths)
        {
            this.paths = paths.SelectMany(x => x.GetRange(0, x.Count - 1)).ToList();
            this.paths.Add(0);
        }

        public void SetPaths(List<int> flattened)
        {
            this.paths = flattened.GetRange(0, flattened.Count - 1); 
        } 
    }
}
