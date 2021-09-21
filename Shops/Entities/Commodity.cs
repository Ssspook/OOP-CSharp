using Shops.Tools;

namespace Shops.Entities
{
    public class Commodity
    {
        private static uint id = 0;
        public Commodity(string name)
        {
            if (name == null)
                throw new ShopsException("Commodity name cannot be null");

            id++;
            Id = id;
            Name = name;
        }

        public string Name { get; }
        public uint Id { get; }
    }
}
