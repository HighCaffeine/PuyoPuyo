//Canvas에 있는 모든 gameObject의 크기를 화면에 맞게 맞춰줍니다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformScaler : MonoBehaviour
{
    [SerializeField] private List<RectTransform> scalingTransformList = new List<RectTransform>();
    public float scale;
    
    private void Awake()
    {
        scale = 1920f / 2540f;
        Scaling();
    }

    private void Scaling()
    {
        for (int i = 0; i < scalingTransformList.Count; i++)
        {
            scalingTransformList[i].localScale *= scale;
        }
    }
}
