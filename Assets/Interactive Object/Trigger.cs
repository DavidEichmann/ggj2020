using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Interactive_Object
{
    public class Trigger : MonoBehaviour
    {

        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        void OnTriggerEnter(Collider collide)
        {
            if (collide.tag == "Player")
            {
                OnEnter.Invoke();               
            }
        }

        void OnTriggerExit(Collider collide)
        {
            if (collide.tag == "Player")
            {
                OnExit.Invoke();
            }
        }
    }
}
