using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterAR
{
    public class Cannon : MonoBehaviour
    {
        public delegate void CannonHandler(Vector3 spawnPoint);
        public static CannonHandler OnShoot = delegate { };

        [SerializeField]
        private Animator cannonFlameAC;
        [SerializeField]
        private AudioSource shootSound;
        [SerializeField]
        private bool isRight = false;

        private Animator canonAnimator;
        private IEnumerator delayRightCannonShootCorutine;
        private bool unlockBullets = false;

        private void OnEnable()
        {
            FireWeapon.StartShoot += Shoot;
            FireWeapon.StopShoot += Stop;
            GroundFinding.OnStartGame += UnlockBullets;
        }

		private void OnDisable()
		{
            FireWeapon.StartShoot -= Shoot;
            FireWeapon.StopShoot += Stop;
            GroundFinding.OnStartGame -= UnlockBullets;
		}

		private void Start()
		{
            canonAnimator = GetComponent<Animator>();
		}

        public void DeployBullet() {
            OnShoot(cannonFlameAC.transform.position);
            shootSound.Play();
        }

        /**
         * Shoot has to starts.
         * First starts to shoot immediately
         * Second waits for a moment
         * This way we can shoot with 2 guns separate
         **/
		private void Shoot()
        {
            if (!unlockBullets)
                return;
            
            if (!isRight)
            {
                canonAnimator.SetBool("Shoot", true);
                cannonFlameAC.SetBool("Shoot", true);
            }
            else {
                if (delayRightCannonShootCorutine != null)
                    StopCoroutine(delayRightCannonShootCorutine);

                delayRightCannonShootCorutine = DelayRightCannonShoot();
                StartCoroutine(delayRightCannonShootCorutine);
            }
        }

        private void Stop() {
            canonAnimator.SetBool("Shoot", false);
            cannonFlameAC.SetBool("Shoot", false);
            if (delayRightCannonShootCorutine != null)
                StopCoroutine(delayRightCannonShootCorutine);
        }

        private void UnlockBullets()
        {
            unlockBullets = true;
        }

		IEnumerator DelayRightCannonShoot() {
            yield return new WaitForSeconds(0.15f);
            canonAnimator.SetBool("Shoot", true);
            cannonFlameAC.SetBool("Shoot", true);
        }
    }
}