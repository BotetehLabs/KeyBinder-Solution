using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Keybind_Solution
{
    public partial class Form1 : Form
    {
        // Variables for storing each key
        private Keys? Hotkey = null;
        private Keys? Key1 = null;
        private Keys? Key2 = null;
        private Keys? Key3 = null;

        private bool isRecording = false;
        private Button activeButton;
        private TextBox activeTextBox;
        private bool hotkeyPressed = false;
        private bool isActive = false; // Track if the program is active

        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            button1.Text = "Record Hotkey";
            button2.Text = "Record Key #1";
            button3.Text = "Record Key #2";
            button4.Text = "Record Key #3";

            // Disable textboxes 1-4
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button8.Enabled = true;
            button9.Enabled = false;

            // Initialize textboxes to "NA" by default
            textBox1.Text = "NA";
            textBox2.Text = "NA";
            textBox3.Text = "NA";
            textBox4.Text = "NA";

            // Set up the timer
            Timer timer1 = new Timer();
            timer1.Interval = 100; // Check every 100 ms
            timer1.Tick += Timer_Tick;
            timer1.Start();
        }

        // Button click event handlers
        private void button1_Click(object sender, EventArgs e)
        {
            StartRecording(button1, textBox1);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            StartRecording(button2, textBox2);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            StartRecording(button3, textBox3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StartRecording(button4, textBox4);
        }

        // Method to start/stop recording
        private void StartRecording(Button button, TextBox textBox)
        {
            if (!isRecording)
            {
                isRecording = true;
                activeButton = button;
                activeTextBox = textBox;
                button.Text = "Cancel";
                textBox.Text = "Recording...";
            }
            else
            {
                // Cancel recording
                isRecording = false;
                ResetButtonAndTextBox(button, textBox, cancelled: true);
            }
        }

        // Override ProcessCmdKey to capture key presses globally
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (isRecording && activeTextBox != null)
            {
                // Save the key to the corresponding variable based on the active button
                if (activeButton == button1)
                {
                    Hotkey = keyData;
                    textBox1.Text = Hotkey.ToString();
                }
                else if (activeButton == button2)
                {
                    Key1 = keyData;
                    textBox2.Text = Key1.ToString();
                }
                else if (activeButton == button3)
                {
                    Key2 = keyData;
                    textBox3.Text = Key2.ToString();
                }
                else if (activeButton == button4)
                {
                    Key3 = keyData;
                    textBox4.Text = Key3.ToString();
                }

                isRecording = false;
                ResetButtonAndTextBox(activeButton, activeTextBox, cancelled: false);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Check for mouse clicks for recording
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (isRecording && activeTextBox != null)
            {
                if (activeButton == button1)
                {
                    Hotkey = (e.Button == MouseButtons.Left) ? Keys.LButton : Keys.RButton;
                    textBox1.Text = Hotkey.ToString();
                }
                else if (activeButton == button2)
                {
                    Key1 = (e.Button == MouseButtons.Left) ? Keys.LButton : Keys.RButton;
                    textBox2.Text = Key1.ToString();
                }
                else if (activeButton == button3)
                {
                    Key2 = (e.Button == MouseButtons.Left) ? Keys.LButton : Keys.RButton;
                    textBox3.Text = Key2.ToString();
                }
                else if (activeButton == button4)
                {
                    Key3 = (e.Button == MouseButtons.Left) ? Keys.LButton : Keys.RButton;
                    textBox4.Text = Key3.ToString();
                }

                isRecording = false;
                ResetButtonAndTextBox(activeButton, activeTextBox, cancelled: false);
                return;
            }

            base.OnMouseDown(e);
        }

        // Method to reset button and textbox to original state
        private void ResetButtonAndTextBox(Button button, TextBox textBox, bool cancelled)
        {
            if (button == button1)
            {
                button.Text = "Record Hotkey";
            }
            else if (button == button2)
            {
                button.Text = "Record Key #1";
            }
            else if (button == button3)
            {
                button.Text = "Record Key #2";
            }
            else if (button == button4)
            {
                button.Text = "Record Key #3";
            }

            if (cancelled)
            {
                textBox.Text = "NA";

                if (button == button1) { Hotkey = null; }
                if (button == button2) { Key1 = null; }
                if (button == button3) { Key2 = null; }
                if (button == button4) { Key3 = null; }
            }

            activeButton = null;
            activeTextBox = null;
        }

        // Clear Key1 and reset textBox2
        private void button5_Click(object sender, EventArgs e)
        {
            Key1 = null;
            textBox2.Text = "NA";
        }

        // Clear Key2 and reset textBox3
        private void button6_Click(object sender, EventArgs e)
        {
            Key2 = null;
            textBox3.Text = "NA";
        }

        // Clear Key3 and reset textBox4
        private void button7_Click(object sender, EventArgs e)
        {
            Key3 = null;
            textBox4.Text = "NA";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Hotkey == null)
            {
                MessageBox.Show("Hotkey cannot be null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isActive = true; // Set to true when starting
            button8.Enabled = false;
            button9.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            isActive = false; // Set to false when stopping
            button9.Enabled = false;
            button8.Enabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isActive) // Check if the program is active
            {
                return; // Do nothing if not active
            }

            // Check for hotkey activation
            if (Hotkey.HasValue && GetAsyncKeyState((int)Hotkey.Value) < 0 && !hotkeyPressed)
            {
                hotkeyPressed = true;

                // Create an array for keys to press together
                List<Keys> keysToPress = new List<Keys>();

                if (Key1.HasValue) keysToPress.Add(Key1.Value);
                if (Key2.HasValue) keysToPress.Add(Key2.Value);
                if (Key3.HasValue) keysToPress.Add(Key3.Value);

                // Send keys together
                if (keysToPress.Count > 0)
                    SendKeysTogether(keysToPress.ToArray());
            }
            else if (hotkeyPressed && GetAsyncKeyState((int)Hotkey.Value) == 0)
            {
                hotkeyPressed = false;
            }
        }

        // Helper method to send keys together
        private void SendKeysTogether(Keys[] keys)
        {
            foreach (var key in keys)
            {
                keybd_event((byte)key, 0, 0, 0); // Key down
            }

            foreach (var key in keys)
            {
                keybd_event((byte)key, 0, 2, 0); // Key up
            }
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


    }
}
