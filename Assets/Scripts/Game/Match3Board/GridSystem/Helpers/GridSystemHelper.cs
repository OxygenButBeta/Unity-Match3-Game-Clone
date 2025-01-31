using System;
using UnityEngine;

namespace O2.Grid{
    public static class GridSystemHelper{
        public static bool SimulateGravity<T>(Grid<T> grid, Predicate<GridElement<T>> fallCondition,
            Action<GridElement<T>> fallAction){
            bool hasFallen = false;
            foreach (var element in grid.IterateAll(false)){
                int temp = element.Index.y;
                while (temp > 0){
                    var above = grid.gridArray[element.Index.x, temp - 1];
                    if (above.IsFilled && !fallCondition(above))
                        break; // below element is not empty

                    temp--;
                }

                if (temp == element.Index.y)
                    continue; // no need to fall

                hasFallen = true;
                grid.SwapValues(element, grid.gridArray[element.Index.x, temp]);
                fallAction(element);
            }
            return hasFallen;
        }
    }
}