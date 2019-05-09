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

        private void OnEnable()
        {
            GroundFinding.OnGroundDetected += ShowPutPortalText;
        }

        private void OnDisable()
        {
            GroundFinding.OnGroundDetected -= ShowPutPortalText;
        }


        private void ShowPutPortalText()
        {
            floorDetectionText.SetActive(false);
            putPortalText.SetActive(true);
        }
    }
}
