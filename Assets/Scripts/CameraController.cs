using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 50f; 
    public float zoomSpeed = 10f; 
    public float minSize = 5f;   
    public float maxSize = 100f;  
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");   

        Vector3 moveDirection = new Vector3(horizontal, vertical, 0f);
        
     
        transform.position += moveDirection * moveSpeed * Time.deltaTime;


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            cam.orthographicSize -= scroll * zoomSpeed;

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
        }
    }
}