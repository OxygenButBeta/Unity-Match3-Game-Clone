using O2.Grid;

namespace Match3{
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
}