namespace Simülator.Models
{
    /// <summary>
    /// Branch operasyonlarında koşul sağlanması durumunda belirtilen pc ın belirtilen etiketin 
    /// adresine gitmesi için oluşturulan bir liste. 
    /// Form da _PcWithLines adıyla kullanılıyor.
    /// </summary>

    public class PcWithLine
    {
        #region Properties
        public int PC
        {
            get;
            set;
        }

        public int CommandLine
        {
            get;
            set;
        }
        #endregion
    }
}
