using System;
using System.IO;


namespace ConsoleEmulator
{
    class Program
    {
        private static void Main()
        {

            ConsoleEmulator consoleEmul = new ConsoleEmulator();

            consoleEmul.Error = ShowConsoleError;
            consoleEmul.Info = ShowConsoleInfo;
            consoleEmul.ConsEmulOut = ShowConsoleOut;

            string commandEnter = "";

            consoleEmul.ShowConcole();

            while (commandEnter !="exit")
            {
                string command = ""; // Команда
                string param = "";  //  Параметр

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Введите команду: ");
                Console.ForegroundColor = ConsoleColor.White;

                commandEnter = Console.ReadLine().ToString();

                if (commandEnter.Contains(" "))
                {
                    //  Строка содержит пробел
                    //  Значит введена комманда с параметром

                    string[] command_split = commandEnter.Split(" ");
                
                    //Console.WriteLine(command_split.Length);

                    command = command_split[0]; // Команда
                    param = "";  //  Параметр

                    if (command_split.Length>2)
                    {
                        param = command_split[1];
                        for (int i = 2; i < command_split.Length; i++)
                        {
                            //  Сбор строки параметра обратно через пробел...
                            param += " " + command_split[i];
                        }
                    }
                    else if (command_split.Length == 2)
                    {
                        param = command_split[1];
                    }
                }
                else
                {
                    //  Введена комманда без параметра
                    command = commandEnter;
                }

                //  Debug:
                //Console.WriteLine($"Команда: {command}");
                //Console.WriteLine($"Параметр: {param}");

                switch (command)
                {
                    case "help":
                        //  Показать заставку меню
                        consoleEmul.ShowConcole();
                        break;

                    case "sp":
                        //  Вывести текущий путь
                        ShowConsoleInfo("Текущий каталог:");
                        ShowConsoleInfo(consoleEmul.ShowPath());
                        break;

                    case "sd":
                        //  Вывести папки в текущем каталоге
                        
                        ShowConsoleInfo("Список папок в текущем каталоге:");

                        Console.ForegroundColor = ConsoleColor.Cyan;

                        consoleEmul.ShowDirectories();

                        Console.ResetColor();
                        break;

                    case "sf":
                        //  Вывести файлы в текущей директории.

                        ShowConsoleInfo("Список файлов в текущем каталоге:");

                        Console.ForegroundColor = ConsoleColor.Gray;

                        consoleEmul.ShowFiles();

                        Console.ResetColor();

                        break;

                    case "gp":
                        //  Перейти в указанный каталог
                        consoleEmul.GoPath(param);
                        
                        ShowConsoleInfo(consoleEmul.ShowPath());
                        break;

                    case "cf":
                        // Создать текстовый файл
                        consoleEmul.CreateFile(param);
                        break;

                    case "rf":
                        //  Вывод на экран терминала содержимого файла
                        consoleEmul.ReadFile(param);
                        break;

                    case "cpf":
                        //  Копирование файла
                        if (!param.Contains(" >> "))
                        {
                            ShowConsoleError("Ошибка: Нарушен синтаксис команды.");
                            ShowConsoleInfo("cpf <FileSource> >> <FileDestination>");
                            break;
                        }

                        string[] split_param = param.Split(" >> ");

                        if (split_param.Length > 2)
                        {
                            ShowConsoleError("Ошибка: слишком много параметров.");
                            ShowConsoleInfo("cpf <FileSource> >> <FileDestination>");
                        }
                        else
                        {
                            consoleEmul.CopyFile(split_param[0], split_param[1]);
                        }
                        break;

                    case "rmf":
                        // Удалить файл
                        consoleEmul.RemoveFile(param);
                        break;

                    case "exit":
                        //  Выход
                        break;

                    default:
                        ShowConsoleError("Команда не распознана. Пожалуйста попробуйте снова.");
                        ShowConsoleInfo("Введите 'help' для показа списка команд.");
                        break;
                }

                //  Debug:
                //Console.WriteLine(command_enter);
            }
        }

        static void ShowConsoleError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        static void ShowConsoleInfo(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        static void ShowConsoleOut(string msg)
        {
            Console.WriteLine(msg);
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