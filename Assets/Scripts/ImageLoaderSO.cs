using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "New Image Tools", menuName = "Image Tool")]
public class ImageLoaderSO : ScriptableObject
{
    public Action OnFetchDataStart;
    public Action OnFetchDataEnd;
    
    [HideInInspector] public Coroutine fetchingDataCoroutine;
    [HideInInspector] public List<ImagesData> imagesData = new List<ImagesData>();
    [HideInInspector] public bool fetching = false;

    [SerializeField] private string imagesDirectory;
    [Header("--- PAGING ---")]
    public bool paging;
    [SerializeField] private int numberOfElementsOnPage;
    
    private string[] imagesPath;
    private const string PNG_EXTENSION = "png";


    public int NumberOfElementsOnPage {
        get 
        {
            if (paging && numberOfElementsOnPage <= 0)
            {
                Debug.LogError("NumberOfElementsOnPage has an invalid value. Set default: 1");
                numberOfElementsOnPage = 1;
            }

            return paging ? numberOfElementsOnPage : imagesPath.Length; 
        } 
    }
    public int PagesNumber
    {
        get
        {
            if (paging)
                return imagesPath.Length % NumberOfElementsOnPage > 0 ? imagesPath.Length / NumberOfElementsOnPage + 1 : imagesPath.Length / NumberOfElementsOnPage;
            else
                return 1;
        }
    }


    private void OnEnable()
    {
        CleanUp();
    }
    
    private void OnDisable()
    {
        CleanUp();
    }

    public IEnumerator GetImagesData(int page = 0)
    {
        CleanUp();
        OnFetchDataStart?.Invoke();
        fetching = true;

        var filters = new string[] { PNG_EXTENSION };
        imagesPath = FetchFilesFrom(imagesDirectory, filters, false);

        for (int i = page * NumberOfElementsOnPage; i < (paging ? page * NumberOfElementsOnPage + NumberOfElementsOnPage : imagesPath.Length); i++)
        {
            if (i >= imagesPath.Length) continue;

            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + imagesPath[i]))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(imagesPath[i]);
                    Texture2D spriteTexture = DownloadHandlerTexture.GetContent(uwr);

                    imagesData.Add(new ImagesData()
                    {
                        image = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f)),
                        name = fileInfo.Name,
                        creationDate = TimeSpan.FromTicks(fileInfo.CreationTime.Ticks)
                    });

                    uwr.Dispose();
                }
            }
        }

        fetching = false;
        OnFetchDataEnd?.Invoke();
    }

    private void CleanUp()
    {
        if(imagesData.Count > 0)
        {
            foreach (var imageData in imagesData)
            {
                if(imageData.image != null) 
                    Destroy(imageData.image.texture);
            }
        }
        imagesData.Clear();
    }

    public string[] FetchFilesFrom(string dir, string[] filters, bool isRecursive)
    {
        List<string> filesFound = new List<string>();
        try
        {
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(dir, string.Format("*.{0}", filter), searchOption));
            }
        }
        catch (Exception)
        {
            if (dir.Equals(string.Empty))
                Debug.LogError("You have to provide a directory with png files! You should fix it in ImageLoader.");
            else
                Debug.LogError($"Invalid directory: {dir}! You should fix it in ImageLoader.");
        }

        return filesFound.ToArray();
    }
}

[Serializable]
public class ImagesData
{
    public Sprite image;
    public string name;
    public TimeSpan creationDate;

    public ImagesData() {}
}