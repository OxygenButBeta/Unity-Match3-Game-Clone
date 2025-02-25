using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace O2.Grid{
    /// <summary>
    /// Utility class to help with grid operations.
    /// </summary>
    public static class Matching{
        /// <summary>
        /// Check if two grid elements are neighbours.
        /// </summary>
        /// <param name="firstCell"></param>
        /// <param name="secondCell"></param>
        /// <returns></returns>
        public static bool IsNeighbour(Vector2Int firstCell, Vector2Int secondCell){
            if (Mathf.Abs(firstCell.x - secondCell.x) == 1 && Mathf.Abs(firstCell.y - secondCell.y) == 0)
                return true;

            return Mathf.Abs(firstCell.y - secondCell.y) == 1 && Mathf.Abs(firstCell.x - secondCell.x) == 0;
        }

       

        public static bool HasConsecutiveTrues(bool[] arr, int n){
            if (arr.Length < n) return false;

            int count = 0;

            foreach (bool value in arr){
                count = value ? count + 1 : 0;
                if (count >= n) return true;
            }

            return false;
        }

        public static bool FindMatchesNonAlloc<T>(
            [NotNull] HashSet<Vector2Int> matches,
            IEqualityComparer<T> comparer,
            Grid<T> grid,
            bool allow2x2Matches = true){
            matches.Clear();

            var width = grid.gridData.width;
            var height = grid.gridData.height;

            for (var i = 0; i < width * height; i++){
                var x = i % width;
                var y = i / width;

                GridNode<T> currentNode = grid.GetGridElementAt(x, y);

                // Horizontal Match
                if (x < width - 2 &&
                    comparer.Equals(grid.GetGridElementAt(x + 1, y).Item, currentNode.Item) &&
                    comparer.Equals(grid.GetGridElementAt(x + 2, y).Item, currentNode.Item)){
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x + 1, y));
                    matches.Add(new Vector2Int(x + 2, y));
                }

                // Vertical Match
                if (y < height - 2 &&
                    comparer.Equals(grid.GetGridElementAt(x, y + 1).Item, currentNode.Item) &&
                    comparer.Equals(grid.GetGridElementAt(x, y + 2).Item, currentNode.Item)){
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x, y + 1));
                    matches.Add(new Vector2Int(x, y + 2));
                }

                // 2x2 Square Match
                if (allow2x2Matches && x < width - 1 && y < height - 1 &&
                    comparer.Equals(grid.GetGridElementAt(x + 1, y).Item, currentNode.Item) &&
                    comparer.Equals(grid.GetGridElementAt(x, y + 1).Item, currentNode.Item) &&
                    comparer.Equals(grid.GetGridElementAt(x + 1, y + 1).Item, currentNode.Item)){
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x + 1, y));
                    matches.Add(new Vector2Int(x, y + 1));
                    matches.Add(new Vector2Int(x + 1, y + 1));
                }
            }

            return matches.Count > 0;
        }
    }

}