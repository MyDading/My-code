using System;
using System.Collections.Generic;
using System.Text;

// 使用其中的 Timer 组件
using System.Windows.Forms;

namespace jyt上位机
{
    public class Signal
    {

        #region 变量

        // 可通过构造函数参数改变的变量
        // Sin, Cos, Step 信号的周期，单位s
        public double Cycle = 10;
        // 信号发生频率, Hz，或 Samples / s
        public int Interval = 10;

        // 实时信号
        // 随机信号 
        public double Random = 0;
        // 三角波
        public double Triangle = 0;
        // Sin 信号
        public double Sine = 0;


        // Sin, Cos, Step 信号的周期，单位s
        int Cycle_ms;
        // 时钟毫秒数
        long lTicks = 0;
        // 在周期内的毫秒数
        long iTicksInCycle;
        // 随机数发生器
        System.Random rnd = new System.Random();
        // 时钟
        Timer timer = new Timer();

        #endregion


        // 类的构造函数
        public Signal()
        {
            Initialization();
        }

        // 类的构造函数，重载（Overload）函数
        public Signal(double Cycle, int Interval)
        {
            this.Cycle = Cycle;
            this.Interval = Interval;
            Initialization();
        }


        void Initialization()
        {
            // 计算以毫秒为单位的周期;
            Cycle_ms = (int)(Cycle * 10000);
            // 时钟的时间间隔
            timer.Interval = Interval;
            //启动时钟
            timer.Start();
            // 设置时钟的消息响应函数
            timer.Tick += new EventHandler(timer_Tick);
        }

        public void Stop()
        {
            timer.Stop();
        }


        void timer_Tick(object sender, EventArgs e)
        {
            // 总时间，ms
            lTicks += timer.Interval;
            // 周期内的时间,ms
            iTicksInCycle = lTicks % Cycle_ms;
            // 相位角
            double angle = iTicksInCycle * 2 * Math.PI / Cycle_ms;
            Sine = Math.Sin(angle);
            Triangle = iTicksInCycle * 1.0 / Cycle_ms;
            Random = rnd.NextDouble();
        }


    }
}