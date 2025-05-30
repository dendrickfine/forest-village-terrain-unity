using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gui : MonoBehaviour
{
    public void loadscene()
    {
        SceneManager.LoadScene("terrain");
    }

    public void KeluarGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
