using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterAR
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform gunForwardVector;

        private bool unlockBullets = false;

        public void OnEnable()
        {
            Cannon.OnShoot += SpawnOneBullet;
            GroundFinding.OnStartGame += UnlockBullets;
        }

        public void OnDisable()
        {
            Cannon.OnShoot -= SpawnOneBullet;
            GroundFinding.OnStartGame -= UnlockBullets;
        }

        public void SpawnOneBullet(Vector3 spawnPoint)
        {
            if (!unlockBullets)
                return;
            
            transform.GetChild(0).GetComponent<Bullet>().SpawnPoint = spawnPoint;
            transform.GetChild(0).GetComponent<Bullet>().Shoot(gunForwardVector.transform.forward);
        }

        private void UnlockBullets() {
            unlockBullets = true;
        }
    }
}
