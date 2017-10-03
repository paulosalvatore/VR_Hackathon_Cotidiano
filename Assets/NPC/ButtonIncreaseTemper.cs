using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ButtonIncreaseTemper : MonoBehaviour
{

    public NPC npc;

    public void Onclick_IncreaseTemper()
    {
        npc.feelings.IncreaseTemper();
    }
	
    public void Onclick_IncreaseHappiness()
    {
        npc.feelings.IncreaseHappiness();
    }
	
}