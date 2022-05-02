namespace DAA_VRP
{
    public interface ILocalSearch
    {
        Solution Search(Problem problem, Solution solution);
        Solution Shake(Problem problem, Solution solution);
    }
}
