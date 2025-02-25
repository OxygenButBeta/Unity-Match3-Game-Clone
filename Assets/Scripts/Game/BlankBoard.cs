using Match3;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class BlankBoard : MonoBehaviour{
    [SerializeField] GameObject blankPrefab;
    [SerializeField] GameBoardBase targetBoard;

    private void Awake(){
        for (var x = 0; x < targetBoard.gridData.width; x++)
        for (var y = 0; y < targetBoard.gridData.height; y++)
            Instantiate(blankPrefab, targetBoard.WorldGrid.GetWorldPosition(x, y), Quaternion.identity, transform)
                .isStatic = true;
    }
}