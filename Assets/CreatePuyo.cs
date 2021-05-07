//뿌요를 만들어주고 더이상 가져올 뿌요가 없으면 다시 만들어줍니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePuyo : MonoBehaviour
{
    [SerializeField] private PuyoController puyoController;
    [SerializeField] private CanvasScaler canvasScaler;

    public List<Transform> puyoBag = new List<Transform>(); //재충전용

    public Transform puyoTransform;

    public Image bottomPuyoSprite;
    public Image upperPuyoSprite;

    public Puyo bottomPuyo;
    public Puyo upperPuyo;
    private Transform copyPuyo;

    public float scale;
    public float scalerResolution;
    [SerializeField] private float canvasScalerValue;

    private int j;

    private void OnEnable()
    {
        puyoController = GetComponent<PuyoController>();
        CreatePuyoMethod();
    }

    public void ReFill()
    {
        CreatePuyoMethod();
    }

    private int firstSetScale = 0;
    
    private void CreatePuyoMethod()
    {
        j = 0;
        //canvasScalerValue = Screen.width / canvasScaler.referenceResolution.x;
        canvasScalerValue = Screen.width / 1440;

        for (int i = 0; i < 4; i++) // 0  0, 1, 2, 3  / 1  1, 2, 3 / 2 2, 3/  3, 3
        {    
            for (j = (i == 0 ? 0 : i); j < 4; j++)
            {
                bottomPuyoSprite = puyoTransform.GetChild(0).GetChild(0).GetComponent<Image>();
                upperPuyoSprite = puyoTransform.GetChild(1).GetChild(0).GetComponent<Image>();
                bottomPuyo = puyoTransform.GetChild(0).GetChild(0).GetComponent<Puyo>();
                upperPuyo = puyoTransform.GetChild(1).GetChild(0).GetComponent<Puyo>();
                
                bottomPuyoSprite.sprite = puyoController.puyoGeneralSprites[i];
                upperPuyoSprite.sprite = puyoController.puyoGeneralSprites[j];

                bottomPuyo.puyoData.color = puyoController.puyoColors[i];
                upperPuyo.puyoData.color = puyoController.puyoColors[j];
                
                bottomPuyo.puyoData.xPos = 4;
                upperPuyo.puyoData.xPos = 4;

                bottomPuyo.puyoData.yPos = 2;
                upperPuyo.puyoData.yPos = 1;
                
                //bottomPuyo.name = ReturnStringColorCodeToInt(bottomPuyo.puyoData.color).ToString();
                //upperPuyo.name = ReturnStringColorCodeToInt(upperPuyo.puyoData.color).ToString();
                copyPuyo = Instantiate(puyoTransform);
                
                if (firstSetScale == 0)
                {
                    scale = copyPuyo.localScale.x * canvasScalerValue;
                    firstSetScale++;
                }
                copyPuyo.localScale = new Vector3(scale, scale, scale);
                
                copyPuyo.SetParent(puyoController.puyoParent);

                copyPuyo.gameObject.SetActive(false);
            }
        }
    }

    private int returnCodeValue;

    private int ReturnStringColorCodeToInt(string name)
    {
        switch (name)
        {
            case "red":
                returnCodeValue = 1;
                break;
            case "green":
                returnCodeValue = 2;
                break;
            case "blue":
                returnCodeValue = 3;
                break;
            case "yellow":
                returnCodeValue = 4;
                break;
        }

        return returnCodeValue;
    }
}
