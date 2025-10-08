using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Console_Portfolio
{
    /// <summary>
    /// 콘솔창에 문자열 출력 관련 유틸 클래스
    /// </summary>
    public static class OutputManager
    {
        private static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 인자로 받은 문자열을 콘솔 중앙에 출력하는 확장 메서드
        /// </summary>
        /// <param name="str"> 출력할 문자열 </param>
        /// <param name="height"> 높이: 몇 번째 줄에 출력될지 </param>
        public static void WriteMiddle(this string str, int height)
        {
            if (string.IsNullOrEmpty(str)) return;

            var strHalfLength = (Console.WindowWidth / 2) - (str.Length / 2); //콘솔창 절반 길이 - 문자열 절반 길이 = 문자열이 중앙에 출력됨

            Console.SetCursorPosition(strHalfLength, height);
            Console.Write(str);
        }

        public static void WriteMiddle(this string[] strs, int height)
        {
            if (strs.Length == 0) return;

            for(int i = 0; i < strs.Length; i++)
            {
                var strHalfLength = (Console.WindowWidth / 2) - (strs[i].Length / 2);

                Console.SetCursorPosition(strHalfLength, height + i);
                Console.Write(strs[i]);
            }
        }

        /// <summary>
        /// 테두리를 그려주는 메서드
        /// </summary>
        public static void PrintOutline()
        {
            //문자를 한 번에 제출하기 위해 StringBuilder에 저장 후 출력
            //문자열이 계속 수정되기 때문에 StringBuilder 사용

            for (int i = 0; i < Console.WindowHeight - 1; i++)
            {
                //어차피 커서의 x축은 항상 0부터 시작하기 때문에 0으로 고정
                for (int j = 0; j < Console.WindowWidth - 1; j++)
                {
                    if (i == 0)
                    {
                        //상단부 아웃라인 그리기
                        if (j == 0)
                            sb.Append("┏");
                        else if (j == Console.WindowWidth - 2)
                            sb.Append("┓");
                        else
                            sb.Append("━");
                    }
                    else if (i == Console.WindowHeight - 2)
                    {
                        //하단부 아웃라인 그리기
                        if (j == 0)
                            sb.Append("┗");
                        else if (j == Console.WindowWidth - 2)
                            sb.Append("┛");
                        else
                            sb.Append("━");
                    }
                    else
                    {
                        //중단부 아웃라인 그리기
                        if (j == 0 || j == Console.WindowWidth - 2)
                            sb.Append("┃");
                        else
                            sb.Append(" ");
                    }
                }
                sb.AppendLine();
            }

            Console.Write(sb);
        }

        /// <summary>
        /// 테두리를 제외한 안의 내용을 지워주는 메서드
        /// </summary>
        public static void ConsoleClear()
        {
            for (int i = 1; i < Console.WindowHeight -2; i++)
            {
                Console.SetCursorPosition(2, i);

                Console.Write(new string(' ', Console.WindowWidth - 5));
            }
        }
    }
}
