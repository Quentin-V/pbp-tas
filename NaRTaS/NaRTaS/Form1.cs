using Memory;
using WindowsInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaRTaS
{
    public partial class Form1 : Form
    {
        Mem mem;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mem = new Mem();
            mem.OpenProcess("penumbra");
            timer1.Enabled = true;
 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float player_x = mem.ReadFloat("penumbra.exe+002DCAF0,0x158,0x38,0x274,0x48");
            float player_z = mem.ReadFloat("penumbra.exe+002DCAF0,0x158,0x38,0x274,0x50");
            float player_y = mem.ReadFloat("penumbra.exe+002DCAF0,0x158,0x38,0x274,0x4C");

            string level1 = mem.ReadString("penumbra.exe+002DCAF0,0x174,0x24,0xF4");
            string level2 = mem.ReadString("penumbra.exe+002DCAF0,0x174,0x24,0xF4,0x0");

            float igt = mem.ReadFloat("penumbra.exe+002DCAF0,0x188,0x4C,0x1C");


            playerCoords.Text = $"X: {player_x} | Z: {player_z} | Y: {player_y}";
            inGameTime.Text = $"{igt}";

            if (level1 != null && level1.Length > 0 && !level1.StartsWith("level"))
            {
                levelName.Text = $"{level2}";
            }
            else
            {
                levelName.Text = $"{level1}";
            }
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            using (var Keyboard = WindowsInput.Capture.Global.KeyboardAsync())
            {
                //Capture all events from the keyboard
                //Keyboard.KeyEvent += Keyboard_KeyEvent;

            }
        }

        //private static void Keyboard_KeyEvent(object sender, EventSourceEventArgs<KeyboardEvent> e)
        //{

        //    if (e.Data?.KeyDown?.Key == WindowsInput.Events.KeyCode.A || e.Data?.KeyUp?.Key == WindowsInput.Events.KeyCode.A)
        //    {
        //       e.Next_Hook_Enabled = false;
        //    }

        //}

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

    }
}
