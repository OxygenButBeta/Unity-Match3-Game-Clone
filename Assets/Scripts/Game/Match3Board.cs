using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3{
    public class Match3Board : ActiveGameBoard<Candy>{
        [SerializeField] ScriptableCandy[] scriptableCandies;
        [SerializeField] Candy prefab;
        [SerializeField, TabGroup("Game Options")] float fallDuration = 0.5f;
        [SerializeField, TabGroup("Game Options")] bool allow2x2Matches = true;

        Vector2Int dragStartIndex;
        Vector2Int dragEndIndex;
        bool ignoreInput;
        GridGravityOptions<Candy> gridGravityOptions;

        #region Trash

        void SetCameraToCenter(){
            var center = grid.GetGridElementAt(width / 2, height / 2);
            var y = center.Item.transform.position.y + center.Item.transform.position.y / 2;
            var camera = Camera.main;
            camera.transform.position = new Vector3(camera.transform.position.x, y, camera.transform.position.z);
        }

        #endregion


        protected override void OnAwake(){
            foreach (var element in grid.IterateAll()){
                element.Item = Instantiate(prefab, grid.GetWorldPosition(element), Quaternion.identity, transform);
                element.Item.SetScriptableCandy(scriptableCandies[Random.Range(0, scriptableCandies.Length)]);
            }

            gridGravityOptions = new GridGravityOptions<Candy>(
                (element) => element.Item.IsExploded,
                (element) => element.Item.transform.DOMove(grid.GetWorldPosition(element), fallDuration)
                    .SetEase(Ease.OutBack));

            SetCameraToCenter();
            ProcessMatches().Forget();
        }

        protected override void ExecuteValidatedMove(GridElement<Candy> firstGridElement,
            GridElement<Candy> secondGridElement){
            if (ignoreInput)
                return;

            ignoreInput = true;
            ExecuteSwipe(firstGridElement, secondGridElement).Forget();
        }

        async UniTaskVoid ExecuteSwipe(GridElement<Candy> firstGridElement, GridElement<Candy> secondGridElement){
            // Swap the values and Swap in the visual
            await VisualGridUtilities.SwapWorldPosition(firstGridElement, secondGridElement);
            grid.SwapValues(firstGridElement, secondGridElement);

            HashSet<Vector2Int> matchResult = FindMatches();

            // Swap back if no match
            if (matchResult.Count == 0){
                await VisualGridUtilities.SwapWorldPosition(firstGridElement, secondGridElement);
                grid.SwapValues(firstGridElement, secondGridElement);
                ignoreInput = false;
                return;
            }

            await ProcessMatches();
            ignoreInput = false;
        }

        async UniTask ProcessMatches(){
            while (true){
                HashSet<Vector2Int> matchIndices = FindMatches();

                if (matchIndices.Count == 0)
                    break; // No more matches

                await ExplodeMatches(matchIndices);

                // Gravity Simulation
                var hasFallen = GridSystemUtilities.SimulateGravity(grid, gridGravityOptions);

                if (hasFallen)
                    await UniTask.Delay(Convert.ToInt32(fallDuration * 1000)); // Wait for the fall to finish

                await SpawnNewCandies();
            }
        }

        private async UniTask SpawnNewCandies(){
            List<UniTask> spawnTasks = new();

            foreach (var element in grid.IterateAll(true)){
                if (!element.IsStatic)
                    continue;

                element.IsStatic = false;
                element.Item.gameObject.SetActive(true);
                element.Item.IsExploded = false;
                element.IsFilled = true;
                element.Item.SetScriptableCandy(scriptableCandies[Random.Range(0, scriptableCandies.Length)]);
                var pos = grid.GetWorldPosition(element);
                element.Item.transform.position = new Vector3(pos.x, 10 + pos.y, pos.z);
                spawnTasks.Add(element.Item.transform.DOMove(pos, .6f).SetEase(Ease.InOutQuad).ToUniTask());
            }

            await UniTask.WhenAll(spawnTasks);
        }

        async UniTask ExplodeMatches(HashSet<Vector2Int> matchIndices){
            List<UniTask> explodeTasks = new();
            foreach (var gridItem in grid.GetGridElements(matchIndices)){
                if (gridItem.IsStatic) // Don't explode static items
                    continue;
                explodeTasks.Add(CreateAnim(gridItem));
            }

            await UniTask.WhenAll(explodeTasks);

            Debug.Log("Exploded");

            foreach (var gridItem in grid.GetGridElements(matchIndices)){
                gridItem.IsStatic = true;
                gridItem.Item.gameObject.SetActive(false);
                gridItem.Item.transform.localScale = Vector3.one * .6f;
                gridItem.Item.IsExploded = true;
                gridItem.IsFilled = false;
            }
        }

        private UniTask CreateAnim(GridElement<Candy> gridItem){
            return gridItem.Item.transform
                .DOScale(0f, 0.3f)
                .SetEase(Ease.InOutQuad).ToUniTask();
        }


        HashSet<Vector2Int> FindMatches(){
            HashSet<Vector2Int> match = new();

            for (int i = 0; i < width * height; i++){
                var x = i % width;
                var y = i / width;
                GridElement<Candy> currentElement = grid.GetGridElementAt(x, y);

                // Horizontal
                if (x < width - 2 &&
                    grid.GetGridElementAt(x + 1, y).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x + 2, y).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy)){
                    match.Add(new Vector2Int(x, y));
                    match.Add(new Vector2Int(x + 1, y));
                    match.Add(new Vector2Int(x + 2, y));
                }

                // Vertical
                if (y < height - 2 &&
                    grid.GetGridElementAt(x, y + 1).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x, y + 2).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy)){
                    match.Add(new Vector2Int(x, y));
                    match.Add(new Vector2Int(x, y + 1));
                    match.Add(new Vector2Int(x, y + 2));
                }

                // 2x2 Square
                if (!allow2x2Matches)
                    continue;
                if (x < width - 1 && y < height - 1 &&
                    grid.GetGridElementAt(x + 1, y).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x, y + 1).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x + 1, y + 1).Item.scriptableCandy
                        .Equals(currentElement.Item.scriptableCandy)){
                    match.Add(new Vector2Int(x, y));
                    match.Add(new Vector2Int(x + 1, y));
                    match.Add(new Vector2Int(x, y + 1));
                    match.Add(new Vector2Int(x + 1, y + 1));
                }
            }

            return match;
        }
    }
}