using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public float m_brakeMultiplier = 1;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // float v = CrossPlatformInputManager.GetAxis("Vertical");
            //  float v = Input.GetAxis("Vertical");
            float v = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)|| (Input.GetAxis("RT")>0.5)) 
                ? 
                    1 :
                    ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("JoystickB")>0.5)    //(Input.GetAxis("RT") > 0.5)
                    ?
                        -1:
                        0);

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            if (v != 0 || h != 0 || handbrake != 0)
                m_Car.Move(h, v, -handbrake * m_brakeMultiplier, handbrake * m_brakeMultiplier); 
            else
            {
               
                for (int i = 0; i < 4; i++)
                {
                    m_Car.m_WheelColliders[i].motorTorque = 0;
                    if (m_Car.CurrentSpeed > 5)
                    {
                        m_Car.m_WheelColliders[i].brakeTorque = m_Car.m_MaxHandbrakeTorque * 0.01f;
                      //m_Car.m_Rigidbody.velocity = Vector3.Lerp(m_Car.m_Rigidbody.velocity, Vector3.zero, Time.deltaTime);

                    }
                    else
                        m_Car.m_WheelColliders[i].brakeTorque = 0;
                }

            }

#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
