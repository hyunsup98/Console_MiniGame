using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Console_Portfolio
{
    internal class Program
    {
        public static Stopwatch Time = new Stopwatch();         //프레임 제어를 위한 Stopwatch

        //타이틀 아스키 코드
        private static string title = "#     #             #####                      \r\n##   ## # #    # # #     #   ##   #    # ######\r\n# # # # # ##   # # #        #  #  ##  ## #     \r\n#  #  # # # #  # # #  #### #    # # ## # ##### \r\n#     # # #  # # # #     # ###### #    # #     \r\n#     # # #   ## # #     # #    # #    # #     \r\n#     # # #    # #  #####  #    # #    # ######";
        //title을 '\n' 기준으로 나눠 담는 타이틀 배열
        private static string[] titles;

        static void Main(string[] args)
        {
            //프로그램 실행 시 1회 실행
            Start();

            //매 프레임 (0.1초 마다) 실행
            while (true)
            {
                Update();

            }
        }

        #region 실행 흐름 메서드
        //게임 실행시 초기화 메서드
        private static void Start()
        {
            #region 외부 클래스들 Start 메서드 및 초기화

            GameManager.Instance.Start();

            #endregion


            Time.Start();       //프레임 재생 시작
            SetWindowInit();    //콘솔창 관련 초기화 (크기 지정 등)

            titles = title.Split('\n');

            SoundManager.Instance.PlayLoop("ChillLofiR.mp3", 0.7f);

            MainMenu();
        }

        //매 프레임 실행되는 메서드
        private static void Update()
        {
            //0.1초마다 프레임 갱신
            if (Time.ElapsedMilliseconds >= 1000)
            {
                Time.Restart();

                if (GameManager.Instance.CurrentGameState == GameState.Main)
                {
                    MainMenu();
                }
            }
        }
        #endregion

        //콘솔창 초기화
        private static void SetWindowInit()
        {
            Console.SetWindowSize(120, 50);

        }

        #region 메인 메뉴 관리
        private static void MainMenu()
        {
            Console.CursorVisible = false;
            #region 메인화면 아웃라인(테두리)
            //중단의 아웃라인 그리기
            for (int i = 0; i < Console.WindowHeight - 1; i++)
            {
                //어차피 커서의 x축은 항상 0부터 시작하기 때문에 0으로 고정

                for (int j = 0; j < Console.WindowWidth - 1; j++)
                {
                    Console.SetCursorPosition(j, i);

                    //상단부 아웃라인 그리기
                    if (i == 0)
                    {
                        if (j == 0)
                            Console.Write("┏");
                        else if (j == Console.WindowWidth - 2)
                            Console.Write("┓");
                        else
                            Console.Write("━");
                    }

                    //중단부 아웃라인 그리기
                    if (i != 0 && i != Console.WindowHeight - 2)
                        if (j == 0 || j == Console.WindowWidth - 2)
                            Console.Write("┃");

                    //하단부 아웃라인 그리기
                    if(i == Console.WindowHeight - 2)
                    {
                        if (j == 0)
                            Console.Write("┗");
                        else if (j == Console.WindowWidth - 2)
                            Console.Write("┛");
                        else
                            Console.Write("━");
                    }
                }
            }
            #endregion

            //타이틀 출력
            if(titles.Length != 0)
            {
                var stringHalfLength = titles[0].Length / 2;        //콘솔창 가로 길이 - 문자열의 절반 길이 → 타이틀을 정가운데 배치하기 위함
                var height = 5;                                     //타이틀 높이 지정

                for(int i = height; i < titles.Length + height; i++)
                {
                    Console.SetCursorPosition((Console.WindowWidth / 2) - stringHalfLength, i);

                    Console.Write(titles[i - height]);
                }
            }

        }
        #endregion
    }
}
