using O2.Grid;
using UnityEngine;

namespace Match3{
    public class RowExplosionBehaviour : ICandyBehaviour{
        public void OnExplode(Match3Board board, GridElement<Candy> selfGridElement){
            // Explode the row
            Vector2Int gridPos = selfGridElement;
            for (int x = 0; x < board.grid.width; x++){
                var element = board.grid.GetGridElementAt(x, gridPos.y);

                if (element.Index == gridPos){
                    continue;
                }

                if (!element.Item.IsExploded)
                    element.Item.Explode(board, element);
            }
        }
    }
}