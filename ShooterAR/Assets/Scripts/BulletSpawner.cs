using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterAR
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform gunForwardVector;

        public void OnEnable()
        {
            Cannon.OnShoot += SpawnOneBullet;
        }

        public void OnDisable()
        {
            Cannon.OnShoot -= SpawnOneBullet;
        }

        public void SpawnOneBullet(Vector3 spawnPoint)
        {
            transform.GetChild(0).GetComponent<Bullet>().SpawnPoint = spawnPoint;
            transform.GetChild(0).GetComponent<Bullet>().Shoot(gunForwardVector.transform.forward);
        }
    }
}
