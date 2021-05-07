//뿌요의 데이터를 관리하는 곳입니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoreData
{
    public string score;
}

public class PuyoDataMethod : MonoBehaviour
{
    private GameController gameController;
    private PuyoController puyoController;
    private PuyoDataMethod puyoDataMethod;
    [SerializeField] private Text fieldStatus;
    
    private BitArray connectedCheck = new BitArray(4);
    private string connectedResultReturn;

    private int count;
    private int value;
    private int[,] copyFieldData;
    private bool maxXPos;
    private bool minXPos;
    private bool maxYPos;
    
    private int setValue;
    private bool status;

    private bool foundPuyo;
    private int colorCode;

    public bool gameOver;

    private int score;
    [SerializeField] private Text scoreText;

    private void Awake()
    {
        gameController = GetComponent<GameController>();
        puyoController = GetComponent<PuyoController>();
        puyoDataMethod = GetComponent<PuyoDataMethod>();

        copyFieldData = new int[gameController.fieldMax_Y - 1, gameController.fieldMax_X - 1];
    }
    
    public void ScoreSaveToJson()
    {
        ScoreData scoreData = new ScoreData();
        scoreData.score = score.ToString();
        string puyoScore = JsonUtility.ToJson(scoreData);

        File.WriteAllText(Application.dataPath + "/PuyoScoreJson.json", puyoScore);
    }

    public IEnumerator RefreshScore(int destroyCount)
    {
        for (int i = 0; i < destroyCount; i++)
        {
            scoreText.text = (int.Parse(scoreText.text) + 60).ToString();

            yield return new WaitForSeconds(0.02f);
        }

        yield return null;
    }

    public void CheckFieldStatus()
    {
        fieldStatus.text = null;
        for (int y = 0; y < gameController.fieldMax_Y; y++)
        {
            for (int x = 0; x < gameController.fieldMax_X; x++)
            {
                fieldStatus.text += gameController.field[y, x].ToString();
            }
            fieldStatus.text += "\n";
        }
    }
    
    private int GetPuyoCount(int lineNumber)
    {
        count = 0;

        for (int i = 13; i > 0; i--)
        {
            if (gameController.field[lineNumber, i] != 0)
            {
                count++;
            }
        }

        return count;
    }

    public int HowManyBottomPuyo(int puyoXPos, int puyoYPos)
    {
        count = 0;

        for (int y = puyoYPos; y < gameController.fieldMax_Y; y++)
        {
            if (gameController.field[y, puyoXPos - 1] != 0)
            {
                count++;
            }
        }

        return count;
    }

    public bool GameOverCheck(Puyo bottomPuyo, Puyo upperPuyo)
    {
        if (bottomPuyo.puyoData.yPos == 1 || bottomPuyo.puyoData.yPos == 2)
        {
            gameOver = true;
        }

        if (upperPuyo.puyoData.yPos == 1 || upperPuyo.puyoData.yPos == 2)
        {
            gameOver = true;
        }

        return gameOver;
    }
    
    private string[] foundPuyoSplitName;
    private Transform getPuyoReturnTransform;
    private Transform getChildFromParent;

    public Transform GetPuyo(int x, int y, int colorCode)
    {
        switch (colorCode)
        {
            case 1:
                for (int i = 0; i < puyoController.redPuyoParent.childCount; i++)
                {
                    getChildFromParent = puyoController.redPuyoParent.GetChild(i);
                    foundPuyoSplitName = getChildFromParent.name.Split('_');

                    if (foundPuyoSplitName[0] == x.ToString() && foundPuyoSplitName[1] == y.ToString())
                    {
                        getPuyoReturnTransform = getChildFromParent;
                        break;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < puyoController.greenPuyoParent.childCount; i++)
                {
                    getChildFromParent = puyoController.greenPuyoParent.GetChild(i);
                    foundPuyoSplitName = getChildFromParent.name.Split('_');

                    if (foundPuyoSplitName[0] == x.ToString() && foundPuyoSplitName[1] == y.ToString())
                    {
                        getPuyoReturnTransform = getChildFromParent;
                        break;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < puyoController.bluePuyoParent.childCount; i++)
                {
                    getChildFromParent = puyoController.bluePuyoParent.GetChild(i);
                    foundPuyoSplitName = getChildFromParent.name.Split('_');

                    if (foundPuyoSplitName[0] == x.ToString() && foundPuyoSplitName[1] == y.ToString())
                    {
                        getPuyoReturnTransform = getChildFromParent;
                        break;
                    }
                }
                break;
            case 4:
                for (int i = 0; i < puyoController.yellowPuyoParent.childCount; i++)
                {
                    getChildFromParent = puyoController.yellowPuyoParent.GetChild(i);
                    foundPuyoSplitName = getChildFromParent.name.Split('_');

                    if (foundPuyoSplitName[0] == x.ToString() && foundPuyoSplitName[1] == y.ToString())
                    {
                        getPuyoReturnTransform = getChildFromParent;
                        break;
                    }
                }
                break;
        }

        return getPuyoReturnTransform;
    }

    public void SetPuyoNameGeneralToPos(Transform puyo, Puyo puyoData)
    {
        puyo.name = puyoData.puyoData.xPos + "_" + puyoData.puyoData.yPos + "_" + puyoData.puyoData.color;
        gameController.field[puyoData.puyoData.yPos - 1, puyoData.puyoData.xPos - 1] = puyoDataMethod.ReturnColorCode(puyoData.puyoData.color);
    }

    public void PuyoMoveToEachColorParent(Transform puyo, int colorCode)
    {
        switch (colorCode)
        {
            case 1:
                puyo.SetParent(puyoController.redPuyoParent);
                break;
            case 2:
                puyo.SetParent(puyoController.greenPuyoParent);
                break;
            case 3:
                puyo.SetParent(puyoController.bluePuyoParent);
                break;
            case 4:
                puyo.SetParent(puyoController.yellowPuyoParent);
                break;
        }
    }

    public int StringColorToIntColorCode(string stringColor)
    {
        value = 0;

        switch (stringColor)
        {
            case "red":
                value = 1;
                break;
            case "green":
                value = 2;
                break;
            case "blue":
                value = 3;
                break;
            case "yellow":
                value = 4;
                break;
        }

        return value;
    }

    private void CopyFieldDataMethod(int mode)
    {
        // 1 gameField -> copyField
        // 2 copyField -> gameField

        switch (mode)
        {
            case 1:
                for (int y = 0; y < gameController.fieldMax_Y; y++)
                {
                    for (int x = 0; x < gameController.fieldMax_X; x++)
                    {
                        copyFieldData[y, x] = gameController.field[y, x];
                    }
                }
                break;
            case 2:
                for (int y = 0; y < gameController.fieldMax_Y; y++)
                {
                    for (int x = 0; x < gameController.fieldMax_X; x++)
                    {
                        gameController.field[y, x] = copyFieldData[y, x];
                    }
                }
                break;
        }
    }
    
    public string ConnectedPuyoCheck(Puyo puyoData)
    {
        connectedCheck.Set(0, false);
        connectedCheck.Set(1, false);
        connectedCheck.Set(2, false);
        connectedCheck.Set(3, false);
        connectedResultReturn = null;

        maxXPos = false;
        minXPos = false;
        maxYPos = false;

        if (puyoData.puyoData.xPos == gameController.fieldMax_X)
            maxXPos = true;

        if (puyoData.puyoData.xPos == 0)
            minXPos = true;

        if (puyoData.puyoData.yPos == gameController.fieldMax_Y)
            maxYPos = true;

        if (puyoData.puyoData.yPos > 1)
        {
            //상 1000
            if (gameController.field[puyoData.puyoData.yPos - 2, puyoData.puyoData.xPos - 1] == ReturnColorCode(puyoData.puyoData.color))
            {
                connectedCheck.Set(0, true);
            }
        }

        if (puyoData.puyoData.xPos < 6)
        {
            //우 0100
            if (!maxXPos && gameController.field[puyoData.puyoData.yPos - 1, puyoData.puyoData.xPos] == ReturnColorCode(puyoData.puyoData.color))
            {
                connectedCheck.Set(1, true);
            }
        }

        if (puyoData.puyoData.yPos < 14)
        {
            //하 0010
            if (!maxYPos && gameController.field[puyoData.puyoData.yPos, puyoData.puyoData.xPos - 1] == ReturnColorCode(puyoData.puyoData.color))
            {
                connectedCheck.Set(2, true);
            }
        }

        if (puyoData.puyoData.xPos > 1)
        {
            //좌 0001
            if (!minXPos && gameController.field[puyoData.puyoData.yPos - 1, puyoData.puyoData.xPos - 2] == ReturnColorCode(puyoData.puyoData.color))
            {
                connectedCheck.Set(3, true);
            }
        }


        for (int i = 0; i < connectedCheck.Count; i++)
        {
            if (connectedCheck[i])
            {
                connectedResultReturn += "1";
            }
            else if (!connectedCheck[i])
            {
                connectedResultReturn += "0";
            }
        }

        return connectedResultReturn;
    }

    public int ReturnColorCode(string color)
    {
        switch (color)
        {
            case "red":
                return 1;
            case "green":
                return 2;
            case "blue":
                return 3;
            case "yellow":
                return 4;
        }

        return 0;
    }

    public void SetPuyoDataInField(Puyo puyo, int x, int y)
    {
        setValue = 0;

        switch (puyo.puyoData.color)
        {
            case "red":
                setValue = 1;
                break;
            case "green":
                setValue = 2;
                break;
            case "blue":
                setValue = 3;
                break;
            case "yellow":
                setValue = 4;
                break;
        }
        
        gameController.field[y, x] = setValue;
    }
    
    public bool FoundPuyo(Puyo puyo, string checkDirection)
    {
        foundPuyo = false;
        colorCode = 0;

        switch (checkDirection)
        {
            case "left":
                if (puyo.puyoData.xPos == 1)
                {
                    foundPuyo = false;
                    break;
                }

                colorCode = gameController.field[puyo.puyoData.yPos - 1, puyo.puyoData.xPos - 2];

                if (colorCode != 0)
                {
                    foundPuyo = true;
                }
                else
                {
                    foundPuyo = false;
                }

                break;
            case "right":
                if (puyo.puyoData.xPos == 6)
                {
                    foundPuyo = false;
                    break;
                }

                colorCode = gameController.field[puyo.puyoData.yPos - 1, puyo.puyoData.xPos];

                if (colorCode != 0)
                {
                    foundPuyo = true;
                }
                else
                {
                    foundPuyo = false;
                }

                break;
            case "bottom":
                if (puyo.puyoData.yPos == 14)
                {
                    foundPuyo = false;
                    break;
                }

                colorCode = gameController.field[puyo.puyoData.yPos, puyo.puyoData.xPos - 1];

                if (colorCode != 0)
                {
                    foundPuyo = true;
                }
                else
                {
                    foundPuyo = false;
                }
                break;
        }

        return foundPuyo;
    }
}
