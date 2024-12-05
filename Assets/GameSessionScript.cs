using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void ReloadGame(){
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    public void Quit(){
        Application.Quit();
    }
}
