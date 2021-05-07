//뿌요의 회전에 관여하는 함수들이 모여있습니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RotateMethod : MonoBehaviour
{
    private GameController gameController;
    private PuyoController puyoController;
    private PuyoFinishPoint puyoFinishPoint;
    private PuyoMoveMethod puyoMoveMethod;
    
    private float upperXPos;
    private float upperYPos;
    private float bottomXPos;
    private float bottomYPos;

    private Vector2 newUpperPuyoPos;
    private Vector2 newBottomPuyoPos;

    private float puyoSizeUseForRoatate;
    
    private void Awake()
    {
        puyoController = GetComponent<PuyoController>();
        gameController = GetComponent<GameController>();
        puyoFinishPoint = GetComponent<PuyoFinishPoint>();
        puyoMoveMethod = GetComponent<PuyoMoveMethod>();
        leftRightCheckBitArray.Set(0, false);
        leftRightCheckBitArray.Set(1, false);
        leftRightCheckBitArray.Set(2, false);
        leftRightCheckBitArray.Set(3, false);
    }

    [SerializeField] private int puyoRotateNumber = 0;
    [SerializeField] private string puyoConnectedStatus;
                     
    [SerializeField] private bool ThereIsSomethingToTheLeftOfBottomPuyo = false;
    [SerializeField] private bool ThereIsSomethingToTheRightOfBottomPuyo = false;
    [SerializeField] private bool ThereIsSomethingToTheLeftOfUpperPuyo = false;
    [SerializeField] private bool ThereIsSomethingToTheRightOfUpperPuyo = false;
 
    [SerializeField] private bool bottomPuyoIsTopOfSomething = false;
    [SerializeField] private bool upperPuyoIsTopOfSomething = false;

    private void CheckPuyoBetweenAnotherPuyoOrWallAndCheckPuyoIsTopOfSomething(Puyo bottomPuyoData, Puyo upperPuyoData)
    {
        if (bottomPuyoData.puyoData.xPos == 1)
        {
            ThereIsSomethingToTheLeftOfBottomPuyo = true;
        }
        else if (gameController.field[bottomPuyoData.puyoData.yPos - 1, bottomPuyoData.puyoData.xPos - 2] != 0)
        {
            ThereIsSomethingToTheLeftOfBottomPuyo = true;
        }

        if (upperPuyoData.puyoData.xPos == 1)
        {
            ThereIsSomethingToTheLeftOfUpperPuyo = true;
        }
        else if (gameController.field[upperPuyoData.puyoData.yPos - 1, upperPuyoData.puyoData.xPos - 2] != 0)
        {
            ThereIsSomethingToTheLeftOfUpperPuyo = true;
        }

        if (bottomPuyoData.puyoData.xPos == 6)
        {
            ThereIsSomethingToTheRightOfBottomPuyo = true;
        }
        else if (gameController.field[bottomPuyoData.puyoData.yPos - 1, bottomPuyoData.puyoData.xPos] != 0)
        {
            ThereIsSomethingToTheRightOfBottomPuyo = true;
        }

        if (upperPuyoData.puyoData.xPos == 6)
        {
            ThereIsSomethingToTheRightOfUpperPuyo = true;
        }
        else if (gameController.field[upperPuyoData.puyoData.yPos - 1, upperPuyoData.puyoData.xPos] != 0)
        {
            ThereIsSomethingToTheRightOfUpperPuyo = true;
        }

        if (bottomPuyoData.puyoData.yPos == 14)
        {
            bottomPuyoIsTopOfSomething = true;
        }
        else if (gameController.field[bottomPuyoData.puyoData.yPos, bottomPuyoData.puyoData.xPos - 1] != 0)
        {
            bottomPuyoIsTopOfSomething = true;
        }

        if (upperPuyoData.puyoData.yPos == 14)
        {
            upperPuyoIsTopOfSomething = true;
        }
        else if (gameController.field[upperPuyoData.puyoData.yPos, upperPuyoData.puyoData.xPos - 1] != 0)
        {
            upperPuyoIsTopOfSomething = true;
        }
    }

    private void PuyoCanRotateCheck(Puyo bottomPuyoData, Puyo upperPuyoData, bool rotateLeft, bool rotateRight)
    {
        //0 밑에 있는 뿌요가 1칸/2칸짜리 구멍에 들어 갔는지 확인(들어갔다면 15, 16회전 안들어갔으면 밑에 1부터체크)
        //1 upperPuyo(rotate), bottomPuyo(center)의 위치파악 (가로/세로상태, upper랑 bottom의 xpos, ypos 위치 관계)
        //2-1(가로일경우) center의 밑에 뿌요/벽(0이 아니거나 y값(14) 초과)
        //2-2(세로일경우) center의 옆에 뿌요/벽(0이 아니거나 x값이 0이거나 7일경우)
        //3 1,2둘 다 아닐경우 위치따라 1~8번 회전
        
        CheckPuyoBetweenAnotherPuyoOrWallAndCheckPuyoIsTopOfSomething(bottomPuyoData, upperPuyoData);
        puyoConnectedStatus = CheckSomethingOfPuyoLeftAndRight(bottomPuyoData, upperPuyoData);

        //가로
        if (bottomPuyoData.puyoData.xPos != upperPuyoData.puyoData.xPos)
        {
            
            // c r
            if (bottomPuyoData.puyoData.xPos < upperPuyoData.puyoData.xPos)
            {
                if (bottomPuyoIsTopOfSomething)
                {
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 4;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 14;
                    }
                }
                else
                {
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 4;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 6;
                    }
                }
            }
            // r c
            else if (bottomPuyoData.puyoData.xPos > upperPuyoData.puyoData.xPos)
            {
                if (bottomPuyoIsTopOfSomething)
                {
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 11;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 8;
                    }
                }
                else
                {
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 2;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 8;
                    }
                }
            }
        }
        //세로
        else if (bottomPuyoData.puyoData.yPos != upperPuyoData.puyoData.yPos)
        {
            switch (puyoConnectedStatus)
            {
                //u
                //b
                case "1111":
                    //2번 연속 누를경우
                    break;
                case "1110":
                    if (rotateLeft)
                    {
                        //2번 연속
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 18;
                    }
                    break;
                case "1101":
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 17;
                    }
                    else if (rotateRight)
                    {
                        //2번 연속
                    }
                    break;
                case "1010":
                    //u
                    //b
                    if (bottomPuyoData.puyoData.yPos > upperPuyoData.puyoData.yPos)
                    {
                        if (rotateLeft)
                        {
                            puyoRotateNumber = 9;
                        }
                        else if (rotateRight)
                        {
                            puyoRotateNumber = 5;
                        }
                    }
                    //b
                    //u
                    else
                    {
                        if (rotateLeft)
                        {
                            puyoRotateNumber = 3;
                        }
                        else if (rotateRight)
                        {
                            puyoRotateNumber = 10;
                        }
                    }
                    break;
                case "0100":
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 1;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 18;
                    }
                    break;
                case "1000":
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 17;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 5;
                    }
                    break;
                case "0101":
                    //u
                    //b
                    if (bottomPuyoData.puyoData.yPos > upperPuyoData.puyoData.yPos)
                    {
                        if (rotateLeft)
                        {
                            puyoRotateNumber = 1;
                        }
                        else if (rotateRight)
                        {
                            puyoRotateNumber = 12;
                        }
                    }
                    //b
                    //u
                    else if (bottomPuyoData.puyoData.yPos < upperPuyoData.puyoData.yPos)
                    {
                        if (rotateLeft)
                        {
                            puyoRotateNumber = 13;
                        }
                        else if (rotateRight)
                        {
                            puyoRotateNumber = 7;
                        }
                    }
                    break;
                case "1100":
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 17;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 18;
                    }
                    break;

                    //b
                    //u
                case "1011":
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 3;
                    }
                    else if (rotateRight)
                    {
                        //2번 연속
                    }
                    break;
                case "0111":
                    if (rotateLeft)
                    {
                        //2번 연속
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 7;
                    }
                    break;
                case "0001":
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 3;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 7;
                    }
                    break;
                case "0010":
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 3;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 7;
                    }
                    break;
                case "0011"://이거 도는거 안될수도있음
                    if (rotateLeft)
                    {
                        puyoRotateNumber = 3;
                    }
                    else if (rotateRight)
                    {
                        puyoRotateNumber = 7;
                    }
                    break;
                case "0000":
                    if (bottomPuyoData.puyoData.yPos > upperPuyoData.puyoData.yPos)
                    {
                        if (rotateLeft)
                        {
                            puyoRotateNumber = 1;
                        }
                        if (rotateRight)
                        {
                            puyoRotateNumber = 5;
                        }
                    }

                    if (bottomPuyoData.puyoData.yPos < upperPuyoData.puyoData.yPos)
                    {
                        if (rotateLeft)
                        {
                            puyoRotateNumber = 3;
                        }
                        if (rotateRight)
                        {
                            puyoRotateNumber = 7;
                        }
                    }
                    break;
            }
        }
    }

    private BitArray leftRightCheckBitArray = new BitArray(4);
    private string somethingOfLeftRightReturnValue;
    
    //bottomLeft 1000
    //bottomRight 0100
    //upperLeft 0010
    //upperRight 0001
    private string CheckSomethingOfPuyoLeftAndRight(Puyo bottomPuyoData, Puyo upperPuyoData)
    {
        if (ThereIsSomethingToTheLeftOfBottomPuyo)
        {
            leftRightCheckBitArray.Set(0, true);
        }

        if (ThereIsSomethingToTheRightOfBottomPuyo)
        {
            leftRightCheckBitArray.Set(1, true);
        }

        if (ThereIsSomethingToTheLeftOfUpperPuyo)
        {
            leftRightCheckBitArray.Set(2, true);
        }

        if (ThereIsSomethingToTheRightOfUpperPuyo)
        {
            leftRightCheckBitArray.Set(3, true);
        }

        for (int i = 0; i < leftRightCheckBitArray.Length; i++)
        {
            if (leftRightCheckBitArray[i])
            {
                somethingOfLeftRightReturnValue += "1";
            }
            else if (!leftRightCheckBitArray[i])
            {
                somethingOfLeftRightReturnValue += "0";
            }
        }
        
        return somethingOfLeftRightReturnValue;
    }
    
    private void RotatePuyo(Puyo bottomPuyoData, Puyo upperPuyoData, bool rotateLeft, bool rotateRight)
    {
        PuyoCanRotateCheck(bottomPuyoData, upperPuyoData, rotateLeft, rotateRight);

        switch (puyoRotateNumber)
        {
            //  r(upperPuyo)
            //  c(bottomPuyo)

            //  r   
            //  c  ->  r c
            //
            //1 2 3    1 2 3
            case 1:
                upperPuyoData.puyoData.xPos -= 1;
                upperPuyoData.puyoData.yPos += 1;
                upperPuyoPlayAnimationNumber = 15;
                break;
            //           
            //r c  ->    c
            //           r
            //1 2 3    1 2 3
            case 2:
                upperPuyoData.puyoData.xPos += 1;
                upperPuyoData.puyoData.yPos += 1;
                upperPuyoPlayAnimationNumber = 14;
                break;
            //     
            //  c  ->    c r
            //  r  
            //1 2 3    1 2 3
            case 3:
                upperPuyoData.puyoData.xPos += 1;
                upperPuyoData.puyoData.yPos -= 1;
                upperPuyoPlayAnimationNumber = 13;
                break;
            //           r
            //  c r ->   c
            //
            //1 2 3    1 2 3
            case 4:
                upperPuyoData.puyoData.xPos -= 1;
                upperPuyoData.puyoData.yPos -= 1;
                upperPuyoPlayAnimationNumber = 12;
                break;
            //  r   
            //  c  ->   c r
            //
            //1 2 3   1 2 3
            case 5:
                upperPuyoData.puyoData.xPos += 1;
                upperPuyoData.puyoData.yPos += 1;
                upperPuyoPlayAnimationNumber = 1;
                break;
            //    
            //  c r  ->  c
            //           r
            //1 2 3    1 2 3
            case 6:
                upperPuyoData.puyoData.xPos -= 1;
                upperPuyoData.puyoData.yPos += 1;
                upperPuyoPlayAnimationNumber = 2;
                break;
            //     
            //  c  ->  r c
            //  r
            //1 2 3    1 2 3
            case 7:
                upperPuyoData.puyoData.xPos -= 1;
                upperPuyoData.puyoData.yPos -= 1;
                upperPuyoPlayAnimationNumber = 16;
                break;
            //           r    
            //r c   ->   c
            //
            //1 2 3    1 2 3
            case 8:
                upperPuyoData.puyoData.xPos += 1;
                upperPuyoData.puyoData.yPos -= 1;
                upperPuyoPlayAnimationNumber = 3;
                break;
            //r   
            //c     ->  r c
            //
            //1 2 3     1 2 3
            case 9:
                bottomPuyoData.puyoData.xPos += 1;
                upperPuyoData.puyoData.yPos += 1;
                bottomPuyoPlayAnimationNumber = 6;
                upperPuyoPlayAnimationNumber = 11;
                break;
            //   
            //c     ->  r c
            //r
            //1 2 3     1 2 3
            case 10:
                bottomPuyoData.puyoData.xPos += 1;
                upperPuyoData.puyoData.yPos -= 1;
                bottomPuyoPlayAnimationNumber = 1;
                upperPuyoPlayAnimationNumber = 4;
                break;
            //       
            //        ->   c
            //r c          r
            //1 2 3      1 2 3
            case 11:
                bottomPuyoData.puyoData.yPos -= 1;
                upperPuyoData.puyoData.xPos += 1;
                bottomPuyoPlayAnimationNumber = 7;
                upperPuyoPlayAnimationNumber = 17;
                break;
            //    r   
            //    c  ->   c r
            //
            //1 2 3     1 2 3
            case 12:
                bottomPuyoData.puyoData.xPos -= 1;
                upperPuyoData.puyoData.yPos += 1;
                bottomPuyoPlayAnimationNumber = 12;
                upperPuyoPlayAnimationNumber = 5;
                break;
            //       
            //    c  ->   c r
            //    r
            //1 2 3     1 2 3
            case 13:
                bottomPuyoData.puyoData.xPos -= 1;
                upperPuyoData.puyoData.yPos -= 1;
                bottomPuyoPlayAnimationNumber = 11;
                upperPuyoPlayAnimationNumber = 10;
                break;
            //     
            //       ->   c
            //  c r       r
            //1 2 3     1 2 3
            case 14:
                bottomPuyoData.puyoData.yPos -= 1;
                upperPuyoData.puyoData.xPos -= 1;
                bottomPuyoPlayAnimationNumber = 2;
                upperPuyoPlayAnimationNumber = 6;
                break;

            //두번 연속 누를경우
            //x r x    x c x
            //x c x -> x r x 
            //x x x    x x x
            //1 2 3    1 2 3
            case 15:
                bottomPuyoData.puyoData.yPos -= 1;
                upperPuyoData.puyoData.yPos += 1;
                if (rotateLeft)
                {
                    bottomPuyoPlayAnimationNumber = 5;
                    upperPuyoPlayAnimationNumber = 9;
                }
                if (rotateRight)
                {
                    bottomPuyoPlayAnimationNumber = 3;
                    upperPuyoPlayAnimationNumber = 7;
                }
                break;
            //x c x    x r x
            //x r x -> x c x 
            //x x x    x x x
            //1 2 3    1 2 3
            case 16:
                bottomPuyoData.puyoData.yPos += 1;
                upperPuyoData.puyoData.yPos -= 1;
                if (rotateLeft)
                {
                    bottomPuyoPlayAnimationNumber = 9;
                    upperPuyoPlayAnimationNumber = 19;
                }
                if (rotateRight)
                {
                    bottomPuyoPlayAnimationNumber = 10;
                    upperPuyoPlayAnimationNumber = 20;
                }
                break;
            //  r      r c
            //x c x -> x   x
            //x x x    x x x
            case 17:
                bottomPuyoData.puyoData.yPos -= 1;
                upperPuyoData.puyoData.xPos -= 1;
                bottomPuyoPlayAnimationNumber = 8;
                upperPuyoPlayAnimationNumber = 18;
                break;
            //  r        c r
            //x c x -> x   x
            //x x x    x x x
            case 18:
                bottomPuyoData.puyoData.yPos -= 1;
                upperPuyoData.puyoData.xPos += 1;
                bottomPuyoPlayAnimationNumber = 3;
                upperPuyoPlayAnimationNumber = 8;
                break;
        }
    }

    [SerializeField] private int bottomPuyoPlayAnimationNumber;
    [SerializeField] private int upperPuyoPlayAnimationNumber;
    

    private string[] bottomPuyoRotateName = { "", "BottomRight_WallRotate1", "BottomRight_FloorRotate", "BottomRight_HoleRotate1",
                                                    "BottomRight_StairRotate", "BottomLeft_HoleRotate1", "BottomLeft_WallRotate1",
                                                    "BottomLeft_FloorRotate", "BottomLeft_StairRotate", "BottomLeft_HoleRotate2",
                                                    "BottomRight_HoleRotate2", "BottomLeft_WallRotate2", "BottomRight_WallRotate2",
                                                    "Bottom_MaintananceRightWallRotate", "Bottom_MaintananceLeftWallRotate", "Bottom_MaintananceFloorRotate",
                                                    "Bottom_MaintananceStairRightRotate", "Bottom_MaintananceHoleRotate1", "Bottom_MaintananceHoleRotate2",
                                                    "Bottom_MaintananceStairLeftRotate" };

    private string[] upperPuyoRotateName = { "", "UpperRight_UpToRight", "UpperRight_RightToDown", "UpperRight_LeftToUp",
                                                "UpperRight_WallRotate1", "UpperRight_WallRotate2", "UpperRight_FloorRotate",
                                                "UpperRight_HoleRotate1", "UpperRight_StairRotate", "UpperLeft_HoleRotate1",
                                                "UpperLeft_WallRotate2", "UpperLeft_WallRotate1", "UpperLeft_RightToUp",
                                                "UpperLeft_DownToRight", "UpperLeft_LeftToDown", "UpperLeft_UpToLeft",
                                                "UpperRight_DownToLeft", "UpperLeft_FloorRotate", "UpperLeft_StairRotate",
                                                "UpperLeft_HoleRotate2", "UpperRight_HoleRotate2", "Upper_MaintananceRightWallRotate",
                                                "Upper_MaintananceLeftWallRotate", "Upper_MaintananceFloorRotate", "Upper_MaintananceStairRightRotate",
                                                "Upper_MaintananceHoleRotate1", "Upper_MaintananceHoleRotate2", "Upper_MaintananceStairLeftRotate" };

    [SerializeField] private Text text;

    public void RotateButton(Puyo bottomPuyoData, Puyo upperPuyoData, bool rotateLeft, bool rotateRight)
    {
        RotatePuyo(bottomPuyoData, upperPuyoData, rotateLeft, rotateRight);
        puyoMoveMethod.InitializingMoveCheckValue(puyoController.bottomPuyoData, puyoController.upperPuyoData);
            
        text.text = "bottomXPos : " + puyoController.bottomPuyoData.puyoData.xPos + "\n"
                    + "bottomYPos : " + puyoController.bottomPuyoData.puyoData.yPos + "\n"
                    + "upperXPos : " + puyoController.upperPuyoData.puyoData.xPos + "\n"
                    + "upperYPos : " + puyoController.upperPuyoData.puyoData.yPos + "\n"
                    + "rotateNumber" + puyoRotateNumber + "\n"
                    + "bottomAniNumber : " + bottomPuyoPlayAnimationNumber + "\n"
                    + "upperAniNumber : " + upperPuyoPlayAnimationNumber + "\n"
                    + "bottomRotateName : " + bottomPuyoRotateName[bottomPuyoPlayAnimationNumber] + "(" + bottomPuyoPlayAnimationNumber + ")" + "\n"
                    + "upperRotateName : " + upperPuyoRotateName[upperPuyoPlayAnimationNumber] + "(" + upperPuyoPlayAnimationNumber + ")"
                    + "rotateStatus : " + puyoConnectedStatus + "\n";
        puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);

        if (bottomPuyoPlayAnimationNumber != 0)
        {
            puyoController.bottomAni.Play(bottomPuyoRotateName[bottomPuyoPlayAnimationNumber]);
            
            if (bottomPuyoPlayAnimationNumber == 1 || bottomPuyoPlayAnimationNumber == 6)
            { 
                PuyoMaintanancePositionEndOfAnimation(puyoController.bottomPuyo, puyoController.upperPuyo,
                                                            puyoController.bottomPuyoParent, puyoController.upperPuyoParent,
                                                            puyoController.bottomPuyoData, puyoController.upperPuyoData, "WallLeft", rotateLeft, rotateRight);

                puyoController.bottomAni.Play(bottomPuyoRotateName[bottomPuyoPlayAnimationNumber]);
                puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);
            }
            else if (bottomPuyoPlayAnimationNumber == 11 || bottomPuyoPlayAnimationNumber == 12)
            {
                PuyoMaintanancePositionEndOfAnimation(puyoController.bottomPuyo, puyoController.upperPuyo,
                                                            puyoController.bottomPuyoParent, puyoController.upperPuyoParent,
                                                            puyoController.bottomPuyoData, puyoController.upperPuyoData, "WallRight", rotateLeft, rotateRight);
                
                puyoController.bottomAni.Play(bottomPuyoRotateName[bottomPuyoPlayAnimationNumber]);
                puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);
            }
            else if (bottomPuyoPlayAnimationNumber == 2 || bottomPuyoPlayAnimationNumber == 7)
            {

                PuyoMaintanancePositionEndOfAnimation(puyoController.bottomPuyo, puyoController.upperPuyo,
                                                            puyoController.bottomPuyoParent, puyoController.upperPuyoParent,
                                                            puyoController.bottomPuyoData, puyoController.upperPuyoData, "FloorRotate", rotateLeft, rotateRight);

                puyoController.bottomAni.Play(bottomPuyoRotateName[bottomPuyoPlayAnimationNumber]);
                puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);
            }
            else if (bottomPuyoPlayAnimationNumber == 3 || bottomPuyoPlayAnimationNumber == 5)
            {
                PuyoMaintanancePositionEndOfAnimation(puyoController.bottomPuyo, puyoController.upperPuyo,
                                                            puyoController.bottomPuyoParent, puyoController.upperPuyoParent,
                                                            puyoController.bottomPuyoData, puyoController.upperPuyoData, "HoleRotate1", rotateLeft, rotateRight);

                puyoController.bottomAni.Play(bottomPuyoRotateName[bottomPuyoPlayAnimationNumber]);
                puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);
            }
            else if (bottomPuyoPlayAnimationNumber == 4 || bottomPuyoPlayAnimationNumber == 8)
            {
                PuyoMaintanancePositionEndOfAnimation(puyoController.bottomPuyo, puyoController.upperPuyo,
                                                            puyoController.bottomPuyoParent, puyoController.upperPuyoParent,
                                                            puyoController.bottomPuyoData, puyoController.upperPuyoData, "StairRotate", rotateLeft, rotateRight);

                puyoController.bottomAni.Play(bottomPuyoRotateName[bottomPuyoPlayAnimationNumber]);
                puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);
            }
            else if (bottomPuyoPlayAnimationNumber == 9 || bottomPuyoPlayAnimationNumber == 10)
            {
                PuyoMaintanancePositionEndOfAnimation(puyoController.bottomPuyo, puyoController.upperPuyo,
                                                            puyoController.bottomPuyoParent, puyoController.upperPuyoParent,
                                                            puyoController.bottomPuyoData, puyoController.upperPuyoData, "HoleRotate2", rotateLeft, rotateRight);

                puyoController.bottomAni.Play(bottomPuyoRotateName[bottomPuyoPlayAnimationNumber]);
                puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);
            }
        }

        text.text = "bottomXPos : " + puyoController.bottomPuyoData.puyoData.xPos + "\n"
                    + "bottomYPos : " + puyoController.bottomPuyoData.puyoData.yPos + "\n"
                    + "upperXPos : " + puyoController.upperPuyoData.puyoData.xPos + "\n"
                    + "upperYPos : " + puyoController.upperPuyoData.puyoData.yPos + "\n"
                    + "rotateNumbber" + puyoRotateNumber + "\n"
                    + "bottomAniNumber : " + bottomPuyoPlayAnimationNumber + "\n"
                    + "upperAniNumber : " + upperPuyoPlayAnimationNumber + "\n"
                    + "bottomRotateName : " + bottomPuyoRotateName[bottomPuyoPlayAnimationNumber] + "(" + bottomPuyoPlayAnimationNumber + ")" + "\n"
                    + "upperRotateName : " + upperPuyoRotateName[upperPuyoPlayAnimationNumber] + "(" + upperPuyoPlayAnimationNumber + ")";
        puyoController.upperAni.Play(upperPuyoRotateName[upperPuyoPlayAnimationNumber]);

        puyoFinishPoint.SetFinishPointYPos(puyoController.bottomPuyoData, puyoController.upperPuyoData,
                                            puyoController.bottomFinishTransform, puyoController.upperFinishTransform);

        InitializingRotateValue();
    }

    private void InitializingRotateValue()
    {
        bottomPuyoPlayAnimationNumber = 0;
        upperPuyoPlayAnimationNumber = 0;
        puyoRotateNumber = 0;
        leftRightCheckBitArray.Set(0, false);
        leftRightCheckBitArray.Set(1, false);
        leftRightCheckBitArray.Set(2, false);
        leftRightCheckBitArray.Set(3, false);
        somethingOfLeftRightReturnValue = null;
        puyoConnectedStatus = null;
        ThereIsSomethingToTheLeftOfBottomPuyo = false;
        ThereIsSomethingToTheLeftOfUpperPuyo = false;
        ThereIsSomethingToTheRightOfBottomPuyo = false;
        ThereIsSomethingToTheRightOfUpperPuyo = false;
        bottomPuyoIsTopOfSomething = false;
        upperPuyoIsTopOfSomething = false;
    }

    private float bottomPuyoNewXPos;
    private float bottomPuyoNewYPos;
    private float upperPuyoNewXPos;
    private float upperPuyoNewYPos;
    private float bottomPuyoParentNewXPos;
    private float bottomPuyoParentNewYPos;
    private float upperPuyoParentNewXPos;
    private float upperPuyoParentNewYPos;
    private float bottomMoveParentNewXPos;
    private float bottomMoveParentNewYPos;
    private float upperMoveParentNewXPos;
    private float upperMoveParentNewYPos;
    
    private void PuyoMaintanancePositionEndOfAnimation(Transform bottomPuyo, Transform upperPuyo, Transform bottomPuyoParent, 
                                                        Transform upperPuyoParent, Puyo bottomPuyoData, Puyo upperPuyoData,
                                                        string status, bool rotateLeft, bool rotateRight)
    {
        bottomPuyoNewXPos = bottomPuyo.position.x;
        bottomPuyoNewYPos = bottomPuyo.position.y;
        upperPuyoNewXPos = upperPuyo.position.x;
        upperPuyoNewYPos = upperPuyo.position.y;

        bottomPuyoParentNewXPos = bottomPuyoParent.position.x;
        bottomPuyoParentNewYPos = bottomPuyoParent.position.y;
        upperPuyoParentNewXPos = upperPuyoParent.position.x;
        upperPuyoParentNewYPos = upperPuyoParent.position.y;

        switch (status)
        {
            case "WallLeft":
                bottomPuyoParentNewXPos += puyoController.puyoSize;
                upperPuyoParentNewXPos += puyoController.puyoSize;
                bottomPuyoNewXPos -= puyoController.puyoSize;
                upperPuyoNewXPos -= puyoController.puyoSize;

                bottomPuyoPlayAnimationNumber = 14;
                upperPuyoPlayAnimationNumber = 22;
                break;
            case "WallRight":
                bottomPuyoParentNewXPos -= puyoController.puyoSize;
                upperPuyoParentNewXPos -= puyoController.puyoSize;
                bottomPuyoNewXPos += puyoController.puyoSize;
                upperPuyoNewXPos += puyoController.puyoSize;

                bottomPuyoPlayAnimationNumber = 13;
                upperPuyoPlayAnimationNumber = 21;
                break;
            case "FloorRotate":
                bottomPuyoParentNewYPos += puyoController.puyoSize;
                upperPuyoParentNewYPos += puyoController.puyoSize;
                bottomPuyoNewYPos -= puyoController.puyoSize;
                upperPuyoNewYPos -= puyoController.puyoSize;

                bottomPuyoPlayAnimationNumber = 15;
                upperPuyoPlayAnimationNumber = 23;
                break;
            case "StairRotate":
                bottomPuyoParentNewYPos += puyoController.puyoSize;
                upperPuyoParentNewYPos += puyoController.puyoSize;
                bottomPuyoNewYPos -= puyoController.puyoSize;
                upperPuyoNewYPos -= puyoController.puyoSize;
                
                if (rotateLeft)
                {
                    bottomPuyoPlayAnimationNumber = 19;
                    upperPuyoPlayAnimationNumber = 27;
                }
                else if (rotateRight)
                {
                    bottomPuyoPlayAnimationNumber = 16;
                    upperPuyoPlayAnimationNumber = 24;
                }
                break;
            case "HoleRotate1":
                bottomPuyoParentNewYPos += puyoController.puyoSize;
                upperPuyoParentNewYPos -= puyoController.puyoSize;
                bottomPuyoNewYPos -= puyoController.puyoSize;
                upperPuyoNewYPos += puyoController.puyoSize;

                bottomPuyoPlayAnimationNumber = 17;
                upperPuyoPlayAnimationNumber = 25;
                break;
            case "HoleRotate2":
                bottomPuyoParentNewYPos -= puyoController.puyoSize;
                upperPuyoParentNewYPos += puyoController.puyoSize;
                bottomPuyoNewYPos += puyoController.puyoSize;
                upperPuyoNewYPos -= puyoController.puyoSize;

                bottomPuyoPlayAnimationNumber = 18;
                upperPuyoPlayAnimationNumber = 26;
                break;
        }

        bottomPuyoParent.position = puyoController.SetNewVector2(bottomPuyoParentNewXPos, bottomPuyoParentNewYPos);
        upperPuyoParent.position = puyoController.SetNewVector2(upperPuyoParentNewXPos, upperPuyoParentNewYPos);
        bottomPuyo.position = puyoController.SetNewVector2(bottomPuyoNewXPos, bottomPuyoNewYPos);
        upperPuyo.position = puyoController.SetNewVector2(upperPuyoNewXPos, upperPuyoNewYPos);
    }
}
