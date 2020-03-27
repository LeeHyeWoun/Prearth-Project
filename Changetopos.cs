using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Changetopos : MonoBehaviour {

    public RawImage RI_map;
    public Texture map1, map2, map3;

    private int mapCount = 1;

    void Start()
    {
        //진행상황에 따라 texture 변경
    }

    public void BackButton()
    {
        if (mapCount> 1) {
            mapCount--;
            setMap();
        }
    }
    public void NextButton()
    {
        if (mapCount < 3)
        {
            mapCount++;
            setMap();
        }
    }

    void setMap() {
        switch (mapCount)
        {
            case 1:
                RI_map.texture = map1;
                break;
            case 2:
                RI_map.texture = map2;
                break;
            case 3:
                RI_map.texture = map3;
                break;
        }
    }
}
