using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour
{
    public void GoToGameScene()
    {
        // Replace "MenuScene" with the name of your menu scene
        SceneManager.LoadScene("Ocean");
    }
}
