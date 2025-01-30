using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Match3
{
    /// <summary>
    /// This class is responsible for handling the input of the game board.
    /// </summary>
    public sealed class GameBoardInputHandler : MonoBehaviour
    {
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

        // Subscribe to the input system events.
        private void Awake()
        {
            _mainCamera = Camera.main;
            inputSystemUiInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
            inputSystemUiInputModule.leftClick.action.started += OnPointerDown;
            inputSystemUiInputModule.leftClick.action.canceled += OnPointerUp;
        }

        // Unsubscribe from the input system events.
        private void OnDisable()
        {
            inputSystemUiInputModule.leftClick.action.started -= OnPointerDown;
            inputSystemUiInputModule.leftClick.action.canceled -= OnPointerUp;
        }

        // Get the start point of the swipe.
        void OnPointerDown(InputAction.CallbackContext context)
        {
            Vector2 position = Mouse.current.position.ReadValue();
            startPoint = _mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, -10));
        }

        // Get the end point of the swipe and send the move to the game board.
        void OnPointerUp(InputAction.CallbackContext context)
        {
            // Send the move to the game board.
            gameBoardBase.ExecuteMove(new BoardActionMove()
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
        float GetDirectionAngle(Vector2 position)
        {
            endPoint = _mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y));
            var angle = Vector2.SignedAngle(Vector2.up, endPoint - startPoint);
            return angle;
        }
    }
}