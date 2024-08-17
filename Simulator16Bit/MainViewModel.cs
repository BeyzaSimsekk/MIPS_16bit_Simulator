namespace Simülator
{
    // MainViewModel sınıfı
    public class MainViewModel
    {
        private static readonly MainViewModel m_MainViewModel = new MainViewModel();
        public static MainViewModel Instance => m_MainViewModel;

        public MemoriesViewModel MemoriesVM { get; } = new MemoriesViewModel();

        private MainViewModel()
        {
        }
    }
}
