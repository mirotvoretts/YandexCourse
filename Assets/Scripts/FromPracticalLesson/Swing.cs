using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AnimationCurve _dynamic;
    [SerializeField] private float _angle;
    [SerializeField] private float _distance;
    [SerializeField] private Transform _center;

    private float _time;

    private Rigidbody2D _rb;
    private DistanceJoint2D _joint;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _joint = GetComponent<DistanceJoint2D>();
    }
    
    void Update()
    {
        float currentSpeed = _speed + _dynamic.Evaluate(_angle / -180);
        _time += Time.deltaTime * currentSpeed;
        _angle = Mathf.PingPong(_time, 180) * -1;

        Vector3 center = new Vector3(_center.position.x, _center.position.y, 0f);
        Vector3 target = center + (new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad), 0f) * _distance);

        _rb.position = target;
    }
}
