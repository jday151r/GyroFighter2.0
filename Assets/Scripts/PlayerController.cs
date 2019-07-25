using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    public float moveSpeed;
    public float movementDrag;
    public float defaultForwardSpeed;
    public float boostedForwardSpeed;
    [HideInInspector] public float forwardSpeed;

    public Vector3 velocity;
    public Vector2 screenBoundaries;

    [Header("Input")]
    public char lastKeyPressed;
    public Vector2 inputVector;

    [Header("Barrel Rolling")]
    public float barrelSpeed; //Velocity added upon rolling
    public float barrelTimer; //Maximum time between presses when double-pressing
    public float barrelLerpFactor; //Lerp amount over time for barrel rotation
    public float barrelLimit; //Value the barrelLerp must be smaller than to re-enable rolling
    public float barrelLerp; //Decay for player rotation (1 upon initial roll, lerps to 0 by barrelLerpFactor)

    private float barrelTimeStamp; //Timestamp used for double-press checking
    private float barrelRotation; //Desired rotation upon rolling (360 or -360 based on direction)

    [Header("Visuals")]
    public float rollAngle; //Rotate the plane left/right
    public float yawAngle; //Point the plane left/right
    public float pitchAngle; //Point the plane up/down

    [Header("References")]
    public Rigidbody rBody;
    public GameObject normalEffects;
    public GameObject boostEffects;
    public GameObject leftBooster;
    public GameObject rightBooster;
    public GameObject model;

    [Header("Other")]
    public float deltaTime;

    void Start()
    {
        if (!rBody) rBody = GetComponent<Rigidbody>();
        forwardSpeed = defaultForwardSpeed;
    }
    private void Update()
    {
        deltaTime = Time.deltaTime * 60;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            normalEffects.SetActive(false);
            boostEffects.SetActive(true);
            forwardSpeed = boostedForwardSpeed;
        }
        else
        {
            normalEffects.SetActive(true);
            boostEffects.SetActive(false);
            forwardSpeed = defaultForwardSpeed;
        }
    }
    void LateUpdate()
    {
        #region Input/Movement

        //Get input from input axes
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Only add force if we are within the screen bounds
        if ((inputVector.x < 0 && transform.position.x > -screenBoundaries.x) ||
           (inputVector.x > 0 && transform.position.x < screenBoundaries.x))
            velocity.x += (inputVector.x * moveSpeed);
        if ((inputVector.y < 0 && transform.position.y > -screenBoundaries.y) ||
           (inputVector.y > 0 && transform.position.y < screenBoundaries.y))
            velocity.y += (inputVector.y * moveSpeed);
        //Set the forward speed
        velocity.z = (forwardSpeed + velocity.z) / 2;
        //Cut the velocity by drag
        velocity *= movementDrag;
        //Translate by the velocity after it is calculated
        transform.Translate(velocity * deltaTime);
        //Clamp MUST come after input, so the player isn't clamped before their inputs are suppressed
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -screenBoundaries.x, screenBoundaries.x),
                                         Mathf.Clamp(transform.position.y, -screenBoundaries.y, screenBoundaries.y),
                                         transform.position.z);

        //Barrel roll(double-press left or right)
        barrelLerp = Mathf.Lerp(barrelLerp, 0, barrelLerpFactor * deltaTime);
        if (Input.GetKeyDown(KeyCode.A) && barrelLerp < barrelLimit) BarrelRoll('A', false);
        if (Input.GetKeyDown(KeyCode.D) && barrelLerp < barrelLimit) BarrelRoll('D', true);

        #endregion
    }
    void FixedUpdate()
    {
        //Rotate the plane based on its velocity, by the given maximum P/Y/R angles
        model.transform.rotation = Quaternion.Euler(-velocity.y * pitchAngle * deltaTime,
                                                  velocity.x * yawAngle * deltaTime,
                                                  (-velocity.x * deltaTime * rollAngle));// + (barrelRotation * barrelLerp));
    }

    void BarrelRoll(char lastKey, bool direction)
    {
        //direction: left = false, true = right

        if (lastKeyPressed == lastKey && Time.time - barrelTimeStamp < barrelTimer)
        {
            if (direction)
            {
                leftBooster.SetActive(true);
                rightBooster.SetActive(true);
                velocity.x = barrelSpeed;
                barrelRotation = 360;
            }
            else
            {
                leftBooster.SetActive(true);
                rightBooster.SetActive(true);
                velocity.x = -barrelSpeed;
                barrelRotation = -360;
            }
            lastKeyPressed = ' '; //Prevent the player from rolling without double-pressing again
            barrelLerp = 1;

        }
        else
        {
            lastKeyPressed = lastKey;
            barrelTimeStamp = Time.time;
        }
    }
}