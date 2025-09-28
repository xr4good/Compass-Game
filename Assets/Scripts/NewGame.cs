using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class NewGame : MonoBehaviour
{
    public AudioClip winSfx;
    public AudioClip loseSfx;
    public AudioSource audioSource;
    public GameObject Locomotion;

    public Slider timeSlider;
    public Camera mainCamera;
    private bool loseSfxPlaying = false;

    public Canvas looseMSG;
    public Canvas winMSG;
    public GameObject tpobj;
    void Start()
    {
        int tp = PlayerPrefs.GetInt("tp");
        if (tp == 0)
        {
            tpobj.SetActive(false);
            Debug.Log("Tp desativado");
        }
        
        if (looseMSG) looseMSG.enabled = false;
        if (winMSG) winMSG.enabled = false;
    }
    void Update()
    {
        
        if (timeSlider.value == 0 && !loseSfxPlaying)
        {
            loseSfxPlaying = true;
            loseGame();
        }
    }

    public void winGame()
    {
        Debug.Log("You won the game!");
        audioSource.PlayOneShot(winSfx);
        StartCoroutine(WaitAndChangeScene(true));
    }

    void loseGame()
    {
        Debug.Log("You lost the game!");
        audioSource.PlayOneShot(loseSfx);
        StartCoroutine(WaitAndChangeScene(false)); 
    }

    IEnumerator WaitAndChangeScene(bool win)
    {
        Locomotion.SetActive(false);

        if (win) winMSG.enabled = true;
        else looseMSG.enabled = true;

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }
}
