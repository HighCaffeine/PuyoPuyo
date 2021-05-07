//키 입력을 받아주고 필드의 정보를 갖고있고 다음 뿌요를 보내줍니다. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private RotateMethod rotateMethod;
    [SerializeField] private PuyoMoveMethod puyoMoveMethod;
    [SerializeField] private PuyoController puyoController;
    [SerializeField] private CreatePuyo createPuyo;
    [SerializeField] private SortingMethod sortingMethod;
    [SerializeField] private TransformScaler transformScaler;

    public int[,] field;

    public int fieldMax_X;
    public int fieldMax_Y;

    public delegate void PuyoClosedMsg();
    public event PuyoClosedMsg puyoClosedMsg;

    private int setScale;

    public bool permitPushButton = true;
    [SerializeField] private float buttonDelay;

    private int randNum;

    private Transform nextPuyo;
    private Transform secondPuyo;
    private Transform firstPuyo;
    private Transform fieldPuyo;
    private Transform puyoTransform;
    
    private void Awake()
    {
        puyoMoveMethod = GetComponent<PuyoMoveMethod>();
        rotateMethod = GetComponent<RotateMethod>();
        sortingMethod = GetComponent<SortingMethod>();
        transformScaler = GetComponent<TransformScaler>();
    }

    private void Start()
    {
        field = new int[fieldMax_Y, fieldMax_X];
        InitField();

        puyoController = GetComponent<PuyoController>();
        createPuyo = GetComponent<CreatePuyo>();

        ShowNextPuyo();
    }

    public bool canButtonDoubleClick;
    private float canButtonDoubleClickTimeCount;

    public void MovePuyoWhenPushTheButton(string direction)
    {
        if (permitPushButton && !puyoController.lockButton)
        {
            switch (direction)
            {
                case "left":
                    puyoMoveMethod.MovePuyoHorizontal(puyoController.bottomPuyoParent, puyoController.upperPuyoParent, puyoController.bottomPuyoData, puyoController.upperPuyoData, "left");
                    break;
                case "right":
                    puyoMoveMethod.MovePuyoHorizontal(puyoController.bottomPuyoParent, puyoController.upperPuyoParent, puyoController.bottomPuyoData, puyoController.upperPuyoData, "right");
                    break;
                case "down":
                    puyoMoveMethod.MovePuyoVertical(puyoController.bottomPuyoParent, puyoController.upperPuyoParent, puyoController.bottomPuyoData, puyoController.upperPuyoData);
                    break;
                case "leftRotate":
                    rotateMethod.RotateButton(puyoController.bottomPuyoData, puyoController.upperPuyoData, true, false);
                    break;
                case "rightRotate":
                    rotateMethod.RotateButton(puyoController.bottomPuyoData, puyoController.upperPuyoData, false, true);
                    break;
            }

            permitPushButton = false;
            StartCoroutine(ButtonDelay());
        }
    }

    private IEnumerator CountButtonDoubleClick()
    {
        canButtonDoubleClick = true;
        canButtonDoubleClickTimeCount = 0;

        while (canButtonDoubleClickTimeCount <= 0.08)
        {
            canButtonDoubleClickTimeCount += 0.02f;
        }

        canButtonDoubleClick = false;

        yield return null;
    }

    private IEnumerator ButtonDelay()
    {
        float time = 0f;

        while (time <= buttonDelay)
        {
            time += 0.2f;
            yield return new WaitForSeconds(0.02f);
        }

        permitPushButton = true;

        yield return null;
    }

    private void InitField()
    {
        for (int y = 0; y < fieldMax_Y; y++)
        {
            for (int x = 0; x < fieldMax_X; x++)
            {
                field[y, x] = 0;
            }
        }
    }

    [SerializeField] private Transform firstShowPuyoTransform;
    [SerializeField] private Transform secondShowPuyoTransform;
    [SerializeField] private Transform firstShowPuyoPos;
    [SerializeField] private Transform secondShowPuyoPos;
    [SerializeField] private Transform puyoStartPos;

    public void ShowNextPuyo()
    {
        if (puyoController.puyoParent.childCount == 0)
        {
            createPuyo.ReFill();
        }

        randNum = Random.Range(0, puyoController.puyoParent.childCount - 1);

        nextPuyo = puyoController.puyoParent.GetChild(randNum);

        if (secondShowPuyoTransform.childCount != 0)
        {
            if (firstShowPuyoTransform.childCount != 0)
            {
                fieldPuyo = firstShowPuyoTransform.GetChild(0);
                fieldPuyo.position = puyoStartPos.position;
                fieldPuyo.SetParent(puyoController.fieldPuyoParent);
                fieldPuyo.gameObject.SetActive(true);
                
                if (setScale == 0)
                {
                    setScale++;
                    createPuyo.scalerResolution = fieldPuyo.localScale.x / createPuyo.scale;

                    puyoController.fieldWidth = puyoController.fieldHeightAndWidth.rect.width / createPuyo.scalerResolution;
                    puyoController.fieldHeight = puyoController.fieldHeightAndWidth.rect.height / createPuyo.scalerResolution;

                    puyoController.puyoSize = puyoController.fieldWidth / 6;
                }

                StartCoroutine(puyoController.MovePuyo(fieldPuyo));
            }

            secondPuyo = secondShowPuyoTransform.GetChild(0);
            secondPuyo.SetParent(firstShowPuyoTransform);
            secondPuyo.position = firstShowPuyoPos.position;
            secondPuyo.gameObject.SetActive(true);

            nextPuyo.SetParent(secondShowPuyoTransform);
            nextPuyo.position = secondShowPuyoPos.position;
            nextPuyo.gameObject.SetActive(true);
        }
        else
        {
            nextPuyo.SetParent(secondShowPuyoTransform);
            nextPuyo.transform.position = secondShowPuyoTransform.position;
            nextPuyo.gameObject.SetActive(true);
        }

        if (puyoController.fieldPuyoParent.childCount == 0)
        {
            ShowNextPuyo();
        }
    }

    private string[] nameValue;

    private int GetColorCodeStringToIntInShowPuyoPhase(string name)
    {
        nameValue = name.Split('_');

        return int.Parse(nameValue[1]);
    }
}
