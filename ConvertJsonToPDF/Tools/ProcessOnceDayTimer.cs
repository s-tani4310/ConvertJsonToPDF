﻿using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ConvertJsonToPDF.Tools
{
    internal class AppMain
    {
        public static void Main(string[] args)
        {
            // 内部で1秒に1回時刻をチェックするように指定してオブジェクトを作成
            var timer =
                new ProcessOnceDayTimer(
                    TimeSpan.FromSeconds(1), // タイマーが時間を確認する間隔
                    DateTimeUtil.GetTime(16, 10)); // この時間を超えると処理を実行する

            timer.Elapsed += () =>
            {
                // 時間が来たら実行する処理を指定する
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] Execute!!");
            };

            timer.Start();

            Console.WriteLine("Wait...");
            Console.ReadLine();

            // 16:10を超えると以下が表示される
            // >[2022/07/12 16:10] Execute!

            // 次の日の16:10を超えると以下が表示される
            // >[2022/07/13 16:10] Execute!

            timer.Dispose(); // 使い終わったらDisposeを呼び出して破棄する
        }
    }
    
    /// <summary>
         /// 1日1回だけ指定した時刻に処理を実行するタイマークラス
         /// </summary>
    public class ProcessOnceDayTimer : IDisposable
    {
        // Fields - - - - - - - - - - - - - - - - - - - -

        // 定周期処理用のタイマー
        private Timer _timer;
        // 最後に処理を実行した時刻
        DateTime _nextExecTime;
        // 排他制御用
        private readonly object _logckObj = new object();


        // Events - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 一日一回指定した時刻に実行する処理を設定するハンドラー
        /// </summary>
        /// <remarks>
        /// この通知は環境によって Rx とか MessagePipe に変更しても良い。
        /// </remarks>
        public event Action Elapsed;


        // Props - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 処理が実行される時間を取得します。
        /// <para>時刻部分しか参照しません。日付は無視されます。</para>
        /// </summary>
        public DateTime ExectionTime { get; init; }


        // Constructors & Methods - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// タイマーの実行間隔を指定してオブジェクトを作成します。
        /// </summary>
        public ProcessOnceDayTimer(TimeSpan interval, DateTime execTime)
        {
            _timer = new Timer(interval.TotalMilliseconds);
            _timer.Elapsed += OnElapsed;
            ExectionTime = execTime;
        }

        /// <summary>
        /// タイマーを開始します。
        /// </summary>
        public void Start()
        {
            if (_timer == null) throw new ObjectDisposedException("This object is disposed.");
            lock (_logckObj) _timer.Start();

            // 次の日時を作成する

            var now = DateTime.Now;
            var next =
                new DateTime(now.Year, now.Month, now.Day,
                    ExectionTime.Hour, ExectionTime.Minute, ExectionTime.Second);
            if (now > next)
            {
                // 起動予定時刻を過ぎていたら次の日を設定
                next = next.AddDays(1);
            }
            _nextExecTime = next;
        }

        /// <summary>
        /// タイマーを停止します。
        /// </summary>
        public void Stop()
        {
            if (_timer == null) throw new ObjectDisposedException("This object is disposed.");
            lock (_logckObj) _timer.Stop();
        }

        // IDisposable の実装
        public void Dispose()
        {
            if (_timer == null) return; // 解放済み

            lock (_logckObj)
            {
                Elapsed = null;
                _timer.Stop();
                using (_timer) { }
                GC.SuppressFinalize(this);
            }
        }

        // 定周期で実行されるメソッド
        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_logckObj)
            {
                try
                {
                    _timer.Stop();

                    var now = DateTime.Now;
                    if (_nextExecTime > now)
                    {
                        return; // 1日1回まで
                    }

                    if (_timer == null)
                    {
                        return; // 実行前にもう一度オブジェクトが有効か確認する
                    }

                    Elapsed?.Invoke();

                    _nextExecTime = _nextExecTime.AddDays(1); // 次回実行時刻を設定
                }
                finally
                {
                    _timer.Start();
                }
            }
        }
    }

    // 時刻作成用のヘルパークラス
    public static class DateTimeUtil
    {
        /// <summary>
        /// 有効な時間と分の組み合わせを取得します。
        /// </summary>
        public static DateTime GetTime(int hour, int min)
        {
            var time = DateTime.MinValue;
            return new DateTime(time.Year, time.Month, time.Day, hour, min, 0);
        }
    }
}