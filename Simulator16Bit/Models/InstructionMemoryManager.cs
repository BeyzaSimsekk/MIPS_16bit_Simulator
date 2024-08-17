using Simülator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public partial class InstructionMap
{
    public class InstructionMemoryManager
    {
        private Form1 _Form;
        private DataGridView _instructionMemoryDtGrid;
        private InstructionMemory _instructionMemory;
        private readonly Dictionary<string, string> opcodeMap;
        private readonly Dictionary<string, string> functionCodeMap;
        private readonly Registers registers;

        public InstructionMemoryManager(DataGridView dataGridView, Form1 form)
        {
            _Form = form;
            _instructionMemoryDtGrid = dataGridView;
            _instructionMemory = new InstructionMemory();
            opcodeMap = InitializeOpcodeMap();
            functionCodeMap = InitializeFunctionCodeMap();
            registers = new Registers();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            _instructionMemoryDtGrid.ColumnCount = 3;
            _instructionMemoryDtGrid.Columns[0].Name = "Address";
            _instructionMemoryDtGrid.Columns[1].Name = "Code";
            _instructionMemoryDtGrid.Columns[2].Name = "Source";
        }

        public void AddEntry(string address, string code, string source)
        {
            _instructionMemory.AddEntry(address, code, source);
            UpdateDataGridView();
        }

        public void UpdateDataGridView()
        {
            _instructionMemoryDtGrid.Rows.Clear();
            foreach (var entry in _instructionMemory.GetEntries())
            {
                _instructionMemoryDtGrid.Rows.Add(entry.Address, entry.Code, entry.Source);
            }
        }

        private Dictionary<string, string> InitializeOpcodeMap()
        {
            return new Dictionary<string, string>
            {
                //R-Types
                #region 16 Bitte Kullanılmayanlar
                { "nor",  "0000" },
                { "sra" , "0000" },
                { "mfhi" , "0000" },
                { "mflo" ,  "0000"},
                { "mult" , "0000" },
                { "div" ,  "0000"},
	            #endregion

                { "add", "0000" },
                { "sub", "0000" },
                { "and", "0000" },
                { "or",  "0000" },
                { "xor" , "0000" },
                { "slt", "0000" },
                { "jr" , "0000" },
                

                //I-Types
                { "lw",  "0001" },
                { "sw",  "0010" },
                { "beq", "0011" },
                { "bne" , "0100" },
                { "addi" , "0101" },
                { "andi" , "0110" },
                { "ori" , "0111" },
                { "slti" , "1000" },
                { "lui",  "1101" },
                

                //J-Types
                { "j",   "1011" },
                { "jal" , "1100" },


                //S-Types
                { "sll" , "1001" },
                { "srl" , "1010" },

            };
        }

        private Dictionary<string, string> InitializeFunctionCodeMap()
        {
            return new Dictionary<string, string>
            {
                { "add", "000" },
                { "sub", "001" },
                { "and", "010" },
                { "or",  "011" },
                { "xor",  "100" },
                { "slt", "101" },
                { "jr",  "110" },
                { "mult",  "111" },
                
                ///1 fonksiyon için yer var: 111
                ///
                ///{ "nor",  "101" },
                ///{ "sra",  "110" },
                ///{ "mult",  "101" },
                ///{ "div",  "101" },
            };

        }

        public void Assembler(int pc, string line)
        {
            string code = string.Empty;

            var parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var instruction = parts[0];

            if (opcodeMap.ContainsKey(instruction))
            {
                var opcode = opcodeMap[instruction];

                switch (instruction)
                {
                    case "add":
                    case "sub":
                    case "and":
                    case "or":
                    case "xor":
                    case "slt":
                    case "mult":
                    //case "div":
                        code = $"{opcode}{registers.GetRegisterBinaryCode(parts[3].Replace("$", ""))}{registers.GetRegisterBinaryCode(parts[2].Replace("$", ""))}{registers.GetRegisterBinaryCode(parts[1].Replace("$", ""))}{functionCodeMap[instruction]}";
                        break;
                    case "jr":
                        code = $"{opcode}{(registers.GetRegisterBinaryCode(parts[1].Replace("$", "")))}000000{functionCodeMap[instruction]}";
                        break;
                    //case "mfhi":
                    //case "mflo":
                        //code = $"{opcode}000000{registers.GetRegisterBinaryCode(parts[1].Replace("$", ""))}{functionCodeMap[instruction]}";
                        //break;
                    case "sll":
                    case "srl":
                    //case "sra":
                        code = $"{opcode}{registers.GetRegisterBinaryCode(parts[2].Replace("$", ""))}{registers.GetRegisterBinaryCode(parts[1].Replace("$", ""))}{_Form.GetRegisterValue(parts[3].Replace("$", "").PadLeft(6,'0'))}";
                        break;
                    ///SHIFT AMOUNT KISMI: {""}
                    ///
                    /// 
                    ///örnek input:
                    ///addi $t1, $zero, 1
                    ///addi $t2, $zero, -20
                    ///sll $s0, $t0, $t1
                    ///srl $s1, $t0, $t1
                    ///sra $s3, $t2, $t1
                    ///Exit:
                    ///
                    /// 
                    ///bu örnekteki $t1'ler shift amount olarak geçiyor. öncesinde yapılan addi işlemleri ile içine değer atanan
                    ///bu $t1 register ının içindeki değer alınmalı. Form1.cs de tanımlanan shift operasyonları yapılmalı. Bu yüzden
                    ///Registers.cs de tanımlanan GetRegisterBinaryCode() fonksyionu işe yaramaz. ÇÜnkü o fonksiyon register ın içinde
                    ///tuttuğu değeri değil, değişmeyen makine kodunu çağırıyor.
                    ///

                    case "beq":
                    case "bne":
                        int a = _Form._Labels.FirstOrDefault(x => x.Name == parts[3]).PC;
                        string binary1 = Convert.ToString(a, 2);
                        code = $"{opcode}{registers.GetRegisterBinaryCode(parts[2].Replace("$", ""))}{registers.GetRegisterBinaryCode(parts[1].Replace("$", ""))}{binary1}";
                        break;

                    case "addi":
                    case "slti":
                    case "andi":
                    case "ori":
                        int immediateValue = int.Parse(parts[3]);
                        // Negatif sayıların binary değerini hesapla
                        string immediateBinary = Convert.ToString(immediateValue & 0x3F, 2); // 6 bit olarak sınırla
                        code = $"{opcode}{registers.GetRegisterBinaryCode(parts[2].Replace("$", ""))}{registers.GetRegisterBinaryCode(parts[1].Replace("$", ""))}{immediateBinary.PadLeft(6, '0')}";
                        break;
                    case "lui":
                        int luiImmediateValue = int.Parse(parts[2]);
                        string luiImmediateBinary = Convert.ToString(luiImmediateValue & 0x3F, 2); // 6 bit olarak sınırla
                        code = $"{opcode}000{registers.GetRegisterBinaryCode(parts[1].Replace("$", ""))}{luiImmediateBinary}";
                        break;
                    case "lw":
                    case "sw":
                        var offsetParts = parts[2].Trim(')').Replace("$", "").Split('(');
                        code = $"{opcode}{registers.GetRegisterBinaryCode(parts[1].Replace("$", ""))}{Convert.ToString(int.Parse(offsetParts[0]), 2).PadLeft(6, '0')}{registers.GetRegisterBinaryCode(offsetParts[1].Replace("$", ""))}";
                        break;
                    case "j":
                    case "jal":
                        int b = _Form._Labels.FirstOrDefault(x => x.Name == parts[1]).PC;
                        string binary2 = Convert.ToString(b, 2);
                        code = $"{opcode}{binary2}";
                        break;
                        ///LABELLAR İÇİN ADRES TUTMASI/ALMASI
                        /// 
                        /// örnek input: 
                        ///                             INSTRUCTION MEMORYDEKİ ADRESLERİ
                        ///addi $t0, $zero, 15          //address in decimal: 0
                        ///addi $t1, $zero, 25          //address in decimal: 4
                        ///jal Loop                     //address in decimal: 8
                        ///Loop:
                        ///slt $s0, $t1, $t0            //address in decimal: 12
                        ///addi $t0, $t0, 5             //address in decimal: 16
                        ///beq $s0, $zero, Loop         //address in decimal: 20
                        ///addi $t0, $zero, 15          //address in decimal: 24
                        ///addi $t1, $zero, 25          //address in decimal: 28
                        ///Loop2:
                        ///slt $t2, $t0, $t1            //address in decimal: 32
                        ///addi $t0, $t0, 5             //address in decimal: 36
                        ///bne $t2, $zero, Loop2        //address in decimal: 40
                        ///addi $t3, $zero, 1           //address in decimal: 44
                        ///addi $t4, $t4, 1             //address in decimal: 48
                        ///slt $t5, $t3, $t4            //address in decimal: 52  
                        ///bne $t5, $zero, LastLabel    //address in decimal: 56
                        ///jr $ra                       //address in decimal: 60
                        ///LastLabel:
                        ///addi $t0, $zero, 15          //address in decimal: 64
                        ///j Exit                       //address in decimal: 68
                        ///addi $t1, $zero, 25          //address in decimal: 72
                        ///Exit:
                        ///
                        /// 
                        /// bu kısımda ikinci {} öge için öyle bir şey tanımlanmalı ki:
                        ///ilk jal okunduktan sonra ikinci kısım için Loop'un içindeki ilk komutun adresini almalı. 
                        ///Bu örnekte decimal olarak "12" hexadecimal olarak "c".
                }

                string hexValue = string.Empty;
                if (!string.IsNullOrEmpty(code))
                {
                    int partValueInt = Convert.ToInt32(code, 2);
                    hexValue = $"0x{partValueInt:X8}";
                }

                AddEntry($"0x{pc.ToString("X8")}", hexValue, line);
            }
            else
            {
                throw new NotSupportedException($"Komut '{instruction}' desteklenmiyor.");
            }

        }

    }
}
