using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Slide : MonoBehaviour
{
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speed;

    private Rigidbody2D _rb2d;
    
    private Vector2 _targetVelocity;
    
    private ContactFilter2D _contactFilter;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private readonly List<RaycastHit2D> _hitBufferList = new(16);

    public Vector2 GroundNormal { get; private set; }
    public bool IsGrounded { get; private set; }

    private void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_layerMask);
        _contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        var alongSurface = Vector2.Perpendicular(GroundNormal);

        _targetVelocity = alongSurface * (Vector2.Dot(alongSurface, _velocity.normalized) * _speed);
    }

    private void FixedUpdate()
    {
        _velocity += Physics2D.gravity * (_gravityModifier * Time.deltaTime);
        _velocity.x = _targetVelocity.x;

        IsGrounded = false;

        var deltaPosition = _velocity * Time.deltaTime;
        var moveAlongGround = new Vector2(GroundNormal.y, -GroundNormal.x);
        var direction = moveAlongGround * deltaPosition.x;

        MovementLogic(direction, false);

        direction = Vector2.up * deltaPosition.y;

        MovementLogic(direction, true);
    }

    private void MovementLogic(Vector2 direction, bool yMovement)
    {
        var distance = direction.magnitude;

        if (distance > Constants.MinMoveDistance)
        {
            var count = _rb2d.Cast(direction, _contactFilter, _hitBuffer, distance + Constants.ShellRadius);

            _hitBufferList.Clear();

            for (var i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            foreach (var hit in _hitBufferList)
            {
                var currentNormal = hit.normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    IsGrounded = true;
                    if (yMovement)
                    {
                        GroundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                var projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity -= projection * currentNormal;
                }

                var modifiedDistance = hit.distance - Constants.ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rb2d.position += direction.normalized * distance;
    }
}