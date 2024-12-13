using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{
    public void GoToMenuScene()
    {
        // Replace "MenuScene" with the name of your menu scene
        SceneManager.LoadScene("Title Screen");
    }
}
