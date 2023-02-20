using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Slide : MonoBehaviour
{
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speed;

    private Rigidbody2D _rb2d;

    private Vector2 _groundNormal;
    private Vector2 _targetVelocity;
    private bool _grounded;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);

    private const float MinMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;

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
        var alongSurface = Vector2.Perpendicular(_groundNormal);

        _targetVelocity = alongSurface * _speed;
    }

    private void FixedUpdate()
    {
        _velocity += Physics2D.gravity * (_gravityModifier * Time.deltaTime);
        _velocity.x = _targetVelocity.x;

        _grounded = false;

        var deltaPosition = _velocity * Time.deltaTime;
        var moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        var move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    private void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinMoveDistance)
        {
            var count = _rb2d.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);

            _hitBufferList.Clear();

            for (var i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            foreach (var t in _hitBufferList)
            {
                var currentNormal = t.normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    _grounded = true;
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                var projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                var modifiedDistance = t.distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rb2d.position = _rb2d.position + move.normalized * distance;
    }
}