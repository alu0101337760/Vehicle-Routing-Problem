namespace DAA_VRP
{
    public class VRP
    {
        int numberOfClients = -1;
        int numberOfVehicles = -1;
        List<List<int>> distanceMatrix = new List<List<int>>();

        private int RetrieveNumberOfClients(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]);

        }

        private int RetrieveNumberOfVehicles(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]);
        }

        private void buildDistanceMatrix(List<string> lines)
        {
            for (int i = 0; i < numberOfClients; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < numberOfClients; j++)
                {
                    row.Add(int.Parse(lines[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[j]));
                }
                distanceMatrix.Add(row);
            }
        }

        public void BuildFromFile(string filename)
        {
            List<string> lines = new List<string>(File.ReadAllLines(filename));
            this.numberOfClients = RetrieveNumberOfClients(lines[0]);
            this.numberOfVehicles = RetrieveNumberOfVehicles(lines[1]);
            buildDistanceMatrix(lines.GetRange(3, lines.Count - 4));

        }



        public GreedySolution SolveGreedy()
        {
            HashSet<int> nodes = new HashSet<int>(Enumerable.Range(1, numberOfClients).ToList());
            List<List<int>> paths = new List<List<int>>();            

            for (int i = 0; i < numberOfVehicles; i++)
            {
                paths.Add(new List<int>());
            }

            

            throw new NotImplementedException();
        }

        public GreedySolution SolveGrasp()
        {
            throw new NotImplementedException();
        }

    }
}
