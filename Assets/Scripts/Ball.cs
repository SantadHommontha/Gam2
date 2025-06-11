using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Public")]
    [SerializeField] private float maxForce = 10;
   
    private Rigidbody2D rb;
    public Transform TARGET;
    [SerializeField] private bool isClick = false;
    [SerializeField] private bool shot = false;
    private float force = 0;
    // Mouse
    private Vector2 startMousePosition;
    private Vector2 currentMousePosition;



    // Touch
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 dragDirection;
    [SerializeField] private Vector2 oppositeDirection;

    private bool isDragging = false;
    private int touchId = -1;
    private Ball ball;

    public Vector2 DragDirection => dragDirection;
    public Vector2 OppositeDirection => oppositeDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            CalculateOpposite();
            rb.simulated = false;
        }

        if (Input.GetMouseButtonUp(0) && isClick)
        {
            shot = true;
            isClick = false;
        }

    }
    private void AddForce(Vector2 _direction, ForceMode2D _forceMode2D = ForceMode2D.Impulse)
    {
        AddForce(_direction, force, _forceMode2D);
    }
    private void AddForce(Vector2 _direction, float _force, ForceMode2D _forceMode2D = ForceMode2D.Impulse)
    {
        rb.simulated = true;
        Vector2 forceVector = _direction * _force;
        rb.AddForce(forceVector, _forceMode2D);
    }
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            // วนลูปผ่าน Touch ทั้งหมดที่กำลังใช้งานอยู่
            foreach (Touch touch in Input.touches)
            {
                // ถ้ายังไม่ได้เริ่มลาก และ Touch นี้เพิ่งเริ่มต้น
                if (!isDragging && touch.phase == TouchPhase.Began)
                {
                    // สร้าง Ray จากตำแหน่ง Touch
                    Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchWorldPos, Vector2.zero); // ยิง Ray จากจุดเดียว

                    // ตรวจสอบว่า Ray โดน Collider ของ Sprite นี้หรือไม่
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        isDragging = true;
                        touchId = touch.fingerId; // บันทึก ID ของนิ้วที่แตะ
                        startTouchPosition = touchWorldPos;
                        Debug.Log("Touch Began on Sprite at: " + startTouchPosition);
                        break; // ออกจากลูปเมื่อพบ Touch ที่เกี่ยวข้อง
                    }
                }
                // ถ้ากำลังลาก และ Touch นี้คือ Touch ที่เรากำลังติดตาม และกำลังเคลื่อนที่
                else if (isDragging && touch.fingerId == touchId && touch.phase == TouchPhase.Moved)
                {
                    currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    dragDirection = currentTouchPosition - startTouchPosition;
                    oppositeDirection = -dragDirection;

                    Debug.Log("Drag Direction: " + dragDirection);
                    Debug.Log("Opposite Direction: " + oppositeDirection);

                    // คุณสามารถนำ oppositeDirection ไปใช้กับ GameObject อื่นๆ ได้ที่นี่
                    // เช่น:
                    // transform.position += (Vector3)oppositeDirection * Time.deltaTime * speed;
                }
                // ถ้ากำลังลาก และ Touch นี้คือ Touch ที่เรากำลังติดตาม และยกนิ้วออก
                else if (isDragging && touch.fingerId == touchId && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    Debug.Log("Touch Ended. Final Opposite Direction: " + oppositeDirection);
                    isDragging = false;
                    touchId = -1; // รีเซ็ต ID
                    dragDirection = Vector2.zero;
                    oppositeDirection = Vector2.zero;
                    break; // ออกจากลูป
                }
            }
        }

    }
    void FixedUpdate()
    {
        if (shot)
        {
            shot = false;
            AddForce(oppositeDirection, force);
        }
    }
    void OnMouseDrag()
    {
        if (isClick)
        {
            CalculateOpposite();
        }
    }

    private void CalculateOpposite()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        Vector3 vectorToMouse = mouseWorldPosition - transform.position;

        Vector3 oppositeDirection = -vectorToMouse;

        if (vectorToMouse.magnitude > 0.01f)
        {
            oppositeDirection.Normalize();
        }
        else
        {
            oppositeDirection = Vector3.zero;
        }

        this.oppositeDirection = oppositeDirection;

        float distance = UnityEngine.Mathf.Clamp01(vectorToMouse.magnitude);
      
        this.force = UnityEngine.Mathf.Clamp(maxForce * distance, 0, maxForce);
       
        TARGET.position = transform.position + oppositeDirection;
    }
    void OnMouseDown()
    {
        isClick = true;
    }
}
