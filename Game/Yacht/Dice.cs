using System;

namespace Console_Portfolio.Game.Yacht
{
    public class Dice
    {
        private int number;     //주사위의 숫자 정보
        public int Number
        {
            get { return number; }
            set
            {
                if (value < 1)
                    value = 1;
                else if (value > 6)
                    value = 6;

                number = value;
            }
        }

        private bool isSelected;    //true면 선택된거라 리롤시 돌리지 않음
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if(value == true)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                isSelected = value;
                Console.SetCursorPosition(numberPosX, numberPosY);
                Console.Write(number);
                Console.ResetColor();
            }
        }

        public int numberPosX { get; set; }     //주사위 숫자가 보여질 console.setcursorposition X 좌표, 선택 시 빨간색으로 덮어주기 위함
        public int numberPosY { get; set; }     //주사위 숫자가 보여질 console.setcursorposition Y 좌표

        //점수를 넣고 나서 주사위 초기화
        public void DiceInit()
        {
            isSelected = false;
        }
    }
}
