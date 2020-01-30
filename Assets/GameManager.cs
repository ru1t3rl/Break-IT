using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Object> levels;
    public Object victory;

    public static bool allDead;

    int currentLevel = 0;
    bool activated = false;

    void Start(){
        currentLevel = 0;
        LoadLevel();
    }

    void Update(){
        if(allDead){
            NextLevel();
            allDead = false;
        }
    }

    void NextLevel(){
        currentLevel++;
        LoadLevel();
    }

    void LoadLevel(){
        if(currentLevel != 0)
            SceneManager.UnloadSceneAsync(levels[currentLevel - 1].name);
        
        if(currentLevel < levels.Count){
            SceneManager.LoadSceneAsync(levels[currentLevel].name, LoadSceneMode.Additive);
        }
        else   
            SceneManager.LoadSceneAsync(victory.name, LoadSceneMode.Single);
    }
}

