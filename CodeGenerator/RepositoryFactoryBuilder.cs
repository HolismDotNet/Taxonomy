using Holism.CodeGenerator;

namespace Holism.Taxonomy.CodeGenerator
{
    public class RepositoryFactoryBuilder : RepositoryFactoryGenerator
    {
        public override string OutputFolder => @"%HolismProjectsRoot%\Taxonomy\DataAccess\";

        public override string Namespace => @"Holism.Taxonomy.DataAccess";
    }
}
