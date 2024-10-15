namespace TestTask1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainViewModel(TaskViewModel taskViewModel)
        {
            CurrentViewModel = taskViewModel;
        }
    }
}
