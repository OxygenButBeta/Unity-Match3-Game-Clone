using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3{
    public class Match3Board : ActiveGameBoard<Candy>{
        [SerializeField] ScriptableCandy[] scriptableCandies;
        [SerializeField] Candy prefab;

        Vector2Int dragStartIndex;
        Vector2Int dragEndIndex;
        bool ignoreInput;

        #region Trash

        void SetCameraToCenter(){
            var center = grid.GetGridElementAt(width / 2, height / 2);
            var y = center.Item.transform.position.y + center.Item.transform.position.y / 2;
            var camera = Camera.main;
            camera.transform.position = new Vector3(camera.transform.position.x, y, camera.transform.position.z);
        }

        #endregion

        private void Awake(){
            grid = new Grid<Candy>(width, height, cellSize, origin);
            foreach (var element in grid.IterateAll()){
                element.Item = Instantiate(prefab, grid.GetWorldPosition(element), Quaternion.identity, transform);
                element.Item.SetScriptableCandy(scriptableCandies[Random.Range(0, scriptableCandies.Length)]);
            }

            SetCameraToCenter();
        }

        void SetTransform(GridElement<Candy> element){
            element.Item.transform.DOMove(grid.GetWorldPosition(element), 0.5f).SetEase(Ease.OutBack);
        }

        bool IsCandyExploded(GridElement<Candy> element) => element.Item.IsExploded;

        protected override void ExecuteValidatedMove(GridElement<Candy> firstGridElement,
            GridElement<Candy> secondGridElement){
            if (ignoreInput)
                return;

            ignoreInput = true;
            Swap(firstGridElement, secondGridElement).Forget();
        }

        async UniTaskVoid Swap(GridElement<Candy> firstGridElement, GridElement<Candy> secondGridElement){
            await SwapWorldPosition(firstGridElement, secondGridElement);
            if (!HasAnyNeighbour(firstGridElement, secondGridElement,
                    DirectionUtility.GetRelativeDirection(secondGridElement, firstGridElement))){
                await SwapWorldPosition(firstGridElement, secondGridElement);
                ignoreInput = false;
                return;
            }

            grid.SwapValues(firstGridElement, secondGridElement);

            FindMatchesEndExplode();
            if (GridSystemHelper.SimulateGravity(grid, IsCandyExploded, SetTransform)){
                FindMatchesEndExplode();
            }

            await SpawnNewCandies();
            ignoreInput = false;
        }

        private async UniTask SpawnNewCandies(){
            foreach (var element in grid.IterateAll(true)){
                if (!element.IsStatic){
                    continue;
                }

                element.IsStatic = false;
                element.Item.gameObject.SetActive(true);
                element.Item.IsExploded = false;
                element.IsFilled = true;
                element.Item.SetScriptableCandy(scriptableCandies[Random.Range(0, scriptableCandies.Length)]);
                var pos = grid.GetWorldPosition(element);
                element.Item.transform.position = new Vector3(pos.x, 10 + pos.y, pos.z);
                await UniTask.Delay(30);
                element.Item.transform.DOMove(pos, .6f).SetEase(Ease.InOutQuad);
            }
        }

        void FindMatchesEndExplode(){
            foreach (var gridItem in FindMatches(3)){
                if (gridItem.IsStatic)
                    continue;

                gridItem.IsStatic = true;
                gridItem.Item.gameObject.SetActive(false);
                gridItem.Item.IsExploded = true;
                gridItem.IsFilled = false;
            }
        }

        async UniTask SwapWorldPosition(GridElement<Candy> start, GridElement<Candy> end){
            start.Item.transform.DOMove(end.Item.transform.position, 0.5f).SetEase(Ease.OutBack);
            end.Item.transform.DOMove(start.Item.transform.position, 0.5f).SetEase(Ease.OutBack);
            await UniTask.Delay(500);
        }

        bool HasAnyNeighbour(GridElement<Candy> element, Vector2Int targetPosition, Direction ignoreDirection){
            foreach (Direction direction in DirectionUtility.AllDirections){
                if (direction == ignoreDirection)
                    continue;

                if (IsMatchingWithNeighborAtPosition(element, targetPosition, direction.ToVector2Int()))
                    return true;
            }

            return false;
        }

        bool IsMatchingWithNeighborAtPosition(GridElement<Candy> element, Vector2Int targetPosition,
            Vector2Int direction){
            if (!grid.IsIndexWithinBounds(targetPosition + direction))
                return false;

            return grid.GetGridElementIfIsNotDisabled(targetPosition + direction, out var gridItem) &&
                   gridItem.Item.scriptableCandy.Equals(element.Item.scriptableCandy);
        }

        int FindMatchesOnGivenDirection(Vector2Int origin, Direction direction, int myOrder){
            if (grid.TryToGetGridElementAt(origin + direction.ToVector2Int(), out var element)){
                if (element.Item.scriptableCandy.Equals(grid.GetGridElementAt(origin).Item.scriptableCandy)){
                    return FindMatchesOnGivenDirection(element.Index, direction, myOrder + 1);
                }

                return myOrder;
            }

            Matrix4x4 x = transform.localToWorldMatrix;
            return myOrder;
        }

        bool IsMatchingWithNeighbor(GridElement<Candy> element, Direction from, out GridElement<Candy> neighbour){
            foreach (Direction direction in DirectionUtility.AllDirections){
                if (direction == from)
                    continue;

                if (grid.TryToGetGridElementAt(element.Index + from.ToVector2Int(), out neighbour)){
                    if (neighbour.Item.scriptableCandy.Equals(element.Item.scriptableCandy))
                        return true;
                }
            }

            neighbour = null;
            return false;
        }


        IEnumerable<GridElement<Candy>> FindMatches(int matchLengthMin){
            var matches = new List<GridElement<Candy>>();

            for (var y = 0; y < height; y++){
                var currentMatch = new List<GridElement<Candy>>();

                for (var x = 0; x < width; x++){
                    var current = grid.GetGridElementAt(x, y);
                    if (current.IsStatic)
                        continue;

                    if (currentMatch.Count == 0 ||
                        current.Item.scriptableCandy.Equals(currentMatch[0].Item.scriptableCandy)){
                        currentMatch.Add(current);
                    }
                    else{
                        if (currentMatch.Count >= matchLengthMin)
                            matches.AddRange(currentMatch);

                        currentMatch.Clear();
                        currentMatch.Add(current);
                    }
                }

                if (currentMatch.Count >= matchLengthMin)
                    matches.AddRange(currentMatch);
            }

            for (var x = 0; x < width; x++){
                var currentMatch = new List<GridElement<Candy>>();

                for (var y = 0; y < height; y++){
                    var current = grid.GetGridElementAt(x, y);
                    if (current.IsStatic)
                        continue;

                    if (currentMatch.Count == 0 ||
                        current.Item.scriptableCandy.Equals(currentMatch[0].Item.scriptableCandy)){
                        currentMatch.Add(current);
                    }
                    else{
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