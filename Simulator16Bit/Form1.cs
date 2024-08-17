using Simülator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using static InstructionMap;


namespace Simülator
{
    public partial class Form1 : Form
    {


        private readonly Registers _registers = new Registers();
        public List<LabelWithPC> _Labels = new List<LabelWithPC>();
        private InstructionMemoryManager _instructionMemoryManager;
        private List<PcWithLine> _PcWithLines = new List<PcWithLine>();
        private int currentLineIndex = 0;
        private int raIndex = 0;

        private int hi;
        private int lo;

        private Dictionary<string, Action<string, string, string>> algoMap = new Dictionary<string, Action<string, string, string>>();
        private Dictionary<string, Action<string, string>> memoryMap = new Dictionary<string, Action<string, string>>();
        private Dictionary<string, Action<string>> jumpMap = new Dictionary<string, Action<string>>();
        private readonly Dictionary<string, string> opcodeMap = new Dictionary<string, string>();


        public Form1()
        {
            InitializeComponent();
            InitializeInstructionMap();


            List<int> registerValues = new List<int>(); // Burada registerValues listesini uygun değerlerle doldurmanız gerekiyor
            AddDataToDataGridView(dtgrid_register, registerValues);
            _instructionMemoryManager = new InstructionMemoryManager(dtgrid_instMem, this);
            InitializeMemoryDataGridView();


        }
        private void InitializeInstructionMap()
        {
            #region Algorithmic Operations
            // Instruction map initialization
            algoMap["add"] = PerformAddOperation;
            algoMap["sub"] = PerformSubOperation;
            algoMap["and"] = PerformAndOperation;
            algoMap["or"] = PerformOrOperation;
            algoMap["xor"] = PerformXorOperation;
            algoMap["slt"] = PerformSltOperation;
            algoMap["sll"] = PerformSllOperation;
            algoMap["srl"] = PerformSrlOperation;
            algoMap["sra"] = PerformSraOperation;
            algoMap["mult"] = PerformMulOperation;
            algoMap["div"] = PerformDivOperation;
            jumpMap["mfhi"] = PerformMfhiOperation;
            jumpMap["mflo"] = PerformMfloOperation;

            algoMap["beq"] = PerformBeqOperation;
            algoMap["bne"] = PerformBneOperation;
            algoMap["addi"] = PerformAddiOperation;
            algoMap["slti"] = PerformSltiOperation;
            algoMap["andi"] = PerformAndiOperation;
            algoMap["ori"] = PerformOriOperation;
            algoMap["muli"] = PerformMuliOperation;
            #endregion

            #region Memory Operations
            memoryMap["lui"] = PerformLuiOperation;
            memoryMap["move"] = PerformMoveOperation;

            //HATALI: Memory işlemlerimiz doğru çalışmıyor.
            memoryMap["lw"] = PerformLwOperation;
            memoryMap["sw"] = PerformSwOperation;
            #endregion

            #region Jump Operations

            jumpMap["j"] = PerformJumpOperation;
            jumpMap["jr"] = PerformJrOperation;
            jumpMap["jal"] = PerformJalOperation;

            #endregion

            #region Opcodes of Instructions

            opcodeMap["add"] = "000000";
            opcodeMap["sub"] = "000000";
            opcodeMap["and"] = "000000";
            opcodeMap["or"] = "000000";
            opcodeMap["xor"] = "000000";
            opcodeMap["slt"] = "000000";
            opcodeMap["jr"] = "000000";
            opcodeMap["sll"] = "000000";
            opcodeMap["srl"] = "000000";
            opcodeMap["sra"] = "000000";
            opcodeMap["mfhi"] = "000000";
            opcodeMap["mflo"] = "000000";
            opcodeMap["mult"] = "000000";
            opcodeMap["div"] = "000000";

            opcodeMap["beq"] = "000100";
            opcodeMap["bne"] = "000101";
            opcodeMap["addi"] = "001000";
            opcodeMap["slti"] = "001010";
            opcodeMap["andi"] = "001100";
            opcodeMap["ori"] = "001101";
            opcodeMap["lui"] = "001111";
            opcodeMap["lw"] = "100011";
            opcodeMap["lb"] = "100000";
            opcodeMap["sw"] = "101011";
            opcodeMap["sb"] = "101000";

            opcodeMap["j"] = "000010";
            opcodeMap["jal"] = "000011";

            #endregion


        }


        private void AddDataToDataGridView(DataGridView dataGridView, List<int> registerValues)
        {
            int counter = 0;
            foreach (var property in typeof(Registers).GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    int rowIndex = dataGridView.Rows.Add(property.Name);
                    dataGridView.Rows[rowIndex].Cells[1].Value = counter++; // Increment counter for each row

                    // Check if the property is "sp" to set its initial value
                    if (property.Name == "sp")
                    {
                        dataGridView.Rows[rowIndex].Cells[2].Value = "0x00000100"; // Initial value for sp in hex
                    }
                    else
                    {
                        dataGridView.Rows[rowIndex].Cells[2].Value = "0x00000000"; // Initial value for other registers
                    }
                }
            }

            foreach (int value in registerValues)
            {
                DataGridViewRow row = new DataGridViewRow();
                foreach (var property in typeof(Registers).GetProperties())
                {
                    if (property.PropertyType == typeof(int))
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell
                        {
                            Value = property.GetValue(_registers)
                        });
                    }
                }
                dataGridView.Rows.Add(row);
            }
        }

        private void UpdateRegisterHexValue(string registerName, int value)
        {
            // DataGridView'deki ilgili register hücresini güncelle
            foreach (DataGridViewRow row in dtgrid_register.Rows)
            {
                if (row.Cells[0].Value.ToString() == registerName)
                {
                    row.Cells[2].Value = $"0x{value.ToString("X8")}";
                    break;
                }
            }
        }

        private void InitializeMemoryDataGridView()
        {
            // Bellek konumlarını DataGridView'e ekle
            foreach (var mem in MainViewModel.Instance.MemoriesVM.Memories)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dtgrid_memory);
                row.Cells[0].Value = $"0x{mem.Location:X8}";
                row.Cells[1].Value = $"0x{mem.Part0.PartValue:X8}";
                //row.Cells[2].Value = $"0x{mem.Part4.PartValue:X8}";
                //row.Cells[3].Value = $"0x{mem.Part8.PartValue:X8}";
                //row.Cells[4].Value = $"0x{mem.Part12.PartValue:X8}";
                //row.Cells[5].Value = $"0x{mem.Part16.PartValue:X8}";
                //row.Cells[6].Value = $"0x{mem.Part20.PartValue:X8}";
                //row.Cells[7].Value = $"0x{mem.Part24.PartValue:X8}";
                //row.Cells[8].Value = $"0x{mem.Part28.PartValue:X8}";
                dtgrid_memory.Rows.Add(row);
            }

        }


        public int GetRegisterValue(string registerName)
        {
            var propertyInfo = typeof(Registers).GetProperty(registerName);
            if (propertyInfo != null)
            {
                return (int)propertyInfo.GetValue(_registers);
            }
            else if (int.TryParse(registerName, out int constantValue))
            {
                return constantValue;
            }
            else
            {
                return 0; // Varsayılan değeri döndürün
            }
        }

        private void SetRegisterValue(string registerName, int value)
        {
            var propertyInfo = typeof(Registers).GetProperty(registerName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(_registers, value);
                // Register hexadecimal değerini güncelle
                UpdateRegisterHexValue(registerName, value);
            }
            else
            {
                // Kayıt adı geçersiz olduğunda kullanıcıya bir hata mesajı göster
                Console.WriteLine($"Error: Invalid register name '{registerName}'");
                // veya alternatif bir hata işlemi gerçekleştir
            }
        }

        public void UpdateDataGridView(string registerName, int value)
        {
            // DataGridView'deki ilgili register hücresini güncelle
            foreach (DataGridViewRow row in dtgrid_register.Rows)
            {
                if (row.Cells[0].Value.ToString() == registerName)
                {
                    row.Cells[1].Value = value;
                    break;
                }
            }
        }

        private void UpdateHiLoRegisters()
        {
            UpdateRegisterHexValue("hi", hi);
            UpdateRegisterHexValue("lo", lo);
        }

        #region Operations

        private void PerformAddOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);
            int result = value1 + value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformSubOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);
            int result = value1 - value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformAddiOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = int.Parse(registerParse3);

            int result = value1 + value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformAndOperation(string registerParse1, string registerParse2, string registerParse3)
        {

            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);

            if (value1 == value2)
            {
                // Eğer eşitse, sonucu 1 olarak ayarla
                txt_output.Text = "1";
                SetRegisterValue(registerParse1, 1);
            }
            else
            {
                // Eğer eşit değilse, sonucu 0 olarak ayarla
                txt_output.Text = "0";
                SetRegisterValue(registerParse1, 0);
            }
        }



        private void PerformOrOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);

            // İki değeri bitwise OR işlemine tabi tut
            int result = value1 | value2;

            SetRegisterValue(registerParse1, result);

            txt_output.Text = result.ToString();
        }



        private void PerformXorOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);

            // İki değeri XOR işlemine tabi tut
            int result = value1 ^ value2;  //int result = (value1 == value2 || value1 == value2 == 0 || value1 == value2 == 1) ? 0 : 1;

            SetRegisterValue(registerParse1, result);

            txt_output.Text = result.ToString();
        }

        private void PerformAndiOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = int.Parse(registerParse3);

            int result = value1 & value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformOriOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = int.Parse(registerParse3);

            int result = value1 | value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformMulOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);
            long result = (long)value1 * value2;
            lo = (int)(result & 0xFFFFFFFF);
            hi = (int)(result >> 32);
            SetRegisterValue(registerParse1, lo);
            txt_output.Text = lo.ToString();
            UpdateHiLoRegisters(); // Update hi and lo registers in the DataGridView
        }

        private void PerformDivOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);
            if (value2 != 0)
            {
                lo = value1 / value2;
                hi = value1 % value2;
                SetRegisterValue(registerParse1, lo);
                txt_output.Text = lo.ToString();
                UpdateHiLoRegisters(); // Update hi and lo registers in the DataGridView
            }
            else
            {
                txt_output.Text = "Division by zero error";
            }
        }

        private void PerformMfhiOperation(string registerParse1)
        {
            // hi kaydındaki değeri hedef kayda kopyala
            SetRegisterValue(registerParse1, hi);
        }

        private void PerformMfloOperation(string registerParse1)
        {
            // lo kaydındaki değeri hedef kayda kopyala
            SetRegisterValue(registerParse1, lo);
        }

        private void PerformMuliOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = int.Parse(registerParse3);

            int result = value1 * value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformLuiOperation(string registerParse1, string immediateValue)
        {
            int value = Convert.ToInt32(immediateValue, 16); // String'i onaltılık integer'a çevirme
            int result = value << 16; // Immediate değeri sola kaydırarak üst 16 biti belirtilen değere dönüştürür
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformSllOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);

            int result = value1 << value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformSrlOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);

            int result = value1 >> value2;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformSraOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);

            int result = value1 >> value2;
            // İşaret bitini korumak için aritmetik sağa kaydırma işlemi
            if (value1 < 0)
            {
                result |= (-1 << (32 - value2));
            }
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformSltOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = GetRegisterValue(registerParse3);

            int result = value1 < value2 ? 1 : 0;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformSltiOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse2);
            int value2 = int.Parse(registerParse3);

            int result = value1 < value2 ? 1 : 0;
            SetRegisterValue(registerParse1, result);
            txt_output.Text = result.ToString();
        }

        private void PerformMoveOperation(string destinationRegister, string sourceRegister)
        {
            int value = GetRegisterValue(sourceRegister);
            SetRegisterValue(destinationRegister, value);
            txt_output.Text = value.ToString();
        }

        private void PerformBeqOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse1);
            int value2 = GetRegisterValue(registerParse2);

            if (registerParse3 == "exit")
            {
                var label = _Labels.FirstOrDefault(x => x.Name.Equals(registerParse3));
                currentLineIndex = _PcWithLines.FirstOrDefault(x => x.PC == label.PC - 2).CommandLine;
                SetProgramCounter(currentLineIndex); // Program sayacını son PC değerinin 2 fazlası olarak ayarla
            }
            else
            {
                if (value1 == value2)
                {
                    var label = _Labels.FirstOrDefault(x => x.Name.Equals(registerParse3));
                    currentLineIndex = _PcWithLines.FirstOrDefault(x => x.PC == label.PC).CommandLine - 1;
                }
            }
            
        }

        private void PerformBneOperation(string registerParse1, string registerParse2, string registerParse3)
        {
            int value1 = GetRegisterValue(registerParse1);
            int value2 = GetRegisterValue(registerParse2);

            if (registerParse3 == "exit")
            {
                var label = _Labels.FirstOrDefault(x => x.Name.Equals(registerParse3));
                currentLineIndex = _PcWithLines.FirstOrDefault(x => x.PC == label.PC - 2).CommandLine;
                SetProgramCounter(currentLineIndex); // Program sayacını son PC değerinin 2 fazlası olarak ayarla
            }
            else
            {
                if (value1 != value2)
                {
                    var label = _Labels.FirstOrDefault(x => x.Name.Equals(registerParse3));
                    currentLineIndex = _PcWithLines.FirstOrDefault(x => x.PC == label.PC).CommandLine - 1;
                }
            }

           
        }

        private void PerformLwOperation(string targetRegister, string memoryAccess)
        {
            string[] parts = memoryAccess.Split(new char[] { '(', ')' });

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid memory access format");
            }

            string offsetStr = parts[0].Trim();
            string baseRegister = parts[1].Trim();

            if (!int.TryParse(offsetStr, out int offset))
            {
                throw new ArgumentException($"Invalid offset value: {offsetStr}");
            }

            // Base register kontrolü
            if (baseRegister.StartsWith("$"))
            {
                baseRegister = baseRegister.Substring(1); // $ işaretini kaldır
            }

            if (!IsValidRegister(baseRegister))
            {
                throw new ArgumentException($"Invalid base register: {baseRegister}");
            }

            int baseAddress = GetRegisterValue("pc"); // PC değerini base adres olarak kullan

            // Bellek adresini hesapla
            int memoryAddress = baseAddress + offset;
            txt_output.Text = $"Memory address: 0x{memoryAddress:X8}";
        }



        private void PerformSwOperation(string registerParse1, string registerParse2)
        {
            // Bellek adresini elde etmek için gerekli olan ayrıştırma işlemi
            string[] parts = registerParse2.Split(new char[] { '(', ')' });

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid memory access format");
            }

            string offsetStr = parts[0].Trim();
            string baseRegister = parts[1].Trim();

            // Offset değerini kontrol et
            if (!int.TryParse(offsetStr, out int offset))
            {
                throw new ArgumentException($"Invalid offset value: {offsetStr}");
            }

            // Base register kontrolü
            if (baseRegister.StartsWith("$"))
            {
                baseRegister = baseRegister.Substring(1); // $ işaretini kaldır
            }

            // Base adresi al
            int baseAddress = GetRegisterValue(baseRegister);

            // Bellek adresini hesapla
            int memoryAddress = baseAddress + offset;

            txt_output.Text = $"Memory address: 0x{memoryAddress:X8}";

            // Belleğe yazma işlemi
            int valueToStore = GetRegisterValue(registerParse1);
            byte[] bytesToStore = BitConverter.GetBytes(valueToStore);
            MainViewModel.Instance.MemoriesVM.StoreWord((uint)memoryAddress, bytesToStore);
        }



        private void PerformJumpOperation(string targetAddressStr)
        {
            if (!int.TryParse(targetAddressStr, out int targetAddress))
            {
                throw new ArgumentException($"Invalid target address value: {targetAddressStr}");
            }

            txt_output.Text = $"Jumped to address: 0x{targetAddress:X8}";
        }



        private void PerformJrOperation(string baseRegister)
        {
            if (!IsValidRegister(baseRegister))
            {
                throw new ArgumentException($"Invalid base register: {baseRegister}");
            }

            int targetAddress = GetRegisterValue(baseRegister);
            txt_output.Text = $"Jumped to address: 0x{targetAddress:X8}";
        }



        private void PerformJalOperation(string targetAddressStr)
        {
            if (!int.TryParse(targetAddressStr, out int targetAddress))
            {
                throw new ArgumentException($"Invalid target address value: {targetAddressStr}");
            }

            int returnAddress = GetProgramCounter() + 4; // Şu anki PC + 4
            SetRegisterValue("ra", returnAddress); // Dönüş adresini $ra kaydına ayarla
            txt_output.Text = $"Jumped to address: 0x{targetAddress:X8}, return address set to: 0x{returnAddress:X8}";
        }

        #endregion


        private int GetProgramCounter()
        {
            // Program sayacını döndürmek için "pc" kaydını kullanıyoruz
            return GetRegisterValue("pc");
        }

        private void SetProgramCounter(int lineIndex)
        {
            string pcStr = dtgrid_instMem[0, lineIndex].Value.ToString().Trim();
            int pc = Convert.ToInt32(pcStr, 16) + 2;

            SetRegisterValue("pc", pc);
            UpdateDataGridView("pc", pc);
        }


        private bool IsValidRegister(string registerName)
        {
            var propertyInfo = typeof(Registers).GetProperty(registerName);
            return propertyInfo != null;
        }

        private void UpdateMemoryDataGridView(MemoryPartModel memoryLocation)
        {
            // Bellek DataGridView'inde ilgili hücreyi güncelle
            foreach (DataGridViewRow row in dtgrid_memory.Rows)
            {
                if (row.Cells[0].Value.ToString() == $"0x{memoryLocation.Location:X8}")
                {
                    // Bellek hücresinin değerini güncelle
                    int columnIndex = memoryLocation.Index;
                    row.Cells[columnIndex].Value = $"0x{memoryLocation.PartValue:X8}";
                    break;
                }
            }
        }

        private string GetMemoryDataGridView(MemoryPartModel memoryLocation)
        {
            // Bellek DataGridView'inde ilgili hücreyi güncelle
            foreach (DataGridViewRow row in dtgrid_memory.Rows)
            {
                if (row.Cells[0].Value.ToString() == $"0x{memoryLocation.Location:X8}")
                {
                    // Bellek hücresinin değerini güncelle
                    int columnIndex = memoryLocation.Index;
                    return row.Cells[columnIndex].Value.ToString();
                }
            }
            return string.Empty;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            ProcessAllLines();
        }

        private void btn_line_Click(object sender, EventArgs e)
        {
            if (currentLineIndex < dtgrid_instMem.Rows.Count)
            {
                ProcessLine(currentLineIndex);
                currentLineIndex++;
            }
        }

        /// <summary>
        /// Run Step tanımlaması döngüye alınarak Run için tanımlama yapıldı.
        /// </summary>
        private void ProcessAllLines()
        {
            int maxRowCount = dtgrid_instMem.Rows.Count;
            while (currentLineIndex + 1 <= maxRowCount)
            {
                ProcessLine(currentLineIndex);
                currentLineIndex++;
            }
            btn_start.Enabled = false;
        }


        private void ProcessLine(int lineIndex)
        {
            if (lineIndex + 1 >= dtgrid_instMem.Rows.Count)
            {
                btn_line.Enabled = false;
                return;
            }
            string line = dtgrid_instMem[2, lineIndex].Value.ToString().Trim();

            if (string.IsNullOrEmpty(line))
            {
                MessageBox.Show($"Line {lineIndex + 1} is invalid. No space found.");
                return;
            }

            ///Komutların parçalanmasına göre durumlar oluşturuldu.
            ///4 parçaya ayrılıyorsa algomap operasyonları gerçekleştiriliyor. ("add" "$t0" "$zero" "$t1" )
            ///3 parçaya ayrılıyorsa memorymap operasyonları gerçekleştiriliyor. ("lw" "$t0" "[0] [zero]"  )
            ///2 parçaya ayrılıyorsa jumpmap operasyonları gerçekleştiriliyor. ("jal" "Loop" )

            string[] splits = line.Split(' ');

            if (splits.Length == 4)
            {
                if (algoMap.ContainsKey(splits[0]))
                {
                    var destinationRegister = splits[1].Replace(",", "").Replace("$", "");
                    var targetRegister = splits[2].Replace(",", "").Replace("$", "");
                    var sourceRegister = splits[3].Replace("$", "");
                    var operation = algoMap[splits[0]];
                    operation(destinationRegister, targetRegister, sourceRegister);
                }
                SetProgramCounter(currentLineIndex);
            }
            else if (splits.Length == 3)
            {
                if (memoryMap.ContainsKey(splits[0]))
                {
                    var targetRegister = splits[1].Replace(",", "").Replace("$", "");
                    var sourceRegister = splits[2].Replace("($", " ").Replace(")", "").Split(' ');

                    if (splits[0] == "sw")
                    {
                        MemoryPartModel memoryPartModel = new MemoryPartModel
                        {
                            PartValue = GetRegisterValue(targetRegister),
                            Location = int.Parse(sourceRegister[0]) + GetRegisterValue(sourceRegister[1]), 
                            Index = 1,//$zero
                        };
                        UpdateMemoryDataGridView(memoryPartModel);
                    }
                    else if (splits[0] == "lw")
                    {
                        MemoryPartModel memoryPartModel = new MemoryPartModel
                        {
                            Location = int.Parse(sourceRegister[0]) + GetRegisterValue(sourceRegister[1]),
                            Index = 1, //$zero
                        };
                        string result = GetMemoryDataGridView(memoryPartModel).Substring(2);
                        int intValue = Convert.ToInt32(result, 16);
                        SetRegisterValue(targetRegister, intValue);
                    }
                    else if (splits[0] == "lui")
                    {
                        // PerformLuiOperation metodunu çağırmak için
                        PerformLuiOperation(targetRegister, sourceRegister[0]);
                    }
                }
                SetProgramCounter(currentLineIndex);
            }
            else if (splits.Length == 2)
            {
                if (jumpMap.ContainsKey(splits[0]))
                {
                    if (splits[0] == "jr")
                    {
                        int ra = GetRegisterValue("ra");
                        currentLineIndex = _PcWithLines.FirstOrDefault(x => x.PC == ra).CommandLine - 1;
                    }
                    else if (splits[0] == "mfhi")
                    {
                        var destinationRegister = splits[1].Replace(",", "").Replace("$", "");
                        var operation = jumpMap[splits[0]];
                        operation(destinationRegister);

                    }
                    else if (splits[0] == "mflo")
                    {
                        var destinationRegister = splits[1].Replace(",", "").Replace("$", "");
                        var operation = jumpMap[splits[0]];
                        operation(destinationRegister);

                    }
                    else if (splits[0] == "j")
                    {
                        var label = _Labels.FirstOrDefault(x => x.Name.Equals(splits[1]));
                        currentLineIndex = _PcWithLines.FirstOrDefault(x => x.PC == label.PC - 2).CommandLine;
                    }
                    else if (splits[0] == "jal")
                    {
                        var label = _Labels.FirstOrDefault(x => x.Name.Equals(splits[1]));
                        currentLineIndex = _PcWithLines.FirstOrDefault(x => x.PC == label.PC - 2).CommandLine;
                        SetRegisterValue("ra", GetProgramCounter() + 2);
                    }
                    SetProgramCounter(currentLineIndex);
                }
            }
            else if (splits.Length == 1)
            {
            }
        }



        #region Clear Operations
        private void btn_clear_Click(object sender, EventArgs e)
        {
            // Register tablosundaki değerleri sıfırla
            ClearRegisterValues();

            // Input alanını temizle
            ClearInputArea();

            // Instruction memory satırlarını temizle (başlıkları hariç)
            ClearInstructionMemory();

            // Data memory'de value'ları sıfırla
            ClearDataMemoryValues();

            btnAssembly.Enabled = true;
        }

        private void ClearRegisterValues()
        {
            // Register tablosundaki değerleri sıfırla
            foreach (DataGridViewRow row in dtgrid_register.Rows)
            {
                row.Cells[2].Value = "0x00000000";
            }
        }

        private void ClearInputArea()
        {
            // Input alanını temizle
            // (Eğer bir TextBox kullanıyorsanız)
            txt_input.Text = string.Empty;

            // (Eğer başka bir kontrol kullanıyorsanız, o kontrolün temizlenmesi gerekmektedir.)
        }

        private void ClearInstructionMemory()
        {
            // Instruction memory satırlarını temizle (başlıkları hariç)
            for (int i = 1; i < dtgrid_instMem.Rows.Count; i++) // Başlık satırını atlamak için i = 1'den başlatıyoruz
            {
                if (i == 1)
                {
                    // İlk satırı temizle
                    for (int j = 0; j < dtgrid_instMem.Columns.Count; j++)
                    {
                        dtgrid_instMem.Rows[i].Cells[j].Value = string.Empty;
                    }
                }
                else
                {
                    // Diğer satırları temizle
                    for (int j = 0; j < dtgrid_instMem.Columns.Count; j++)
                    {
                        dtgrid_instMem.Rows[i].Cells[j].Value = string.Empty;
                    }
                }
            }
        }

        private void ClearDataMemoryValues()
        {
            // Data memory'de value'ları sıfırla
            foreach (DataGridViewRow row in dtgrid_memory.Rows)
            {
                for (int i = 1; i < row.Cells.Count; i++)
                {
                    row.Cells[i].Value = "0x00000000";
                }
            }
        }

        #endregion

        private void btnAssembly_Click(object sender, EventArgs e)
        {

            int currentIndex = 0;
            int pcItr = 0;
            if (!string.IsNullOrEmpty(txt_input.Text))
            {
                /// Etiketleri tespit edip listeye ekler
                while (currentIndex < txt_input.Lines.Length)
                {
                    #region Etiketleri Tuttuğumuz İşlem
                    string line = txt_input.Lines[currentIndex].Trim().Replace(",", "");
                    string[] splits = line.Split(' ');

                    if (splits.Length == 1)
                    {
                        _Labels.Add(new LabelWithPC
                        {
                            Name = splits[0].Replace(":", ""),
                            PC = pcItr
                        });
                    }
                    else
                    {
                        pcItr += 2;
                    }
                    #endregion
                    currentIndex++;
                }

                /// Satır Satır PC ları tutar
                pcItr = 0;
                int itr = 0;
                currentIndex = 0;

                while (currentIndex < txt_input.Lines.Length)
                {

                    string line = txt_input.Lines[currentIndex].Trim().Replace(",", "");
                    string[] splits = line.Split(' ');

                    if (splits.Length != 1) //eğer etiket değilse
                    {
                        _instructionMemoryManager.Assembler(pcItr, txt_input.Lines[currentIndex]);
                        _PcWithLines.Add(new PcWithLine
                        {
                            PC = pcItr,
                            CommandLine = itr
                        });
                        pcItr += 2;
                        itr++;
                    }
                    currentIndex++;
                }

                btn_line.Enabled = true;
                btn_start.Enabled = true;
                btnAssembly.Enabled = false;
            }
        }
    }
}
