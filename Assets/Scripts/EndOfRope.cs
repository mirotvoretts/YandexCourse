using UnityEngine;

public class EndOfRope : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _playerJoint;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Constants.PlayerTag)) return;
        if (_playerJoint.enabled) return;

        Player.Instance.ResetRotation();
        
        ChangeCouplingPoint();
    }

    private void ChangeCouplingPoint()
    {
        _playerJoint.connectedBody = _rb;
        _playerJoint.enabled = true;
    }
}
