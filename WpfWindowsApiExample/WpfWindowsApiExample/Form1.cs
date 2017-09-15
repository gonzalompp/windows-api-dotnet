using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfWindowsApiExample.Classes;

namespace WpfWindowsApiExample
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        internal VirtualKeyShort wVk;
        internal ScanCodeShort wScan;
        internal KEYEVENTF dwFlags;
        internal int time;
        internal UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)]
        internal KEYBDINPUT ki;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        internal uint type;
        internal InputUnion U;
        internal static int Size
        {
            get { return Marshal.SizeOf(typeof(INPUT)); }
        }
    }

    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern uint SendInput(
        uint nInputs,
        [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
        int cbSize);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var procesos = Process.GetProcesses();
            //IntPtr window = FindWindow(null,textBox1.Text);
            foreach (Process proc in procesos)
            {
                Console.WriteLine("Titulo: "+ proc.MainWindowTitle);

                if (proc.MainWindowTitle.Contains("ABCDIN"))
                {
                    this.SendToBack();

                    IntPtr handle = proc.MainWindowHandle;

                    SetForegroundWindow(handle);

                    Thread.Sleep(2000);
                    Console.WriteLine("---- CONTINUE -----");

                    //presiona teclas
                    //0x7A (F11)
                    INPUT[] Inputs = new INPUT[1];
                    INPUT Input = new INPUT();

                    Input.type = 1; // 1 = Keyboard Input
                    Input.U.ki.wScan = ScanCodeShort.KEY_D; //F11
                    Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
                    Inputs[0] = Input;

                    SendInput(1, Inputs, INPUT.Size);
                }
            }
        }
    }
}
