using Holism.Framework;

namespace Holism.Taxonomy.CodeGenerator
{
    public class GeneratorConfig : Config
    {
        public static string ConnectionString
        {
            get
            {
                var connectionString = GetConnectionString(ConnectionStringName);
                return connectionString;
            }
        }

        public const string ConnectionStringName = "Taxonomy";
    }
}
