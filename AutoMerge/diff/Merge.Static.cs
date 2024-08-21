﻿using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// Static API class for the merge portion of DiffLib.
    /// </summary>
    [PublicAPI]
    public static class Merge
    {
        /// <summary>
        /// Performs a merge using a 3-way merge, returning the final merged output.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collections being merged.
        /// </typeparam>
        /// <param name="commonBase">
        /// The common base/ancestor of both <paramref name="left"/> and <paramref name="right"/>.
        /// </param>
        /// <param name="left">
        /// The left side being merged with the <paramref name="right"/>.
        /// </param>
        /// <param name="right">
        /// The right side being merged with the <paramref name="left"/>.
        /// </param>
        /// <param name="aligner">
        /// A <see cref="IDiffElementAligner{T}"/> implementation that will be responsible for lining up common vs. left and common vs. right as well as left vs. right
        /// during the merge.
        /// </param>
        /// <param name="conflictResolver">
        /// A <see cref="IMergeConflictResolver{T}"/> implementation that will be used to resolve conflicting modifications between left and right.
        /// </param>
        /// <param name="comparer">
        /// A <see cref="IEqualityComparer{T}"/> implementation that will be used to compare elements of all the collections. If <c>null</c> is specified then
        /// <see cref="EqualityComparer{T}.Default"/> will be used.
        /// </param>
        /// <returns>
        /// The final merged collection of elements from <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <exception cref="MergeConflictException">
        /// The <paramref name="conflictResolver"/> threw a <see cref="MergeConflictException"/> to indicate a failure to resolve a conflict.
        /// </exception>
        [NotNull, ItemCanBeNull]
        public static IEnumerable<T> Perform<T>([NotNull] IList<T> commonBase, [NotNull] IList<T> left, [NotNull] IList<T> right, [NotNull] IDiffElementAligner<T> aligner, [NotNull] IMergeConflictResolver<T> conflictResolver, [CanBeNull] IEqualityComparer<T> comparer = null)
        {
            return Perform(commonBase, left, right, new DiffOptions(), aligner, conflictResolver, comparer);
        }

        public static IEnumerable<string> Perform_s([NotNull] IList<string> commonBase, [NotNull] IList<string> left, [NotNull] IList<string> right, [NotNull] IDiffElementAligner<string> aligner, [NotNull] IMergeConflictResolver<string> conflictResolver, [CanBeNull] IEqualityComparer<string> comparer = null)
        {
            return Perform_s(commonBase, left, right, new DiffOptions(), aligner, conflictResolver, comparer);
        }

        /// <summary>
        /// Performs a merge using a 3-way merge, returning the final merged output.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collections being merged.
        /// </typeparam>
        /// <param name="commonBase">
        /// The common base/ancestor of both <paramref name="left"/> and <paramref name="right"/>.
        /// </param>
        /// <param name="left">
        /// The left side being merged with the <paramref name="right"/>.
        /// </param>
        /// <param name="right">
        /// The right side being merged with the <paramref name="left"/>.
        /// </param>
        /// <param name="diffOptions">
        /// A <see cref="DiffOptions"/> object specifying options to the diff algorithm, or <c>null</c> if defaults should be used.
        /// </param>
        /// <param name="aligner">
        /// A <see cref="IDiffElementAligner{T}"/> implementation that will be responsible for lining up common vs. left and common vs. right as well as left vs. right
        /// during the merge.
        /// </param>
        /// <param name="conflictResolver">
        /// A <see cref="IMergeConflictResolver{T}"/> implementation that will be used to resolve conflicting modifications between left and right.
        /// </param>
        /// <param name="comparer">
        /// A <see cref="IEqualityComparer{T}"/> implementation that will be used to compare elements of all the collections. If <c>null</c> is specified then
        /// <see cref="EqualityComparer{T}.Default"/> will be used.
        /// </param>
        /// <returns>
        /// The final merged collection of elements from <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <exception cref="MergeConflictException">
        /// The <paramref name="conflictResolver"/> threw a <see cref="MergeConflictException"/> to indicate a failure to resolve a conflict.
        /// </exception>
        [NotNull, ItemCanBeNull]
        public static IEnumerable<T> Perform<T>([NotNull] IList<T> commonBase, [NotNull] IList<T> left, [NotNull] IList<T> right, [CanBeNull] DiffOptions diffOptions, [NotNull] IDiffElementAligner<T> aligner, [NotNull] IMergeConflictResolver<T> conflictResolver, [CanBeNull] IEqualityComparer<T> comparer = null)
        {
            if (commonBase == null)
                throw new ArgumentNullException(nameof(commonBase));
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (aligner == null)
                throw new ArgumentNullException(nameof(aligner));
            if (conflictResolver == null)
                throw new ArgumentNullException(nameof(conflictResolver));

            diffOptions = diffOptions ?? new DiffOptions();
            comparer = comparer ?? EqualityComparer<T>.Default;
            Assume.That(comparer != null);

            return new Merge<T>(commonBase, left, right, aligner, conflictResolver, comparer, diffOptions);            

        }

        public static IEnumerable<string> Perform_s(
            [NotNull] IList<string> commonBase, 
            [NotNull] IList<string> left, 
            [NotNull] IList<string> right, 
            [CanBeNull] DiffOptions diffOptions, 
            [NotNull] IDiffElementAligner<string> aligner, 
            [NotNull] IMergeConflictResolver<string> conflictResolver, 
            [CanBeNull] IEqualityComparer<string> comparer = null)
        {
            if (commonBase == null)
                throw new ArgumentNullException(nameof(commonBase));
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (aligner == null)
                throw new ArgumentNullException(nameof(aligner));
            if (conflictResolver == null)
                throw new ArgumentNullException(nameof(conflictResolver));

            diffOptions = diffOptions ?? new DiffOptions();
            comparer = comparer ?? EqualityComparer<string>.Default;
            Assume.That(comparer != null);

            return (IEnumerable<string>)(new Merge_s(commonBase, left, right, aligner, conflictResolver, comparer, diffOptions));

        }
    }
}