using System.Windows;
using System.Windows.Controls;
using TestTask1.ViewModels;

namespace TestTask1.Views
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : UserControl
    {
        public TaskView()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                ((TaskViewModel)DataContext).FilesGenerated = () => 
                MessageBox.Show("Файлы были успешно сгенерированы и сохранены.", "Готово", MessageBoxButton.OK);
                ((TaskViewModel)DataContext).FilesMerged = (count) =>
                MessageBox.Show($"Файлы были успешно объединены. Удалено строк - {count}", "Готово", MessageBoxButton.OK);
                ((TaskViewModel)DataContext).IncorrectDirPath = () =>
               MessageBox.Show($"Директория пустая или не существует", "Ошибка", MessageBoxButton.OK);
                ((TaskViewModel)DataContext).FilesImported = () =>
                MessageBox.Show("Файл успешно импортирован в БД.", "Готово", MessageBoxButton.OK);
                ((TaskViewModel)DataContext).IncorrectFilePath = () =>
                MessageBox.Show($"Файл пустой или не существует", "Ошибка", MessageBoxButton.OK);
                ((TaskViewModel)DataContext).ImportError = () =>
                MessageBox.Show($"Ошибка при импорте данных в БД", "Ошибка", MessageBoxButton.OK);
                ((TaskViewModel)DataContext).QueryCompleted = (sum, median) =>
                MessageBox.Show($"Сумма целых - {sum}, Медиана дробных - {median}", "Готово", MessageBoxButton.OK);
            };
           
        }
    }
}
