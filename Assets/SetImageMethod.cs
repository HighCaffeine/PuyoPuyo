//같은색의 뿌요를 찾아서 이미지를 넣어줍니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageMethod : MonoBehaviour
{
    private PuyoDataMethod puyoDataMethod;
    private PuyoController puyoController;

    private BitArray puyoCheck = new BitArray(4);
    private string puyoCheckResult;
    private Image puyoImage;

    private Sprite sprite;

    private void Awake()
    {
        puyoController = GetComponent<PuyoController>();
        puyoDataMethod = GetComponent<PuyoDataMethod>();
    }

    private void SetImage(Image image, string check, Puyo puyo)
    {
        switch (check)
        {
            case "0010":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 0);
                break;
            case "1000":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 1);
                break;
            case "1010":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 2);
                break;
            case "0100":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 3);
                break;
            case "0110":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 4);
                break;
            case "1100":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 5);
                break;
            case "1110":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 6);
                break;
            case "0001":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 7);
                break;
            case "0011":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 8);
                break;
            case "1001":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 9);
                break;
            case "1011":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 10);
                break;
            case "0101":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 11);
                break;
            case "0111":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 12);
                break;
            case "1101":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 13);
                break;
            case "1111":
                image.sprite = GetPuyoFieldImage(puyo.puyoData.color, 14);
                break;
        }
    }
    
    private Sprite GetPuyoFieldImage(string color, int num)
    {
        sprite = puyoController.puyoGeneralSprites[puyoDataMethod.ReturnColorCode(color) - 1];

        switch (color)
        {
            case "red":
                sprite = puyoController.redFieldCheckSprites[num];
                break;
            case "green":
                sprite = puyoController.greenFieldCheckSprites[num];
                break;
            case "blue":
                sprite = puyoController.blueFieldCheckSprites[num];
                break;
            case "yellow":
                sprite = puyoController.yellowFieldCheckSprites[num];
                break;
        }

        return sprite;
    }

    private void ChangePuyoImage(Puyo puyo, Transform puyoTransform)
    {
        puyoCheckResult = null;
        puyoCheckResult = puyoDataMethod.ConnectedPuyoCheck(puyo);


        if (puyo.puyoData.connectedStatus != puyoCheckResult)
        {
            puyo.puyoData.connectedStatus = puyoCheckResult;
            puyoImage = puyoTransform.GetComponent<Image>();
            SetImage(puyoImage, puyoCheckResult, puyo);
        }
    }

    private int connectedChildCount;
    private Puyo connectedPuyoData;
    private Transform puyo;

    public void SetPuyoConnectedImage()
    {
        for (int i = 1; i <= 4; i++)
        {
            switch (i)
            {
                case 1:
                    connectedChildCount = puyoController.redPuyoParent.childCount;
                    SetConnectedImage(connectedChildCount, puyoController.redPuyoParent);
                    break;
                case 2:
                    connectedChildCount = puyoController.greenPuyoParent.childCount;
                    SetConnectedImage(connectedChildCount, puyoController.greenPuyoParent);
                    break;
                case 3:
                    connectedChildCount = puyoController.bluePuyoParent.childCount;
                    SetConnectedImage(connectedChildCount, puyoController.bluePuyoParent);
                    break;
                case 4:
                    connectedChildCount = puyoController.yellowPuyoParent.childCount;
                    SetConnectedImage(connectedChildCount, puyoController.yellowPuyoParent);
                    break;
            }
        }
    }

    private void SetConnectedImage(int childCount, Transform puyoParent)
    {
        for (int i = 0; i < childCount; i++)
        {
            puyo = puyoParent.GetChild(i).GetChild(0);
            connectedPuyoData = puyo.GetComponent<Puyo>();
            ChangePuyoImage(connectedPuyoData, puyo);
        }
    }
}
