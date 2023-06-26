using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelController : MonoBehaviour
{

    public GameObject EliteButton;
    public GameObject NormalButton;
    public GameObject NoviceButton;
    public GameObject LegendaryButton;
    public Slider WaterMeter;
    public GameObject Levels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Novice()
    {
        WaterMeter.value = 1;
        Destroy(Levels);
    }
    public void Legendary()
    {
        WaterMeter.value = 4;
        Destroy(Levels);
    }
    public void Normal()
    {
        WaterMeter.value = 2;
        Destroy(Levels);
       
    }
    public void Elite()
    {
        WaterMeter.value = 3;
        Destroy(Levels);
    }
}
