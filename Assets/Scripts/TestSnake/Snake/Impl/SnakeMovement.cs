using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestSnake.Snake.Impl
{
    public class SnakeMovement : IMovement
    {
        private const float OFFSET_FROM_LAND = .5f;
        private const float ALIGN_SPEED = 8f;
        private const float CHECK_FORWARD_DISTANCE = 1f;
        private const float SEGMENT_MIN_RECORD_DIST = 0.05f;
        private const int MAX_HISTORY_SAMPLES = 4096;

        private readonly List<Vector3> _tailPositions = new(capacity: 256);
        private readonly ISnake _snake;
        private Vector3 _groundNormal = Vector3.up;
        
        private struct HeadSample
        {
            public Vector3 Position;
            public Vector3 Up;
            public float DistanceFromStart;
        }
        
        private float _totalPathLength;

        private readonly List<HeadSample> _headHistory = new(MAX_HISTORY_SAMPLES);
        
        public SnakeMovement(ISnake snake)
        {
            _snake = snake;
        }

        public void Move(Vector2 inputDirection)
        {

            var head = _snake.Body.First();
            CheckGround(head);
            
            MoveHead(inputDirection, head);
            MoveTail();
        }

        private void CheckGround(ASnakeNode head)
        {
            //first check is something in front of head
            var rayForwardCheck = new Ray(head.transform.position, head.transform.forward);

            if (Physics.Raycast(rayForwardCheck, out var hitForward, CHECK_FORWARD_DISTANCE, LayerMaskGame.Map))
            {
                _groundNormal = hitForward.normal;
                return;
            }

            var ray = new Ray(head.transform.position, -head.transform.up);

            if (!Physics.Raycast(ray, out var hitUnder, int.MaxValue, LayerMaskGame.Map)) return;

            _groundNormal = hitUnder.normal;
        }

        private void MoveHead(Vector2 input, ASnakeNode head)
        {
            head.Rigidbody.MovePosition(head.transform.position + _groundNormal * OFFSET_FROM_LAND);

            var targetUp = _groundNormal;
            var currentUp = head.transform.up;
            var smoothedUp = Vector3.Slerp(currentUp, targetUp, Time.deltaTime * ALIGN_SPEED);

            var forward = Vector3.ProjectOnPlane(head.transform.forward, smoothedUp).normalized;

            if (input.x != 0)
            {
                var turnAmount = input.x * _snake.Data.TurnSpeed * Time.deltaTime;
                var turnRotation = Quaternion.AngleAxis(turnAmount, smoothedUp);
                forward = turnRotation * forward;
            }

            var targetRotation = Quaternion.LookRotation(forward, smoothedUp);
            head.Rigidbody.MoveRotation(Quaternion.Slerp(
                head.Rigidbody.rotation,
                targetRotation,
                Time.deltaTime * _snake.Data.TurnSpeed));

            var moveDirection = Vector3.ProjectOnPlane(head.transform.forward, _groundNormal).normalized;
            var targetPosition = head.transform.position + moveDirection;

            head.Rigidbody.MovePosition(Vector3.MoveTowards(
                head.transform.position,
                targetPosition,
                _snake.Data.MoveSpeed * Time.deltaTime
            ));

            _tailPositions.Add(head.transform.position);

            RecordHeadSample(head.transform.position, _groundNormal);
        }
        
        private void RecordHeadSample(Vector3 pos, Vector3 up)
        {
            if (_headHistory.Count == 0)
            {
                _headHistory.Add(new HeadSample
                {
                    Position = pos,
                    Up = up,
                    DistanceFromStart = 0f
                });
                _totalPathLength = 0f;
                return;
            }

            var last = _headHistory[^1];
            var d = Vector3.Distance(last.Position, pos);
            if (d < SEGMENT_MIN_RECORD_DIST) return;

            _totalPathLength += d;

            _headHistory.Add(new HeadSample
            {
                Position = pos,
                Up = up,
                DistanceFromStart = _totalPathLength
            });

            var maxNeeded = _snake.Body.Count * _snake.Data.BodySpace + 1f;

            while (_headHistory.Count > 2 &&
                   _totalPathLength - _headHistory[0].DistanceFromStart > maxNeeded)
            {
                _headHistory.RemoveAt(0);
            }
        }

        private void MoveTail()
        {
            if (_headHistory.Count < 2) return;

            int histIndex = _headHistory.Count - 1;

            var node = _snake.Body.First.Next;
            int segmentIndex = 1;

            while (node != null && histIndex > 0)
            {
                float targetDist = _totalPathLength - segmentIndex * _snake.Data.BodySpace;

                // Move backward in history only once
                while (histIndex > 0 &&
                       _headHistory[histIndex - 1].DistanceFromStart > targetDist)
                {
                    histIndex--;
                }

                var newer = _headHistory[histIndex];
                var older = _headHistory[Mathf.Max(0, histIndex - 1)];

                float segLen = newer.DistanceFromStart - older.DistanceFromStart;
                float t = segLen > 0.0001f
                    ? Mathf.InverseLerp(newer.DistanceFromStart, older.DistanceFromStart, targetDist)
                    : 0f;

                Vector3 pos = Vector3.Lerp(newer.Position, older.Position, t);
                Vector3 up = Vector3.Slerp(newer.Up, older.Up, t).normalized;

                var segment = node.Value;

                Vector3 targetPos = pos + up * OFFSET_FROM_LAND;

                // movement
                segment.Rigidbody.MovePosition(Vector3.MoveTowards(
                    segment.transform.position,
                    targetPos,
                    _snake.Data.MoveSpeed * 6f * Time.deltaTime
                ));

                // rotation
                Vector3 forward = (newer.Position - pos).normalized;
                Quaternion rot = Quaternion.LookRotation(forward, up);
                segment.Rigidbody.MoveRotation(Quaternion.Slerp(
                    segment.Rigidbody.rotation,
                    rot,
                    Time.deltaTime * 8f
                ));

                segmentIndex++;
                node = node.Next;
            }
        }
    }
}