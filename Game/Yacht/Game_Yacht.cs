using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Console_Portfolio.Game.Yacht
{
    public enum Yacht_State
    {
        Init,
        Playing,
        GameOver
    }

    public class Game_Yacht
    {
        private const int DICE_COUNT = 5;       //주사위 개수 (기본 5개)
        private const int MAXTURN = 13;         //최대 턴 수
        private Yacht_State yachtState = Yacht_State.Init;

        private int currentTurn = 0;
        public int CurrentTurn
        {
            get { return currentTurn; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value >= MAXTURN)
                    GameOver();

                currentTurn = value;
            }
        }

        private Random random = new Random();

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

        //족보 리스트, 처음은 족보임을 알려주기 위한 문자열 = 족보 리스트에 해당 x
        private string[] categories =
        {
            "족보" ,"Ones", "Twos", "Threes", "Fours", "Fives", "Sixes", "Bonus +35", "Three of a Kind", "Four of a Kind", "Full House",
            "Small Straight", "Large Straight", "Yahtzee", "Chance", "Sum"
        };

        //플레이어 점수판 플레이어 1, 2
        private YachtScoreBoard p1 = new YachtScoreBoard();
        private YachtScoreBoard p2 = new YachtScoreBoard();

        private bool isP1Turn = true;       //true면 p1이 할 차례

        private int selectCategoriesIndex;  //어떤 족보를 선택할지 시각적으로 보여주는 인덱스
        public int SelectCategoriesIndex
        {
            get { return selectCategoriesIndex; }
            set
            {
                if (value == 0)
                    value = 13;
                else if (value > 13)
                    value = 1;

                selectCategoriesIndex = value;
                SelectCategories();
            }
        }

        private int prevSelectPosY = 0;

        int[] counts = new int[7];          //주사위의 눈금 별로 카운팅하는 배열

        private int rerollCount = 2;        //주사위 리롤 횟수
        public int RerollCount
        {
            get { return rerollCount; }
            set
            {
                if (value >= 0)
                {
                    rerollCount = value;
                    Reroll_Dice();
                }
                else
                {
                    rerollCount = value;
                }
            }
        }

        //생성자

        public Game_Yacht()
        {
            for (int i = 0; i < DICE_COUNT; i++)
            {
                Dice dice = new Dice();

                dices.Add(dice);
            }
        }

        //게임 데이터 초기화 (게임 시작, 재시작 시에만 해줌)
        public void Yacht_Init()
        {
            rerollCount = 2;
            selectCategoriesIndex = 1;
            prevSelectPosY = 1;
            CurrentTurn = 0;
            isP1Turn = true;

            //점수 초기화
            p1.YachtScoreInit();
            p2.YachtScoreInit();

            //주사위 초기화
            foreach (var dice in dices)
            {
                dice.DiceInit();
            }

            yachtState = Yacht_State.Init;
        }

        //야추 게임 시작 메서드
        public void YachtGame()
        {
            while (true)
            {
                //처음 시작 상태
                if (yachtState == Yacht_State.Init)
                {
                    Yacht_Init();
                    Reroll_Dice();
                    SelectCategories();
                    yachtState = Yacht_State.Playing;
                }

                //플레이 중인 상태
                if (yachtState == Yacht_State.Playing)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);

                        switch (key.Key)
                        {
                            case ConsoleKey.R:
                                RerollCount--;
                                SoundManager.Instance.PlayEffect("Abstract1.mp3", 0.7f);
                                break;

                            case ConsoleKey.D1:
                                dices[0].IsSelected = !dices[0].IsSelected;
                                break;

                            case ConsoleKey.D2:
                                dices[1].IsSelected = !dices[1].IsSelected;
                                break;

                            case ConsoleKey.D3:
                                dices[2].IsSelected = !dices[2].IsSelected;
                                break;

                            case ConsoleKey.D4:
                                dices[3].IsSelected = !dices[3].IsSelected;
                                break;

                            case ConsoleKey.D5:
                                dices[4].IsSelected = !dices[4].IsSelected;
                                break;

                            case ConsoleKey.UpArrow:
                                SelectCategoriesIndex--;
                                break;

                            case ConsoleKey.DownArrow:
                                SelectCategoriesIndex++;
                                break;

                            case ConsoleKey.Enter:
                                AddScore();
                                break;
                        }
                    }
                }

                //게임오버 상태
                if (yachtState == Yacht_State.GameOver)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);

                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                GameManager.Instance.CurrentGameState = GameState.Title;
                                Yacht_Init();
                                return;

                            case ConsoleKey.D2:
                                Yacht_Init();
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 점수판 출력 메서드
        /// </summary>
        public void ScoreBoard()
        {
            int row = categories.Length * 2 + 1;    //스코어보드에 필요한 줄수

            int boardPosX = 13;      //점수판 x좌표
            int boardPosY = 13;     //점수판 y좌표

            int sectionSize = 20;
            int playerBoardSize = 10;

            string[] sections =
            {
                "=======", "ones", "twos", "threes", "fours", "fives", "sixes", "bonus"
            };

            var textScoreBoard = "점수판";
            Console.SetCursorPosition(boardPosX + (21 - OutputManager.StringCount(textScoreBoard) / 2), boardPosY - 2);
            OutputManager.WriteColor(textScoreBoard, ConsoleColor.Green);
            Console.Write($" 현재 턴 {CurrentTurn + 1}/{MAXTURN}");

            //점수판 테두리 그리기
            for (int i = 0; i < row; i++)
            {
                Console.SetCursorPosition(boardPosX, boardPosY + i);

                if (i == 0)
                {
                    Console.Write("┌" + new string('─', sectionSize) +
                        "┬" + new string('─', playerBoardSize) +
                        "┬" + new string('─', playerBoardSize) + "┐");
                }
                else if (i == row - 1)
                {
                    Console.Write("└" + new string('─', sectionSize) +
                        "┴" + new string('─', playerBoardSize) +
                        "┴" + new string('─', playerBoardSize) + "┘");
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        Console.Write("│" + new string('─', sectionSize) +
                        "┼" + new string('─', playerBoardSize) +
                        "┼" + new string('─', playerBoardSize) + "│");
                    }
                    else
                    {
                        Console.Write("│" + new string(' ', sectionSize) +
                        "│" + new string(' ', playerBoardSize) +
                        "│" + new string(' ', playerBoardSize) + "│");
                    }
                }
            }

            //"p1", "p2" 문자 출력
            Console.SetCursorPosition(boardPosX + sectionSize + 2 + (playerBoardSize / 2 - 1), boardPosY + 1);
            Console.Write("P1");
            Console.SetCursorPosition(boardPosX + sectionSize + playerBoardSize + 3 + (playerBoardSize / 2 - 1), boardPosY + 1);
            Console.Write("P2");

            int ascii = 97;

            //족보 리스트 이름 문자 출력
            for (int i = 0; i < categories.Length; i++)
            {
                string str = categories[i];
                int x = boardPosX + 1 + (sectionSize / 2 - OutputManager.StringCount(str) / 2);
                int y = boardPosY + (i * 2 + 1);

                //각 족보의 번호 출력
                if (i != 0 && i != 7 && i != 15)
                {
                    Console.SetCursorPosition(boardPosX - 5, y);
                    Console.Write($"{(char)ascii++})");
                }

                //각 족보 이름 출력
                Console.SetCursorPosition(x, y);
                Console.Write(str);
            }

            PlayerScoreBoard(p1, boardPosX + sectionSize + 2 + (playerBoardSize / 2 - 1), boardPosY + 3);
            PlayerScoreBoard(p2, boardPosX + sectionSize + playerBoardSize + 3 + (playerBoardSize / 2 - 1), boardPosY + 3);

            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// 점수판에 플레이어 점수 출력하기
        /// </summary>
        private void PlayerScoreBoard(YachtScoreBoard player, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            WritePlayerScore(player.ones);

            Console.SetCursorPosition(x, y + 2);
            WritePlayerScore(player.twos);

            Console.SetCursorPosition(x, y + 4);
            WritePlayerScore(player.threes);

            Console.SetCursorPosition(x, y + 6);
            WritePlayerScore(player.fours);

            Console.SetCursorPosition(x, y + 8);
            WritePlayerScore(player.fives);

            Console.SetCursorPosition(x, y + 10);
            WritePlayerScore(player.sixes);

            Console.SetCursorPosition(x, y + 12);
            var sum = (player.ones ?? 0) + (player.twos ?? 0) + (player.threes ?? 0) + (player.fours ?? 0) + (player.fives ?? 0) + (player.sixes ?? 0);
            OutputManager.WriteColor($"{sum}/63", ConsoleColor.Blue);

            Console.SetCursorPosition(x, y + 14);
            WritePlayerScore(player.threeOfAKind);

            Console.SetCursorPosition(x, y + 16);
            WritePlayerScore(player.fourOfAKind);

            Console.SetCursorPosition(x, y + 18);
            WritePlayerScore(player.fullHouse);

            Console.SetCursorPosition(x, y + 20);
            WritePlayerScore(player.smallStraight);

            Console.SetCursorPosition(x, y + 22);
            WritePlayerScore(player.largeStraight);

            Console.SetCursorPosition(x, y + 24);
            WritePlayerScore(player.yahtzee);

            Console.SetCursorPosition(x, y + 26);
            WritePlayerScore(player.chance);

            Console.SetCursorPosition(x, y + 28);
            OutputManager.WriteColor(player.GetSumScore().ToString(), ConsoleColor.Blue);
        }

        /// <summary>
        /// 해당 족보 변수에 값이 있으면 초록색으로 출력, 없으면 하얀색으로 0 출력
        /// </summary>
        /// <param name="category"></param>
        private void WritePlayerScore(int? category)
        {
            if (category.HasValue)
            {
                OutputManager.WriteColor(category.ToString(), ConsoleColor.Green);
            }
            else
            {
                Console.Write("0");
            }
        }

        /// <summary>
        /// 주사위 리롤 메서드
        /// </summary>
        public void Reroll_Dice()
        {
            OutputManager.ConsoleClear();

            //주사위 출력
            Console.ForegroundColor = ConsoleColor.Blue;
            var string_Dice = "주사위";
            string_Dice.WriteMiddle(2);
            Console.ResetColor();

            Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
            OutputManager.WriteColor($"(리롤 횟수: {RerollCount})", ConsoleColor.Green);

            //각각의 주사위 이미지 출력할 기본 좌표
            int startX = 41;
            int startY = 4;

            for (int i = 0; i < dices.Count; i++)
            {
                //선택된 주사위를 continue로 무시하니 겹침 현상 발생 -> 선택되도 다시 출력

                if (!dices[i].IsSelected)
                {
                    //선택 안된 주사위만 새로 눈 갱신
                    int num = random.Next(1, 7);
                    dices[i].Number = num;
                }

                var strings = numbers[dices[i].Number - 1].Split('\n');

                for (int j = 0; j < strings.Length; j++)
                {
                    Console.SetCursorPosition(startX + (i * 8), startY + j);
                    Console.Write(strings[j]);
                }

                Console.SetCursorPosition(startX + (i * 8) + 2, startY + 4);
                dices[i].numberPosX = startX + (i * 8) + 2;
                dices[i].numberPosY = startY + 4;
                dices[i].IsSelected = dices[i].IsSelected;
            }

            //dices[i] 번째 주사위의 number에 해당하는 counts 배열 인덱스에 +1 해줌
            Array.Clear(counts, 0, counts.Length);
            for (int i = 0; i < dices.Count; i++)
            {
                counts[dices[i].Number]++;
            }

            ScoreBoard();
            SelectCategories();
            ShowScoreList();
        }

        /// <summary>
        /// 점수를 추가하는 메서드
        /// </summary>
        private void AddScore()
        {
            YachtScoreBoard player;

            if (isP1Turn)
                player = p1;
            else
                player = p2;

            //해당 족보 변수에 이미 점수가 저장되어 있으면 false로 바꿈
            bool isEmpty = true;

            //현재 카테고리 인덱스에 따라 맞는 족보에 점수 넣어주기
            switch (selectCategoriesIndex)
            {
                case 1:
                    isEmpty = CheckAlreadyScore(ref player.ones, ScoreAddNumber(counts, 1));
                    break;

                case 2:
                    isEmpty = CheckAlreadyScore(ref player.twos, ScoreAddNumber(counts, 2));
                    break;

                case 3:
                    isEmpty = CheckAlreadyScore(ref player.threes, ScoreAddNumber(counts, 3));
                    break;

                case 4:
                    isEmpty = CheckAlreadyScore(ref player.fours, ScoreAddNumber(counts, 4));
                    break;

                case 5:
                    isEmpty = CheckAlreadyScore(ref player.fives, ScoreAddNumber(counts, 5));
                    break;

                case 6:
                    isEmpty = CheckAlreadyScore(ref player.sixes, ScoreAddNumber(counts, 6));
                    break;

                case 7:
                    isEmpty = CheckAlreadyScore(ref player.threeOfAKind, ScoreThreeOfAKind(counts));
                    break;

                case 8:
                    isEmpty = CheckAlreadyScore(ref player.fourOfAKind, ScoreFourOfAKind(counts));
                    break;

                case 9:
                    isEmpty = CheckAlreadyScore(ref player.fullHouse, ScoreFullHouse(counts));
                    break;

                case 10:
                    isEmpty = CheckAlreadyScore(ref player.smallStraight, ScoreSmallStraight(counts));
                    break;

                case 11:
                    isEmpty = CheckAlreadyScore(ref player.largeStraight, ScoreLargeStraight(counts));
                    break;

                //야찌는 따로 관리

                case 13:
                    isEmpty = CheckAlreadyScore(ref player.chance, ScoreChance(counts));
                    break;

                default:
                    break;
            }

            //야찌 족보에 넣으면
            if (SelectCategoriesIndex == 12)
            {
                if (player.yahtzee.HasValue)
                {
                    //야찌 족보에 이미 값이 있을 때
                    if (player.yahtzee > 0)
                    {
                        //야찌 점수를 먹었을 경우 50점 추가 + 한 턴 더 하기 / 0점이 들어가있으면 그냥 못넣음
                        player.yahtzee += 50;
                        SoundManager.Instance.PlayEffect("Modern1.mp3", 0.5f);
                        NextTurn(isP1Turn);
                        return;
                    }
                    else
                    {
                        SoundManager.Instance.PlayEffect("Retro5.mp3", 0.5f);
                        return;
                    }
                }
                else
                {
                    //야찌 족보에 값이 없으면
                    player.yahtzee = ScoreYahtzee(counts);
                    SoundManager.Instance.PlayEffect("Modern1.mp3", 0.5f);
                }
            }

            if (!isEmpty) return;

            //정상적으로 점수를 넣었을 경우 다음 플레이어 턴으로 넘김
            NextTurn(!isP1Turn);
        }

        //다음 턴으로 넘김
        private void NextTurn(bool isP1Turn)
        {
            if (this.isP1Turn == false && isP1Turn == true)
                CurrentTurn++;

            if (yachtState == Yacht_State.GameOver) return;

            foreach (var dice in dices)
            {
                dice.DiceInit();
            }

            rerollCount = 2;
            selectCategoriesIndex = 1;
            prevSelectPosY = 1;
            this.isP1Turn = isP1Turn;
            Reroll_Dice();
        }

        private void GameOver()
        {
            int sizeX = 60;
            int sizeY = 15;

            for (int i = 0; i < sizeY; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth / 2) - (sizeX / 2), (Console.WindowHeight / 2) - (sizeY / 2) + i);

                if (i == 0)
                    Console.Write("┌" + new string('─', sizeX) + "┐");
                else if (i == sizeY - 1)
                    Console.Write("└" + new string('─', sizeX) + "┘");
                else
                    Console.Write("│" + new string(' ', sizeX) + "│");
            }

            int p1Score = p1.GetSumScore();
            int p2Score = p2.GetSumScore();

            string victoryMsg = string.Empty;

            if (p1Score > p2Score)
                victoryMsg = "플레이어 1이 이겼습니다!";
            else if (p1Score == p2Score)
                victoryMsg = "비겼습니다!";
            else
                victoryMsg = "플레이어 2가 이겼습니다!";

            Console.ForegroundColor = ConsoleColor.Green;
            OutputManager.WriteMiddle(victoryMsg, 20);
            Console.ResetColor();

            OutputManager.WriteMiddle($"플레이어 1 점수: {p1Score}", 23);
            OutputManager.WriteMiddle($"플레이어 2 점수: {p2Score}", 25);
            OutputManager.WriteMiddle($"1: 타이틀 화면으로 돌아가기   2: 재시작하기", 30);

            yachtState = Yacht_State.GameOver;
        }

        //플레이어의 해당 족보 점수 변수에 값이 있으면 패스, 없으면 점수를 넣어주는 메서드
        private bool CheckAlreadyScore(ref int? category, int score)
        {
            if (category.HasValue)
            {
                SoundManager.Instance.PlayEffect("Retro5.mp3", 0.5f);
                return false;
            }
            else
            {
                category = score;
                SoundManager.Instance.PlayEffect("Modern1.mp3", 0.5f);
                return true;
            }
        }

        /// <summary>
        /// 굴려진 주사위의 눈금을 기준으로 각각의 족보 점수 계산 및 출력 메서드
        /// </summary>
        private void ShowScoreList()
        {
            int listPosX = 75;
            int listPosY = 14;

            Console.SetCursorPosition(listPosX, listPosY - 2);

            OutputManager.WriteColor(isP1Turn ? "< p1 차례 >" : "< p2 차례 >", ConsoleColor.Green);

            #region 각 족보 점수 출력
            Console.SetCursorPosition(listPosX, listPosY);
            Console.Write($"a) Ones -> {ScoreAddNumber(counts, 1)}");

            Console.SetCursorPosition(listPosX, listPosY + 2);
            Console.Write($"b) Twos -> {ScoreAddNumber(counts, 2)}");

            Console.SetCursorPosition(listPosX, listPosY + 4);
            Console.Write($"c) Threes -> {ScoreAddNumber(counts, 3)}");

            Console.SetCursorPosition(listPosX, listPosY + 6);
            Console.Write($"d) Fours -> {ScoreAddNumber(counts, 4)}");

            Console.SetCursorPosition(listPosX, listPosY + 8);
            Console.Write($"e) Fives -> {ScoreAddNumber(counts, 5)}");

            Console.SetCursorPosition(listPosX, listPosY + 10);
            Console.Write($"f) Sixes -> {ScoreAddNumber(counts, 6)}");

            Console.SetCursorPosition(listPosX, listPosY + 12);
            Console.Write($"g) Three of a Kind -> {ScoreThreeOfAKind(counts)}");

            Console.SetCursorPosition(listPosX, listPosY + 14);
            Console.Write($"h) Four of a Kind -> {ScoreFourOfAKind(counts)}");

            Console.SetCursorPosition(listPosX, listPosY + 16);
            Console.Write($"i) Full House -> {ScoreFullHouse(counts)}");

            Console.SetCursorPosition(listPosX, listPosY + 18);
            Console.Write($"j) Small Straight -> {ScoreSmallStraight(counts)}");

            Console.SetCursorPosition(listPosX, listPosY + 20);
            Console.Write($"k) Large Straight -> {ScoreLargeStraight(counts)}");

            Console.SetCursorPosition(listPosX, listPosY + 22);
            Console.Write($"l) Yahtzee -> {ScoreYahtzee(counts)}");

            Console.SetCursorPosition(listPosX, listPosY + 24);
            Console.Write($"m) Chance -> {ScoreChance(counts)}");
            #endregion
        }

        /// <summary>
        /// 어떤 족보에 점수를 넣을지 선택하는 커서 메서드
        /// </summary>
        private void SelectCategories()
        {
            int listPosX = 70;
            int listPosY = 14;

            if (prevSelectPosY != 0)
            {
                Console.SetCursorPosition(listPosX, prevSelectPosY);
                Console.Write(new string(' ', 2));
            }

            prevSelectPosY = listPosY + ((SelectCategoriesIndex - 1) * 2);
            Console.SetCursorPosition(listPosX, prevSelectPosY);
            Console.Write(new string('▶', 1));
        }

        #region 족보 계산 메서드
        //Ones ~ Sixes 까지 계산
        private int ScoreAddNumber(int[] dice, int num)
        {
            int sum = 0;

            for (int i = 1; i < dice.Length; i++)
            {
                if (i == num)
                    sum = i * dice[i];
            }

            return sum;
        }

        //Three of a Kind 계산
        private int ScoreThreeOfAKind(int[] dice)
        {
            int sum = 0;
            bool isThreeofaKind = false;

            for (int i = 1; i < dice.Length; i++)
            {
                sum += i * dice[i];
                if (dice[i] >= 3)
                    isThreeofaKind = true;
            }

            return isThreeofaKind ? sum : 0;
        }

        //Four of a Kind 계산
        private int ScoreFourOfAKind(int[] dice)
        {
            int sum = 0;
            bool isFourOfaKind = false;

            for (int i = 1; i < dice.Length; i++)
            {
                sum += i * dice[i];
                if (dice[i] >= 4)
                    isFourOfaKind = true;
            }

            return isFourOfaKind ? sum : 0;
        }

        //Full House 계산
        private int ScoreFullHouse(int[] dice)
        {
            bool isTwo = false;
            bool isThree = false;

            for (int i = 1; i < dice.Length; i++)
            {
                if (dice[i] == 2)
                    isTwo = true;

                if (dice[i] == 3)
                    isThree = true;
            }

            return isTwo && isThree ? 25 : 0;
        }

        //Small Straight 계산
        private int ScoreSmallStraight(int[] dice)
        {
            bool[] isBeing = new bool[dice.Length];

            for (int i = 1; i < dice.Length; i++)
            {
                if (dice[i] >= 1)
                    isBeing[i] = true;
            }

            if ((isBeing[1] && isBeing[2] && isBeing[3] && isBeing[4]) ||
                (isBeing[2] && isBeing[3] && isBeing[4] && isBeing[5]) ||
                (isBeing[3] && isBeing[4] && isBeing[5] && isBeing[6]))
                return 30;

            return 0;
        }

        //Large Straight 계산
        private int ScoreLargeStraight(int[] dice)
        {
            bool[] isBeing = new bool[dice.Length];

            for (int i = 1; i < dice.Length; i++)
            {
                if (dice[i] >= 1)
                    isBeing[i] = true;
            }

            if ((isBeing[1] && isBeing[2] && isBeing[3] && isBeing[4] && isBeing[5]) ||
                (isBeing[2] && isBeing[3] && isBeing[4] && isBeing[5] && isBeing[6]))
                return 40;

            return 0;
        }

        //Yahtzee 계산
        private int ScoreYahtzee(int[] dice)
        {
            for (int i = 1; i < dice.Length; i++)
            {
                if (dice[i] == 5)
                    return 50;
            }

            return 0;
        }

        //Chance 계산
        private int ScoreChance(int[] dice)
        {
            int sum = 0;

            for (int i = 1; i < dice.Length; i++)
            {
                sum += i * dice[i];
            }

            return sum;
        }
        #endregion
    }
}
