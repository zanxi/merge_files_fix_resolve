﻿using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This class adds extension methods for <see cref="IList{T}"/>.
    /// </summary>
    [PublicAPI]
    public static class ListExtensions
    {
        /// <summary>
        /// Mutate the specified list to have the same elements as another list, by inserting or removing as needed. The end result is that
        /// <paramref name="target"/> will have equivalent elements as <paramref name="source"/>, in the same order and positions.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the lists.
        /// </typeparam>
        /// <param name="target">
        /// The list to mutate. Elements will possibly be inserted into or deleted from this list.
        /// </param>
        /// <param name="source">
        /// The list to use as the source of mutations for <paramref name="target"/>.
        /// </param>
        /// <param name="comparer">
        /// The optional <see cref="IEqualityComparer{T}"/> to use when comparing elements.
        /// If not specified/<c>null</c>, <see cref="EqualityComparer{T}.Default"/> will be used.
        /// </param>
        /// <param name="aligner">
        /// The <see cref="IDiffElementAligner{T}"/> to use when aligning elements.
        /// If not specified/<c>null</c>, <see cref="BasicReplaceInsertDeleteDiffElementAligner{T}"/> will be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="target"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="source"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// The main purpose of this method is to avoid clearing and refilling the list from scratch and instead
        /// make adjustments to it to have the right elements. Useful in conjunction with UI bindings and
        /// similar that react to changes to the list.
        /// </remarks>
        public static void MutateToBeLike<T>([NotNull] this IList<T> target, [NotNull] IList<T> source, [CanBeNull] IEqualityComparer<T> comparer = null, [CanBeNull] IDiffElementAligner<T> aligner = null)
        {
            MutateToBeLike(target, source, new DiffOptions(), comparer, aligner);
        }

        /// <summary>
        /// Mutate the specified list to have the same elements as another list, by inserting or removing as needed. The end result is that
        /// <paramref name="target"/> will have equivalent elements as <paramref name="source"/>, in the same order and positions.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the lists.
        /// </typeparam>
        /// <param name="target">
        /// The list to mutate. Elements will possibly be inserted into or deleted from this list.
        /// </param>
        /// <param name="source">
        /// The list to use as the source of mutations for <paramref name="target"/>.
        /// </param>
        /// <param name="options">
        /// A <see cref="DiffOptions"/> object specifying options to the diff algorithm, or <c>null</c> if defaults should be used.
        /// </param>
        /// <param name="comparer">
        /// The optional <see cref="IEqualityComparer{T}"/> to use when comparing elements.
        /// If not specified/<c>null</c>, <see cref="EqualityComparer{T}.Default"/> will be used.
        /// </param>
        /// <param name="aligner">
        /// The <see cref="IDiffElementAligner{T}"/> to use when aligning elements.
        /// If not specified/<c>null</c>, <see cref="BasicReplaceInsertDeleteDiffElementAligner{T}"/> will be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="target"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="source"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// The main purpose of this method is to avoid clearing and refilling the list from scratch and instead
        /// make adjustments to it to have the right elements. Useful in conjunction with UI bindings and
        /// similar that react to changes to the list.
        /// </remarks>
        public static void MutateToBeLike<T>([NotNull] this IList<T> target, [NotNull] IList<T> source, [CanBeNull] DiffOptions options, [CanBeNull] IEqualityComparer<T> comparer = null, [CanBeNull] IDiffElementAligner<T> aligner = null)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            options = options ?? new DiffOptions();
            comparer = comparer ?? EqualityComparer<T>.Default;
            aligner = aligner ?? new BasicReplaceInsertDeleteDiffElementAligner<T>();

            var sections = Diff.CalculateSections(target, source, options, comparer);
            var items = Diff.AlignElements(target, source, sections, aligner).ToList();

            Assume.That(items != null);

            int targetIndex = 0;
            foreach (var item in items)
            {
                switch (item.Operation)
                {
                    case DiffOperation.Match:
                        targetIndex++;
                        break;

                    case DiffOperation.Insert:
                        target.Insert(targetIndex, item.ElementFromCollection2.Value);
                        targetIndex++;
                        break;

                    case DiffOperation.Delete:
                        target.RemoveAt(targetIndex);
                        break;

                    case DiffOperation.Replace:
                    case DiffOperation.Modify:
                        target[targetIndex] = item.ElementFromCollection2.Value;
                        targetIndex++;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
