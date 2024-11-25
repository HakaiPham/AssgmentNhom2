using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairControll : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform crossHair;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        crossHair.position = mousePosition;
    }
}
