namespace DAA_VRP
{
    public interface ILocalSearch
    {
        Solution Search(Problem problem, Solution solution, bool shaking = false);
    }
}
