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
        private float inputTimer;
        private bool maxReached;
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
                maxReached = false;
                OnDown.Invoke();
            }
            if (Input.GetKey(key) && inputTimer < maxTime && !maxReached /*timePressed + maxTime > Time.time*/) 
            {
                OnHold.Invoke();
                inputTimer += Time.unscaledDeltaTime;
            }
            if (Input.GetKeyUp(key))
            {
                OnUp.Invoke();
                inputTimer = 0;
            }
            if (inputTimer >= maxTime && !maxReached) 
            { 
                OnMaxReached.Invoke();
                maxReached = true;
            }
        }
    }
}
