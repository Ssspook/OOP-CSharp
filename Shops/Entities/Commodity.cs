namespace Shops.Entities
{
    public class Commodity
    {
        private static uint id = 0;
        public Commodity(string name)
        {
            id++;
            Id = id;
            Name = name;
        }

        public string Name { get; }
        public uint Id { get; }
    }
}
