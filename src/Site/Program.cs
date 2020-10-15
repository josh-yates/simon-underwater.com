using System.Threading.Tasks;
using Statiq.App;

namespace Site
{
    public class Program
    {
        public static async Task<int> Main(string[] args) =>
        await Bootstrapper
            .Factory
            .CreateDefault(args)
            .RunAsync();
    }
}
