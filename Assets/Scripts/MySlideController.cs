using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySlideController : MonoBehaviour
{
    private MyLMS myLMS;

    public string presentationName = "CanYouFindMe";

    private List<Texture2D> slides = null;
    private bool applingSlide = false;
    private int currentSlideIndex = 0;

    public RawImage slideDisplay;

    public Slider loadingBar;

    void Awake()
    {
        myLMS = FindFirstObjectByType<MyLMS>();
        slides = new List<Texture2D>();
        loadingBar.enabled = true;
        loadingBar.value = 0;

        if (slideDisplay == null)
        {
            Debug.LogError("RawImage (slideDisplay) não está atribuído!");
        }

        if (myLMS == null)
        {
            Debug.LogWarning("MyLMS não encontrado na cena!");
        }
    }

    void Start()
    {
        if (myLMS != null)
        {
            Debug.Log(myLMS.folderName);
        }
    }

    void Update()
    {
        if (myLMS != null && myLMS.isLoadingSlides)
        {   
            // Debug.Log("DESGRALADLASLDSALDLSADLSALDL");
            loadingBar.maxValue = myLMS.slideCount;
            loadingBar.value = myLMS.downloadedCount;

            if (myLMS.slideCount - 1 == myLMS.downloadedCount)
            {
                loadingBar.value = myLMS.slideCount;
            }
            
        }

        if (myLMS != null && myLMS.slidesFetched && slides.Count == 0)
        {   
            if (myLMS.slides.ContainsKey(presentationName))
            {
                TouchScreenKeyboard.Open("Ola");
                loadingBar.gameObject.SetActive(false);
                slides = myLMS.slides[presentationName];
                Debug.Log($"Slides carregados: {slides.Count}");
            }
        }

        if (slides.Count > 0 && applingSlide == false)
        {
            StartCoroutine(SliderTester());
        }
    }

    private void ApplySlide(Texture2D texture)
    {
        slideDisplay.enabled = true;
        if (slideDisplay != null)
        {   
            // Debug.Log("Temm display");

            slideDisplay.texture = texture;
        }
    }

    private IEnumerator SliderTester()
    {
        applingSlide = true;

        // Debug.Log("Tentando aplicar slide " + currentSlideIndex);
        ApplySlide(slides[currentSlideIndex]);
        yield return new WaitForSeconds(2);

        currentSlideIndex++;

        if (currentSlideIndex > slides.Count - 1)
            currentSlideIndex = 0;

        applingSlide = false;
    }
}
