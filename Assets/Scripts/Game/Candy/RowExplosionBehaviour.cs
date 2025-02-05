using O2.Grid;
using UnityEngine;

namespace Match3{
    public class RowExplosionBehaviour : ICandyBehaviour{
        public void OnExplode(Match3Board board, GridElement<Candy> selfGridElement){
            // Explode the row
            Vector2Int gridPos = selfGridElement;
            for (var x = 0; x < board.grid.width; x++){
                GridElement<Candy> element = board.grid.GetGridElementAt(x, gridPos.y);

                if (element.Index == gridPos)
                    continue;

                if (!element.Item.IsExploded){
                    element.IsFilled = false;
                    element.Item.ExplodeImmediate(true);
                }
            }
        }
    }

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

    public class CircleExplosionBehaviour : ICandyBehaviour{
        [SerializeField] int radius = 1;

        public void OnExplode(Match3Board board, GridElement<Candy> selfGridElement){
            Vector2Int gridPos = selfGridElement.Index;

            for (var x = -radius; x <= radius; x++)
            for (var y = -radius; y <= radius; y++){
                Vector2Int targetPos = gridPos + new Vector2Int(x, y);

                if (!board.grid.IsIndexWithinBounds(targetPos))
                    continue;

                if (!(Vector2Int.Distance(gridPos, targetPos) <= radius))
                    continue;

                GridElement<Candy> element = board.grid.GetGridElementAt(targetPos.x, targetPos.y);
                if (element.Index == gridPos)
                    continue;

                if (element.Item.IsExploded)
                    continue;

                element.IsFilled = false;
                element.Item.ExplodeImmediate(true);
            }
        }
    }

    public class SameTypeExplosionBehaviour : ICandyBehaviour{
        public void OnExplode(Match3Board board, GridElement<Candy> selfGridElement){
            foreach (var element in board.grid.IterateAll()){
                if (element.Index == selfGridElement.Index)
                    continue;

                if (!element.Item.scriptableCandy.Equals(selfGridElement.Item.scriptableCandy)){
                    continue;
                }

                if (!element.Item.IsExploded){
                    element.IsFilled = false;
                    element.Item.ExplodeImmediate(true);
                }
            }
        }
    }

    public class RandomExplosionBehaviour : ICandyBehaviour{
        [SerializeField] int maxExplosions = 6;

        public void OnExplode(Match3Board board, GridElement<Candy> selfGridElement){
            for (int i = 0; i < maxExplosions; i++){
                {
                    var x = Random.Range(0, board.grid.width);
                    var y = Random.Range(0, board.grid.height);

                    if (!board.grid.TryToGetGridElementAt(new(x, y), out var element)){
                        maxExplosions++;
                        continue;
                    }


                    if (element.Index == selfGridElement.Index)
                        continue;


                    if (!element.Item.scriptableCandy.Equals(selfGridElement.Item.scriptableCandy)){
                        if (!element.Item.IsExploded){
                            element.IsFilled = false;
                            element.Item.ExplodeImmediate(true);
                        }
                    }
                }
            }
        }
    }
}