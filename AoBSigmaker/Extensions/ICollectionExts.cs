namespace AoBSigmaker.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // ReSharper disable once InconsistentNaming
    public static class ICollectionExts
    {
        #region Public Methods and Operators

        public static IEnumerable<IEnumerable<T>> Split<T>(this ICollection<T> self, int chunkSize)
        {
            var splitList = new List<List<T>>();
            var chunkCount = (int)Math.Ceiling(self.Count / (double)chunkSize);

            for (var c = 0; c < chunkCount; c++)
            {
                var skip = c * chunkSize;
                var take = skip + chunkSize;
                var chunk = new List<T>(chunkSize);

                for (var e = skip; (e < take) && (e < self.Count); e++)
                {
                    chunk.Add(self.ElementAt(e));
                }

                splitList.Add(chunk);
            }

            return splitList;
        }

        #endregion
    }
}