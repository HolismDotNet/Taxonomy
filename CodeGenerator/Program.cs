namespace Holism.Taxonomy.CodeGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var modelBuilder = new ModelBuilder();
            modelBuilder.Generate();
            modelBuilder.SaveModels();

            var contextBuilder = new DbContextBuilder();
            contextBuilder.Generate();
            contextBuilder.SaveDbContexts();

            var repositoryBuilder = new RepositoryBuilder();
            repositoryBuilder.Generate();
            repositoryBuilder.SaveRepositories();

            var RepositoryBuilder = new RepositoryBuilder();
            RepositoryBuilder.Generate(repositoryBuilder.Tables);
            RepositoryBuilder.Save();
        }
    }
}
