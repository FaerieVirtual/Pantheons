using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
    public class PlayerInput
    {
        public KeyCode key;

        public UnityEvent OnDown = new();
        public UnityEvent OnUp = new();
        public UnityEvent OnHold = new();
        public UnityEvent OnMaxReached = new();

        public float timePressed = new();
        private float maxTime;

        public PlayerInput(KeyCode key, float maxTime)
        {
            this.key = key;
            this.maxTime = maxTime;
        }

        public void Update() 
        {
            if (Input.GetKeyDown(key))
            {
                timePressed = Time.time;
                OnDown.Invoke();
            }
            if (Input.GetKey(key) && timePressed + maxTime > Time.time) 
            {
                OnHold.Invoke();
            }
            if (Input.GetKeyUp(key))
            {
                if (timePressed + maxTime < Time.time) { OnMaxReached.Invoke(); }
                else { OnUp.Invoke(); }
            }
        }
    }
}
