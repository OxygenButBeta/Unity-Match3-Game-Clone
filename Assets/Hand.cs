using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Sequence = DG.Tweening.Sequence;

public class Hand : MonoBehaviour{
    [SerializeField] GameObject hand;
    [SerializeField] Camera _mainCamera;
    [SerializeField, Required] InputSystemUIInputModule inputSystemUiInputModule;

    private void Awake(){
        _mainCamera = Camera.main;
        inputSystemUiInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
        inputSystemUiInputModule.leftClick.action.started += OnPointerDown;
        inputSystemUiInputModule.leftClick.action.canceled += OnPointerUp;
    }

    private void OnPointerUp(InputAction.CallbackContext obj){
        Sequence sequence = DOTween.Sequence();
        sequence.Append(hand.transform.DOScale(Vector3.one * .6f, .2f)).Join(hand.transform.DORotate(Vector3.forward*30, .2f));
    }

    private void OnPointerDown(InputAction.CallbackContext obj){
        Sequence sequence = DOTween.Sequence();
        sequence.Append(hand.transform.DOScale(Vector3.one * .4f, .2f)).Join(hand.transform.DORotate(Vector3.forward*50, .2f));
    }

    private void Update(){
        var mousePos = Input.mousePosition;
        hand.transform.position =
            _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,50));
    }
}