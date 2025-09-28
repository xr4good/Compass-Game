using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Newtonsoft.Json.Linq;
using SeriousGameComponents.LmsComponent;
using UnityEngine;
using UnityEngine.Networking;

public class MyLMS : LmsLoader 
{
    public string BASE_URL = "http://18.116.24.34";
    public string username = "mgon";
    public string password = "Mat_1234";
    public int courseId = 3;
    public string topicName = "CanYouFindMe";
    public string folderName = "Slides";

    [HideInInspector]
    public Dictionary<string, List<Texture2D>> slides;
    private string token = null;

    
    [HideInInspector]
    public bool isLoadingCourses = false;
    [HideInInspector]
    public bool coursesFeteched = false;
    [HideInInspector]
    public bool isLoadingSlides = false;
    [HideInInspector]
    public bool slidesFetched = false;
    [HideInInspector]
    public int slideCount = 0;
    [HideInInspector]
    public int downloadedCount = 0;

    public static MyLMS Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(CheckAlive());
        StartCoroutine(DoLogin());

        slides = new Dictionary<string, List<Texture2D>>();
    
    }
    // Update is called once per frame
    void Update()
    {
        // if (token != null && !isLoadingCourses && !coursesFeteched)
        // {
        //     StartCoroutine(getCursos());
        // }

        if (token != null && !isLoadingSlides && !slidesFetched)
        {
            StartCoroutine(getSlides());
        }
    }

    IEnumerator CheckAlive()
    {
        string uri = $"{BASE_URL}:30000/api/alive";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Resposta: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator DoLogin()
    {
        string uri = $"{BASE_URL}:30000/api/login?username={username}&password={password}";
        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(uri, ""))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string json = webRequest.downloadHandler.text;
                JObject obj = JObject.Parse(json);
                token = obj["token"].ToString();
                Debug.Log("Token: " + token);
            }
            else
            {
                Debug.LogError("Erro login: " + webRequest.error);
            }
        }
    }

    IEnumerator getCursos()
    {
        isLoadingCourses = true;
        string uri = $"{BASE_URL}:30000/api/cursos?username={username}&token={token}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string json = webRequest.downloadHandler.text;

                JArray array = JArray.Parse(json);
                string displayName = array[0]["displayname"].ToString();
                Debug.Log("Displayname: " + displayName);
                coursesFeteched = true;
            }
            else
            {
                Debug.LogError("Erro login: " + webRequest.error);
            }
        }
        isLoadingCourses = false;
    }

    IEnumerator getSlides()
    {
        isLoadingSlides = true;

        slides.Clear();

        string uri = $"{BASE_URL}:30000/api/conteudos?course_id={courseId}&token={token}";

        // Debug.Log("URI: " + uri);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string json = webRequest.downloadHandler.text;

                JArray topics = JArray.Parse(json);

                foreach (JObject topic in topics)
                {
                    if (topic["name"].ToString() == topicName)
                    {   
                        // Debug.Log(topic["name"].ToString());
                        JArray modules = (JArray)topic["modules"];

                        foreach (JObject module in modules)
                        {
                            if (module["name"].ToString() == folderName)
                            {
                                // Debug.Log(module["name"].ToString());
                                // Debug.Log(module["modname"].ToString());
                                JArray contents = (JArray)module["contents"];

                                slideCount = contents.Count;
                                Debug.Log("Slides na pasta: " + slideCount);
                                
                                List<Texture2D> textures = new List<Texture2D>();

                                foreach (JObject file in contents)
                                {
                                    string fileUrl = file["fileurl"].ToString().Replace("http://moodle:8080", BASE_URL);

                                    fileUrl += $"&token={token}";
                                    Debug.Log("File Url Download: " + fileUrl);

                                    using (UnityWebRequest fileRequest = UnityWebRequestTexture.GetTexture(fileUrl))
                                    {
                                        yield return fileRequest.SendWebRequest();

                                        if (fileRequest.result == UnityWebRequest.Result.Success)
                                        {
                                            Texture2D texture = DownloadHandlerTexture.GetContent(fileRequest);
                                            textures.Add(texture);
                                        }
                                        else
                                        {
                                            Debug.LogWarning($"Erro ao baixar imagem: {fileUrl}");
                                        }
                                    }
                                    downloadedCount++;
                                    // Debug.Log("Downloaded count: " + downloadedCount);

                                }

                                if (!slides.ContainsKey(topicName))
                                    slides[topicName] = new List<Texture2D>();

                                slides[topicName].AddRange(textures);

                            }
                        }
                    }
                }

                slidesFetched = true;
            }
            else
            {
                Debug.LogError($"Erro ao buscar conteúdo: {webRequest.error}");
            }
        }
        isLoadingSlides = false;
    }
    
}
