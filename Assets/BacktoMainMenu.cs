using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacktoMainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        audiomanager.instance.PlaySFX(audiomanager.instance.dinamaiMusa);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BacktoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        audiomanager.instance.PlaySFX(audiomanager.instance.buttonpress1);
    }
}
