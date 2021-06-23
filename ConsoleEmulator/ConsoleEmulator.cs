using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleEmulator
{

    public delegate void Message(string message);
    class ConsoleEmulator
    {

        public Message Info;
        public Message Error;
        public Message ConsEmulOut;

        string getFileName(string FullPath)
        {
            string[] path = FullPath.Split("\\");
            return path[path.Length - 1];
        }

        public void ShowConcole()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(":::   ConsoleEmulator - Эмулятор консоли   :::\n");

            Console.WriteLine("Список возможных команд:\n");
            
            Console.WriteLine("\thelp                                    - Показать это сообщение;\n");

            Console.WriteLine("\tsp                                      - Показать путь текущей директории;");
            Console.WriteLine("\tsd                                      - Показать папки текущей директории;");
            Console.WriteLine("\tsf                                      - Показать файлы текущей директории;\n");

            Console.WriteLine("\tgp <Path>                               - Переход в указанный каталог;\n");

            Console.WriteLine("\tcf <FileName>                           - Создать файл или дописать содержимое, если такой существует;");
            Console.WriteLine("\trf <FileName>                           - Вывод на экран консоли/терминала содержимого текстового файла;\n");

            Console.WriteLine("\tcpf <FileSource> >> <FileDestination>   - Копировать в указанное место;\n");

            Console.WriteLine("\trmf <FileName>                          - Удалить указанный файл;\n");

            Console.WriteLine("\texit    - Выход.\n");
        }

        public string ShowPath()
        {   //  Показать путь текущей директории
            //  вывод полного пути, где сейчас находитесь в структуре каталогов
            //  ShowPath или SP или sp(без параметров)

            return Directory.GetCurrentDirectory();       
    
        }

        public void ShowDirectories()
        {
            //  Показать паки в текущей директории
            var dirs = Directory.GetDirectories(ShowPath());
            foreach (var dir in dirs)
            {
                string[] folders = dir.Split("\\");
                ConsEmulOut?.Invoke(folders[folders.Length - 1]);
            }
        }

        public void ShowFiles()
        {
            //  Показать файлы в текущей директории
            bool folder_emptu = true;
            var files = Directory.GetFiles(ShowPath());
            foreach (var file_path in files)
            {
                folder_emptu = false;

                ConsEmulOut?.Invoke(getFileName(file_path));
            }
            if (folder_emptu)
            {
                Error?.Invoke("Ошибка: Текущая папка пуста.");
            }
        }

        public void GoPath(string Path)
        {   //  Переход в указанный каталог
            //  GoPath или GP или gp(один параметр - <Path>)
            //  Path - каталог для перехода

            if (Path == "")
            {
                //  Ошибка:
                //  Нельзя вызвать без параметра
                Error?.Invoke("Ошибка: Нельзя вызвать 'gp' без параметра.");
                return;
            }

            if (Path == "..")
            {   //  Переход на уровень выше
                string[] folders = ShowPath().Split("\\");
                string newPath = "";

                if (folders.Length>1)
                {
                    newPath = folders[0];
                    for (int i = 1; i < folders.Length-1; i++)
                    {
                        newPath += "\\" + folders[i];
                    }

                    Directory.SetCurrentDirectory(newPath);
                }
                else
                {
                    return;
                }
            }
            else
            {   //  Переход в указанный каталог

                //  Проверить существует ли такая директория
                if (Directory.Exists(Path))
                {
                    Directory.SetCurrentDirectory(Path);
                }
                else
                {
                    Error?.Invoke("Ошибка: Указанного пути не существует.");
                }
            }
        }

        public void CreateFile(string NameFile)
        {
            //ConsoleKeyInfo btn;   //  Хотел сделать завершение ввода по Escape... Но возникла проблема проглатывания первых символов. :(
            //  Создание текстового файла
            //  NameFile - имя файла
            if (NameFile == "")
            {   //  Ошибка:
                //  Неуказано имя. Нельзя создать файл без имени.
                Error?.Invoke("Ошибка: Неуказано имя. Нельзя создать файл без имени.");
                return;
            }
            else
            {
                //  Создать файл с указанным именем в указанной директории.
                
                Info?.Invoke("Введите текст файла.\nНовая строка клавиша: 'Enter'.\nЗавершить ввод: пустая строка.");
                
                string msg;
                
                do
                {
                    msg = "";

                    using var file = new StreamWriter(NameFile,true);
                    
                    msg = Console.ReadLine();
                    if (msg != "")
                    {
                        //  Не записывать если строка пустая... Пустая строка используется для выхода...
                        file.WriteLine(msg);
                    }

                    //btn = Console.ReadKey(); //  Хотел сделать завершение ввода по Escape... Но возникла проблема проглатывания первых символов. :(

                } while (msg != "");
                //} while (btn.Key != ConsoleKey.Escape); //  Хотел сделать завершение ввода по Escape... Но возникла проблема проглатывания первых символов. :(
                Info?.Invoke($"Файл: {NameFile} создан.");
            }
        }

        public void CopyFile(string PathSource, string PathDestination)
        {
            //  Копирование в указанное место

            //  PathSource - путь к источнику
            //  PathDestination - путь назначения

            if (PathSource == PathDestination)
            {   //  Ничего не делать.  равен 
                return;
            }

            Console.WriteLine(PathSource);

            if (!File.Exists(PathSource))
            {
                Error?.Invoke("Ошибка: Не обнаружен файл источника.");
            }
            else
            {
                
                File.Copy(PathSource, PathDestination,true);
                Info?.Invoke($"Файл: {PathSource}, скопирован в {PathDestination}");
            }
        }

        public void RemoveFile(string NameFile)
        {
            //  Удаление файла
            //  Path - путь к файлу

            if (NameFile == "")
            {
                Error?.Invoke("Ошибка: Неуказано имя. Нельзя удалить файл без имени.");
                return;
            }

            bool file_found = false;

            var files = Directory.GetFiles(ShowPath());
            foreach (var file_path in files)
            {
                string[] path = file_path.Split("\\");
                string file_name = path[path.Length-1];
                
                if (file_name == NameFile)
                {
                    file_found = true;
                    File.Delete(ShowPath() + "\\" + NameFile);
                    Info?.Invoke($"Файл: {NameFile} удален");
                }
            }
            if (!file_found)
            {
                Error?.Invoke("Ошибка: Указанный файл не существует.");
            }
        }

        public void ReadFile(string NameFile)
        {
            //  Вывод на экран консоли/терминала содержимого текстового файла
            if (NameFile == "")
            {
                Error?.Invoke("Ошибка: Не указан файл.");
                return;
            }

            bool file_found = false;

            var files = Directory.GetFiles(ShowPath());
            foreach (var file_path in files)
            {
                if (NameFile == getFileName(file_path))
                {
                    file_found = true;

                    string line;

                    try
                    {
                        StreamReader sr = new StreamReader(file_path);

                        line = sr.ReadLine();

                        while (line != null)
                        {
                            ConsEmulOut?.Invoke(line);
                            line = sr.ReadLine();
                        }
                        sr.Close();
                    }
                    catch (Exception e)
                    {
                        Error?.Invoke($"Ошибка:  {e.Message}");
                    }
                    finally
                    {
                        Info?.Invoke($"Содержимое файла: {getFileName(file_path)} выведено.");
                    }
                }
            }

            if (!file_found)
            {
                Error?.Invoke("Ошибка: Файл с указанным именем не существует.");
            }
        }
    }
}
//Signature - подпись
/*
-------------------------------------------
|                                         |
|   "Компьютерная академия ШАГ"           |
|   Курс: PD_011                          |
|   Предмет: Платформа Microsoft .NET     |
|            и язык программирования C#   |
|                                         |
|   Исполнитель: Курицын Алексей          |
|   Преподаватель: Старинин Андрей        |
|                                         |
|   Екатеринбург - 2021                   |
|                                         |
-------------------------------------------*/