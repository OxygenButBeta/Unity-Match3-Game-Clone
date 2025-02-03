using System.Collections.Generic;
using System.Linq;
using O2.Extensions;
using UnityEngine;

namespace O2.Grid{
    /// <summary>
    /// Grid class that holds a 2D array of GridElement.
    /// It also provides methods to get and set values in the grid.
    /// A GridElement is a class that holds an item and a position in the grid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Grid<T> : WorldGrid{
        // The 2D array of GridElement.
        internal readonly GridElement<T>[,] gridArray;

        //Constructor
        public Grid(int width, int height, float cellSize, Vector3 origin) : base(width, height, cellSize, origin){
            gridArray = new GridElement<T>[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                gridArray[x, y] = new GridElement<T>(x, y);
        }

        /// <summary>
        /// Swap the values of the two GridElement.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void SwapValues(GridElement<T> start, GridElement<T> end){
            var startIndex = start.Index;
            var endIndex = end.Index;

            (start.Index, end.Index) = (end.Index, start.Index);

            gridArray[startIndex.x, startIndex.y] = end;
            gridArray[endIndex.x, endIndex.y] = start;
        }

        /// <summary>
        /// Set the value of the GridElement at the given index.
        /// Same As accessing with [][] (Indexer) operator.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GridElement<T> GetGridElementAt(int x, int y) => gridArray[x, y];

        /// <summary>
        /// Set the value of the GridElement at the given index.
        /// Same As accessing with [][] (Indexer) operator.
        /// (Extension Method for Vector2Int)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GridElement<T> GetGridElementAt(Vector2Int index) => gridArray[index.x, index.y];


        public IEnumerable<GridElement<T>> GetGridElements(IEnumerable<Vector2Int> indices){
            return indices.Select(GetGridElementAt);
        }
        public GridElement<T> GetGridElementWithWorldPosition(Vector3 worldPosition) =>
            GetGridElementAt(GetElementIndexFromWorldPosition(worldPosition));

        public override bool IsIndexWithinBounds(Vector2Int index) => gridArray.IsIndexWithinBounds(index.x, index.y);

        /// <summary>
        /// A Safe way to get the GridElement at the given index.
        /// If the index is out of bounds, it will return false.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        public bool TryToGetGridElementAt(Vector2Int index, out GridElement<T> gridElement){
            if (IsIndexWithinBounds(index)){
                gridElement = GetGridElementAt(index);
                return true;
            }

            gridElement = null;
            return false;
        }

        /// <summary>
        /// Try to get the GridElement at the given index.
        /// If the index is out of bounds, or the GridElement is disabled, it will return false.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        public bool GetGridElementIfNotStatic(Vector2Int index, out GridElement<T> gridElement){
            if (TryToGetGridElementAt(index, out gridElement))
                return !gridElement.IsStatic;
            return false;
        }

        /// <summary>
        /// Iterates through all the GridElement in the grid.
        /// If includeDisabledItems is false, it will skip the disabled items.
        /// </summary>
        /// <param name="includeStatics"></param>
        /// <returns></returns>
        public IEnumerable<GridElement<T>> IterateAll(bool includeStatics = true){
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++){
                if (includeStatics)
                    yield return GetGridElementAt(x, y);
                else{
                    var temp = GetGridElementAt(x, y);
                    if (!temp.IsStatic)
                        yield return temp;
                }
            }
        }
    }
}