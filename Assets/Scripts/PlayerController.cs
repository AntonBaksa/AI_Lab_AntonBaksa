using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float rotateSpeed = 360f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get raw input values
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        if(inputDirection.sqrMagnitude > 0.01f)
        {
            //calculate how far to move per frame
            Vector3 displacement = inputDirection * moveSpeed * Time.deltaTime;

            //apply movement to the objects position
            transform.position += displacement;

            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);

            //make rotation smooth
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
