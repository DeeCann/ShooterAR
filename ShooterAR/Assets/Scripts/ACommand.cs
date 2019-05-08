using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterAR
{
    /**
     *  Command Pattern Abstract Class
     **/
    public abstract class Command
    {
        public abstract void Execute();
        public abstract void Stop();
    }

    public class FireWeapon : Command
    {
        public delegate void ShootHandler();
        public static ShootHandler StartShoot = delegate { };
        public static ShootHandler StopShoot = delegate { };

        public override void Execute()
        {
            StartShoot();     
        }

        public override void Stop()
        {
            StopShoot();
        }
    }
}
