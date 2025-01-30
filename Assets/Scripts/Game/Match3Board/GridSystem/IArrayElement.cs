using UnityEngine;

namespace O2.Grid
{
    /// <summary>
    /// Simple interface to represent an element in an array to abstract the index.
    /// </summary>
    public interface IArrayElement
    {
        /// <summary>
        /// Index of the grid element in the grid. (x, y)
        /// </summary>
        public Vector2Int Index { get; set; }
    }
}