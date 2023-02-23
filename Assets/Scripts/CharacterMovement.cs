using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Slide))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    
    private Rigidbody2D _rb2d;
    private Slide _slide;

    private void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _slide = GetComponent<Slide>();
    }

    private void FixedUpdate()
    {
        TryToJump();
    }
    
    private void TryToJump()
    {
        if (Input.GetAxis(Constants.JumpInput) > 0 && _slide.IsGrounded)
        {
            _rb2d.position += _slide.GroundNormal  * _jumpForce;
        }
    }
}