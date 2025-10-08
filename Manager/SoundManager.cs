using System;
using NAudio.Wave;

namespace Console_Portfolio
{
    /// <summary>
    /// 효과음, 브금 재생 관리 클래스
    /// </summary>
    public class SoundManager : Singleton<SoundManager>
    {
        private readonly string audioFilePath;  //사운드 파일 경로

        private AudioFileReader audioFile;      //오디오 데이터 읽기 담당
        private WaveOutEvent outputDevice;      //사운드 출력 담당

        private bool isRegisterEvent;           //사운드 반복 이벤트 등록했는지 체크하는 bool 변수

        /// <summary>
        /// 사운드 매니저 생성자
        /// </summary>
        /// <param name="audioFilePath"> 오디오 파일이 있는 경로 </param>
        public SoundManager()
        {
            audioFilePath = "Sound\\";
            isRegisterEvent = false;
        }

        //사운드 재생: 1회성
        public void Play(string fileName, float volume)
        {
            //audioFile과 outputDevice가 이미 있으면 해제 먼저 하기 → 예외처리 방지
            if(audioFile != null || outputDevice != null) Dispose();

            string path = audioFilePath + fileName;

            audioFile = new AudioFileReader(path);
            audioFile.Volume = volume;
            outputDevice = new WaveOutEvent();

            outputDevice.Init(audioFile);
            outputDevice.Play();
        }

        //사운드 재생: 반복
        public void PlayLoop(string fileName, float volume)
        {
            if (audioFile != null || outputDevice != null) Dispose();

            string path = audioFilePath + fileName;

            audioFile = new AudioFileReader(path);
            audioFile.Volume = volume;
            outputDevice = new WaveOutEvent();

            outputDevice.Init(audioFile);

            if (!isRegisterEvent)
            {
                outputDevice.PlaybackStopped += PlaybackStoppedHandler;
                isRegisterEvent = true;
            }

            outputDevice.Play();
        }

        //1회성 사운드 ex) UI 효과음 등등
        //사용하고 바로 해제
        public void PlayEffect(string fileName, float volume)
        {
            string path = audioFilePath + fileName;

            var effectFile = new AudioFileReader(path);
            effectFile.Volume = volume;
            var effectDevice = new WaveOutEvent();

            effectDevice.Init(effectFile);
            effectDevice.Play();

            effectDevice.PlaybackStopped += (s, e) =>
            {
                effectDevice?.Dispose();
                effectFile?.Dispose();
            };
        }

        //사용한 리소스 해제, 사운드 멈춤
        public void Dispose()
        {
            if (isRegisterEvent)
            {
                outputDevice.PlaybackStopped -= PlaybackStoppedHandler;
                isRegisterEvent = false;
            }

            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
        }

        //반복재생을 위한 Handler
        //audioFile의 재생 위치를 다시 0으로 되돌림
        public void PlaybackStoppedHandler(object sender, StoppedEventArgs e)
        {
            audioFile.Position = 0;
            outputDevice.Play();
        }
    }
}
