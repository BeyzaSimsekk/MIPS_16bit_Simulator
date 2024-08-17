using System.Collections.Generic;

namespace Simülator
{
    public class Registers
    {
        public int r0 { get; set; }
        public int r1 { get; set; }
        public int r2 { get; set; }
        public int r3 { get; set; }
        public int a0 { get; set; }
        public int v0 { get; set; }
        public int sp { get; set; }
        public int ra { get; set; }

        public int pc { get; set; }
        public int hi { get; set; }
        public int lo { get; set; }



        //not: registerBinaryCodes, daha anlaşılır olması adına ileride Dictionary değil de Liste şeklinde de belirtilebilir.
        private readonly Dictionary<string, string> registerBinaryCodes = new Dictionary<string, string>
        {
            { "r0", "000" },
            { "r1", "001" },
            { "r2", "010" },
            { "r3", "011" },
            { "a0", "100" },
            { "v0", "101" },
            { "sp", "110" },
            { "ra", "111" },
        };


        public Registers()
        {
            // Initialize registers with zero
            r0 = 0;
            r1 = 0;
            r2 = 0;
            r3 = 0;
            a0 = 0;
            v0 = 0;
            sp = 256;
            ra = 0;
            hi = 0;
            lo = 0;
        }
        public string GetRegisterBinaryCode(string registerName)
        {
            return registerBinaryCodes.ContainsKey(registerName) ? registerBinaryCodes[registerName] : "000";
        }
    }
}
