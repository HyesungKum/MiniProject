using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public GameObject optionWindow;


    public void _onClickStart()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("SampleScene");
    }

    public void _onClickOptin()
    {
        if (!optionWindow.activeInHierarchy)
        {
            optionWindow.SetActive(true);
        }
    }

    public void _onClickQuit()
    {
        Application.Quit();
    }

    public void _onClickCloseOption()
    {
        if (optionWindow.activeInHierarchy)
        {
            optionWindow.SetActive(false);
        }
    }

}
