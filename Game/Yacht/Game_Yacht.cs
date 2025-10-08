using System;
using System.Collections.Generic;

namespace Console_Portfolio.Game.Yacht
{
    //각각의 스코어를 저장할 클래스
    public class YachtScoreBoard
    {
        public int aces = 0;
        public int deuces = 0;
        public int threes = 0;
        public int fours = 0;
        public int fives = 0;
        public int sixes = 0;
        public int bonus = 0;

        public int threeOfAKind = 0;
        public int fourOfAKind = 0;
        public int fullHouse = 0;
        public int smallStraight = 0;
        public int largeStraight = 0;
        public int yacht = 0;
        public int choice = 0;

        public int totalScore = 0;
    }


    public class Game_Yacht
    {
        const int DICE_COUNT = 5;    //주사위 개수 (기본 5개)

        Random random = new Random();

        #region 주사위 이미지 문자열
        //주사위의 숫자를 이미지 형식으로 보여주기 위한 문자열
        private string[] numbers =
        {
            "□□□\n" +
            "□■□\n" +
            "□□□",

            "□□■\n" +
            "□□□\n" +
            "■□□",

            "□□■\n" +
            "□■□\n" +
            "■□□",

            "■□■\n" +
            "□□□\n" +
            "■□■",

            "■□■\n" +
            "□■□\n" +
            "■□■",

            "■□■\n" +
            "■□■\n" +
            "■□■",
        };
        #endregion

        private List<Dice> dices = new List<Dice>();    //DICE_COUNT

        private YachtScoreBoard player1;
        private YachtScoreBoard player2;



        //Aces
        //Deuces
        //Threes
        //Fours
        //Fives
        //Sixes
        //Bonus

        //3 of a Kind
        //4 of a Kind
        //Full House
        //S. Straight
        //L. Straight
        //Yacht
        //Choice


        //생성자

        public Game_Yacht()
        {
            for(int i = 0; i < DICE_COUNT; i++)
            {
                Dice dice = new Dice();

                dices.Add(dice);
            }
        }

        /// <summary>
        /// 점수판 출력 메서드
        /// </summary>
        public void ScoreBoard()
        {
            Console.SetCursorPosition(3, 3);

            Console.Write(new string('┌', 1));
            Console.Write(new string('─', 15));
            Console.Write(new string('┬', 1));
            Console.Write(new string('─', 10));
            Console.Write(new string('┬', 1));
            Console.Write(new string('─', 10));
            Console.Write(new string('┐', 1));



        }

        /// <summary>
        /// 주사위 리롤 메서드
        /// </summary>
        public void Reroll_Dice()
        {
            //주사위 출력
            Console.ForegroundColor = ConsoleColor.Blue;
            var string_Dice = "Dice";
            string_Dice.WriteMiddle(2);
            Console.ResetColor();

            //각각의 주사위 이미지 출력할 기본 좌표
            int startX = 41;
            int startY = 4;

            for(int i = 0; i < dices.Count; i++)
            {
                int num = random.Next(1, 7);
                dices[i].Number = num;

                var strings = numbers[num - 1].Split('\n');

                for (int j = 0; j < strings.Length; j++)
                {
                    Console.SetCursorPosition(startX + (i * 8), startY + j);
                    Console.Write(strings[j]);
                }
            }
        }


    }
}
