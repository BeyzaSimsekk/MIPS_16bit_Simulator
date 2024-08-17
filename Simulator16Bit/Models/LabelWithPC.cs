namespace Simülator.Models
{
    /// <summary>
    /// Her bir etiketin (label) pc adresini tutması için liste.
    /// Prosedür, jump, branch ve bazı assemble operasyonları için gerekli.
    /// Form da _Labels adıyla kullanılıyor.
    /// </summary>

    public class LabelWithPC
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }

        public int PC
        {
            get;
            set;
        }
        #endregion
    }
}
