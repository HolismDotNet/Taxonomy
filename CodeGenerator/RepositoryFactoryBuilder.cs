using Holism.CodeGenerator;

namespace Holism.Taxonomy.CodeGenerator
{
    public class RepositoryBuilder : RepositoryGenerator
    {
        public override string OutputFolder => @"%HolismProjectsRoot%\Taxonomy\DataAccess\";

        public override string Namespace => @"Holism.Taxonomy.DataAccess";
    }
}
