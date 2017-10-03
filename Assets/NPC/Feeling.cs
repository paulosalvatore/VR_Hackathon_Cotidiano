using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Feeling
{

    private float _temper;
    public float Temper
    {
        get
        {
            return _temper;
        }

        set
        {
            //-1 = aggressive, 1 = quiet
            if (value < -1)
            {
                _temper = -1;
            }
            else if (value > 1)
            {
                _temper = 1;
            }
            else
            {
                _temper = value;
            }
        }
    }

    private float _happiness;
    public float Happiness
    {
        get
        {
            return _happiness;
        }

        set
        {
            // -1 = very sad, 1 = very happy
            if (value < -1)
            {
                _happiness = -1;
            }
            else if (value > 1)
            {
                _happiness = 1;
            }
            else
            {
                _happiness = value;
            }
        }
    }

    public void IncreaseTemper()
    {
        Temper += 0.1f;
    }

    public void DecreaseTemper()
    {
        Temper -= 0.1f;
    }

    public void IncreaseHappiness()
    {
        Happiness += 0.1f;
    }

    public void DecreaseHappiness()
    {
        Happiness -= 0.1f;
    }

}