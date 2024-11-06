using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneChanger : MonoBehaviour
{
    public void Change()
    {
        SceneManager.LoadScene("Play_Scene");
    }
}
