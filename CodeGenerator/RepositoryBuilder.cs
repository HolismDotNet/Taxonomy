using Holism.CodeGenerator;

namespace Holism.Taxonomy.CodeGenerator
{
    public class RepositoryBuilder : RepositoryGenerator
    {
        public RepositoryBuilder()
            : base(GeneratorConfig.ConnectionString)
        {
        }

        public override string OutputFolder => @"%HolismProjectsRoot%\Taxonomy\DataAccess\Repositories\";

        public override string Namespace => @"Holism.Taxonomy.DataAccess.Repositories";
    }
}
