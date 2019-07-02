using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace jyt上位机
{
    public partial class Form1 : Form
    {
        
        string  data,runtime2;
        int time =0;
        int num;
        float  tem,cbi;
        float resize = 0;  //溫度調整變量
        public Form1()
        {
             
            InitializeComponent();
        }

        public class ComboBoxItem   //新建一个类储存combobox值
        {
            private string _text = null;
            private object _value = null;
            public string Text { get { return this._text; } set { this._text = value; } }
            public object Value { get { return this._value; } set { this._value = value; } }
            public override string ToString()
            {
                return this._text;
            }
        }
            public void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollToCaret();
        }
        Random rd = new Random();//生成隨機數對象
        //int p = rd.Next();
        System.Timers.Timer t = new System.Timers.Timer(1000);
        int point_X = 0;
        /// <summary>
        /// y坐标
        /// </summary>
       // int point_Y = 0;
        private void Form1_Load(object sender, EventArgs e)//窗口初始化
        {
            //******添加ComboBox控件的值******//
            ComboBoxItem cbi1 = new ComboBoxItem();//新建ComboBoxItem()對象
            cbi1.Text = "0.25";                                            //設置ComboBox控件的Text與對應的數值
            cbi1.Value = "900";
            comboBox3.Items.Add(cbi1);                           //添加到ComboBox3控件中
            //ComboBoxItem cbi2 = new ComboBoxItem();
            //cbi2.Text = "0.5";
            //cbi2.Value = "1800";
            //comboBox3.Items.Add(cbi2);
            ComboBoxItem cbi3 = new ComboBoxItem();
            cbi3.Text = "0.5";
            cbi3.Value = "1800";
            comboBox3.Items.Add(cbi3);
            ComboBoxItem cbi4 = new ComboBoxItem();
            cbi4.Text = "1";
            cbi4.Value = "3600";
            comboBox3.Items.Add(cbi4);
            ComboBoxItem cbi5 = new ComboBoxItem();
            cbi5.Text = "1.5";
            cbi5.Value = "5400";
            comboBox3.Items.Add(cbi5);
            ComboBoxItem cbi6 = new ComboBoxItem();
            cbi6.Text = "2";
            cbi6.Value = "7200";
            comboBox3.Items.Add(cbi6);
            ComboBoxItem cbi7 = new ComboBoxItem();
            cbi7.Text = "2.5";
            cbi7.Value = "9000";
            comboBox3.Items.Add(cbi7);
            ComboBoxItem cbi8 = new ComboBoxItem();
            cbi8.Text = "3";
            cbi8.Value = "10800";
            comboBox3.Items.Add(cbi8);
            ComboBoxItem cbi9 = new ComboBoxItem();
            cbi9.Text = "3.5";
            cbi9.Value = "12600";
            comboBox3.Items.Add(cbi9);
            ComboBoxItem cbi10 = new ComboBoxItem();
            cbi10.Text = "4";
            cbi10.Value = "14400";
            comboBox3.Items.Add(cbi10);
            //******添加ComboBox控件的值功能結束******//

            for (int i = 0; i <=31 ; i++)  //初始化32路曲線
             {
                axTChart1.Series(i).AddXY(0, 0, "", 0);
             }

            //t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；
            //t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            this.checkBox1.Checked = false; //初始化checkBox狀態
           // this.checkBox2.Checked = true ;
            for (int i = 1; i < 20; i++) //添加20路串口選項
            {
                comboBox1.Items.Add("COM" + i.ToString());
                //添加串口
            }
            for (int i = 0; i < 8; i++) //添加波特率選擇項
            {
                double cf = Math.Pow(2, i);
                double boud = 300 * cf;
                comboBox2.Items.Add(boud.ToString());
            }
            comboBox1.Text = "選擇COM";
            comboBox2.Text = "9600";
            // t.Enabled = true;
            //axTChart1.Series(0).AddXY(0, 0, "", 0);
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);//生成串口事件觸發接收事件對象
        }
        private void theout(object source, System.Timers.ElapsedEventArgs e)//曲線繪製方法
        {
            
            for (int i = 0; i < 32; i++)
            {
                
                if (num == i + 1)
                {
                    //if (tem - axTChart1.Series(i).YValues.Last <= 15)
                  //  {
                       // tem = Convert.ToSingle(axTChart1.Series(i).YValues.Last);
                        axTChart1.Series(i).AddXY(point_X, tem+resize , "", 0);  //添加32路曲線數據
                        point_X++;
                   // }
                }
            }
        }

        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)//串口数据接收事件
        {
            theout("",null );
            System.Threading.Thread.Sleep(100);
            data = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(DisplayText));
            
            
        }
        private void DisplayText(object sender, EventArgs e) {
            
            textBox1.Text = "";//先清除上一次的数据
            textBox1.Text += data;
            string s = textBox1.Text.Substring(1, 2);  //從字符串第2個開始截取長度為2的字符
            num = int.Parse(s);   //字符型轉INT型
            //textBox4.Text = a1.ToString();    INT型轉字符型
            string temData = textBox1.Text;  //textBox1內容賦值給字符型temData
            string t1 = temData.Substring(3, 6);  //從字符串tem第3個開始截取長度為6的字符
            tem = float.Parse(t1);  //字符型轉float型
            textBox3.Text = tem.ToString();//float型轉字符型并顯示在textBox3中
        }
        private DateTime StartTime;
        private void button1_Click(object sender, EventArgs e)
        {
            try                                                                //拋出下列語句運行時可能產生的異常
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Open();
                button1.Enabled = false;
                button2.Enabled = true;
            }
            catch {
                MessageBox.Show("error", "error"); //異常拋出後提示錯誤信息
            }
            if (serialPort1 .IsOpen )//預留控制Arduino語句
            {
                //serialPort1.Write("R");
            }
            //*****計時語句******//
            StartTime = DateTime.Now;
            System.Threading.Thread P_thread = new System.Threading.Thread(//線程託管
                () =>
                {
                    while (true)
                    {
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            this.Refresh();
                            TimeSpan P_TimeSpan = DateTime.Now - StartTime;
                            Graphics P_graphics = CreateGraphics();
                            // P_graphics.DrawString("Time: " + DateTime.Now.ToString("yyyy年M月d日 h时mm分ss秒 dddd"),
                            //new Font("宋体", 20),
                            //Brushes.Purple,
                            //new Point(10, 10)
                            // );
                            toolStripStatusLabel1.Text = DateTime.Now.ToString();//string.Format("StartTime: {0}天{1}时{2}分{3}秒",
                                                                                 // StartTime.Day,StartTime.Hour,StartTime.Minute,StartTime.Second);
                            toolStripStatusLabel2.Text = string.Format("" + "RunTime: {1}时{2}分{3}秒",
                                P_TimeSpan.Days, P_TimeSpan.Hours, P_TimeSpan.Minutes, P_TimeSpan.Seconds);
                        });
                        System.Threading.Thread.Sleep(1000);
                    }
                });
            P_thread.IsBackground = true;
            P_thread.Start();
            //*****計時語句結束******//

            if (timer1.Enabled  == true)
            {
                timer1.Enabled = false;//timer在运行时点击该按钮则停止计时
            }
            else
            {
                timer1.Enabled = true;//timer停止时点击该按钮timer开始计时
            }
            comboBox1.Enabled = false;
            //comboBox3.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try //拋出下列語句運行時可能產生的異常
            {
                serialPort1.Close();
                button1.Enabled = true ;
                button2.Enabled = false;
            }
            catch//異常拋出後提示錯誤信息
            {
                MessageBox.Show("error", "error");
            }
        }
        
            private void Button3_Click(object sender, EventArgs e)
        {

            axTChart1.Export.ShowExport() ;//顯示數據導出頁面
          
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void Timer1_Tick(object sender, EventArgs e)//設置計時到達時間后停止測試、彈出數據導出頁面
        {
            time = time+1;
            runtime2 = Convert.ToString(time );
            // textBox2.Text = runtime2;
            ComboBoxItem myItem = (ComboBoxItem)comboBox3.Items[comboBox3.SelectedIndex];
            if (runtime2 == myItem.Value.ToString())
            {
               // try
                //{
                    serialPort1.Close();//關閉串口
                    button1.Enabled = true;
                    button2.Enabled = false;
               // }
               // catch
               // {
                   // MessageBox.Show("error", "error");
               // }
                timer1.Enabled = false;//停止 timer1計時器運行
                axTChart1.Export.ShowExport();//顯示數據導出頁面

            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            for (int p = 0; p < 32; p++)
            {
                axTChart1.Series(p).Clear();//清空曲線數據
                axTChart1.Series(p).AddXY(0, 0, "", 0);//初始化曲線坐標
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                axTChart1.Panel.Gradient.Visible = true;
            }
            else
            {
                axTChart1.Panel.Gradient.Visible = false;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                axTChart1.Aspect.View3D = true;
            }
           else
            {
                axTChart1.Aspect.View3D = false;
            }
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBoxItem myItem = (ComboBoxItem)comboBox3.Items[comboBox3.SelectedIndex];
            //MessageBox.Show(myItem.Value.ToString());
            
            //textBox3.Text = myItem.Value.ToString();
            
            
        }
    }
}
