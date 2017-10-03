using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Post : MonoBehaviour
{

    [TextArea(2, 3)]
    public string message;
    public PostType type;

    public enum PostType
    {
        Sent,
        Received
    }

}