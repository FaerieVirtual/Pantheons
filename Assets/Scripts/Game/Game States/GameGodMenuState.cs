using UnityEngine.SceneManagement;

namespace Assets.Scripts.Backend.Game.Game_States
{
    internal class GameGodMenuState : GameState
    {
        public GameGodMenuState(GameStatemachine machine) : base(machine)
        {
            this.machine = machine;
        }

        public override void EnterState()
        {
            GameManager.Instance.Area = "GodMenu";
            //if (SceneManager.GetActiveScene().buildIndex != 1) levelManager.LoadScene(1, false);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
