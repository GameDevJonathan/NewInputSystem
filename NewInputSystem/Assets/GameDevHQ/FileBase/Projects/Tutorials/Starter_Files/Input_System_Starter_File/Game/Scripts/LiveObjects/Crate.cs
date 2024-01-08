using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

namespace Game.Scripts.LiveObjects
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private float _punchDelay;
        [SerializeField] private GameObject _wholeCrate, _brokenCrate;
        [SerializeField] private Rigidbody[] _pieces;
        [SerializeField] private BoxCollider _crateCollider;
        [SerializeField] private InteractableZone _interactableZone;
        private bool _isReadyToBreak = false;
        [SerializeField] private float _punchPower;
        [SerializeField] private int _punchLevel;
        [SerializeField] private int remainingPieces;
        [SerializeField] private bool charging = false;

        private List<Rigidbody> _brakeOff = new List<Rigidbody>();

        private void OnEnable()
        {
            //InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
            InteractableZone.onHoldStarted += InteractableZone_onHoldStarted;
            InteractableZone.onHoldEnded += InteractableZone_onHoldEnded;
        }

        private void InteractableZone_onHoldEnded(int zoneID)
        {
            Debug.Log("Release");
            if (zoneID == 6 && charging)
            {
                charging = false;
                StopCoroutine(PunchingPower());

                if (_punchPower > 0)
                {
                    _punchLevel = 1;
                }
                
                if (_punchPower > 1)
                {
                    _punchLevel = 2;
                }
                
                if (_punchPower > 2)
                {
                    _punchLevel = 3;
                }
                
                if (_punchPower > 3)
                {
                    _punchLevel = 4;
                }


                InteractableZone_onZoneInteractionAlgorhythm(_punchLevel);
            }
        }

        private void InteractableZone_onHoldStarted(int zoneID)
        {
            Debug.Log("Hold");
            if (zoneID == 6 && !charging)
            {
                charging = true;
                StartCoroutine(PunchingPower());
            }
        }

        //private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        //{

        //    if (_isReadyToBreak == false && _brakeOff.Count > 0)
        //    {
        //        _wholeCrate.SetActive(false);
        //        _brokenCrate.SetActive(true);
        //        _isReadyToBreak = true;
        //    }

        //    if (_isReadyToBreak && zone.GetZoneID() == 6) //Crate zone            
        //    {
        //        if (_brakeOff.Count > 0)
        //        {
        //            BreakPart();
        //            StartCoroutine(PunchDelay());
        //        }
        //        else if (_brakeOff.Count == 0)
        //        {
        //            _isReadyToBreak = false;
        //            _crateCollider.enabled = false;
        //            _interactableZone.CompleteTask(6);
        //            Debug.Log("Completely Busted");
        //        }
        //    }
        //}

        private void InteractableZone_onZoneInteractionAlgorhythm(int level)
        {

            if (_isReadyToBreak == false && _brakeOff.Count > 0)
            {
                _wholeCrate.SetActive(false);
                _brokenCrate.SetActive(true);
                _isReadyToBreak = true;
            }

            if (_isReadyToBreak) //Crate zone            
            {
                if (_brakeOff.Count > 0)
                {
                    BreakPart(level);
                    StartCoroutine(PunchDelay());
                }
                //else if (_brakeOff.Count == 0)
                //{
                //    _isReadyToBreak = false;
                //    _crateCollider.enabled = false;
                //    _interactableZone.CompleteTask(6);
                //    Debug.Log("Completely Busted");
                //}
            }
        }

        private void Start()
        {
            _brakeOff.AddRange(_pieces);
            remainingPieces = _pieces.Length;
        }



        public void BreakPart(int level)
        {
            if (level > remainingPieces)
            {
                level = remainingPieces;
            }
            else
            {
                remainingPieces -= level;
            }            
            
            Debug.Log("break level: " + level);
            
            for (int i = 0; i < level; i++)
            {
                int rng = Random.Range(0, _brakeOff.Count);
                _brakeOff[rng].constraints = RigidbodyConstraints.None;
                _brakeOff[rng].AddForce(new Vector3(1f, 1f, 1f), ForceMode.Force);
                _brakeOff.Remove(_brakeOff[rng]);
            }
            
            if (_brakeOff.Count == 0)
            {
                _isReadyToBreak = false;
                _crateCollider.enabled = false;
                _interactableZone.CompleteTask(6);
                Debug.Log("Completely Busted");
            }
            _punchPower = 0;
        }

        IEnumerator PunchDelay()
        {
            float delayTimer = 0;
            while (delayTimer < _punchDelay)
            {
                yield return new WaitForEndOfFrame();
                delayTimer += Time.deltaTime;
            }

            _interactableZone.ResetAction(6);
        }

        IEnumerator PunchingPower()
        {
            while (_punchPower < 3 && charging)
            {
                _punchPower += Time.deltaTime;                
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDisable()
        {
            //InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
            InteractableZone.onHoldStarted -= InteractableZone_onHoldStarted;
            InteractableZone.onHoldEnded -= InteractableZone_onHoldEnded;
        }
    }
}
