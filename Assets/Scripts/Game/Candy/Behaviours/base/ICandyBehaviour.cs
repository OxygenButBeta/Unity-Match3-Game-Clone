using Cysharp.Threading.Tasks;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public interface ICandyBehaviour{
        UniTask OnExplodeTask(Match3Board board, GridNode<Candy> selfGridNode);
    }
}