using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using System.ComponentModel;
using System.IO;

using DiffLib;

namespace AutoMerge.Resolvers
{

    public class StringSimilarityDiffElementAligner_ : IDiffElementAligner<string>
    {
        [NotNull]
        private readonly IDiffElementAligner<string> _Aligner;
                
        public StringSimilarityDiffElementAligner_(double modificationThreshold = 0.3333)
        {
            _Aligner = new ElementSimilarityDiffElementAligner<string>(StringSimilarity, modificationThreshold);
        }

        private static double StringSimilarity([CanBeNull] string element1, [CanBeNull] string element2)
        {
            element1 = element1 ?? String.Empty;
            element2 = element2 ?? String.Empty;

            //Assume.That(element1 != null && element2 != null);

            if (ReferenceEquals(element1, element2))
                return 1.0;

            if (element1.Length == 0 && element2.Length == 0)
                return 1.0;
            if (element1.Length == 0 || element2.Length == 0)
                return 0.0;

            var element1Array = element1.ToCharArray();
            var element2Array = element2.ToCharArray();
            //Assume.That(element1Array != null && element2Array != null);

            //var diffSections = Diff.CalculateSections(element1Array, element2Array, new DiffOptions()).ToArray();
            int matchLength = 20; // diffSections.Where(section => section.IsMatch).Sum(section => section.LengthInCollection1);
            return (matchLength * 2.0) / (element1.Length + element2.Length + 0.0);
        }

        
        public IEnumerable<DiffElement<string>> Align(IList<string> collection1, int start1, int length1, IList<string> collection2, int start2, int length2)
        {
            return _Aligner.Align(collection1, start1, length1, collection2, start2, length2);
        }
    }

    public class TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver_<T> : IMergeConflictResolver<T>
    {
        private readonly IEqualityComparer<T> _EqualityComparer;
        
        public TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver_([CanBeNull] IEqualityComparer<T> equalityComparer = null)
        {
            _EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        /// <inheritdoc />
        public IEnumerable<T> Resolve(IList<T> commonBase, IList<T> left, IList<T> right)
        {
            return new List<T>();

            //foreach (var item in left)
             //   yield return item;

            //if (left.SequenceEqual(right, _EqualityComparer))
            //    yield break;

            //foreach (var item in right)
                  //yield return item;
        }
    }


public class TextFileFileConflictResolver : IFileConflictResolver
    {
        public bool TryResolve(string commonBaseFilename, string leftFilename, string rightFilename, string mergedFilename)
        {
            var commonBase = File.ReadAllLines(commonBaseFilename);
            var left = File.ReadAllLines(leftFilename);
            var right = File.ReadAllLines(rightFilename);
            try
            {
                //var merged = Merge.Perform(commonBase, left, right, new StringSimilarityDiffElementAligner(), new TakeLeftThenRightIfRightDiffersFromLeft<string>());
                //var merged = Merge.Perform(commonBase, left, right, new StringSimilarityDiffElementAligner(), new TakeLeftThenRightIfRightDiffersFromLeft<string>());
                //var merged = Merge.Perform(commonBase, left, right, new StringSimilarityDiffElementAligner(), new TakeLeftMergeConflictResolver<string>());
                //var merged = Merge.Perform(commonBase, left, right, new StringSimilarityDiffElementAligner(), new TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver<string>());

                //var merged = Merge.Perform(commonBase, left, right, new StringSimilarityDiffElementAligner(), new TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver<string>());

                //var merged = Merge.Perform(commonBase, left, right, new StringSimilarityDiffElementAligner(), new TakeRightThenLeftMergeConflictResolver<string>());
                //var merged = Merge.Perform(commonBase, left, right, new BasicReplaceInsertDeleteDiffElementAligner<string>(), new TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver<string>());

                var merged = Merge.Perform_s(commonBase, left, right, new StringSimilarityDiffElementAligner(), new TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver<string>());

                foreach (string s in merged)
                {
                    Console.WriteLine("merged: "  + s);

                }

                //Console.WriteLine("merged: "  + merged);
                File.WriteAllLines(mergedFilename, merged);
                return true;
            }
            catch (MergeConflictException)
            {
                return false;
            }
        }

        public string[] SupportedExtensions => new[] { ".txt" };
        public string Description => ".txt dummy resolver";
    }
}
