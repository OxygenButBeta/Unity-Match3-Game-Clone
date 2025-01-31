using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Match3{
    /// <summary>
    /// This class is responsible for handling the input of the game board.
    /// </summary>
    [RequireComponent(typeof(GameBoardBase))]
    public sealed class GameBoardInputHandler : MonoBehaviour{
        [SerializeField] InputSystemUIInputModule inputSystemUiInputModule;
        [SerializeField] float AngleThresholdTolerance = 30f;

        [SerializeField, Required, Tooltip("Listener")]
        GameBoardBase gameBoardBase;

        /// <summary>
        /// The start/End and end points of the swipe.
        /// Converted to world space.
        /// </summary>
        Vector2 startPoint, endPoint;

        Camera _mainCamera;

        private void Awake(){
            // Subscribe to the input system events.
            _mainCamera = Camera.main;
            inputSystemUiInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
            inputSystemUiInputModule.leftClick.action.started += OnPointerDown;
            inputSystemUiInputModule.leftClick.action.canceled += OnPointerUp;
        }

        private void OnDisable(){
            // Unsubscribe from the input system events.
            inputSystemUiInputModule.leftClick.action.started -= OnPointerDown;
            inputSystemUiInputModule.leftClick.action.canceled -= OnPointerUp;
        }

        void OnPointerDown(InputAction.CallbackContext context){
            // Get the start point of the swipe.
            Vector2 position = Mouse.current.position.ReadValue();
            startPoint = _mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, -10));
        }

        void OnPointerUp(InputAction.CallbackContext context){
            // Get the end point of the swipe and send the move to the game board.
            // Send the move to the game board.
            gameBoardBase.ExecuteMove(new BoardSwipeActionData()
            {
                startPositionScreenToWorld = startPoint,
                DesignatedDirection =
                    DirectionUtility.DetermineDirection(GetDirectionAngle(Mouse.current.position.ReadValue()),
                        AngleThresholdTolerance)
            });
        }

        /// <summary>
        /// TODO: Move this method to a utility class.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        float GetDirectionAngle(Vector2 position){
            endPoint = _mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y));
            var angle = Vector2.SignedAngle(Vector2.up, endPoint - startPoint);
            return angle;
        }
    }
}