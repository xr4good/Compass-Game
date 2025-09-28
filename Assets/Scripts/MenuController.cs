using GLTFast.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class MenuController : MonoBehaviour
{
    public Slider selectTime;
    //txt mesh pro
    public TMPro.TextMeshProUGUI secondsText;

    public Toggle allowTP;
    void Start()
    {
        selectTime.value = 180;


        if (XRSettings.isDeviceActive)
        {
            Debug.Log("✅ Headset detectado: " + XRSettings.loadedDeviceName);
        }
        else
        {
            Debug.LogWarning("⚠️ Nenhum headset VR ativo.");
        }
    }

    public void UpdateText()
    {
        secondsText.text = selectTime.value.ToString("0") + " Segundos";
        PlayerPrefs.SetFloat("selectTime", selectTime.value);
        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        PlayerPrefs.SetFloat("selectTime", selectTime.value);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void ToogleTp()
    {   
        PlayerPrefs.SetInt("tp", allowTP.isOn ? 1 : 0);
        Debug.Log("TP: " + (allowTP.isOn ? 1 : 0));
    }
}
