using UnityEngine;

namespace Match3.VFX{
    public class CameraScaler : MonoBehaviour{
        [SerializeField] Camera _camera;
        [SerializeField] GameBoardBase gameBoardBase;

        [SerializeField] float offset;

        private void Reset(){
            _camera = Camera.main;
        }

        private void Awake(){
            var center = gameBoardBase.GetWorldCenter();
            _camera.transform.position = new Vector3(center.x, center.y + offset, _camera.transform.position.z);
        }
    }
}