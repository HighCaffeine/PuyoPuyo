//뿌요가 놓아진 뒤 뿌요의 정리를 해줍니다.
//줄마다 내려줄 뿌요가 있으면 내려주고
//같은색 뿌요가 4개 이상 있을때 부숴줍니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingMethod : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private PuyoController puyoController;
    [SerializeField] private PuyoDataMethod puyoDataMethod;
    [SerializeField] private PuyoMoveMethod puyoMoveMethod;
    [SerializeField] private SetImageMethod setImageMethod;

    private List<Transform>[] lineSortingList = new List<Transform>[6];
    private List<int>[] lineDownCount = new List<int>[6];
    
    private List<Transform> line1SortingList = new List<Transform>();
    private List<Transform> line2SortingList = new List<Transform>();
    private List<Transform> line3SortingList = new List<Transform>();
    private List<Transform> line4SortingList = new List<Transform>();
    private List<Transform> line5SortingList = new List<Transform>();
    private List<Transform> line6SortingList = new List<Transform>();

    private List<int> line1DownCount = new List<int>();
    private List<int> line2DownCount = new List<int>();
    private List<int> line3DownCount = new List<int>();
    private List<int> line4DownCount = new List<int>();
    private List<int> line5DownCount = new List<int>();
    private List<int> line6DownCount = new List<int>();

    private void Awake()
    {
        gameController = GetComponent<GameController>();
        puyoController = GetComponent<PuyoController>();
        puyoDataMethod = GetComponent<PuyoDataMethod>();
        puyoMoveMethod = GetComponent<PuyoMoveMethod>();
        setImageMethod = GetComponent<SetImageMethod>();

        lineSortingList[0] = line1SortingList;
        lineSortingList[1] = line2SortingList;
        lineSortingList[2] = line3SortingList;
        lineSortingList[3] = line4SortingList;
        lineSortingList[4] = line5SortingList;
        lineSortingList[5] = line6SortingList;

        lineDownCount[0] = line1DownCount;
        lineDownCount[1] = line2DownCount;
        lineDownCount[2] = line3DownCount;
        lineDownCount[3] = line4DownCount;
        lineDownCount[4] = line5DownCount;
        lineDownCount[5] = line6DownCount;

        destroyCountArray = new int[4];

        puyoParentList[0] = puyoController.redPuyoParent;
        puyoParentList[1] = puyoController.greenPuyoParent;
        puyoParentList[2] = puyoController.bluePuyoParent;
        puyoParentList[3] = puyoController.yellowPuyoParent;

        destroyList[0] = redDestroyPuyo;
        destroyList[1] = greenDestroyPuyo;
        destroyList[2] = blueDestroyPuyo;
        destroyList[3] = yellowDestroyPuyo;
    }

    private Transform lineSortingTransformForAddToList;
    private int bottomSpaceCount;

    private Transform lineSortingTransform;
    private Puyo lineSortingPuyo;

    private void FieldCheckAndSortingPuyo(int lineNumber)
    {
        InitializingLineValue(lineNumber);
        
        for (int y = 13; y > 1; y--)
        {
            if (gameController.field[y - 1, lineNumber - 1] != 0)
            {
                lineSortingTransformForAddToList = puyoDataMethod.GetPuyo(lineNumber, y, gameController.field[y - 1, lineNumber - 1]);
                bottomSpaceCount = gameController.fieldMax_Y - puyoDataMethod.HowManyBottomPuyo(lineNumber, y) - y;

                Debug.Log(lineNumber + "_" + bottomSpaceCount);

                if (bottomSpaceCount > 0)
                {
                    lineSortingList[lineNumber - 1].Add(lineSortingTransformForAddToList);
                    lineDownCount[lineNumber - 1].Add(bottomSpaceCount);

                    bottomSpaceCount = 0;
                }
            }
        }

        StartCoroutine(SortingLine(lineNumber));
    }
    
    private IEnumerator SortingLine(int lineNumber)
    {
        for (int j = 0; j < lineSortingList[lineNumber - 1].Count; j++)
        {
            lineSortingTransform = lineSortingList[lineNumber - 1][j];
            lineSortingPuyo = lineSortingTransform.GetChild(0).GetComponent<Puyo>();

            gameController.field[lineSortingPuyo.puyoData.yPos - 1, lineSortingPuyo.puyoData.xPos - 1] = 0;

            Debug.Log(lineNumber + "_" + lineSortingPuyo.puyoData.yPos + lineDownCount[lineNumber - 1][j]);
            gameController.field[lineSortingPuyo.puyoData.yPos + lineDownCount[lineNumber - 1][j] - 1, lineNumber - 1] 
                = puyoDataMethod.StringColorToIntColorCode(lineSortingPuyo.puyoData.color);

            Debug.Log(lineNumber + "_" + lineDownCount[lineNumber - 1][j]);
            for (int i = 0; i < lineDownCount[lineNumber - 1][j]; i++)
            {
                puyoMoveMethod.MovePuyoVertical(lineSortingTransform, lineSortingPuyo, true);

                yield return new WaitForSeconds(0.08f);
            }

            //Debug.Log(lineSortingPuyo.puyoData.yPos + "_" + lineSortingPuyo.puyoData.xPos);
            lineSortingTransform.name = lineSortingPuyo.puyoData.xPos + "_" + lineSortingPuyo.puyoData.yPos + "_" + lineSortingPuyo.puyoData.color;
        }

        puyoDataMethod.CheckFieldStatus();

        //DestroySortingMethodLaunch();
        yield return null;

        setImageMethod.SetPuyoConnectedImage();
    }

    private void InitializingLineValue(int lineNumber)
    {
        lineSortingTransformForAddToList = null;
        bottomSpaceCount = 0;

        if (lineSortingList[lineNumber - 1].Count != 0)
        {
            lineSortingList[lineNumber - 1].Clear();
            lineDownCount[lineNumber - 1].Clear();
        }
    }

    private int puyoMainColorCode;

    private Transform[] puyoParentList = new Transform[4];
    public List<Transform>[] destroyList = new List<Transform>[4];

    private List<Transform> redDestroyPuyo = new List<Transform>();
    private List<Transform> greenDestroyPuyo = new List<Transform>();
    private List<Transform> blueDestroyPuyo = new List<Transform>();
    private List<Transform> yellowDestroyPuyo = new List<Transform>();

    private List<int> redPuyoXPos = new List<int>();
    private List<int> redPuyoYPos = new List<int>();
    private List<int> greenPuyoXPos = new List<int>();
    private List<int> greenPuyoYPos = new List<int>();
    private List<int> bluePuyoXPos = new List<int>();
    private List<int> bluePuyoYPos = new List<int>();
    private List<int> yellowPuyoXPos = new List<int>();
    private List<int> yellowPuyoYPos = new List<int>();

    private int[] destroyCountArray;

    private int destroyCount = 0;
    private int childCount;

    private int firstAddValue;
    private int secondAddValue;

    private int continueDestroyChildNumber;
    
    private int[,] copyFieldData = new int[14, 6];

    private Transform compareChildPuyo;
    private string[] compareChildPuyoName;

    public void DestroySortingMethodLaunch()
    {
        checkedPuyoXPos.Clear();
        checkedPuyoYPos.Clear();
        DestroyPuyoMethod(1);
        
        checkedPuyoXPos.Clear();
        checkedPuyoYPos.Clear();
        DestroyPuyoMethod(2);
        
        checkedPuyoXPos.Clear();
        checkedPuyoYPos.Clear();
        DestroyPuyoMethod(3);

        checkedPuyoXPos.Clear();
        checkedPuyoYPos.Clear();
        DestroyPuyoMethod(4);
        
        setImageMethod.SetPuyoConnectedImage();
        gameController.ShowNextPuyo();
    }

    private void DestroyPuyoMethod(int colorCode)
    {
        InitializingDestroyValue(colorCode);
        childCount = puyoParentList[colorCode - 1].childCount;
        continueDestroyChildNumber = 0;

        if (childCount >= 4)
        {
            compareChildPuyo = puyoParentList[colorCode - 1].GetChild(0);
            string[] compareChildPuyoName = compareChildPuyo.name.Split('_');

            if (CheckedPuyoCheck(int.Parse(compareChildPuyoName[0]), int.Parse(compareChildPuyoName[1]), colorCode))
            {
                StartCoroutine(DestroyTransformAddToListRecursive(colorCode, compareChildPuyoName, 1));
            }

            destroyCountArray[colorCode - 1] = destroyCount;

            //Debug.Log(destroyCountArray[colorCode - 1]);

            CheckDestroyPuyoName(colorCode);

            if (destroyCount >= 4)
            {
                DestroyMethod(colorCode);
                puyoDataMethod.RefreshScore(destroyCount);
                puyoDataMethod.ScoreSaveToJson();
                
                FieldCheckAndSortingPuyo(1);
                FieldCheckAndSortingPuyo(2);
                FieldCheckAndSortingPuyo(3);
                FieldCheckAndSortingPuyo(4);
                FieldCheckAndSortingPuyo(5);
                FieldCheckAndSortingPuyo(6);
            }
            else
            {
                if (destroyCount != 1)
                {
                    AddToCheckedList(colorCode);
                }
                else
                {
                    checkedPuyoXPos.Add(int.Parse(compareChildPuyoName[0]));
                    checkedPuyoYPos.Add(int.Parse(compareChildPuyoName[1]));
                }
            }
            
            if (childCount - destroyCount >= 4)
            {
                DestroySortingMethodLaunch();
            }

        }
    }
    
    private void CheckDestroyPuyoName(int colorCode)
    {
        for (int i = 0; i < destroyList[colorCode - 1].Count; i++)
        {
            Debug.Log("PuyoName :" + destroyList[colorCode - 1][i].name);
        }
    }
    
    //private string[] comparePuyoName;
    private int currentChildNumber = 0;

    private Transform childPuyoTransform;
    private Transform comparePuyoForAddToList;

    private List<int> checkedPuyoXPos = new List<int>();
    private List<int> checkedPuyoYPos = new List<int>();

    private IEnumerator DestroyTransformAddToListRecursive(int colorCode, string[] puyoName, int currentChildNumber)
    {
        if (destroyCount != 0)
        {
            childPuyoTransform = puyoParentList[colorCode - 1].GetChild(currentChildNumber);
        }
        else
        {
            destroyList[colorCode - 1].Add(compareChildPuyo);
            OverlapTrueValueAddToList(colorCode, puyoName[0], puyoName[1]);
        }

        
        for (int i = 0; i < childCount; i++)
        {
            comparePuyoForAddToList = puyoParentList[colorCode - 1].GetChild(i);
            string[] comparePuyoName = comparePuyoForAddToList.name.Split('_');


            if (Mathf.Abs(int.Parse(puyoName[0]) - int.Parse(comparePuyoName[0])) + Mathf.Abs(int.Parse(puyoName[1]) - int.Parse(comparePuyoName[1])) == 1)
            {
                if (CheckOverlapDestroyValue(int.Parse(puyoName[0]), int.Parse(puyoName[1]), colorCode))
                {
                    destroyList[colorCode - 1].Add(childPuyoTransform);
                    OverlapTrueValueAddToList(colorCode, puyoName[0], puyoName[1]);
                }

                if (CheckOverlapDestroyValue(int.Parse(comparePuyoName[0]), int.Parse(comparePuyoName[1]), colorCode))
                {
                    destroyList[colorCode - 1].Add(comparePuyoForAddToList);
                    OverlapTrueValueAddToList(colorCode, comparePuyoName[0], comparePuyoName[1]);
                    StartCoroutine(DestroyTransformAddToListRecursive(colorCode, comparePuyoName, i));
                }
            }
        }

        yield return null;
    }

    private void AddToCheckedList(int colorCode)
    {
        for (int i = 0; i < destroyList[colorCode - 1].Count; i++)
        {
            switch (colorCode)
            {
                case 1:
                    checkedPuyoXPos.Add(redPuyoXPos[i]);
                    checkedPuyoYPos.Add(redPuyoYPos[i]);
                    break;
                case 2:
                    checkedPuyoXPos.Add(greenPuyoXPos[i]);
                    checkedPuyoYPos.Add(greenPuyoYPos[i]);
                    break;
                case 3:
                    checkedPuyoXPos.Add(bluePuyoXPos[i]);
                    checkedPuyoYPos.Add(bluePuyoYPos[i]);
                    break;
                case 4:
                    checkedPuyoXPos.Add(yellowPuyoXPos[i]);
                    checkedPuyoYPos.Add(yellowPuyoYPos[i]);
                    break;
            }
        }
    }

    private bool CheckedPuyoCheck(int xPos, int yPos, int colorCode)
    {
        if (checkedPuyoXPos.Count != 0)
        {
            for (int i = 0; i < checkedPuyoXPos.Count; i++)
            {
                if (xPos == checkedPuyoXPos[i] && yPos == checkedPuyoYPos[i])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void OverlapTrueValueAddToList(int colorCode, string xPosName, string yPosName)
    {
        switch (colorCode)
        {
            case 1:
              redPuyoXPos.Add(int.Parse(xPosName));
              redPuyoYPos.Add(int.Parse(yPosName));
              break;
            case 2:
              greenPuyoXPos.Add(int.Parse(xPosName));
              greenPuyoYPos.Add(int.Parse(yPosName));
              break;
            case 3:
              bluePuyoXPos.Add(int.Parse(xPosName));
              bluePuyoYPos.Add(int.Parse(yPosName));
              break;
            case 4:
              yellowPuyoXPos.Add(int.Parse(xPosName));
              yellowPuyoYPos.Add(int.Parse(yPosName));
              break;
        }
        
        destroyCount++;
    }

    private int posListCountInOverlapCheck;

    private bool CheckOverlapDestroyValue(int compareXPos, int compareYPos, int colorCode)
    {
        switch (colorCode)
        {
            case 1:
                posListCountInOverlapCheck = redPuyoXPos.Count;
                return CheckOverlap(redPuyoXPos, redPuyoYPos, compareXPos, compareYPos);
            case 2:
                posListCountInOverlapCheck = greenPuyoXPos.Count;
                return CheckOverlap(greenPuyoXPos, greenPuyoYPos, compareXPos, compareYPos);
            case 3:
                posListCountInOverlapCheck = bluePuyoXPos.Count;
                return CheckOverlap(bluePuyoXPos, bluePuyoYPos, compareXPos, compareYPos);
            case 4:
                posListCountInOverlapCheck = yellowPuyoXPos.Count;
                return CheckOverlap(yellowPuyoXPos, yellowPuyoYPos, compareXPos, compareYPos);
        }

        return false;
    }

    private bool CheckOverlap(List<int> xPosList, List<int> yPosList, int xPos, int yPos)
    {
        for (int i = 0; i < posListCountInOverlapCheck; i++)
        {
            if (xPosList[i] == xPos && yPosList[i] == yPos)
            {
                return false;
            }
        }

        posListCountInOverlapCheck = 0;

        return true;
    }
    
    private void InitializingDestroyValue(int colorCode)
    {
        destroyCount = 0;
        childCount = 0;
        

        destroyList[colorCode - 1].Clear();
        destroyCountArray[colorCode - 1] = 0;

        //comparePuyoName = null;

        switch (colorCode)
        {
            case 1:
                redPuyoXPos.Clear();
                redPuyoYPos.Clear();
                break;
            case 2:
                greenPuyoXPos.Clear();
                greenPuyoYPos.Clear();
                break;
            case 3:
                bluePuyoXPos.Clear();
                bluePuyoYPos.Clear();
                break;
            case 4:
                yellowPuyoXPos.Clear();
                yellowPuyoYPos.Clear();
                break;
        }
    }

    private int destroyXPos;
    private int destroyYPos;

    private void DestroyMethod(int colorCode)
    {
        //Debug.Log(destroyList[colorCode - 1].Count);

        for (int count = 0; count < destroyList[colorCode - 1].Count; count++)
        {
            switch (colorCode)
            {
                case 1:
                    destroyXPos = redPuyoXPos[count];
                    destroyYPos = redPuyoYPos[count];

                    break;
                case 2:
                    destroyXPos = greenPuyoXPos[count];
                    destroyYPos = greenPuyoYPos[count];

                    break;
                case 3:
                    destroyXPos = bluePuyoXPos[count];
                    destroyYPos = bluePuyoYPos[count];

                    break;
                case 4:
                    destroyXPos = yellowPuyoXPos[count];
                    destroyYPos = yellowPuyoYPos[count];

                    break;
            }

            //Debug.Log(destroyXPos + "_" + destroyYPos);
            Destroy(destroyList[colorCode - 1][count].gameObject);
            gameController.field[destroyYPos - 1, destroyXPos - 1] = 0;
        }
        
        StartCoroutine(puyoDataMethod.RefreshScore(destroyList[colorCode - 1].Count));
        puyoDataMethod.CheckFieldStatus();
        puyoDataMethod.ScoreSaveToJson();
    }

    private int whileDownCount;

    public void DownPuyoLaunch()
    {
        StartCoroutine(DownPuyoHasNotUnderPuyo(puyoController.bottomPuyoParent, puyoController.bottomPuyoData));
        StartCoroutine(DownPuyoHasNotUnderPuyo(puyoController.upperPuyoParent, puyoController.upperPuyoData));
    }
    
    public IEnumerator DownPuyoHasNotUnderPuyo(Transform puyoTransform, Puyo puyoData)
    {
        if (puyoData.puyoData.yPos != 14)
        {
            if (gameController.field[puyoData.puyoData.yPos, puyoData.puyoData.xPos - 1] == 0)
            {
                whileDownCount = puyoController.ReturnBottomSpaceCount(puyoData.puyoData.xPos, puyoData.puyoData.yPos);
                gameController.field[puyoData.puyoData.yPos - 1, puyoData.puyoData.xPos - 1] = 0;
                gameController.field[puyoData.puyoData.yPos + whileDownCount - 1, puyoData.puyoData.xPos - 1] = puyoDataMethod.StringColorToIntColorCode(puyoData.puyoData.color);

                for (int i = 0; i < whileDownCount; i++)
                {
                    puyoMoveMethod.MovePuyoVertical(puyoTransform, puyoData, true);
                    
                    yield return new WaitForSeconds(0.08f);
                }
                puyoDataMethod.SetPuyoNameGeneralToPos(puyoTransform, puyoData);
            }
        }
        
        puyoDataMethod.CheckFieldStatus();
        yield return null;
    }
}
