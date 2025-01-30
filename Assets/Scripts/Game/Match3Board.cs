using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;

namespace Match3
{
    public class Match3Board : ActiveGameBoard<Candy>
    {
        [SerializeField] ScriptableCandy[] scriptableCandies;
        [SerializeField] Candy prefab;


        Vector2Int dragStartIndex;
        Vector2Int dragEndIndex;

        bool ignoreInput;

        private void Awake()
        {
            grid = new Grid<Candy>(width, height, cellSize, origin);
            foreach (var element in grid.GetActiveElements())
            {
                var obj = Instantiate(prefab, grid.GetWorldPosition(element), Quaternion.identity, transform);
                element.Item = obj;
                obj.SetScriptableCandy(scriptableCandies[Random.Range(0, scriptableCandies.Length)]);
            }

            var center = grid.GetGridElementAt(width / 2, height / 2);
            var y = center.Item.transform.position.y + center.Item.transform.position.y / 2;
            var camera = Camera.main;
            camera.transform.position = new Vector3(camera.transform.position.x, y, camera.transform.position.z);
        }


        protected override IBoardMoveActionValidator<Candy> GetValidator()
        {
            return new Match3MoveActionValidator();
        }

        protected override void ExecuteValidatedMove(GridElement<Candy> firstGridElement,
            GridElement<Candy> secondGridElement)
        {
            if (ignoreInput)
                return;

            ignoreInput = true;
            Swap(firstGridElement, secondGridElement).Forget();
        }

        async UniTaskVoid Swap(GridElement<Candy> firstGridElement, GridElement<Candy> secondGridElement
        )
        {
            await SwapWorldPosition(firstGridElement, secondGridElement);
            if (!HasAnyNeighbour(firstGridElement, secondGridElement,
                    DirectionUtility.GetRelativeDirection(secondGridElement, firstGridElement)))
            {
                await SwapWorldPosition(firstGridElement, secondGridElement);
                ignoreInput = false;
                return;
            }

            grid.SwapValues(firstGridElement, secondGridElement);
            foreach (var gridItem in FindMatches(3))
            {
                if (gridItem.IsDisabled)
                    continue;

                gridItem.IsDisabled = true;
                gridItem.Item.gameObject.SetActive(false);
            }

            ignoreInput = false;
        }

        async UniTask SwapWorldPosition(GridElement<Candy> start, GridElement<Candy> end)
        {
            start.Item.transform.DOMove(end.Item.transform.position, 0.5f).SetEase(Ease.OutBack);
            end.Item.transform.DOMove(start.Item.transform.position, 0.5f).SetEase(Ease.OutBack);
            await UniTask.Delay(500);
        }

        bool HasAnyNeighbour(GridElement<Candy> element, Vector2Int targetPosition, Direction ignoreDirection)
        {
            foreach (var direction in DirectionUtility.AllDirections)
            {
                if (direction == ignoreDirection)
                    continue;

                if (IsMatchingWithNeighborAtPosition(element, targetPosition, direction.ToVector2Int()))
                    return true;
            }

            return false;
        }

        bool IsMatchingWithNeighborAtPosition(GridElement<Candy> element, Vector2Int targetPosition,
            Vector2Int direction)
        {
            if (!grid.IsIndexWithinBounds(targetPosition + direction))
                return false;

            if (grid.GetGridElementIfIsNotDisabled(targetPosition + direction, out var gridItem))
                return gridItem.Item.scriptableCandy.Equals(element.Item.scriptableCandy);

            return false;
        }


        IEnumerable<GridElement<Candy>> FindMatches(int matchLengthMin)
        {
            var matches = new List<GridElement<Candy>>();

            for (var y = 0; y < height; y++)
            {
                var currentMatch = new List<GridElement<Candy>>();

                for (var x = 0; x < width; x++)
                {
                    var current = grid.GetGridElementAt(x, y);
                    if (current.IsDisabled)
                        continue;

                    if (currentMatch.Count == 0 ||
                        current.Item.scriptableCandy.Equals(currentMatch[0].Item.scriptableCandy))
                    {
                        currentMatch.Add(current);
                    }
                    else
                    {
                        if (currentMatch.Count >= matchLengthMin)
                            matches.AddRange(currentMatch);

                        currentMatch.Clear();
                        currentMatch.Add(current);
                    }
                }

                if (currentMatch.Count >= matchLengthMin)
                    matches.AddRange(currentMatch);
            }

            for (var x = 0; x < width; x++)
            {
                var currentMatch = new List<GridElement<Candy>>();

                for (var y = 0; y < height; y++)
                {
                    var current = grid.GetGridElementAt(x, y);
                    if (current.IsDisabled)
                        continue;

                    if (currentMatch.Count == 0 ||
                        current.Item.scriptableCandy.Equals(currentMatch[0].Item.scriptableCandy))
                    {
                        currentMatch.Add(current);
                    }
                    else
                    {
                        if (currentMatch.Count >= matchLengthMin)
                            matches.AddRange(currentMatch);

                        currentMatch.Clear();
                        currentMatch.Add(current);
                    }
                }

                if (currentMatch.Count >= matchLengthMin)
                    matches.AddRange(currentMatch);
            }

            return
                matches;
        }
    }
}