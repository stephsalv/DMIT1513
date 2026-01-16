using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    internal enum CarDriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    internal enum SpeedType
    {
        MPH,
        KPH
    }

    [RequireComponent(typeof(Rigidbody))]
    public class AICarController : MonoBehaviour
    {
        [Header("Car Settings")]
        [SerializeField] private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
        [SerializeField] private SpeedType m_SpeedType = SpeedType.MPH;
        [SerializeField] private float m_Topspeed = 200f;
        [SerializeField] private float m_MaximumSteerAngle = 30f;
        [SerializeField] private float m_FullTorqueOverAllWheels = 500f;
        [SerializeField] private float m_ReverseTorque = 300f;
        [SerializeField] private float m_BrakeTorque = 300f;
        [SerializeField] private float m_MaxHandbrakeTorque = float.MaxValue;
        [SerializeField] private float m_Downforce = 100f;
        [Range(0, 1)][SerializeField] private float m_SteerHelper = 0.5f;
        [Range(0, 1)][SerializeField] private float m_TractionControl = 0.5f;
        [SerializeField] private float m_SlipLimit = 0.5f;
        [SerializeField] private Vector3 m_CentreOfMassOffset;
        [SerializeField] private int m_NoOfGears = 5;
        [SerializeField] private float m_RevRangeBoundary = 1f;

        [Header("Wheels")]
        [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
        [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
        [SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];

        // Wheel rotations
        private Quaternion[] m_WheelMeshLocalRotations;
        private Rigidbody m_Rigidbody;
        private float m_SteerAngle;
        private int m_GearNum;
        private float m_GearFactor;
        private float m_CurrentTorque;
        private float m_OldRotation;

        public float Revs { get; private set; }
        public bool Skidding { get; private set; }
        public float CurrentSpeed
        {
            get
            {
                float speed = m_Rigidbody.linearVelocity.magnitude;
                return m_SpeedType == SpeedType.MPH ? speed * 2.23693629f : speed * 3.6f;
            }
        }
        public float MaxSpeed => m_Topspeed;
        public float CurrentSteerAngle => m_SteerAngle;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.centerOfMass += m_CentreOfMassOffset;

            m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);

            m_WheelMeshLocalRotations = new Quaternion[4];
            for (int i = 0; i < 4; i++)
            {
                if (m_WheelMeshes[i] != null)
                    m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
            }
        }

        /// <summary>
        /// Main move function for AI to control the car.
        /// Call this from your AI system.
        /// </summary>
        /// <param name="steering">-1 to 1</param>
        /// <param name="accel">0 to 1</param>
        /// <param name="footbrake">0 to 1</param>
        /// <param name="handbrake">0 to 1</param>
        public void MoveAI(float steering, float accel, float footbrake, float handbrake)
        {
            UpdateWheelMeshes();
            ApplySteering(steering);
            ApplyDrive(accel, footbrake);
            ApplyHandbrake(handbrake);

            SteerHelper();
            CapSpeed();
            AddDownForce();
            CalculateRevs();
            GearChanging();
            CheckForWheelSpin();
            TractionControl();
        }

        private void UpdateWheelMeshes()
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_WheelColliders[i] == null || m_WheelMeshes[i] == null) continue;

                m_WheelColliders[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
                m_WheelMeshes[i].transform.position = pos;
                m_WheelMeshes[i].transform.rotation = rot;
            }
        }

        private void ApplySteering(float steering)
        {
            steering = Mathf.Clamp(steering, -1f, 1f);
            m_SteerAngle = steering * m_MaximumSteerAngle;
            m_WheelColliders[0].steerAngle = m_SteerAngle;
            m_WheelColliders[1].steerAngle = m_SteerAngle;
        }

        private void ApplyDrive(float accel, float footbrake)
        {
            float thrustTorque = 0f;

            switch (m_CarDriveType)
            {
                case CarDriveType.FourWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 4f);
                    for (int i = 0; i < 4; i++)
                        m_WheelColliders[i].motorTorque = thrustTorque;
                    break;
                case CarDriveType.FrontWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
                    break;
                case CarDriveType.RearWheelDrive:
                    thrustTorque = accel * (m_CurrentTorque / 2f);
                    m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
                    break;
            }

            // Apply brakes or reverse torque
            for (int i = 0; i < 4; i++)
            {
                if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.linearVelocity) < 50f)
                    m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
                else if (footbrake > 0)
                {
                    m_WheelColliders[i].brakeTorque = 0f;
                    m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
                }
            }
        }

        private void ApplyHandbrake(float handbrake)
        {
            handbrake = Mathf.Clamp(handbrake, 0f, 1f);
            if (handbrake <= 0f) return;

            float hbTorque = handbrake * m_MaxHandbrakeTorque;
            m_WheelColliders[2].brakeTorque = hbTorque;
            m_WheelColliders[3].brakeTorque = hbTorque;
        }

        private void CapSpeed()
        {
            float speed = m_Rigidbody.linearVelocity.magnitude;
            switch (m_SpeedType)
            {
                case SpeedType.MPH:
                    speed *= 2.23693629f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.linearVelocity = (m_Topspeed / 2.23693629f) * m_Rigidbody.linearVelocity.normalized;
                    break;
                case SpeedType.KPH:
                    speed *= 3.6f;
                    if (speed > m_Topspeed)
                        m_Rigidbody.linearVelocity = (m_Topspeed / 3.6f) * m_Rigidbody.linearVelocity.normalized;
                    break;
            }
        }

        private void SteerHelper()
        {
            for (int i = 0; i < 4; i++)
            {
                if (!m_WheelColliders[i].GetGroundHit(out WheelHit hit)) return;
                if (hit.normal == Vector3.zero) return;
            }

            if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
            {
                float turnAdjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnAdjust, Vector3.up);
                m_Rigidbody.linearVelocity = velRotation * m_Rigidbody.linearVelocity;
            }
            m_OldRotation = transform.eulerAngles.y;
        }

        private void AddDownForce()
        {
            m_Rigidbody.AddForce(-transform.up * m_Downforce * m_Rigidbody.linearVelocity.magnitude);
        }

        private void GearChanging()
        {
            float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
            float upLimit = (1f / m_NoOfGears) * (m_GearNum + 1);
            float downLimit = (1f / m_NoOfGears) * m_GearNum;

            if (m_GearNum > 0 && f < downLimit) m_GearNum--;
            if (f > upLimit && m_GearNum < (m_NoOfGears - 1)) m_GearNum++;
        }

        private void CalculateGearFactor()
        {
            float f = 1f / m_NoOfGears;
            float target = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
            m_GearFactor = Mathf.Lerp(m_GearFactor, target, Time.deltaTime * 5f);
        }

        private void CalculateRevs()
        {
            CalculateGearFactor();
            float gearFactor = m_GearNum / (float)m_NoOfGears;
            float revMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearFactor));
            float revMax = ULerp(m_RevRangeBoundary, 1f, gearFactor);
            Revs = ULerp(revMin, revMax, m_GearFactor);
        }

        private static float ULerp(float from, float to, float value) => (1 - value) * from + value * to;
        private static float CurveFactor(float factor) => 1 - (1 - factor) * (1 - factor);

        private void CheckForWheelSpin()
        {
            for (int i = 0; i < 4; i++)
            {
                if (!m_WheelColliders[i].GetGroundHit(out WheelHit wheelHit)) continue;

                if (m_WheelEffects[i]?.PlayingAudio == true) m_WheelEffects[i]?.StopAudio();
                m_WheelEffects[i]?.EndSkidTrail();
            }
        }

        private void TractionControl()
        {
            switch (m_CarDriveType)
            {
                case CarDriveType.FourWheelDrive:
                    for (int i = 0; i < 4; i++)
                        if (m_WheelColliders[i].GetGroundHit(out WheelHit hit))
                            AdjustTorque(hit.forwardSlip);
                    break;
                case CarDriveType.RearWheelDrive:
                    for (int i = 2; i <= 3; i++)
                        if (m_WheelColliders[i].GetGroundHit(out WheelHit hit))
                            AdjustTorque(hit.forwardSlip);
                    break;
                case CarDriveType.FrontWheelDrive:
                    for (int i = 0; i <= 1; i++)
                        if (m_WheelColliders[i].GetGroundHit(out WheelHit hit))
                            AdjustTorque(hit.forwardSlip);
                    break;
            }
        }

        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
                m_CurrentTorque -= 10f * m_TractionControl;
            else
            {
                m_CurrentTorque += 10f * m_TractionControl;
                if (m_CurrentTorque > m_FullTorqueOverAllWheels)
                    m_CurrentTorque = m_FullTorqueOverAllWheels;
            }
        }
    }
}
