using Console_Portfolio.Game.Yacht;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Console_Portfolio
{
    internal class Program
    {
        //콘솔창이 변하는 기능은 예정에 없기 때문에 상수로 선언
        private const int windowWidth = 120;
        private const int windowHeight = 50;

        public static Stopwatch time = new Stopwatch();         //업데이트 메서드에 쓰일 프레임 제어를 위한 Stopwatch (0.1초)
        public static Stopwatch time2 = new Stopwatch();        //따로 UI 효과를 주기 위한 Stopwatch (0.5초);

        #region 전역이 아닌 클래스들 선언 구역
        public static Title title = new Title();
        public static Game_Yacht yacht = new Game_Yacht();
        
        #endregion

        static void Main(string[] args)
        {
            //프로그램 실행 시 1회 실행
            Awake();
            Start();

            //매 프레임 (0.1초 마다) 실행
            while (true)
            {
                Update();

            }
        }

        //유니티의 Awake 메서드처럼 동작하기 위해 만듬
        private static void Awake()
        {
            SetWindowInit();    //콘솔창 관련 초기화 (크기 지정 등)

            #region 외부 클래스들 Awake 메서드 및 초기화

            title.Awake();

            #endregion
        }

        #region 실행 흐름 메서드
        //게임 실행시 초기화 메서드
        private static void Start()
        {
            time.Start();       //프레임 재생 시작
            time2.Start();

            SoundManager.Instance.PlayLoop("ChillLofiR.mp3", 0.7f);

            #region 외부 클래스들 Start 메서드 및 초기화

            GameManager.Instance.Start();

            #endregion

        }

        //매 프레임 실행되는 메서드
        private static void Update()
        {
            //0.1초마다 프레임 갱신
            if (time.ElapsedMilliseconds >= 100)
            {
                time.Restart();

                //게임 상태가 타이틀일때
                if (GameManager.Instance.CurrentGameState == GameState.Title)
                {
                    //타이틀 화면일 때 실행
                    if (time2.ElapsedMilliseconds >= 500)
                    {
                        time2.Restart();
                        title.TogglePressButton();
                    }

                    //키 입력 여부만 받아야 계속 반복 실행 가능 / 블로킹 방지
                    if (Console.KeyAvailable)
                    {
                        //아무키나 누르면
                        Console.ReadKey(true);
                        //마을로 이동
                        GameManager.Instance.CurrentGameState = GameState.InGame;
                        OutputManager.ConsoleClear();
                    }
                }

                //게임 상태가 인게임일 때
                if (GameManager.Instance.CurrentGameState == GameState.InGame)
                {
                    yacht.YachtGame();
                }
            }
        }
        #endregion

        //콘솔창 초기화
        private static void SetWindowInit()
        {
            Console.SetWindowSize(windowWidth, windowHeight);
        }
    }
}
