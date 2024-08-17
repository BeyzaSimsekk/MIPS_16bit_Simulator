namespace Simülator
{

    // MemoryMainModel sınıfı
    public class MemoryMainModel
    {
        public int Location { get; }
        public MemoryPartModel Part0 { get; } = new MemoryPartModel();

        public MemoryMainModel(int location)
        {
            Location = location;
        }
    }
}
