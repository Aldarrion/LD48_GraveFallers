using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent
{
    private Transform _transform;
    private Collider2D _collider2D;
    private Func<float> _calculateGroundHeight;
    private float _speed;

    private float _stepTime = 1f / 120;

    private float _accumulatedTime;
    private HashSet<KeyCode> _keysPressed = new HashSet<KeyCode>();

    private int _isOnGround = 0;

    private float _gravityForce = 0;

    private float _jumpForce = 3;
    private float _gravityPull = 1f / 18;
    private float _maxGravity = -10;

    private float _lineThickness = 0.00001f;

    //private ColliderManager _colliderManager;



    private InputReader _inputReader;


    public MovementComponent(Transform transform, Collider2D collider2D, Func<float> calculateGroundHeight, float speed/*, ColliderManager colliderManager*/)
    {
        _transform = transform;
        _collider2D = collider2D;
        _calculateGroundHeight = calculateGroundHeight;
        _speed = speed;
        //_colliderManager = colliderManager;

        _inputReader = new InputReader("");
    }

    public void Update()
    {
        _inputReader.Update();

        _accumulatedTime += Time.deltaTime;

        if (_accumulatedTime >= _stepTime)
        {
            ProcessSteps();

            AfterStepClear();
        }
    }

    private void ProcessSteps()
    {
        Vector3 velocity = new Vector3();


        velocity += new Vector3(3 * _inputReader.GetHorizontal(), 0);

        if (_inputReader.IsAction(InputAction.Jump) && _isOnGround > 0)
        {
            _gravityForce = _jumpForce;
        }

        velocity.y += _gravityForce;
        velocity *= _speed * _stepTime;

        Vector3 totalMovement = new Vector3();

        //while (_accumulatedTime >= _stepTime)
        //{
        //    _accumulatedTime -= _stepTime;


        //    totalMovement += velocity;

        //    _gravityForce = Math.Max(_gravityForce - _gravityPull, _maxGravity);

        //}

        //if (totalMovement.y < -groundHeight)
        //{
        //    totalMovement.y = -groundHeight;
        //}

        //_transform.position += totalMovement;

        while (_accumulatedTime >= _stepTime)
        {
            _accumulatedTime -= _stepTime;

            List<Vector3> positions = CalculateCharacterCorners(velocity);

            foreach (Vector3 position in positions)
            {
                // layer 3 - colliders
                RaycastHit2D[] hits = Physics2D.RaycastAll(position, velocity, velocity.magnitude, 1 << 3);


                foreach (RaycastHit2D hit in hits)
                {
                    velocity = Collide(velocity, position, hit, ref _gravityForce);
                }
                
            }

            _transform.position += velocity;

            _gravityForce = Math.Max(_gravityForce - _gravityPull, _maxGravity);

        }

    }

    private List<Vector3> CalculateCharacterCorners(Vector3 velocity)
    {
        List<Vector3> corners = new List<Vector3>();


        if (velocity.y > 0)
        {
            corners.Add(_transform.position + new Vector3(_collider2D.bounds.extents.x, _collider2D.bounds.extents.y));
            corners.Add(_transform.position + new Vector3(-_collider2D.bounds.extents.x, _collider2D.bounds.extents.y));
        }
        else
        {
            corners.Add(_transform.position + new Vector3(_collider2D.bounds.extents.x, -_collider2D.bounds.extents.y));
            corners.Add(_transform.position + new Vector3(-_collider2D.bounds.extents.x, -_collider2D.bounds.extents.y));
        }

        //corners.Add(_transform.position);

        return corners;
    }

    private Vector3 Collide(Vector3 velocity, Vector3 position, RaycastHit2D hit, ref float gravityForce)
    {
        // Vector3 finalVelocity = new Vector3(hit.point.x, hit.point.y) - position;

        Vector3 pushback;

        //if (Math.Abs(hit.point.y - hit.centroid.y) - hit.collider.bounds.size.y > float.Epsilon)
        //{
        //    pushback = new Vector3(0, hit.point.y - (position.y + velocity.y) + _lineThickness);
        //} else
        //{
        //    pushback = new Vector3(0, hit.point.x - (position.x + velocity.x) + _lineThickness);
        //}

        if (position.y - (hit.collider.bounds.center.y + hit.collider.bounds.extents.y) < -float.Epsilon)
        {
            return velocity;
        }

        pushback = new Vector3(0, hit.collider.bounds.center.y + hit.collider.bounds.extents.y - (position.y + velocity.y));

        DebugHelp.PointPosition = new Vector3(hit.point.x, hit.point.y);

        Vector3 finalVelocity = velocity + pushback;
        //Vector3 finalVelocity = new Vector3(position.x, hit.collider.bounds.center.y + hit.collider.bounds.extents.y) - position;

        //Debug.Log(finalVelocity);

        if (finalVelocity.y > velocity.y && velocity.y < 0)
        {
            _gravityForce = 0;
            _isOnGround = 5;
            Debug.Log("Ground");
        }

        return finalVelocity;
    }

    //private Vector3 CollideVelocity(Vector3 velocity, Line line)
    //{
    //    Vector3 finalVelocity = CollideVelocity(velocity, _transform.position, line);

    //    return finalVelocity;
    //}

    //private Vector3 CollideVelocity(Vector3 velocity, Vector3 point, Line line)
    //{
    //    if (velocity.magnitude == 0)
    //    {
    //        return velocity;
    //    }

    //    // rotate everything so the velocity vector is downwards

    //    Vector3 downwardsVelocity = Vector3.down * velocity.magnitude;

    //    Quaternion quaternion = Quaternion.FromToRotation(velocity, downwardsVelocity);

    //    Vector3 pointA = quaternion * (line.From - point);
    //    Vector3 pointB = quaternion * (line.To - point);

    //    Vector3 leftPoint = pointA.x < pointB.x ? pointA : pointB;
    //    Vector3 rightPoint = pointA.x < pointB.x ? pointB : pointA;

    //    if (leftPoint.x > 0 || rightPoint.x < 0)
    //    {
    //        return velocity;
    //    }

    //    float lineHeight = (pointB.y - pointA.y) * (pointB.x / (pointB.x - pointA.x)) + pointA.y;

    //    if (lineHeight + _lineThickness < 0)
    //    {
    //        float scale = Mathf.Min(velocity.magnitude, -lineHeight - _lineThickness);
    //        return velocity.normalized * scale;
    //    }
    //    else if (lineHeight > 0)
    //    {
    //        return velocity;
    //    }
    //    else
    //    {
    //        // we're in line
    //        return Vector3.zero;
    //    }
    //}

    private void AfterStepClear()
    {

        _isOnGround -= 1;

        _inputReader.Reset();
    }
}
