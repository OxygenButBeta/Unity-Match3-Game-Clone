using O2.Grid;

namespace Match3
{
    /// <summary>
    /// Passive game board that does not require any user input.
    /// It Does not contain the logic to execute a move on the board.
    /// </summary>
    public abstract class PassiveGameBoard : GameBoardBase
    {
        protected WorldGrid grid;
    }
}