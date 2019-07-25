using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    [Header("Gameplay Variables")]
    public float moveSpeed;
    public float moveDrag;
    public float pitchFactor, yawFactor, rollFactor;
    public float rotationLerp;

    private Vector3 inputVector;  
    public Vector2 screenBounds;

    [Header("Physics")]
    public Vector3 velocity;

    [Header("References - Self")]
    public Transform modelAnchor;

    void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if ((inputVector.x < 0 && transform.position.x > -screenBounds.x) ||
           (inputVector.x > 0 && transform.position.x < screenBounds.x))
            velocity.x += inputVector.x * moveSpeed;
        if ((inputVector.y < 0 && transform.position.y > -screenBounds.y) ||
           (inputVector.y > 0 && transform.position.y < screenBounds.y))
            velocity.y += inputVector.y * moveSpeed;

        //Cut the velocity by drag
        velocity *= moveDrag;

        //Clamp the player's position
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -screenBounds.x, screenBounds.x),
                                         Mathf.Clamp(transform.position.y, -screenBounds.y, screenBounds.y),
                                         transform.position.z);

        //Move the plane after calculations are done
        transform.Translate(velocity * Time.deltaTime);

        //Rotate the plane based on its velocity
        modelAnchor.rotation = Quaternion.Lerp(modelAnchor.rotation, Quaternion.Euler(velocity.y * -pitchFactor,
                                                velocity.x * yawFactor,
                                                velocity.x * -rollFactor), rotationLerp);
    }
}