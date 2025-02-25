using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class ColumnExplosionBehaviour : ICandyBehaviour{
        [SerializeField] private float delay;

        readonly List<UniTask> tasks = new();

        public async UniTask OnExplodeTask(Match3Board board, GridNode<Candy> selfGridNode){
            // Explode the row
            Vector2Int gridPos = selfGridNode;
            for (var y = 0; y < board.Grid.gridData.height; y++){
                GridNode<Candy> node = board.Grid.GetGridElementAt(gridPos.x, y);


                if (node.Index == gridPos){
                    node.Item.transform.DOScale(Vector3.one * .75f, delay);
                    continue;
                }

                if (!node.Item.IsExploded){
                    node.Item.transform.DOScale(Vector3.one * .75f, delay).onComplete = () => {
                        node.IsFilled = false;
                        tasks.Add(node.Item.ExplodeAsync(board, selfGridNode));
                    };
                }
            }

            tasks.Add(UniTask.WaitForSeconds(delay));
            await UniTask.WhenAll(tasks);
        }
    }
}