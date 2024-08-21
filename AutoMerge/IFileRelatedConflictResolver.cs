using System;

using JetBrains.Annotations;

namespace AutoMerge
{
    // структура фрагментов  файла для разрешения конфликта
    [UsedImplicitly]
    public interface IFileRelatedConflictResolver
    {
        [NotNull, ItemNotNull]
        string[] SupportedExtensions { get; }

        [NotNull]
        string Description { get; }
    }
}