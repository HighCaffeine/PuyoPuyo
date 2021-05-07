//뿌요의 정보 class
//뿌요의 좌표/색/같은색 뿌요가 이어진 정보를 가지고 있습니다.
using UnityEngine;
using System;

[Serializable]
public struct PuyoData
{
    public string color;
    public int xPos;
    public int yPos;
    public string connectedStatus;
}

public class Puyo : MonoBehaviour
{
    public PuyoData puyoData;
}
