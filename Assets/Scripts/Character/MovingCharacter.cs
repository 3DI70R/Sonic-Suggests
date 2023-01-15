using UnityEngine;

public class MovingCharacter : MonoBehaviour
{
    public Rigidbody characterRigidbody;
    public CapsuleCollider characterCapsule;
    public float maxSpeed;
    public float accelerationSpeed;
    public float decellerationSpeed;

    private Vector3 targetVelocity;
    private bool onGround;
    private Vector3 groundNormal;
    private Rigidbody groundBody;
    private Vector3 direction;
    private float tilt;
    
    public bool OnGround { get { return onGround; } }
    
    public Vector3 GroundNormal { get { return groundNormal; } }
    
    public Rigidbody GroundBody { get { return groundBody; } }
    
    public Vector3 Direction { get { return direction; } }

    public float Tilt { get { return tilt; } }

    private void Update() {
        direction = transform.forward;
    }

    private void FixedUpdate()
    {
        FindGround();
        UpdateVelocity();
    }
    
    public void SetVelocity(Vector2 velocity)
    {
        velocity = Vector2.ClampMagnitude(velocity, 1);
        targetVelocity = Vector3.Lerp(Vector3.zero, new Vector3(velocity.x * maxSpeed, 0, velocity.y * maxSpeed),
                velocity.magnitude);
    }

    private void UpdateVelocity()
    {
        var charVelocity = characterRigidbody.velocity;
        var charHorizontalVelocity = new Vector3(charVelocity.x, 0, charVelocity.z);

        charHorizontalVelocity = ChangeGroundSpeed(charHorizontalVelocity, targetVelocity, 
            accelerationSpeed, onGround ? decellerationSpeed : accelerationSpeed);

        charVelocity.x = charHorizontalVelocity.x;
        charVelocity.z = charHorizontalVelocity.z;

        if (charHorizontalVelocity.magnitude > 0.001f)
        {
            direction = charHorizontalVelocity;
        }

        tilt = 0;
        characterRigidbody.velocity = charVelocity;
    }
    private Vector3 ChangeGroundSpeed(Vector3 actualVelocity, Vector3 targetVelocity, 
        float accelerationSpeed, float decellerationSpeed)
    {
        var change = actualVelocity - targetVelocity;
        var changeMagnitude = change.magnitude;

        if (changeMagnitude > 0.0001f)
        {
            var changeDir = change.normalized;
            var velocityDir = actualVelocity.normalized;
            var dirDot = Vector3.Dot(velocityDir, changeDir.normalized);
            var isAccel = dirDot < 0.5f;
            var changeSpeed = (isAccel ? accelerationSpeed : decellerationSpeed) * Time.fixedDeltaTime;
            var changeSize = Mathf.Min(changeMagnitude, changeSpeed) / changeMagnitude;

            return Vector3.Lerp(actualVelocity, targetVelocity, changeSize);
        }

        return targetVelocity;
    }

    private void FindGround()
    {
        var center = characterCapsule.transform.TransformPoint(characterCapsule.center);
        RaycastHit hitInfo;

        if (Physics.SphereCast(center, characterCapsule.radius, Vector3.down, out hitInfo, characterCapsule.height / 2))
        {
            Debug.DrawLine(center, hitInfo.point);

            onGround = true;
            groundNormal = hitInfo.normal;
            groundBody = hitInfo.rigidbody;
        }
        else
        {
            onGround = false;
            groundNormal = Vector3.up;
            groundBody = null;
        }
    }
}
