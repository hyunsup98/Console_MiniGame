using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Portfolio.Game.Yacht
{
    /// <summary>
    /// 각각의 스코어를 저장할 클래스
    /// 플레이어가 0점을 기록한 것과, 아무것도 기록하지 않은 상태를 구분하기 위해 nullable 사용
    /// </summary>
    public class YachtScoreBoard
    {
        public int? ones = null;
        public int? twos = null;
        public int? threes = null;
        public int? fours = null;
        public int? fives = null;
        public int? sixes = null;

        public int? threeOfAKind = null;
        public int? fourOfAKind = null;
        public int? fullHouse = null;
        public int? smallStraight = null;
        public int? largeStraight = null;
        public int? yahtzee = null;
        public int? chance = null;

        //점수 초기화
        public void YachtScoreInit()
        {
            ones = null;
            twos = null;
            threes = null;
            fours = null;
            fives = null;
            sixes = null;

            threeOfAKind = null;
            fourOfAKind = null;
            fullHouse = null;
            smallStraight = null;
            largeStraight = null;
            yahtzee = null;
            chance = null;
        }

        //보너스 점수 계산 및 반환
        public int GetBonusScore()
        {
            var sum = (ones ?? 0) + (twos ?? 0) + (threes ?? 0) + (fours ?? 0) + (fives ?? 0) + (sixes ?? 0);

            return sum >= 63 ? 35 : 0;
        }

        //총합을 계산 및 반환
        public int GetSumScore()
        {
            var sum = (ones ?? 0) + (twos ?? 0) + (threes ?? 0) + (fours ?? 0) + (fives ?? 0) + (sixes ?? 0) + (threeOfAKind ?? 0) + (fourOfAKind ?? 0) +
                (fullHouse ?? 0) + (smallStraight ?? 0) + (largeStraight ?? 0) + (yahtzee ?? 0) + (chance ?? 0) + GetBonusScore();

            return sum;
        }
    }
}
