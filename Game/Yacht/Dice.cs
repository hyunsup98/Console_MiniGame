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

        public bool isSelected { get; set; }    //true면 선택된거라 리롤시 돌리지 않음
    }
}
