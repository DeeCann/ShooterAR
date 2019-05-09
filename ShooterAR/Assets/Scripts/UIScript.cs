using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ShooterAR
{
    public class UIScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject floorDetectionText;
        [SerializeField]
        private GameObject putPortalText;
        [SerializeField]
        private GameObject counter;
        [SerializeField]
        private GameObject power;

        private void OnEnable()
        {
            GroundFinding.OnGroundDetected += ShowPutPortalText;
            GroundFinding.OnStartGame += ShowUI;
        }

        private void OnDisable()
        {
            GroundFinding.OnGroundDetected -= ShowPutPortalText;
            GroundFinding.OnStartGame -= ShowUI;
        }

        private void ShowUI() {
            counter.SetActive(true);
            power.SetActive(true);
            putPortalText.SetActive(false);
        }

        private void ShowPutPortalText()
        {
            floorDetectionText.SetActive(false);
            putPortalText.SetActive(true);
        }
    }
}
