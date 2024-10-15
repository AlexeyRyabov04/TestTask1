using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TestTask1.Services.Implementations;
using TestTask1.Services.Interfaces;
using TestTask1.ViewModels;

namespace TestTask1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IFilesGeneratorService, FilesGeneratorService>();
            services.AddTransient<IFilesMergerService, FilesMergerService>();
            services.AddTransient<IDataProcessService, DataProcessService>();
            services.AddSingleton<TaskViewModel>();
            services.AddSingleton<MainViewModel>();
            serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow()
            {
                DataContext = serviceProvider.GetRequiredService<MainViewModel>()
            };
            MainWindow.Show();
        }
    }

}
