using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    private HingeJoint2D _joint;
    private Rigidbody2D _rb;

    [SerializeField] private float _swingForce = 1f;

    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();
        _rb = GetComponent<Rigidbody2D>();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        UnhookFromRope();
        SwingOnRope();
    }
    
    private void UnhookFromRope()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _joint.enabled = false;
        }
    }

    private void SwingOnRope()
    {
        var moveHorizontal = Input.GetAxis(Constants.HorizontalInput);
        
        _rb.AddForce(transform.right * (moveHorizontal * _swingForce));
    }

    public void ResetRotation()
    {
        transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
    }
}
