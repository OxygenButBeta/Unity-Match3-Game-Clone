using Match3;
using O2.Grid;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class BlankBoard : MonoBehaviour{
    [SerializeField] GameObject blankPrefab;
    [SerializeField] GameBoardBase targetBoard;

    private void Awake(){
        GridData gridData = targetBoard.gridData;
        for (var x = 0; x < gridData.width; x++)
        for (var y = 0; y < gridData.height; y++)
            Instantiate(blankPrefab, targetBoard.Grid.GetWorldPosition(x, y), Quaternion.identity, transform)
                .isStatic = true;
    }
}