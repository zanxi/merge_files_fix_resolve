using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace AutoMerge.Resolvers
{
    public interface IFragmentConflictResolver : IFileRelatedConflictResolver
    {
        [CanBeNull]
        IEnumerable<string> TryResolve([NotNull] IList<string> commonBase, // список фрагментов исходного файла
            [NotNull] IList<string> left,  // список фрагментов файла 1-го программиста слева
            [NotNull] IList<string> right); // список фрагментов файла 2-го программиста справа
    }
}