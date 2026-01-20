using UnityEngine;

public class UIGroupScaler : MonoBehaviour
{
   public enum MatchMode { Width, Height, Fill, Fit }

    [Header("Settings")]
    public Vector2 refRes = new Vector2(1080, 1920);
    public MatchMode mode = MatchMode.Height;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        ScaleUIGroup();
    }

    void ScaleUIGroup()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float refRatio = refRes.x / refRes.y;

        float scaleFactor = 1f;

       
        float widthScale = (float)Screen.width / refRes.x;
        float heightScale = (float)Screen.height / refRes.y;

        switch (mode)
        {
            case MatchMode.Width:
                scaleFactor = widthScale;
                break;
            case MatchMode.Height:
                scaleFactor = heightScale;
                break;
            case MatchMode.Fill: 
                scaleFactor = Mathf.Max(widthScale, heightScale);
                break;
            case MatchMode.Fit:
                scaleFactor = Mathf.Min(widthScale, heightScale);
                break;
        }

      
        rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }
}
