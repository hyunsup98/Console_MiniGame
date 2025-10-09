using System;
using System.Text;
namespace Console_Portfolio
{
    public class Title
    {
        //타이틀 아스키 코드
        private static string title = "#     #             #####                      \r\n##   ## # #    # # #     #   ##   #    # ######\r\n# # # # # ##   # # #        #  #  ##  ## #     \r\n#  #  # # # #  # # #  #### #    # # ## # ##### \r\n#     # # #  # # # #     # ###### #    # #     \r\n#     # # #   ## # #     # #    # #    # #     \r\n#     # # #    # #  #####  #    # #    # ###### ";
        //title을 '\n' 기준으로 나눠 담는 타이틀 배열
        private static string[] titles;

        private bool isShow_PressButton = true;

        public Title()
        {
            titles = title.Split('\n');     //타이틀 아스키아트를 문자열 배열에 저장
        }

        //초기화
        public void Awake()
        {
            GameManager.Instance.action_StateTitle += TitleScreen;
        }

        #region 타이틀 관리
        private void TitleScreen()
        {
            Console.Clear();
            Console.CursorVisible = false;

            #region 메인화면 아웃라인(테두리)

            //문자를 한 번에 제출하기 위해 StringBuilder에 저장 후 출력
            //문자열이 계속 수정되기 때문에 StringBuilder 사용
            OutputManager.PrintOutline();
            
            #endregion

            //타이틀 아스키아트 출력
            titles.WriteMiddle(5);
        }

        public void TogglePressButton()
        {
            var startMsg = isShow_PressButton ? "                " : "Press Any Button";
            isShow_PressButton = !isShow_PressButton;
            startMsg.WriteMiddle(40);
        }
        #endregion
    }
}
