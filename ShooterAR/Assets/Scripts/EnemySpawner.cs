using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShooterAR
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform sceneCenter;
        [SerializeField]
        private float radius = 5;
        [SerializeField]
        private int startDronesNum = 5;
        [SerializeField]
        private int lvlStep = 3;
        [SerializeField]
        private Text scoreTxt;
        [SerializeField]
        private RectTransform energy;
        [SerializeField]
        private Image vigniette;
        [SerializeField]
        private GameObject attackCollider;
        [SerializeField]
        private GameObject failText;
        [SerializeField]
        private GameObject winText;

        private int dronesKilled = 0;

        private int totalKilledInRound = 0;
        private IEnumerator startSpawningCorutine;
        private IEnumerator vignietteCorutine;

        public int TotalKilled
        {
            set
            {
                totalKilledInRound += value;
                if (totalKilledInRound >= startDronesNum)
                {
                    startDronesNum += lvlStep;
                    SpawnDrones();
                }
            }

            get
            {
                return totalKilledInRound;
            }
        }

        private void Start()
        {
            scoreTxt.text = dronesKilled.ToString();
        }

        private void OnEnable()
        {
            GroundFinding.OnStartGame += SpawnDrones;
            DroneProjectile.OnEnemyTakeHit += Hited;
        }

        private void OnDisable()
        {
            GroundFinding.OnStartGame -= SpawnDrones;
            DroneProjectile.OnEnemyTakeHit -= Hited;
        }

        private void SpawnDrones()
        {
            if (transform.childCount < 2)
            {
                //winText.SetActive(true);
                return;
            }

            if (startSpawningCorutine != null)
                StopCoroutine(startSpawningCorutine);

            startSpawningCorutine = StartSpawning();
            StartCoroutine(startSpawningCorutine);
        }

        public void SpawnMore()
        {
            dronesKilled++;
            scoreTxt.text = dronesKilled.ToString();
            startDronesNum = Random.Range(1, 3);
            SpawnDrones();
        }

        public void Hited()
        {
            energy.sizeDelta = new Vector2(energy.sizeDelta.x - 10, energy.sizeDelta.y);
            if (energy.sizeDelta.x <= 0)
            {
                attackCollider.SetActive(false);
                failText.SetActive(true);
            }

            if (vignietteCorutine != null)
                StopCoroutine(vignietteCorutine);

            vignietteCorutine = Vigniette();
            StartCoroutine(vignietteCorutine);
        }

        IEnumerator Vigniette()
        {
            while (vigniette.color.a < 0.5f)
            {
                vigniette.color = Color.Lerp(vigniette.color, new Color(1, 1, 1, 0.5f), Time.deltaTime * 40f);
                yield return null;
            }
            while (vigniette.color.a > 0.01f)
            {
                vigniette.color = Color.Lerp(vigniette.color, new Color(1, 1, 1, 0), Time.deltaTime * 8f);
                yield return null;
            }
            vigniette.color = new Color(1, 1, 1, 0);
        }

        IEnumerator StartSpawning()
        {
            int spawnNumber = 0;
            while (spawnNumber < startDronesNum)
            {
                transform.GetChild(spawnNumber).gameObject.SetActive(true);
                transform.GetChild(spawnNumber).GetComponent<DroneController>().DroneSpawnPoint = GetPositionAroundObject();
                transform.GetChild(spawnNumber).GetComponent<Animator>().SetBool("ReleaseDron", true);
                spawnNumber++;
                yield return new WaitForSeconds(0.5f);
            }
        }

        Vector3 GetPositionAroundObject()
        {
            //return Vector3.forward * 10;
            Vector3 offset = Random.onUnitSphere * radius;
            offset.y = Mathf.Abs(offset.y / 10);
            return sceneCenter.transform.position + offset;
        }
    }
}