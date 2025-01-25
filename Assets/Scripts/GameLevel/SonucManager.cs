using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SonucManager : MonoBehaviour
{


    public void oyunaYenidenBasla()
    {
        SceneManager.LoadScene("gameLevel");
    }
    public void anaMenuyeDon()
    {
        SceneManager.LoadScene("menuLevel");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
