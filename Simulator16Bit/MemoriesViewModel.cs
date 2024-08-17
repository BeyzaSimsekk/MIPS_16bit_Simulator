using System;
using System.Collections.Generic;


namespace Simülator
{
    // MemoriesViewModel sınıfı
    public class MemoriesViewModel
    {
        public List<MemoryMainModel> Memories { get; } = new List<MemoryMainModel>(256);

        public MemoriesViewModel()
        {
            SetMemories();
        }


        /// <summary>
        /// Hiçbir yerde kullanmıyoruz ama nolur nolmaz diye burada saklamaya karar verdim.
        /// 
        ///public void ClearValues()
        ///{
        ///    foreach (var mem in Memories)
        ///    {
        ///        mem.Part0.PartValue = 0;
        ///        mem.Part4.PartValue = 0;
        ///        mem.Part8.PartValue = 0;
        ///        mem.Part12.PartValue = 0;
        ///        mem.Part16.PartValue = 0;
        ///        mem.Part20.PartValue = 0;
        ///        mem.Part24.PartValue = 0;
        ///        mem.Part28.PartValue = 0;
        ///    }
        ///}
        /// 
        /// </summary>


        public bool IsValidLocation(uint loc, out MemoryPartModel memoryPart)
        {
            const int wordSize = 4; // Dört byte (bir kelime) büyüklüğünde bir bellek bloğu
            foreach (var mem in Memories)
            {
                if (mem.Location <= loc && loc < mem.Location + 4 * wordSize)
                {
                    uint alignedAddress = loc - (loc % wordSize); // Hizalanmış adres

                    switch (alignedAddress - mem.Location)
                    {
                        case 0:
                            memoryPart = mem.Part0;
                            return true;
                        //case wordSize:
                        //    memoryPart = mem.Part4;
                        //    return true;
                        //case 2 * wordSize:
                        //    memoryPart = mem.Part8;
                        //    return true;
                        //case 3 * wordSize:
                        //    memoryPart = mem.Part12;
                        //    return true;
                        //case 4 * wordSize:
                        //    memoryPart = mem.Part16;
                        //    return true;
                        //case 5 * wordSize:
                        //    memoryPart = mem.Part20;
                        //    return true;
                        //case 6 * wordSize:
                        //    memoryPart = mem.Part24;
                        //    return true;
                        //case 7 * wordSize:
                        //    memoryPart = mem.Part28;
                        //    return true;
                        default:
                            break;
                    }
                }
            }

            memoryPart = null;
            return false;
        }


        // Yeni metod: Bellek bloğuna bir kelime yazma
        public void StoreWord(uint address, byte[] value)
        {
            const int wordSize = 4; // Bir kelimenin (word) byte cinsinden boyutu
            uint alignedAddress = address - (address % wordSize); // Adresi kelime sınırlarına hizala

            // Bellek bloğunu bul
            MemoryPartModel memoryPart;
            if (IsValidLocation(alignedAddress, out memoryPart))
            {
                // Belleği güncelle
                int offset = (int)(address - alignedAddress); // Adresin blok içindeki ofseti
                switch (offset)
                {
                    case 0:
                        memoryPart.PartValue = BitConverter.ToInt32(value, 0);
                        break;
                    case 1:
                        memoryPart.PartValue = (int)((memoryPart.PartValue & 0xFFFFFF00) | value[0]);
                        break;
                    case 2:
                        memoryPart.PartValue = (int)((memoryPart.PartValue & 0xFFFF00FF) | ((uint)value[0] << 8));
                        break;
                    case 3:
                        memoryPart.PartValue = (int)((memoryPart.PartValue & 0xFF00FFFF) | ((uint)value[0] << 16));
                        break;
                    default:
                        throw new InvalidOperationException("Invalid offset");
                }

                // Belleği güncelleme metodu çağrılabilir
                //UpdateMemoryDataGridView(memoryPart);
            }
            else
            {
                throw new ArgumentException($"Invalid memory address: 0x{address:X8}");
            }
        }


        private void SetMemories()
        {
            for (int i = 0x0; i <= 0xff; i++)
            {
                Memories.Add(new MemoryMainModel(i));
            }

        }
    }

}
