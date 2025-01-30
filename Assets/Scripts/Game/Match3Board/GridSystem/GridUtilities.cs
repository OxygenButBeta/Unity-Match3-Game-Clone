using UnityEngine;

namespace O2.Grid
{
    /// <summary>
    /// Utility class to help with grid operations.
    /// </summary>
    public static class GridUtilities
    {
        /// <summary>
        /// Check if two grid elements are neighbours.
        /// </summary>
        /// <param name="firstCell"></param>
        /// <param name="secondCell"></param>
        /// <returns></returns>
        public static bool IsNeighbour(Vector2Int firstCell, Vector2Int secondCell)
        {
            if (Mathf.Abs(firstCell.x - secondCell.x) == 1 && Mathf.Abs(firstCell.y - secondCell.y) == 0)
                return true;
            
            return Mathf.Abs(firstCell.y - secondCell.y) == 1 && Mathf.Abs(firstCell.x - secondCell.x) == 0;
        }
    }
}