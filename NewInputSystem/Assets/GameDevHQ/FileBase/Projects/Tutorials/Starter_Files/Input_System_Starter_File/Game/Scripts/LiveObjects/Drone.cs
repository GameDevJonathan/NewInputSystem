using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;

namespace Game.Scripts.LiveObjects
{
    public class Drone : MonoBehaviour
    {
        private enum Tilt
        {
            NoTilt, Forward, Back, Left, Right
        }

        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        private bool _inFlightMode = false;
        [SerializeField]
        private Animator _propAnim;
        [SerializeField]
        private CinemachineVirtualCamera _droneCam;
        [SerializeField]
        private InteractableZone _interactableZone;
        [SerializeField]
        private UpgradedLegacyInput _input;
        

        public static event Action OnEnterFlightMode;
        public static event Action onExitFlightmode;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += EnterFlightMode;
        }

        private void Start()
        {
            _input = new UpgradedLegacyInput();
            _input.Drone.Enable();
        }

        private void EnterFlightMode(InteractableZone zone)
        {
            if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
            {
                _propAnim.SetTrigger("StartProps");
                _droneCam.Priority = 11;
                _inFlightMode = true;
                OnEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                _interactableZone.CompleteTask(4);
            }
        }

        private void ExitFlightMode()
        {            
            _droneCam.Priority = 9;
            _inFlightMode = false;
            UIManager.Instance.DroneView(false);            
        }

        private void Update()
        {
            if (_inFlightMode)
            {
                CalculateTilt();
                CalculateMovementUpdate();

                if (_input.Drone.InteractionExit.WasPerformedThisFrame())
                {
                    _inFlightMode = false;
                    onExitFlightmode?.Invoke();
                    ExitFlightMode();
                }
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
            if (_inFlightMode)
                CalculateMovementFixedUpdate();
        }

        private void CalculateMovementUpdate()
        {

            float rotationValue = _input.Drone.Rotation.ReadValue<float>() * _speed;
            var tempRot = transform.localRotation.eulerAngles;
            tempRot.y += rotationValue ;
            transform.localRotation = Quaternion.Euler(tempRot);
            
            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    var tempRot = transform.localRotation.eulerAngles;
            //    tempRot.y -= _speed / 3;
            //    transform.localRotation = Quaternion.Euler(tempRot);
            //}
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    var tempRot = transform.localRotation.eulerAngles;
            //    tempRot.y += _speed / 3;
            //    transform.localRotation = Quaternion.Euler(tempRot);
            //}
        }

        private void CalculateMovementFixedUpdate()
        {
            float thrusterValue = _input.Drone.Thrusters.ReadValue<float>() * _speed;
            _rigidbody.AddForce(transform.up * thrusterValue, ForceMode.Acceleration);
            //if (Input.GetKey(KeyCode.Space))
            //{
            //    _rigidbody.AddForce(transform.up * _speed, ForceMode.Acceleration);
            //}
            //if (Input.GetKey(KeyCode.V))
            //{
            //    _rigidbody.AddForce(-transform.up * _speed, ForceMode.Acceleration);
            //}
        }

        private void CalculateTilt()
        {
            if (_input.Drone.Movement.ReadValue<Vector2>().x < 0) //left
                transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
            else if (_input.Drone.Movement.ReadValue<Vector2>().x > 0) //right
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
            else if (_input.Drone.Movement.ReadValue<Vector2>().y > 0) //up
                transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
            else if (_input.Drone.Movement.ReadValue<Vector2>().y < 0)//down
                transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
            else
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);

            //if (Input.GetKey(KeyCode.A)) 
            //    transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
            //else if (Input.GetKey(KeyCode.D))
            //    transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
            //else if (Input.GetKey(KeyCode.W))
            //    transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
            //else if (Input.GetKey(KeyCode.S))
            //    transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
            //else 
            //    transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= EnterFlightMode;
        }
    }
}
