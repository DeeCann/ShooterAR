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
        private GameObject radar;
        [SerializeField]
        private GameObject power;
        [SerializeField]
        private Text randomText;

        private string[] ramdomTexts = new string[10] {
            "Don't be childish. Press YES",
            "What a rookie!!! Press YES",
            "This button doesn't work. Press YES",
            "Drones will smash your teddy bear. Press YES",
            "Are you crying???. Press YES",
            "Get yourself together and fight! Press YES",
            "Ohhh ... are you a man or what?? Press YES",
            "Killing is in your blood. Press YES",
            "I guess it was mistake. Press YES",
            "No, I don't accept that. Press YES"
        };

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
            radar.SetActive(true);
            putPortalText.SetActive(false);
        }

        private void ShowPutPortalText()
        {
            floorDetectionText.SetActive(false);
            putPortalText.SetActive(true);
        }

        public void PlayAgain() {
            SceneManager.LoadScene(0);
        }

        public void RandomText()
        {
            randomText.text = ramdomTexts[Random.Range(0, 9)];
        }

		public void Quit()
		{
            Application.Quit();
		}
	}
}
