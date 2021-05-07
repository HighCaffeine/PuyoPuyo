//뿌요가 도착하는 곳을 알려줍니다.
//회전시 값이 이상한 문제가 있습니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuyoFinishPoint : MonoBehaviour
{
    private GameController gameController;
    private PuyoController puyoController;
    private PuyoDataMethod puyoDataMethod;

    private Image bottomFinishPointImage;
    private Image upperFinishPointImage;

    private bool bottomMaxIndex;
    private bool upperMaxIndex;
    
    private float bottomYPos;
    private float upperYPos;

    private int bottomCount;
    private int upperCount;
    private int upper = 0;
    private int bottom = 0;
    private int bottomColorCode;
    private int upperColorCode;

    [SerializeField] private Transform[] finishPointPosList = new Transform[6];

    private void Awake()
    {
        gameController = GetComponent<GameController>();
        puyoController = GetComponent<PuyoController>();
        puyoDataMethod = GetComponent<PuyoDataMethod>();
    }
    
    public void SetPuyoFinishPoint(Transform bottomPuyo, Transform upperPuyo, Transform bottomFinishPoint, Transform upperFinishPoint, 
                                    Puyo bottomPuyoData, Puyo upperPuyoData, Image bottomFinishPointImage, Image upperFinishPointImage)
    {
        bottomMaxIndex = false;
        upperMaxIndex = false;
        bottomFinishPoint.gameObject.SetActive(true);
        upperFinishPoint.gameObject.SetActive(true);
        bottomFinishPoint.localScale = puyoController.SetNewVector2(0.5f, 0.5f);
        upperFinishPoint.localScale = puyoController.SetNewVector2(0.5f, 0.5f);
        bottomColorCode = puyoDataMethod.StringColorToIntColorCode(bottomPuyoData.puyoData.color);
        upperColorCode = puyoDataMethod.StringColorToIntColorCode(upperPuyoData.puyoData.color);

        switch (bottomColorCode)
        {
            case 1:
                bottomFinishPointImage.sprite = puyoController.puyoFinishPointSprites[0];
                break;
            case 2:
                bottomFinishPointImage.sprite = puyoController.puyoFinishPointSprites[1];
                break;
            case 3:
                bottomFinishPointImage.sprite = puyoController.puyoFinishPointSprites[2];
                break;
            case 4:
                bottomFinishPointImage.sprite = puyoController.puyoFinishPointSprites[3];
                break;
        }

        switch (upperColorCode)
        {
            case 1:
                upperFinishPointImage.sprite = puyoController.puyoFinishPointSprites[0];
                break;
            case 2:
                upperFinishPointImage.sprite = puyoController.puyoFinishPointSprites[1];
                break;
            case 3:
                upperFinishPointImage.sprite = puyoController.puyoFinishPointSprites[2];
                break;
            case 4:
                upperFinishPointImage.sprite = puyoController.puyoFinishPointSprites[3];
                break;
        }

        SetFinishPointYPos(bottomPuyoData, upperPuyoData, bottomFinishPoint, upperFinishPoint);
    }

    private float bottomFinishPointYPos;
    private float upperFinishPointYPos;

    public void SetFinishPointYPos(Puyo bottomPuyoData, Puyo upperPuyoData, Transform bottomFinishPoint, Transform upperFinishPoint)
    {
        bottomFinishPointYPos = finishPointPosList[bottomPuyoData.puyoData.xPos - 1].transform.position.y;
        upperFinishPointYPos = finishPointPosList[upperPuyoData.puyoData.xPos - 1].transform.position.y;

        if (bottomPuyoData.puyoData.yPos > upperPuyoData.puyoData.yPos)
        {
            bottomFinishPointYPos += puyoDataMethod.HowManyBottomPuyo(bottomPuyoData.puyoData.xPos, bottomPuyoData.puyoData.yPos) * puyoController.puyoSize;
            upperFinishPointYPos += (puyoDataMethod.HowManyBottomPuyo(upperPuyoData.puyoData.xPos, upperPuyoData.puyoData.yPos) + 1) * puyoController.puyoSize;
        }
        else if (bottomPuyoData.puyoData.yPos < upperPuyoData.puyoData.yPos)
        {
            bottomFinishPointYPos += (puyoDataMethod.HowManyBottomPuyo(bottomPuyoData.puyoData.xPos, bottomPuyoData.puyoData.yPos) + 1) * puyoController.puyoSize;
            upperFinishPointYPos += puyoDataMethod.HowManyBottomPuyo(upperPuyoData.puyoData.xPos, upperPuyoData.puyoData.yPos) * puyoController.puyoSize;
        }
        else
        {
            bottomFinishPointYPos += puyoDataMethod.HowManyBottomPuyo(bottomPuyoData.puyoData.xPos, bottomPuyoData.puyoData.yPos) * puyoController.puyoSize;
            upperFinishPointYPos += puyoDataMethod.HowManyBottomPuyo(upperPuyoData.puyoData.xPos, upperPuyoData.puyoData.yPos) * puyoController.puyoSize;
        }

        bottomFinishPoint.transform.position = puyoController.SetNewVector2(finishPointPosList[bottomPuyoData.puyoData.xPos - 1].transform.position.x, bottomFinishPointYPos);
        upperFinishPoint.transform.position = puyoController.SetNewVector2(finishPointPosList[upperPuyoData.puyoData.xPos - 1].transform.position.x, upperFinishPointYPos);
    }
}
