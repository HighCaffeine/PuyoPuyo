//gameController에서 받은 뿌요를 움직여주는 함수들을 호출하는 메인 함수입니다.
//모든 뿌요의 이미지를 가지고 있고
//게임의 속도를 여기서 정해주고 뿌요가 더 움직일수 있는지 확인해줍니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PuyoController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private CreatePuyo createPuyo;
    [SerializeField] private TransformScaler transformScaler;

    private PuyoMoveMethod puyoMoveMethod;
    private PuyoFinishPoint puyoFinishPoint;
    private PuyoDataMethod puyoDataMethod;
    private SortingMethod sortingMethod;
    private SetImageMethod setImageMethod;

    public string[] puyoColors; //0 : red, 1 : green, 2 : blue, 3 : yellow

    public List<Sprite> puyoGeneralSprites = new List<Sprite>();
    public Sprite cumberPuyoSprite;
    public Sprite[] redFieldCheckSprites;
    public Sprite[] greenFieldCheckSprites;
    public Sprite[] blueFieldCheckSprites;
    public Sprite[] yellowFieldCheckSprites;
    public Sprite transparencyImage;
    public List<Sprite> attackSpriteSprites = new List<Sprite>();
    public List<Sprite> puyoFinishPointSprites = new List<Sprite>();

    public Transform puyoParent;

    [SerializeField] float puyoMoveSpeed;

    private float fieldPuyoStartYPos;
    private float fieldPuyoStartXPos = 0f;

    public Transform fieldPuyoParent;

    public RectTransform fieldHeightAndWidth;

    [SerializeField] private float gameSpeed = 1f;
    [SerializeField] private float puyoHorizontalDistance;
    [SerializeField] private float moveDelay;

    public float puyoSize;
    public float puyoDownDistance;
    public float fieldHeight;
    public float fieldWidth;

    //회전
    private int puyoRotateAniNumber;
    private bool puyoRotateLeft;
    private bool puyoRotateRight;

    public Transform bottomFinishTransform;
    public Transform upperFinishTransform;

    public Transform[] xOnePuyos = new Transform[14];
    public Transform[] xTwoPuyos = new Transform[14];
    public Transform[] xThreePuyos = new Transform[14];
    public Transform[] xFourPuyos = new Transform[14];
    public Transform[] xFivePuyos = new Transform[14];
    public Transform[] xSixPuyos = new Transform[14];

    public Puyo bottomPuyoData;
    public Puyo upperPuyoData;

    public Image bottomFinishPointImage;
    public Image upperFinishPointImage;

    //뿌요 밑에 0 수, 밑에 movePuyo안에 while문 나가는 조건
    private int bottomPuyoBottomCount;
    private int upperPuyoBottomCount;
    private bool putPuyo;
    public float puyoPutDelay;
    public bool bottomCanMoveRightDirection;
    public bool upperCanMoveRightDirection;
    public bool bottomCanMoveLeftDirection;
    public bool upperCanMoveLeftDirection;
    public bool bottomCanMoveVertical;
    public bool upperCanMoveVertical;

    private int checkBottomSpaceMethodReturnValue;

    private void Awake()
    {
        gameController = GetComponent<GameController>();
        createPuyo = GetComponent<CreatePuyo>();
        puyoMoveMethod = GetComponent<PuyoMoveMethod>();
        puyoFinishPoint = GetComponent<PuyoFinishPoint>();
        puyoDataMethod = GetComponent<PuyoDataMethod>();
        sortingMethod = GetComponent<SortingMethod>();
        setImageMethod = GetComponent<SetImageMethod>();
        transformScaler = GetComponent<TransformScaler>();

        for (int i = 0; i < puyoColors.Length; i++)
        {
            puyoColors[i] = puyoGeneralSprites[i].name.Split('_')[0];
        }
    }

    private float coroutineTime;
    private int moveDelayCount;
    private float puyoScale;

    public Transform bottomPuyo;
    public Transform upperPuyo;
    public Transform bottomPuyoParent;
    public Transform upperPuyoParent;
    
    public Transform redPuyoParent;
    public Transform greenPuyoParent;
    public Transform bluePuyoParent;
    public Transform yellowPuyoParent;
    
    public Animator bottomAni;
    public Animator upperAni;
    public AnimationClip[] bottomAnimation;
    public AnimationClip[] upperAnimation;

    public bool lockButton;
    
    public IEnumerator MovePuyo(Transform puyoTransform)
    {
        InitializingMoveValue();

        bottomPuyoParent = puyoTransform.GetChild(0);
        upperPuyoParent = puyoTransform.GetChild(1);

        bottomPuyo = bottomPuyoParent.GetChild(0);
        upperPuyo = upperPuyoParent.GetChild(0);
        
        bottomPuyo.localScale = SetNewVector2(1, 1);
        upperPuyo.localScale = SetNewVector2(1, 1);
        
        bottomPuyoData = bottomPuyo.GetComponent<Puyo>();
        upperPuyoData = upperPuyo.GetComponent<Puyo>();

        bottomFinishPointImage = bottomFinishTransform.GetComponent<Image>();
        upperFinishPointImage = upperFinishTransform.GetComponent<Image>();

        bottomAni = bottomPuyo.GetComponent<Animator>();
        upperAni = upperPuyo.GetComponent<Animator>();
        bottomAnimation = bottomAni.runtimeAnimatorController.animationClips;
        upperAnimation = upperAni.runtimeAnimatorController.animationClips;

        puyoFinishPoint.SetPuyoFinishPoint(bottomPuyo, upperPuyo, bottomFinishTransform, 
                                            upperFinishTransform, bottomPuyoData, upperPuyoData, bottomFinishPointImage, 
                                            upperFinishPointImage);
        
        puyoDataMethod.PuyoMoveToEachColorParent(bottomPuyoParent, puyoDataMethod.StringColorToIntColorCode(bottomPuyoData.puyoData.color));
        puyoDataMethod.PuyoMoveToEachColorParent(upperPuyoParent, puyoDataMethod.StringColorToIntColorCode(upperPuyoData.puyoData.color));
        
        Destroy(puyoTransform.gameObject);
        while (true)
        {
            bottomPuyoBottomCount = puyoDataMethod.HowManyBottomPuyo(bottomPuyoData.puyoData.xPos, bottomPuyoData.puyoData.yPos);
            upperPuyoBottomCount = puyoDataMethod.HowManyBottomPuyo(upperPuyoData.puyoData.xPos, upperPuyoData.puyoData.yPos);
            coroutineTime += 0.2f;
            
            puyoMoveMethod.MovePuyoVertical(bottomPuyoParent, upperPuyoParent, bottomPuyoData, upperPuyoData);

            if (!upperCanMoveVertical && moveDelayCount == 0)
            {
                StartCoroutine(CheckMoveDelay("upper", upperPuyoData));
            }

            if (!bottomCanMoveVertical && moveDelayCount == 0)
            {
                StartCoroutine(CheckMoveDelay("bottom", bottomPuyoData));
            }
            
            if (puyoPutDelay >= 0.15f && (!upperCanMoveVertical || !bottomCanMoveVertical))
            {
                break;
            }
            yield return new WaitForSeconds(moveDelay);
        }

        lockButton = true;

        switch (ReturnCompareYPos(bottomPuyoData.puyoData.yPos, upperPuyoData.puyoData.yPos))
        {
            case 1:
                puyoDataMethod.SetPuyoNameGeneralToPos(bottomPuyoParent, bottomPuyoData);
                puyoDataMethod.SetPuyoNameGeneralToPos(upperPuyoParent, upperPuyoData);
                break;
            case 2:
                puyoDataMethod.SetPuyoNameGeneralToPos(upperPuyoParent, upperPuyoData);
                puyoDataMethod.SetPuyoNameGeneralToPos(bottomPuyoParent, bottomPuyoData);
                break;
            case 3:
                puyoDataMethod.SetPuyoNameGeneralToPos(bottomPuyoParent, bottomPuyoData);
                puyoDataMethod.SetPuyoNameGeneralToPos(upperPuyoParent, upperPuyoData);
                break;
        }

        sortingMethod.DownPuyoLaunch();

        if (puyoDataMethod.GameOverCheck(bottomPuyoData, upperPuyoData))
        {
            puyoDataMethod.ScoreSaveToJson();
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
            
            sortingMethod.DestroySortingMethodLaunch();
            
            yield return null;
        }
    }
    
    public int returnCompareValue;

    // 1 bottomYPos > upperYPos  
    // 2 bottomYPos < upperYPos  
    // 3 bottomYPos == upperYPos
    
    private int ReturnCompareYPos(int bottomYPos, int upperYPos)
    {
        returnCompareValue = 0;

        if (bottomYPos > upperYPos)
        {
            returnCompareValue = 1;
        }

        if (bottomYPos < upperYPos)
        {
            returnCompareValue = 2;
        }

        if (bottomYPos == upperYPos)
        {
            returnCompareValue = 3;
        }

        return returnCompareValue;
    }
    
    private void InitializingMoveValue()
    {
        bottomCanMoveLeftDirection = true;
        bottomCanMoveRightDirection = true;
        bottomCanMoveVertical = true;
        upperCanMoveLeftDirection = true;
        upperCanMoveRightDirection = true;
        upperCanMoveVertical = true;
        gameController.permitPushButton = true;
        puyoPutDelay = 0f;
        lockButton = false;
    }
    
    //bottom이 더 y가 높다 -> bottom, upper가 더 y가 높다 -> upper, 같다 -> equal
    public void CheckPuyoCanMoveVertical()
    {
        if (bottomPuyoData.puyoData.yPos != 14)
        {
            if (gameController.field[bottomPuyoData.puyoData.yPos, bottomPuyoData.puyoData.xPos - 1] != 0)
            {
                bottomCanMoveVertical = false;
            }
        }
        else
        {
            bottomCanMoveVertical = false;
        }

        if (upperPuyoData.puyoData.yPos != 14)
        {
            if (gameController.field[upperPuyoData.puyoData.yPos, upperPuyoData.puyoData.xPos - 1] != 0)
            {
                upperCanMoveVertical = false;
            }
        }
        else
        {
            upperCanMoveVertical = false;
        }
    }
    
    public int ReturnBottomSpaceCount(int xPos, int yPos)
    {
        return gameController.fieldMax_Y - yPos - puyoDataMethod.HowManyBottomPuyo(xPos, yPos);
    }

    private int checkMoveDelayCoroutineBottomXPos;
    private int checkMoveDelayCoroutineBottomYPos;
    private int checkMoveDelayCoroutineUpperXPos;
    private int checkMoveDelayCoroutineUpperYPos;
    private float ifCalledCheckMoveDelayCoroutineAfterPuyoYPosIsNotChangeCheckTime;

    //무조건 뿌요가 놔지는 상황에 불러지는 코루틴
    public IEnumerator CheckMoveDelay(string puyoName, Puyo puyoData)
    {
        moveDelayCount++;

        while (true)
        {
            puyoPutDelay += 0.02f;
            ifCalledCheckMoveDelayCoroutineAfterPuyoYPosIsNotChangeCheckTime += 0.02f;


            //뿌요가 yPos가 바뀌면 최대 뿌요가 같은 xPos에서 움직일 수 있는 시간 초기화
            //뿌요의 놓기전 남은 시간 제어
            switch (puyoName)
            {
                case "bottom":
                    checkMoveDelayCoroutineBottomXPos = puyoData.puyoData.xPos;
                    checkMoveDelayCoroutineBottomYPos = puyoData.puyoData.yPos;

                    if (checkMoveDelayCoroutineBottomXPos != bottomPuyoData.puyoData.xPos)
                    {
                        puyoPutDelay = 0f;
                    }

                    if (checkMoveDelayCoroutineBottomYPos != bottomPuyoData.puyoData.yPos)
                    {
                        ifCalledCheckMoveDelayCoroutineAfterPuyoYPosIsNotChangeCheckTime = 0f;
                    }

                    break;
                case "upper":
                    checkMoveDelayCoroutineUpperXPos = puyoData.puyoData.xPos;
                    checkMoveDelayCoroutineUpperYPos = puyoData.puyoData.yPos;

                    if (checkMoveDelayCoroutineUpperXPos != upperPuyoData.puyoData.xPos)
                    {
                        puyoPutDelay = 0f;
                    }

                    if (checkMoveDelayCoroutineUpperYPos != upperPuyoData.puyoData.yPos)
                    {
                        ifCalledCheckMoveDelayCoroutineAfterPuyoYPosIsNotChangeCheckTime = 0f;
                    }
                    
                    break;
            }

            if (ifCalledCheckMoveDelayCoroutineAfterPuyoYPosIsNotChangeCheckTime >= 3.5f)
            {
                putPuyo = true;
                moveDelayCount = 0;
                gameController.permitPushButton = false;
                break;
            }
            
            if (puyoPutDelay >= 0.15f)
            {
                putPuyo = true;
                moveDelayCount = 0;
                gameController.permitPushButton = false;
                break;
            }
        }

        yield return null;
    }
    
    private Vector2 newPos;
    
    public Vector2 SetNewVector2(float x, float y)
    {
        newPos = new Vector2(x, y);

        return newPos;
    }
    
    public float GetPuyoSize()
    {
        return fieldHeight / gameController.fieldMax_Y / puyoDownDistance;
    }
}
