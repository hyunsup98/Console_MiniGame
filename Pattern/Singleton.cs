using System;

namespace Console_Portfolio
{
    /// <summary>
    /// 제네릭 싱글톤 클래스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T instance;
        private static readonly object _lock = new object();
        public static T Instance
        {
            get
            {
                if(instance == null)    //1차 체크
                {
                    lock(_lock)
                    {
                        //이 스코프 안에서는 하나의 스레드만 실행 가능 (멀티 스레드 환경에서 동시에 들어오는 것을 막기 위함)
                        if(instance == null)    //2차 체크
                        {
                            //lock 키워드는 비싸기 때문에 더블 체크로 최대한 덜 사용하게 함
                            instance = new T();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
