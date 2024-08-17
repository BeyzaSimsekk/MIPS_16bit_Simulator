using System.Collections.Generic;

public partial class InstructionMap
{
    public class InstructionMemory
    {
        private List<InstructionMemoryEntry> _entries;

        public InstructionMemory()
        {
            _entries = new List<InstructionMemoryEntry>();
        }

        public void AddEntry(string address, string code, string source)
        {
            _entries.Add(new InstructionMemoryEntry { Address = address, Code = code, Source = source });
        }

        public List<InstructionMemoryEntry> GetEntries()
        {
            return _entries;
        }
    }
}
