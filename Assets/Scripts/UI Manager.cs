using DG.Tweening;
using Match3;
using O2.Grid;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour{
    [SerializeField] private Match3Board board;
    [SerializeField] TMP_Text remainingMovesText;
    [SerializeField] TMP_Text doneText;

    public int MoveCount;
    [SerializeField] RectTransform levelCompletePanel0;
    [SerializeField] RectTransform levelCompletePanel1;
    [SerializeField] RectTransform levelCompletePanel2;

    Vector3 origin0, origin1, origin2;

    [SerializeField] GameObject[] enableOnActive;

    private void Awake(){
        foreach (var o in enableOnActive){
            o.gameObject.SetActive(true);
        }

        board.OnCandySwap += UpdateRemainingMoves;
        levelCompletePanel0.gameObject.SetActive(false);
        levelCompletePanel1.gameObject.SetActive(false);
        levelCompletePanel2.gameObject.SetActive(false);
        origin0 = levelCompletePanel0.anchoredPosition;
        origin1 = levelCompletePanel1.anchoredPosition;
        origin2 = levelCompletePanel2.anchoredPosition;

        levelCompletePanel0.anchoredPosition = new Vector2(levelCompletePanel0.anchoredPosition.x, -2000);
        levelCompletePanel1.anchoredPosition = new Vector2(levelCompletePanel1.anchoredPosition.x, -2000);
        levelCompletePanel2.anchoredPosition = new Vector2(levelCompletePanel2.anchoredPosition.x, -2000);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.S)){
            // level done
            doneText.text = "Good Job!";
            ShowCompPanel();
        }
    }

    private void ShowCompPanel(){
        levelCompletePanel0.gameObject.SetActive(true);
        levelCompletePanel1.gameObject.SetActive(true);
        levelCompletePanel2.gameObject.SetActive(true);

        levelCompletePanel0.DOAnchorPosY(origin0.y, 1.4f).onComplete = () => {
            levelCompletePanel0.DOAnchorPosY(-2000, 3f).SetDelay(3f);
        };
        levelCompletePanel1.DOAnchorPosY(origin1.y, 1.8f).onComplete = () => {
            levelCompletePanel1.DOAnchorPosY(-2000, 3f).SetDelay(3f);
        };
        levelCompletePanel2.DOAnchorPosY(origin2.y, 1.9f).onComplete = () => {
            levelCompletePanel2.DOAnchorPosY(-2000, 3f).SetDelay(3f);
        };
    }

    private void UpdateRemainingMoves(GridNode<Candy> arg1, GridNode<Candy> arg2){
        Debug.Log("Move Count : " + MoveCount);
        MoveCount--;
        if (MoveCount == 5){
            doneText.text = "Be careful! You have only 5 moves left!";
            ShowCompPanel();
        }

        remainingMovesText.text = "Moves : " + MoveCount;
    }
}