using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutoMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args?.Length != 4)
            {
                Console.Error?.WriteLine("Не указаны  1-й файл 2-й файл и файл слияния");
                Environment.Exit(1);
            }

            var localFilename = args[0] ?? string.Empty;
            var remoteFilename = args[1] ?? string.Empty;
            var baseFilename = args[2] ?? string.Empty;
            var mergedFilename = args[3] ?? string.Empty;

            Console.WriteLine($"automerge.base: {baseFilename}"); // исходный файл
            Console.WriteLine($"automerge.local: {localFilename}"); // 1-й файл слева
            Console.WriteLine($"automerge.remote: {remoteFilename}"); // 2-й файл справа
            Console.WriteLine($"automerge.output: {mergedFilename}"); // файц слияния

            var extension = Path.GetExtension(mergedFilename).ToLowerInvariant(); // расширения файла слияния
            var resolvers = FileResolverFactory.GetResolverForExtension(extension);
            if (!resolvers.Any())
            {
                Console.Error?.WriteLine($"automerge.error: не указано расширение {extension}");
                Environment.Exit(1);
            }

            foreach (var resolver in resolvers)
            {
                if (resolver.TryResolve(baseFilename, localFilename, remoteFilename, mergedFilename))
                {
                    var attr = resolver.GetType().GetCustomAttribute<DescriptionAttribute>(false);
                    if (attr != null)
                        Console.WriteLine($"automerge: конфликт решен с помощью '{attr.Description}' обработчика конфликтов");
                    Console.WriteLine("automerge.success: конфликты успешно разрешены и помечены");
                    Environment.Exit(0);
                }
            }

            Console.Error?.WriteLine($"automerge.error: нет правил автоматического слияния для расширения файла {extension} для объединения файлов");
            Environment.Exit(1);
        }
    }
}
