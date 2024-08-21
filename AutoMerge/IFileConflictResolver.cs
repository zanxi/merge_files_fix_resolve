using System;

using AutoMerge.Resolvers;

using JetBrains.Annotations;

//  структура хранения исходного файла, 1-го программиста, 2-го программиста, файл слияния

namespace AutoMerge
{
    public interface IFileConflictResolver : IFileRelatedConflictResolver
    {    
        bool TryResolve([NotNull] string commonBaseFilename, // ИСХОДНЫЙ ФАЙЛ
            [NotNull] string leftFilename, // 1-Й ПРОГРАММИСТ СЛЕВА
            [NotNull] string rightFilename,  // 2-Й ПРОГРАММИСТ Справа
            [NotNull] string mergedFilename); // результирующий фал слияния
    }
}