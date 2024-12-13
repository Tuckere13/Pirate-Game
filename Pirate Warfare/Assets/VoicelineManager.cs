using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicelineManager : MonoBehaviour
{

    public List<AudioSource> fireCommands;

    private bool canSayFire = true;
    int lastSaidFire = 0; // used so same fire command isnt used back to back


    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(Environment.TickCount);
    }

    public void playFireCommand()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 3f);
        
        if (randomNumber >= 3){
            if (lastSaidFire != 3){
                if (fireCommands[2] != null && fireCommands[2].clip != null){
                    fireCommands[2].Play();

                    canSayFire = false;
                    lastSaidFire = 3;
                }
                else{
                    Debug.LogWarning("AudioSource or AudioClip is missing (Fire 3).");
                }
            }
            else{
                playFireCommand();  // if audio is to be repeated, call function again until another gets picked
            }
            
        }
        else if (randomNumber >= 2){
            if (lastSaidFire != 1){
                if (fireCommands[1] != null && fireCommands[1].clip != null){
                    fireCommands[1].Play();

                    canSayFire = false;
                    lastSaidFire = 2;
                }
                else{
                    Debug.LogWarning("AudioSource or AudioClip is missing (Fire 2).");
                }
            }
            else{
                playFireCommand();  // if audio is to be repeated, call function again until another gets picked
            }
        }
        else if (randomNumber >= 1){
            if (lastSaidFire != 1){
                if (fireCommands[0] != null && fireCommands[0].clip != null) { 
                    fireCommands[0].Play();

                    canSayFire = false;
                    lastSaidFire = 3;
                }
                else{
                    Debug.LogWarning("AudioSource or AudioClip is missing (Fire 1).");
                }
            }
            else{
                playFireCommand();  // if audio is to be repeated, call function again until another gets picked
            }
        }


    }
}
