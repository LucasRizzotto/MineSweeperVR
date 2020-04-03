using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LucasUtilities
{
    public class FloatToVector3Behavior : MonoBehaviour
    {
        public bool IsFloatingEnabled = true;
        [Space(5)]
        public bool UseRigidbody = false;
        public Rigidbody TargetRigidbody;
        public float RigidbodyForce;
        [Space(5)]
        public float TransformPositionSpeed = 0.1f;
        public bool LerpAngles = true;
        public float AngleSpeed = 3f;
        [Space(5)]
        public bool WorldSpace = false;
        public Vector3 TargetPosition;
        [Space(5)]
        public bool CustomInitialPosition = false;
        public Vector3 CustomInitialPositionVector3;

        public bool CustomTarget = false;
        public Transform CustomTargetObject;

        protected Vector3 InitialRotation;

        [Serializable]
        public class FloatToVector3Event : UnityEvent {}
        public FloatToVector3Event OnSetupComplete;

        [SerializeField]
        private Vector3 velocity = Vector3.zero;

        protected virtual void Start()
        {
            InitialRotation = transform.localEulerAngles;

            if(CustomInitialPosition)
            {
                transform.position = CustomInitialPositionVector3;
            }

            if (CustomTarget && CustomTargetObject != null) {
                TargetPosition = CustomTargetObject.position;
            } else {
                if (TargetPosition == null)
                TargetPosition = Vector3.zero;
            }

            if(Application.isPlaying)
            {
                StartCoroutine(WaitFrameThenCallInvoke());
            }
        }

        public IEnumerator WaitFrameThenCallInvoke()
        {
            yield return new WaitForSeconds(0.01f);
            OnSetupComplete.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            if (IsFloatingEnabled)
            {
                if(WorldSpace)
                {
                    if(UseRigidbody)
                    {
                        Vector3 forceDirection = Helpers.FindDirectionToPoint(transform.position, TargetPosition);
                        TargetRigidbody.velocity = (forceDirection * RigidbodyForce);
                    }
                    else
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref velocity, TransformPositionSpeed);
                        if (LerpAngles)
                        {
                            transform.localEulerAngles = new Vector3(
                            Mathf.LerpAngle(transform.eulerAngles.x, InitialRotation.x, TransformPositionSpeed * Time.deltaTime),
                            Mathf.LerpAngle(transform.eulerAngles.y, InitialRotation.y, TransformPositionSpeed * Time.deltaTime),
                            Mathf.LerpAngle(transform.eulerAngles.z, InitialRotation.z, TransformPositionSpeed * Time.deltaTime)
                            );
                        }
                    }
                }
                else
                {
                    if (UseRigidbody)
                    {
                        Vector3 forceDirection = Helpers.FindDirectionToPoint(transform.localPosition, TargetPosition);
                        TargetRigidbody.AddForce(forceDirection * RigidbodyForce);
                    }
                    else
                    {
                        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, TargetPosition, ref velocity, TransformPositionSpeed);
                        if (LerpAngles)
                        {
                            transform.localEulerAngles = new Vector3(
                            Mathf.LerpAngle(transform.localEulerAngles.x, InitialRotation.x, AngleSpeed * Time.deltaTime),
                            Mathf.LerpAngle(transform.localEulerAngles.y, InitialRotation.y, AngleSpeed * Time.deltaTime),
                            Mathf.LerpAngle(transform.localEulerAngles.z, InitialRotation.z, AngleSpeed * Time.deltaTime)
                            );
                        }
                    }
                    
                }
            }
        }
    }
}