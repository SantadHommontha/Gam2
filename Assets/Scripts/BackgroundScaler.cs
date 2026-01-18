using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public enum ScaleMode { FitWidth, FitHeight, FillScreen }

    [Header("Reference Settings")]
    public Vector2 referenceResolution = new Vector2(1080, 1920); // ขนาดที่คุณออกแบบไว้
    public ScaleMode scaleMode = ScaleMode.FitHeight; // เลือกโหมดการปรับ

    void Start()
    {
        ScaleBackground();
    }

    // เรียกใน Update หากต้องการให้รองรับการ Resize หน้าจอบราวเซอร์แบบ Real-time
    void Update()
    {
        ScaleBackground();
    }

    public void ScaleBackground()
    {
        // 1. หาอัตราส่วนระหว่างหน้าจอจริง กับ หน้าจอที่ออกแบบไว้
        float widthRatio = Screen.width / referenceResolution.x;
        float heightRatio = Screen.height / referenceResolution.y;

        float finalScale = 1f;

        switch (scaleMode)
        {
            case ScaleMode.FitWidth:
                // ปรับตามความกว้าง (อาจเห็นขอบดำบน-ล่าง ถ้าจอสูงเกินไป)
                finalScale = widthRatio;
                break;

            case ScaleMode.FitHeight:
                // ปรับตามความสูง (อาจเห็นขอบดำซ้าย-ขวา ถ้าจอกว้างเกินไป)
                finalScale = heightRatio;
                break;

            case ScaleMode.FillScreen:
                // ปรับให้คลุมทั้งจอ (เอาค่าที่มากที่สุดมาใช้ เพื่อไม่ให้เห็นขอบดำ แต่ภาพบางส่วนอาจล้นจอ)
                finalScale = Mathf.Max(widthRatio, heightRatio);
                break;
        }

        // 2. ปรับ Scale ของ Group (LocalScale)
        transform.localScale = new Vector3(finalScale, finalScale, 1f);
    }
}
