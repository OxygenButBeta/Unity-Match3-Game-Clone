using O2.Grid;
using UnityEngine;

namespace Match3{
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