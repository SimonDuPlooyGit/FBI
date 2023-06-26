using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movementsystem : MonoBehaviour
{
    private float StartPosX;
    private float StartPosY;
    private bool CLickedOn;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (CLickedOn == true)
        {
            Vector3 MousePos;
            MousePos = Input.mousePosition;
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);
            CLickedOn = true;
            this.gameObject.transform.localPosition = new Vector3(MousePos.x,MousePos.y,0);
        }
        
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 MousePos;
            MousePos = Input.mousePosition;
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);
            CLickedOn = true;
        }
    }
    private void OnMouseUp()
    {
        CLickedOn = false;
    }
}
