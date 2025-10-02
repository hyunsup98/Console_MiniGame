using System;

namespace Console_Portfolio
{
    /// <summary>
    /// 게임 상태를 보여주는 열거형
    /// </summary>
    public enum GameState
    {
        Main,           //게임을 시작할 때 나오는 메인메뉴 화면 (최초 실행 한 번만 볼 수 있음)
        Village,        //방향키로 돌아다닐 수 있는 맵 화면 (맵에 있는 미니게임에 들어가 미니게임 플레이 가능)
        InGame,         //각각의 미니게임을 플레이할 때 상태
        Pause,          //미니게임 플레이 중 잠깐 일시정지한 상태
        GameOver        //미니게임 종료
    }

    public class GameManager : Singleton<GameManager>
    {
        private GameState currentGameState;
        public GameState CurrentGameState
        {
            get { return currentGameState; }
            set
            {
                if (currentGameState == value) return;

                currentGameState = value;
            }
        }

        public void Start()
        {
            currentGameState = GameState.Main;
        }
    }
}
