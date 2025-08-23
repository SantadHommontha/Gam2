
using System.Collections;
using UnityEngine;

public class PlayAnimationSprite : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField][TextArea] private string description;
#endif
    [Header("Animation Settings")]
    public Sprite[] animationFrames; // Array เก็บ Sprites สำหรับ Animation

    public float frameRate = 0.1f; // ความเร็วในการเปลี่ยนเฟรม (วินาทีต่อเฟรม)

    [SerializeField] private SpriteRenderer spriteRenderer; // Component ที่ใช้แสดงผล Sprite
    private int currentFrameIndex = 0; // Index ของ Sprite ที่กำลังแสดงผล
    private Coroutine animationCoroutine; // ตัวแปรสำหรับเก็บ Coroutine ของ Animation
    private bool isPlayingForward = true; // บอกว่ากำลังเล่นไปข้างหน้าหรือย้อนกลับ

    // สถานะปัจจุบันของ Animation
    public enum AnimationState
    {
        Stopped,
        Playing,
        PlayingReverse,
        FinishPlay
    }
    public AnimationState currentState = AnimationState.Stopped;

    void Awake()
    {
      //  spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject. Please add one.");
            enabled = false; // ปิด Script ถ้าไม่มี SpriteRenderer
        }
        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning("No animation frames assigned. Please assign sprites to the 'Animation Frames' array.");
            enabled = false; // ปิด Script ถ้าไม่มี Sprites
        }
        else
        {
            // ตั้งค่า Sprite เริ่มต้น
          //  spriteRenderer.sprite = animationFrames[currentFrameIndex];
        }
    }

    /// <summary>
    /// ฟังก์ชันสำหรับสั่งเล่น Animation ไปข้างหน้า
    /// </summary>
    public void Play()
    {
        if (animationFrames == null || animationFrames.Length == 0) return;

        StopAnimation(); // หยุด Animation เดิมก่อน ถ้ามี
        isPlayingForward = true;
        currentState = AnimationState.Playing;
        currentFrameIndex = 0;
        animationCoroutine = StartCoroutine(AnimateForward());
        //Debug.Log("Play");
    }

    /// <summary>
    /// ฟังก์ชันสำหรับสั่งเล่น Animation ย้อนกลับ
    /// </summary>
    public void PlayReverse()
    {
        if (animationFrames == null || animationFrames.Length == 0) return;

        StopAnimation(); // หยุด Animation เดิมก่อน ถ้ามี
        isPlayingForward = false;
        currentState = AnimationState.PlayingReverse;
        animationCoroutine = StartCoroutine(AnimateReverse());
    }

    /// <summary>
    /// ฟังก์ชันสำหรับสั่งหยุด Animation
    /// </summary>
    public void Stop()
    {
        StopAnimation();
        currentState = AnimationState.Stopped;
    }

    /// <summary>
    /// ฟังก์ชันภายในสำหรับหยุด Coroutine ของ Animation
    /// </summary>
    private void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    /// <summary>
    /// Coroutine สำหรับเล่น Animation ไปข้างหน้า
    /// </summary>
    private IEnumerator AnimateForward()
    {
        while (currentFrameIndex < animationFrames.Length)
        {
            spriteRenderer.sprite = animationFrames[currentFrameIndex];
            currentFrameIndex++;
         
            // if (currentFrameIndex >= animationFrames.Length)
            // {
            //     currentFrameIndex = 0; // วนกลับไปเริ่มใหม่เมื่อจบ Animation
            // }
            yield return new WaitForSeconds(frameRate);
        }
        currentState = AnimationState.FinishPlay;
    }

    /// <summary>
    /// Coroutine สำหรับเล่น Animation ย้อนกลับ
    /// </summary>
    private IEnumerator AnimateReverse()
    {
        while (currentFrameIndex < 0)
        {
            spriteRenderer.sprite = animationFrames[currentFrameIndex];
            currentFrameIndex--;

            // if (currentFrameIndex < 0)
            // {
            //     currentFrameIndex = animationFrames.Length - 1; // วนกลับไปเริ่มจากท้ายสุดเมื่อถึงเฟรมแรก
            // }
            yield return new WaitForSeconds(frameRate);
        }
        currentState = AnimationState.FinishPlay;
    }

    /// <summary>
    /// ฟังก์ชันสำหรับดึง Index ปัจจุบันของ Sprite ที่กำลังแสดง
    /// </summary>
    /// <returns>Index ของ Sprite ปัจจุบัน</returns>
    public int GetCurrentFrameIndex()
    {
        return currentFrameIndex;
    }

    /// <summary>
    /// ฟังก์ชันสำหรับตั้งค่า Index ของ Sprite ที่จะแสดง
    /// </summary>
    /// <param name="index">Index ของ Sprite ที่ต้องการแสดง</param>
    public void SetFrameIndex(int index)
    {
        if (animationFrames == null || animationFrames.Length == 0) return;
        if (index >= 0 && index < animationFrames.Length)
        {
            currentFrameIndex = index;
            spriteRenderer.sprite = animationFrames[currentFrameIndex];
        }
        else
        {
            Debug.LogWarning("Invalid frame index: " + index);
        }
    }

    // // ตัวอย่างการใช้งานใน Update สำหรับทดสอบ
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Play(); // กด Spacebar เพื่อเล่นไปข้างหน้า
    //     }
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         PlayReverse(); // กด R เพื่อเล่นย้อนกลับ
    //     }
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         Stop(); // กด S เพื่อหยุด
    //     }
    //     if (Input.GetKeyDown(KeyCode.LeftArrow))
    //     {
    //         // เลื่อนไปเฟรมก่อนหน้า
    //         int newIndex = currentFrameIndex - 1;
    //         if (newIndex < 0) newIndex = animationFrames.Length - 1;
    //         SetFrameIndex(newIndex);
    //         Stop(); // หยุด Animation เมื่อเปลี่ยนเฟรมด้วยมือ
    //     }
    //     if (Input.GetKeyDown(KeyCode.RightArrow))
    //     {
    //         // เลื่อนไปเฟรมถัดไป
    //         int newIndex = currentFrameIndex + 1;
    //         if (newIndex >= animationFrames.Length) newIndex = 0;
    //         SetFrameIndex(newIndex);
    //         Stop(); // หยุด Animation เมื่อเปลี่ยนเฟรมด้วยมือ
    //     }
    // }
}
