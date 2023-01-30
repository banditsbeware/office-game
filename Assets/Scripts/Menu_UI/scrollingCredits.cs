using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingCredits : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [SerializeField] private Vector2Int boundaries;
    private Vector3 speedV3;
    private RectTransform trf;
    void Start() 
    {
        trf = GetComponent<RectTransform>();
        speedV3 = new Vector3(0f, speed,  0f);
    }
    void FixedUpdate()
    {
        trf.localPosition += speedV3;
        if (trf.localPosition.y >= boundaries.y)
        {
            trf.anchoredPosition = new Vector3(0, boundaries.x, 0);
        }
    }
}
