using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private RawImage scrollBackground;
    [SerializeField] private float _x, _y;
    
    void Update()
    {
        scrollBackground.uvRect = new Rect(scrollBackground.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, scrollBackground.uvRect.size);
    }
}
