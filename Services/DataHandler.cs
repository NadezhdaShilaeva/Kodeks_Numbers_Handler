using System;
using System.IO;
using System.Collections.Generic;
using CodexNumberHandler.Interfaces;

namespace CodexNumberHandler.Services
{
    /// <summary>
    /// Класс для обработки данных, реализует интерфейс IDataHandler
    /// </summary>
    public class DataHandler: IDataHandler
    {
        public DataHandler() { }

        /// <summary>
        /// Метод реализует обработку данных в указанной директории и записывает результат
        /// в файл в той же директории. 
        /// При отсутствии файла, файл будет создан. При наличии файла, он будет перезаписан.
        /// Метод использует структуру данных самобалансируемое дерево поиска для хранения 
        /// уникальных чисел и поддержания коллекции в отсортированном по убыванию состоянии.
        /// Метод работает за O(n log n).
        /// </summary>
        /// <param name="fromDirName">Путь до директории, в которой требуется обработать данные</param>
        /// <param name="toFileName">Название файла для записи результата</param>
        /// <exception cref="DirectoryNotFoundException">Выбрасывается при отсутствии каталога</exception>
        public void HandleDataOfDirectory(string fromDirName, string toFileName)
        {
            if (!Directory.Exists(fromDirName))
            {
                throw new DirectoryNotFoundException(fromDirName);
            }

            var numbers = new SortedSet<int>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
            ProcessDirectory(fromDirName, numbers, x => x % 4 == 3);
            WriteResult(fromDirName, toFileName, numbers);
        }

        /// <summary>
        /// Метод для обработки директории. Обходит все текстовые файлы директории и всех ее
        /// поддиректорий.Для каждого файла вызывает метод-обработчик ProcessFile.
        /// </summary>
        /// <param name="dirName">Путь до директории</param>
        /// <param name="numbers">Коллекция для записи результата</param>
        /// <param name="predicate">Условие, которому должно удовлетворять число</param>
        private void ProcessDirectory(string dirName, ICollection<int> numbers, Predicate<int> predicate)
        {
            string[] files = Directory.GetFiles(dirName, "*.txt");
            foreach (string filename in files)
            {
                ProcessFile(filename, numbers, predicate);
            }

            string[] subdirectories = Directory.GetDirectories(dirName);
            foreach (string subdirectory in subdirectories)
            {
                ProcessDirectory(subdirectory, numbers, predicate);
            }
        }

        /// <summary>
        /// Метод для обработки данных файла.
        /// Метод считывает каждую строку файла, преобразует в число. Если число удовлетворяет условию
        /// предиката, оно добавляется в коллекцию.
        /// </summary>
        /// <param name="fileName">Yазвание файла</param>
        /// <param name="numbers">Коллекциядля записи результата</param>
        /// <param name="predicate">Условие, которое должно быть выполнено для числа</param>
        private void ProcessFile(string fileName, ICollection<int> numbers, Predicate<int> predicate)
        {
            Console.WriteLine("Processed file '{0}'.", fileName);

            using (StreamReader reader = new StreamReader(fileName))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    int number = int.Parse(line);
                    if (predicate(number))
                    {
                        numbers.Add(number);
                    }
                }
            }
        }

        /// <summary>
        /// Метод для записи результата в файл.
        /// Метод перебирает элементы перечисления и записывает каждое число в файл.
        /// </summary>
        /// <param name="fromDirName">Путь до директории, в котрой будет записан файл</param>
        /// <param name="toFileName">Название файла, в который будет записан результат</param>
        /// <param name="numbers">Результирующая коллекцию чисел для записи в файл</param>
        private void WriteResult(string fromDirName, string toFileName, IEnumerable<int> numbers)
        {
            string fullResultFileName = Path.Combine(fromDirName, toFileName);

            using (StreamWriter writer = new StreamWriter(fullResultFileName, false))
            {
                numbers.ToList().ForEach(number => writer.WriteLine(number));
            }
        }
    }
}
