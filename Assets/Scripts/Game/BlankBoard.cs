using Match3;
using O2.Grid;
using UnityEngine;

public class BlankBoard : PassiveGameBoard
{
    [SerializeField] GameObject blankPrefab;

    private void Awake()
    {
        grid = new Grid<int>(width, height, cellSize, origin);
        for (var x = 0; x < grid.width; x++)
        for (var y = 0; y < grid.height; y++)
            Instantiate(blankPrefab, grid.GetWorldPosition(x, y), Quaternion.identity, transform).isStatic = true;
    }
}