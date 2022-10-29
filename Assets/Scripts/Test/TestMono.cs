using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLDO;

public class TestMono : MonoBehaviour
{    
    void Start()
    {        
        string MainCameraPosX = ConfigManager.ConfigIO.Instance.GetParameter("Camera/CameraSettings/MAINCAMERA/position/X",false);
        Debug.Log("Main Camera Position X: " + MainCameraPosX);
        string GameAtr1 = ConfigManager.ConfigIO.Instance.GetParameter("Game/Vehicle/VehicleMainBreak/SubNode2/Atr1");
        Debug.Log("Game Atr1: " + GameAtr1);
    }    
}
