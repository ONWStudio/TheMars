using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Onw.Test
{
    public class TestComponent : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                SceneManager.LoadScene("MainGameScene");
            }
        }
    }
}
