﻿using UnityEngine;

namespace O2.Grid{
    /// <summary>
    /// A generic class to represent an element in a grid.
    /// It contains the item, index and a flag to disable the element.
    /// It also provides bunch of helper methods to work with the grid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GridNode<T>{
        /// The item of the grid element.
        public T Item;

        /// If True, the grid element is static and cannot be moved in the grid.
        public bool IsStatic;

        /// A flag to disable the grid element.
        public bool IsFilled = true;

        /// The index of the element in the grid. [x, y]
        public Vector2Int Index;

        public GridNode(int x, int y) => Index = new Vector2Int(x, y);

        /// <summary>
        /// Check if two grid elements are neighbours.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsNeighbourOf(GridNode<T> other) => Matching.IsNeighbour(this, other);
        public static implicit operator Vector2Int(GridNode<T> gridNode) => gridNode.Index;
        public static implicit operator T(GridNode<T> gridNode) => gridNode.Item;
    }
}