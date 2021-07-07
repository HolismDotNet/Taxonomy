using Holism.CodeGenerator;

namespace Holism.Taxonomy.CodeGenerator
{
    public class ModelBuilder : ModelGenerator
    {
        public ModelBuilder()
            : base(GeneratorConfig.ConnectionString)
        {
        }

        public override string OutputFolder => @"%HolismProjectsRoot%\Taxonomy\DataAccess\Models\";

        public override string Namespace => @"Holism.Taxonomy.DataAccess.Models";
    }
}
