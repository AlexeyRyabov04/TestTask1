using System.Windows.Input;
using TestTask1.Commands;
using TestTask1.Exceptions;
using TestTask1.Services.Implementations;
using TestTask1.Services.Interfaces;

namespace TestTask1.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private string _generatePath = string.Empty;

		public string GeneratePath
        {
			get { 
				return _generatePath;
			}
			set { 
				_generatePath = value;
				onPropertyChanged(nameof(GeneratePath));
			}
		}

        private string _savePath = string.Empty;

        public string SavePath
        {
            get
            {
                return _savePath;
            }
            set
            {
                _savePath = value;
                onPropertyChanged(nameof(SavePath));
            }
        }

        private string _deleteString = string.Empty;

        public string DeleteString
        {
            get
            {
                return _deleteString;
            }
            set
            {
                _deleteString = value;
                onPropertyChanged(nameof(DeleteString));
            }
        }

        private int _stringsLeft;

        public int StringsLeft
        {
            get
            {
                return _stringsLeft;
            }
            set
            {
                _stringsLeft = value;
                onPropertyChanged(nameof(StringsLeft));
            }
        }
        private int _stringsImported;

        public int StringsImported
        {
            get
            {
                return _stringsImported;
            }
            set
            {
                _stringsImported = value;
                onPropertyChanged(nameof(StringsImported));
            }
        }

        private string _exportFilePath = string.Empty;

        public string ExportFilePath
        {
            get
            {
                return _exportFilePath;
            }
            set
            {
                _exportFilePath = value;
                onPropertyChanged(nameof(ExportFilePath));
            }
        }

        public ICommand GenerateCommand { get; }
        public ICommand MergeCommand { get; }
        public ICommand ImportCommand { get; set; }
        public ICommand CalculateSumAndMedianCommand { get; set; }

        public Action FilesGenerated { get; set; }
        public Action<int> FilesMerged { get; set; }
        public Action IncorrectDirPath { get; set; }
        public Action FilesImported { get; set; }
        public Action IncorrectFilePath { get; set; }
        public Action ImportError { get; set; }
        public Action<long, double> QueryCompleted { get; set; }


        public TaskViewModel(IFilesGeneratorService filesGeneratorService, IFilesMergerService filesMergerService, IDataProcessService dataProcessService)
        {
            filesGeneratorService = new FilesGeneratorService();
            filesMergerService = new FilesMergerService();
            dataProcessService = new DataProcessService();
            //генерация файлов со строками заданного формата
            GenerateCommand = new RelayCommand(async _ =>
            {
                await Task.Run(async () => await filesGeneratorService.GenerateFiles(GeneratePath));
                FilesGenerated?.Invoke();
            }, _ => GeneratePath.Length > 0);

            //Объединение файлов в один
            MergeCommand = new RelayCommand(async _ =>
            {
                try
                {
                    var count = await Task.Run(() => filesMergerService.MergeFiles(GeneratePath, SavePath, DeleteString));
                    FilesMerged?.Invoke(count);
                } catch(IncorrectDirException)
                {
                    IncorrectDirPath?.Invoke();
                }
            }, _ => GeneratePath.Length > 0 && SavePath.Length > 3 && SavePath.Substring(SavePath.Length - 4) == ".txt");

            //Импорт файлов в бд
            ImportCommand = new RelayCommand(async _ =>
            {
                try
                {
                    dataProcessService.CountLines += (imported, left) =>
                    {
                            StringsLeft = left;
                            StringsImported = imported;
                    };
                    await Task.Run(async () => await dataProcessService.SaveFileToDatabase(ExportFilePath));
                    FilesImported?.Invoke();
                }
                catch (IncorrectFileException)
                {
                    IncorrectFilePath?.Invoke();
                }
                catch (Exception)
                {
                    ImportError?.Invoke();
                }
            }, _ => ExportFilePath.Length > 3 && ExportFilePath.Substring(ExportFilePath.Length - 4) == ".txt");

            //Расчёт суммы целых и медианы дробных
            CalculateSumAndMedianCommand = new RelayCommand(_ =>
            {
                var result = dataProcessService.CalculateSumAndMedian();
                QueryCompleted?.Invoke(result.Item1, result.Item2);
            });

        }

       
    }
}
