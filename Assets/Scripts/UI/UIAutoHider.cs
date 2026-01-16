using UnityEngine;

public class UIAutoHider : MonoBehaviour
{
    [Header("Settings")]

    public float hideDistance = 150f;
    public bool smoothFade = true;

    private CanvasGroup canvasGroup;
    private RectTransform uiTransform;
    private Camera mainCam;

    void Awake()
    {

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        uiTransform = GetComponent<RectTransform>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (BallHandle.allball.Count == 0) return;


        Vector2 uiPos = uiTransform.position;



        float minDistance = float.MaxValue;


        foreach (var obj in BallHandle.allball)
        {
            if (obj == null) continue;
            Vector2 screenPos = mainCam.WorldToScreenPoint(obj.transform.position);
            float d = Vector2.Distance(screenPos, uiTransform.position);
            if (d < minDistance) minDistance = d;
        }
        // Debug.Log($"--------MinDistance {minDistance}");

        if (smoothFade)
        {

            float alpha = Mathf.Clamp01(minDistance / hideDistance);
            if (minDistance <= hideDistance)
                alpha = 0;
            canvasGroup.alpha = alpha;
        }
        else
        {

            canvasGroup.alpha = (minDistance < hideDistance) ? 0 : 1;
        }


        canvasGroup.blocksRaycasts = canvasGroup.alpha > 0.5f;
    }
}
