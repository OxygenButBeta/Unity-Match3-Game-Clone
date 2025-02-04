using O2.Grid;

namespace Match3{
    public interface ICandyBehaviour{
        void OnExplode(Match3Board board, GridElement<Candy> selfGridElement);
    }
}