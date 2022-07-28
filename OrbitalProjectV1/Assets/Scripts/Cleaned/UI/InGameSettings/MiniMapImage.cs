using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;

public class MiniMapImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _hovering;
    public static MiniMapImage instance { get; private set; }
    [SerializeField]
    private Camera mainCamera;

    [SerializeField] private Camera _miniMapCamera;

    [SerializeField]
    private RawImage minimapImage;
    private RawImage playerImage;
    [SerializeField]
    private float offset;
    private Canvas[] allcanvasMats;
    private RectTransform minimapTransform;
    private Vector2 originalmapSize;
    public bool screencaptured;
    public List<Transform> minimapIcons;

    private float originalSize;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerImage = GetComponent<RawImage>();
        screencaptured = false;
        //allcanvasMats = FindObjectsOfType<Canvas>(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        _hovering = false;
        originalSize = _miniMapCamera.orthographicSize;
        mainCamera.enabled = false;
        minimapTransform = minimapImage.rectTransform;
        StartCoroutine(WaitForMapToRender());
        originalmapSize = minimapTransform.localScale;
        
        

    }

    private IEnumerator WaitForMapToRender()
    {
        while (!_GameManager.instance.playerspawned)
        {
            yield return null;
        }
        yield return SnapShotMap();
    }

    private IEnumerator SnapShotMap()
    {
        
        yield return null;
        //UITextDescription.instance.StartDescription("Island of the Wild");
        //int width = Screen.width;
        //int height = Screen.height;
        //Texture2D screenshot = new Texture2D(width, height, TextureFormat.ARGB32, false);
        //foreach(Canvas canvas in allcanvasMats)
        //{
        //    canvas.enabled = false;
        //}
        //var texture = ScreenCapture.CaptureScreenshotAsTexture();
        RenderTexture screenTexture = new RenderTexture(_miniMapCamera.scaledPixelWidth, _miniMapCamera.scaledPixelHeight, 16);
        _miniMapCamera.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        _miniMapCamera.Render();
        Texture2D renderedTexture = new Texture2D(_miniMapCamera.scaledPixelWidth, _miniMapCamera.scaledPixelHeight);
        renderedTexture.ReadPixels(new Rect(0, 0, _miniMapCamera.scaledPixelWidth, _miniMapCamera.scaledPixelHeight), 0, 0);
        renderedTexture.Apply();
        //screenshot.ReadPixels(new Rect(minimapImage.rectTransform.position, minimapImage.rectTransform.sizeDelta), 0, 0);
        //screenshot.Apply();
        RenderTexture.active = null;
        //byte[] byteArray = renderedTexture.EncodeToPNG();
        //System.IO.File.WriteAllBytes(Application.dataPath + "/cameracapture.png", byteArray);
        minimapImage.texture = renderedTexture;
        yield return null;
        ChangeMiniCamText();
        mainCamera.enabled = true;
        screencaptured = true;
        //foreach (Canvas canvas in allcanvasMats)
        //{
        //    canvas.enabled = true;
        //}


    }

    private void ChangeMiniCamText()
    {
        _miniMapCamera.cullingMask = LayerMask.GetMask("Post Processing");
        _miniMapCamera.targetTexture = (RenderTexture) playerImage.mainTexture;
    }


    // Update is called once per frame
    void Update()
    {
        if(_hovering && IsMouseWheelRolling())
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        offset = 4 * Input.GetAxis("Mouse ScrollWheel");
        //_miniMapCamera.orthographicSize -= offset;
        //var mapscale = minimapTransform.localScale;
        //mapscale.x /= Mathf.Abs(((originalSize + 2 * 100 * offset) / originalSize));
        foreach (Transform minimapIcon in minimapIcons)
        {
            var scale = minimapIcon.localScale;
            
            scale /= Mathf.Abs(((originalSize + 2 * offset) / originalSize));
            
            if (scale.x < 5)
            {
                minimapIcon.localScale = new Vector2(Mathf.Max(scale.x, 5), Mathf.Max(scale.y, 5));
            }
            else if (scale.x > 20)
            {
                minimapIcon.localScale = new Vector2(Mathf.Min(scale.x, 20), Mathf.Min(scale.y, 20));
            }
            else
            {
                minimapIcon.localScale = scale;
            }
        }     
        
    }

    private bool IsMouseWheelRolling()
    {
        return Input.GetAxis("Mouse ScrollWheel") != 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
    }
}
