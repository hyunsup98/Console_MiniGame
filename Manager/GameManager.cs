using System;
using System.Text;

namespace Console_Portfolio
{
    /// <summary>
    /// 게임 상태를 보여주는 열거형
    /// </summary>
    public enum GameState
    {
        Title,          //게임을 시작할 때 나오는 메인메뉴 화면 (최초 실행 한 번만 볼 수 있음)
        InGame,         //각각의 미니게임을 플레이할 때 상태
    }

    /// <summary>
    /// 전반적인 게임을 관리하는 게임매니저
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        #region 게임 상태 변경 및 상태에 따른 대리자 호출
        public event Action action_StateTitle;      //타이틀 액션
        public event Action action_StateInGame;     //인게임 들어갔을 때 액션

        private GameState currentGameState;
        public GameState CurrentGameState
        {
            get { return currentGameState; }
            set
            {
                //현재 상태가 이미 value 상태면 무시
                if (currentGameState == value) return;

                currentGameState = value;

                switch (value)
                {
                    case GameState.Title:
                        action_StateTitle?.Invoke();
                        break;

                    case GameState.InGame:
                        action_StateInGame?.Invoke();
                        break;
                }
            }
        }
        #endregion

        //게임매니저 초기화 메서드
        public void Start()
        {
            currentGameState = GameState.Title;
            action_StateTitle?.Invoke();

            action_StateInGame += SoundManager.Instance.Dispose;

            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}
