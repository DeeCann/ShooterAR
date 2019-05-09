using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterAR
{
    public class Bullet : MonoBehaviour
    {
        public float ProjectileVelocity = 300f;
        public float RaycastPrediction = 2f;

        private TrailRenderer bullatTrail;
        private Transform parent;
        private Vector3 fwdVector;
        private Vector3 _spawnPoint;
        private RaycastHit hitPoint;

        private bool _isSapwned = false;

        public bool IsSpawned
        {
            set
            {
                _isSapwned = value;
            }

            get
            {
                return _isSapwned;
            }
        }

        public Vector3 SpawnPoint
        {
            set
            {
                _spawnPoint = value;
            }
        }

        public void Start()
        {
            parent = transform.parent;
            bullatTrail = GetComponent<TrailRenderer>();
        }

        public void FixedUpdate()
        {
            if (IsSpawned)
            {
                transform.Translate(fwdVector * Time.deltaTime * 120, Space.World);
                Vector3 step = transform.forward * Time.deltaTime * ProjectileVelocity;
                if (Physics.Raycast(transform.position, transform.forward, out hitPoint, step.magnitude * RaycastPrediction))
                {
                    if (hitPoint.collider.gameObject.GetComponent<DroneController>())
                    {
                        hitPoint.collider.gameObject.GetComponent<DroneController>().TakeHit(10, hitPoint.point);
                    }
                }
            }

            if (Vector3.Distance(transform.position, _spawnPoint) > 500)
                DeSpawn();
        }

        public void Shoot(Vector3 lookAt)
        {
            if (IsSpawned)
                return;

            transform.parent = null;
            transform.position = _spawnPoint;
            transform.rotation = Quaternion.LookRotation(lookAt);

            fwdVector = lookAt;
            IsSpawned = true;
            bullatTrail.enabled = true;
        }

        private void DeSpawn()
        {
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            IsSpawned = false;
            bullatTrail.enabled = false;
        }
    }
}