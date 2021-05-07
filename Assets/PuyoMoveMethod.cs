//뿌요가 움직일때 사용하는 함수들이 있는곳입니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoMoveMethod : MonoBehaviour
{
    private GameController gameController;
    private PuyoController puyoController;
    private PuyoMoveMethod puyoMoveMethod;
    private PuyoDataMethod puyoDataMethod;
    private TransformScaler transformScaler;
    private PuyoFinishPoint puyoFinishPoint;
    
    private float bottomPuyoYPos;
    private float upperPuyoYPos;

    private int count;

    private Transform finishPoint;
    private float bottomPuyoXPos;
    private float upperPuyoXPos;
    
    private bool maxIndex;
    private bool bottomFloating;
    private bool upperFloating;
    private int puyoDownCount;
    private float bottomFinishYPos;
    private float upperFinishYPos;
    
    private void Awake()
    {
        gameController = GetComponent<GameController>();
        puyoController = GetComponent<PuyoController>();
        puyoDataMethod = GetComponent<PuyoDataMethod>();
        puyoMoveMethod = GetComponent<PuyoMoveMethod>();
        transformScaler = GetComponent<TransformScaler>();
        puyoFinishPoint = GetComponent<PuyoFinishPoint>();
    }
    
    public void MovePuyoVertical(Transform bottomPuyo, Transform upperPuyo, Puyo bottomPuyoData, Puyo upperPuyoData)
    {
        puyoController.CheckPuyoCanMoveVertical();
        
        if (puyoController.bottomCanMoveVertical && puyoController.upperCanMoveVertical)
        {
            bottomPuyoYPos = bottomPuyo.position.y;
            upperPuyoYPos = upperPuyo.position.y;
            count = 0;

            bottomPuyoYPos -= puyoController.puyoSize;
            upperPuyoYPos -= puyoController.puyoSize;

            bottomPuyoData.puyoData.yPos += 1;
            upperPuyoData.puyoData.yPos += 1;
            
            bottomPuyo.position = puyoController.SetNewVector2(bottomPuyo.position.x, bottomPuyoYPos);
            upperPuyo.position = puyoController.SetNewVector2(upperPuyo.position.x, upperPuyoYPos);
        }
    }

    private float sortingPuyoNewYPos;

    public void MovePuyoVertical(Transform puyoTransform, Puyo puyoData, bool sorting)
    {
        if (sorting || puyoController.bottomCanMoveVertical && puyoController.upperCanMoveVertical)
        {
            sortingPuyoNewYPos = puyoTransform.position.y;
        
            sortingPuyoNewYPos -= puyoController.puyoSize;

            puyoData.puyoData.yPos += 1;

            puyoTransform.position = puyoController.SetNewVector2(puyoTransform.position.x, sortingPuyoNewYPos);
        }
    }

    public void MovePuyoHorizontal(Transform bottomPuyo, Transform upperPuyo, Puyo bottomPuyoData, Puyo upperPuyoData,
                                        string direction)
    {
        switch (direction)
        {
            case "left":
                if (bottomPuyoData.puyoData.xPos != 1)
                {
                    if (gameController.field[bottomPuyoData.puyoData.yPos - 1, bottomPuyoData.puyoData.xPos - 2] != 0)
                    {
                        puyoController.bottomCanMoveLeftDirection = false;
                    }
                }

                if (upperPuyoData.puyoData.xPos != 1)
                {
                    if (gameController.field[upperPuyoData.puyoData.yPos - 1, upperPuyoData.puyoData.xPos - 2] != 0)
                    {
                        puyoController.upperCanMoveLeftDirection = false;
                    }
                }

                if (puyoController.upperCanMoveLeftDirection && puyoController.bottomCanMoveLeftDirection)
                {
                    bottomPuyoXPos = bottomPuyo.position.x;
                    upperPuyoXPos = upperPuyo.position.x;

                    bottomPuyoXPos -= puyoController.puyoSize;
                    upperPuyoXPos -= puyoController.puyoSize;

                    bottomPuyoData.puyoData.xPos -= 1;
                    upperPuyoData.puyoData.xPos -= 1;

                    bottomPuyo.position = puyoController.SetNewVector2(bottomPuyoXPos, bottomPuyo.position.y);
                    upperPuyo.position = puyoController.SetNewVector2(upperPuyoXPos, upperPuyo.position.y);

                    if (!puyoController.bottomCanMoveRightDirection)
                    {
                        puyoController.bottomCanMoveRightDirection = true;
                    }

                    if (!puyoController.upperCanMoveRightDirection)
                    {
                        puyoController.upperCanMoveRightDirection = true;
                    }
                    puyoFinishPoint.SetFinishPointYPos(bottomPuyoData, upperPuyoData, puyoController.bottomFinishTransform, puyoController.upperFinishTransform);
                }

                if (puyoController.upperPuyoData.puyoData.xPos == 1 || puyoController.bottomPuyoData.puyoData.xPos == 1)
                {
                    puyoController.upperCanMoveLeftDirection = false;
                    puyoController.bottomCanMoveLeftDirection = false;
                }
                break;
            case "right":
                if (bottomPuyoData.puyoData.xPos != 6)
                {
                    if (gameController.field[bottomPuyoData.puyoData.yPos - 1, bottomPuyoData.puyoData.xPos] != 0)
                    {
                        puyoController.bottomCanMoveRightDirection = false;
                    }
                }

                if (upperPuyoData.puyoData.xPos != 6)
                {
                    if (gameController.field[upperPuyoData.puyoData.yPos - 1, upperPuyoData.puyoData.xPos] != 0)
                    {
                        puyoController.upperCanMoveRightDirection = false;
                        
                    }
                }

                if (puyoController.upperCanMoveRightDirection && puyoController.bottomCanMoveRightDirection)
                {
                    bottomPuyoXPos = bottomPuyo.position.x;
                    upperPuyoXPos = upperPuyo.position.x;

                    bottomPuyoXPos += puyoController.puyoSize;
                    upperPuyoXPos += puyoController.puyoSize;

                    bottomPuyoData.puyoData.xPos += 1;
                    upperPuyoData.puyoData.xPos += 1;

                    bottomPuyo.position = puyoController.SetNewVector2(bottomPuyoXPos, bottomPuyo.position.y);
                    upperPuyo.position = puyoController.SetNewVector2(upperPuyoXPos, upperPuyo.position.y);

                    if (!puyoController.upperCanMoveLeftDirection)
                    {
                        puyoController.upperCanMoveLeftDirection = true;
                    }

                    if (!puyoController.bottomCanMoveLeftDirection)
                    {
                        puyoController.bottomCanMoveLeftDirection = true;
                    }
                    puyoFinishPoint.SetFinishPointYPos(bottomPuyoData, upperPuyoData, puyoController.bottomFinishTransform, puyoController.upperFinishTransform);
                }

                if (puyoController.upperPuyoData.puyoData.xPos == 6 || puyoController.bottomPuyoData.puyoData.xPos == 6)
                {
                    puyoController.upperCanMoveRightDirection = false;
                    puyoController.bottomCanMoveRightDirection = false;
                }
                break;
        }

        InitializingMoveCheckValue(bottomPuyoData, upperPuyoData);
    }

    public void InitializingMoveCheckValue(Puyo bottomPuyoData, Puyo upperPuyoData)
    {
        if (upperPuyoData.puyoData.xPos != 1)
        {
            puyoController.upperCanMoveLeftDirection = true;
        }
        else
        {
            puyoController.upperCanMoveLeftDirection = false;
        }

        if (upperPuyoData.puyoData.xPos != 6)
        {
            puyoController.upperCanMoveRightDirection = true;
        }
        else
        {
            puyoController.upperCanMoveRightDirection = false;
        }

        if (bottomPuyoData.puyoData.xPos != 1)
        {
            puyoController.bottomCanMoveLeftDirection = true;
        }
        else
        {
            puyoController.bottomCanMoveLeftDirection = false;
        }

        if (bottomPuyoData.puyoData.xPos != 6)
        {
            puyoController.bottomCanMoveRightDirection = true;
        }
        else
        {
            puyoController.bottomCanMoveRightDirection = false;
        }
    }
}