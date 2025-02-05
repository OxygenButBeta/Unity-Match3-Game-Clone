using O2.Grid;
using UnityEngine;

namespace Match3{
    public class ColumnExplosionBehaviour : ICandyBehaviour{
        public void OnExplode(Match3Board board, GridElement<Candy> selfGridElement){
            // Explode the row
            Vector2Int gridPos = selfGridElement;
            for (var y = 0; y < board.grid.height; y++){
                GridElement<Candy> element = board.grid.GetGridElementAt(gridPos.x, y);

                if (element.Index == gridPos)
                    continue;

                if (!element.Item.IsExploded){
                    element.IsFilled = false;
                    element.Item.ExplodeImmediate(true);
                }
            }
        }
    }
}