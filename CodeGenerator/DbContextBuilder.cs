using Holism.CodeGenerator;

namespace Holism.Taxonomy.CodeGenerator
{
    public class DbContextBuilder : DbContextGenerator
    {
        public DbContextBuilder()
            : base(GeneratorConfig.ConnectionString)
        {
            UsesFixedConnectionString = false;
        }

        public override string OutputFolder => @"%HolismProjectsRoot%\Taxonomy\DataAccess\DbContexts\";

        public override string Namespace => @"Holism.Taxonomy.DataAccess.DbContexts";

        public override string ConnectionStringName => GeneratorConfig.ConnectionStringName;
    }
}
