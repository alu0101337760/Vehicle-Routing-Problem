namespace DAA_VRP
{
    public class Problem
    {
        public string sourceFilename = "";
        public int numberOfClients = -1;
        public int numberOfVehicles = -1;
        public List<List<int>> distanceMatrix = new List<List<int>>();

        public Problem(string filename)
        {
            BuildFromFile(filename);
        }

        /// <summary>
        /// Utility function to retrieve the number of clients 
        /// from a string
        /// </summary>
        private int RetrieveNumberOfClients(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]) + 1;
        }

        /// <summary>
        /// Utility function to retrieve the number of vehicles
        /// from a string
        /// </summary>
        private int RetrieveNumberOfVehicles(string line)
        {
            string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(split[1]);
        }

        /// <summary>
        /// Utility function to build the distance matrix from
        /// a list of strings
        /// </summary>
        private void buildDistanceMatrix(List<string> lines)
        {
            for (int i = 0; i < numberOfClients; i++)
            {
                string[] row = lines[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                distanceMatrix.Add(new List<int>(Array.ConvertAll<string, int>(row, int.Parse)));

            }
        }

        /// <summary>
        /// Function that builds the VRP class from the contents of 
        /// the file given, it must follow the correct format.
        /// </summary>
        /// <param name="filename"></param>
        public void BuildFromFile(string filename)
        {
            sourceFilename = filename;
            List<string> lines = new List<string>(File.ReadAllLines(filename));
            this.numberOfClients = RetrieveNumberOfClients(lines[0]);
            this.numberOfVehicles = RetrieveNumberOfVehicles(lines[1]);
            buildDistanceMatrix(lines.GetRange(3, lines.Count - 3));
        }

    }
}
