using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToInstructions : MonoBehaviour
{
    public void GoToInstruct()
    {
        // Replace "MenuScene" with the name of your menu scene
        SceneManager.LoadScene("Instructions");
    }
}
