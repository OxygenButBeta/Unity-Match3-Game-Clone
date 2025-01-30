using System;

namespace O2.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// A helper method to check if the given index is within the bounds of the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsIndexWithinBounds(this Array array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        /// <summary>
        /// Checks if the given index is within the bounds of the array.
        /// Multi-dimensional arrays are supported.
        /// But the number of dimensions of the array must be equal to the number of bounds.
        /// Otherwise, an ArgumentException will be thrown.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="boundsInts"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsIndexWithinBounds(this Array array, params int[] boundsInts)
        {
            if (array.Rank != boundsInts.Length)
                throw new ArgumentException(
                    "The number of dimensions of the array must be equal to the number of bounds.");

            for (int i = 0; i < array.Rank; i++)
            {
                if (!array.WithinBoundsInDimension(boundsInts[i], i))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///  A helper method to check if the given index is within the bounds of the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        static bool WithinBoundsInDimension(this Array array, int index, int dimension)
        {
            return index >= 0 && index < array.GetLength(dimension);
        }
    }
}