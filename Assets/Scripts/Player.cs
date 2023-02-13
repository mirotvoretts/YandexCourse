using UnityEngine;

public class Player : MonoBehaviour
{
    private HingeJoint2D _joint;
    
    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnhookFromRope();
        }
    }
    
    private void UnhookFromRope()
    {
        _joint.enabled = false;
    }
}
