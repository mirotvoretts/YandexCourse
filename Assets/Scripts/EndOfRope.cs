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
        if (other.TryGetComponent<Player>(out var player) && !_playerJoint.enabled)
        {
            player.ResetRotation();

            ChangeCouplingPoint();
        }
    }

    private void ChangeCouplingPoint()
    {
        _playerJoint.connectedBody = _rb;
        _playerJoint.enabled = true;
    }
}