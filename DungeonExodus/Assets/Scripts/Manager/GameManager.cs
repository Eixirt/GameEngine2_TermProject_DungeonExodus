using KPU.Manager;
using UnityEngine;

namespace Manager
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private DungeonExodusState state;
        public DungeonExodusState State => state;

        /// <summary>
        /// state 설정.
        /// </summary>
        /// <param name="targetState">게임의 상태</param>
        public void SetState(DungeonExodusState targetState)
        {
            state = targetState;
        }

        private void Start()
        {
            state = DungeonExodusState.Initializing;
            
            EventManager.On("game_started", o => SetState(DungeonExodusState.Playing));
            EventManager.On("game_ended", o => SetState(DungeonExodusState.GameEnded));
            EventManager.On("game_paused", o => SetState(DungeonExodusState.Paused));
            EventManager.On("game_resumed", o => SetState(DungeonExodusState.Playing));
        }
    }
}