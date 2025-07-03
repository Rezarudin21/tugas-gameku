using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGames(){
        SceneManager.LoadScene("GameplayScene");
    }

    public void ExitGame(){
        // Ini akan keluar dari game ketika di build (tidak bekerja di Editor)
        Application.Quit();

        // Ini hanya untuk debug di Editor (opsional)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
