using Cysharp.Threading.Tasks;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class RandomExplosionBehaviour : ICandyBehaviour{
        [SerializeField] int maxExplosions = 6;
        
        public UniTask OnExplodeTask(Match3Board board, GridNode<Candy> selfGridNode){
            for (int i = 0; i < maxExplosions; i++){
                {
                    var x = Random.Range(0, board.Grid.gridData.width);
                    var y = Random.Range(0, board.Grid.gridData.height);

                    if (!board.Grid.TryToGetGridElementAt(new(x, y), out var element)){
                        maxExplosions++;
                        continue;
                    }


                    if (element.Index == selfGridNode.Index)
                        continue;


                    if (!element.Item.scriptableCandy.Equals(selfGridNode.Item.scriptableCandy)){
                        if (!element.Item.IsExploded){
                            element.IsFilled = false;
                            element.Item.ExplodeImmediate();
                        }
                    }
                }
            }
            return UniTask.CompletedTask;
        }
    }
}