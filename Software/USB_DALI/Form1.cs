using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace USB_DALI
{
    public partial class Form1 : Form
    {
        public byte[] toslave = new byte[8];
        public byte[] fromslave = new byte[8];
        public int DALIbuf1 = 0xFE7F;
        public int DALIbuf2 = 0x7EFE;
        public int DALIbuf3 = 0x0000;
        public int answerlength = 8;
        public int parcel = 0; //
        public int adr = 0; //adr steps for timer
        public int small_step = 0;
        public int big_step = 0;
        public UInt32 address = 0x00000000;
        public UInt32 probe = 0x00000000;
        public byte daliread = 0x00;
        public UInt32 temp = 0;
        public byte tim4cnt = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public Boolean pause = false;
        //function to calculate CRC16 Modbus
        public UInt16 ModRTU_CRC(byte[] buf, int len)
        {
            UInt16 crc = 0xFFFF;
            for (int pos = 0; pos < len; pos++)
            {
                crc ^= (UInt16)buf[pos];
                for (int i = 8; i != 0; i--)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                        crc >>= 1;
                }
            }
            return (ushort)((crc >> 8) | (crc << 8));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox2.Text;
            button14.Enabled = true;
            try
            {
                serialPort1.Open();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message);
                button14.Enabled = false;
            }
            serialPort1.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = 3;
            comboBox4.SelectedIndex = 0;
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            timer4.Stop();
            tabControl1.SelectedTab = tabPage3;
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            string[] portnames = SerialPort.GetPortNames();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(portnames);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
            button6.Enabled = true;
            button14.Enabled = false;
            button15.Enabled = true;
            comboBox2.Enabled = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            button14.Enabled = true;
            button15.Enabled = false;
            comboBox2.Enabled = true;
        }

        private void tabPage2_Leave(object sender, EventArgs e)
        {
            serialPort1.Close();
            button15.Enabled = false;
            comboBox2.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        //Limit Keyboard
        private void richTextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 49) && e.KeyChar != 8)
                e.Handled = true;
        }


        private void richTextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void richTextBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar >= 58) && (e.KeyChar < 97 || e.KeyChar > 102) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void richTextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void richTextBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox18_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox17_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox16_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox15_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox26_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox25_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox24_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox23_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox22_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox21_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox20_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox19_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox42_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox41_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox40_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox39_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox38_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox37_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox36_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox35_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox34_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox33_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox32_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox31_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox30_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox29_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox28_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void richTextBox27_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button6.PerformClick();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                button1.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            toslave[0] = 0x01; //address
            toslave[1] = 0x02; //function code
            toslave[2] = 0x00; //starting point high byte
            toslave[3] = 0x00; //starting point = relay 0
            toslave[4] = 0x00; //inputs quantity high byte
            toslave[5] = 0x10; //inputs quantity = 16
            UInt16 checksum = ModRTU_CRC(toslave, 6);
            toslave[6] = (byte)(checksum >> 8);
            toslave[7] = (byte)checksum;
            //hex show
            for (int i = 0; i < 8; i++)
            {
                try
                {
                    serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                }
                catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                var h = Convert.ToString(toslave[i], 16);
                while (h.Length < 2) { h = "0" + h; }
                h += ' ';
                richTextBox1.SelectionColor = Color.Blue;
                richTextBox1.SelectedText = h;
            }
            richTextBox1.AppendText(Environment.NewLine);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.BytesToRead < answerlength) { }
            else { this.Invoke(new EventHandler(DoUpdate)); }
        }

        //Here is a place to do text update
        /*
        parcel=
        0 - DALI write direct
        1 - DALI write command
        2 - DALI write special
        3 - DALI read
        4 - DALI check line
        5 - do nothing (during 1utosearch)
         */
        private void DoUpdate(object? s, EventArgs e)
        {
            if (answerlength == 8)
            {
                int bytes = serialPort1.BytesToRead; //quantity of bytes in the buffer
                byte[] buffer = new byte[bytes]; //create an array of buffer size
                if (buffer.Length > 8)
                {
                    serialPort1.Read(buffer, 0, bytes); //read bytes to array
                    if (parcel != 5)
                    {
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = "Incorrect data";
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                }
                else if (buffer.Length == 8)
                {
                    serialPort1.Read(buffer, 0, bytes); //read bytes to array
                    if (parcel != 5)
                    {
                        string newdata = System.Text.Encoding.Default.GetString(buffer);
                        foreach (byte c in buffer)
                        {
                            var h = Convert.ToString(c, 16);
                            while (h.Length < 2) { h = "0" + h; }
                            h += ' ';
                            richTextBox1.SelectionColor = Color.Green;
                            richTextBox1.SelectedText = h;
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                }
            }
            else if (answerlength == 7)
            {
                int bytes = serialPort1.BytesToRead; //quantity of bytes in the buffer
                byte[] buffer = new byte[bytes]; //create an array of buffer size
                if (buffer.Length > 7)
                {
                    serialPort1.Read(buffer, 0, bytes); //read bytes to array
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = "Incorrect data";
                    richTextBox1.AppendText(Environment.NewLine);
                }
                else if (buffer.Length == 7)
                {
                    serialPort1.Read(buffer, 0, bytes); //read bytes to array
                    daliread = buffer[4];
                    if (parcel != 5) //show data if not during autoresearch
                    {
                        //show received data
                        string newdata = System.Text.Encoding.Default.GetString(buffer);
                        foreach (byte c in buffer)
                        {
                            var h = Convert.ToString(c, 16);
                            while (h.Length < 2) { h = "0" + h; }
                            h += ' ';
                            richTextBox1.SelectionColor = Color.Green;
                            richTextBox1.SelectedText = h;
                        }
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    //asked about DALI line state
                    if (parcel == 4)
                    {
                        if (buffer[4] == 0xFF) { label10.Text = "line is powered on"; label10.BackColor = System.Drawing.Color.Green; }
                        else if (buffer[4] == 0x00) { label10.Text = "line is NOT powered"; label10.BackColor = System.Drawing.Color.Red; }
                    }
                }
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            answerlength = 7; parcel = 2;
            toslave[0] = 0x01; //address
            toslave[1] = 0x03; //function code
            toslave[2] = 0x00; //starting point high byte
            toslave[3] = 0x02; //starting point = relay 0
            int filenumber = Convert.ToInt16(numericUpDown4.Value);
            toslave[4] = 0x00;
            toslave[5] = 0x01;
            UInt16 checksum = ModRTU_CRC(toslave, 6);
            toslave[6] = (byte)(checksum >> 8);
            toslave[7] = (byte)checksum;
            for (int i = 0; i < 8; i++)
            {
                try
                {
                    serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                }
                catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                var h = Convert.ToString(toslave[i], 16);
                while (h.Length < 2) { h = "0" + h; }
                h += ' ';
                richTextBox1.SelectionColor = Color.Blue;
                richTextBox1.SelectedText = h;
            }
            richTextBox1.AppendText(Environment.NewLine);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { comboBox3.Enabled = true; timer1.Start(); } else { comboBox3.Enabled = false; timer1.Stop(); }
        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(comboBox3.Text);
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown5.Value == 1) { DALIbuf2 |= (1 << 15); } else { DALIbuf2 &= ~(1 << 15); }
            var b = Convert.ToString(DALIbuf2, 2);
            while (b.Length < 16) { b = "0" + b; }
            label3.Text = b;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            DALIbuf2 &= ~0x7E00;
            int temp = Convert.ToInt16(numericUpDown1.Value) * 512;
            DALIbuf2 |= temp;
            var b = Convert.ToString(DALIbuf2, 2);
            while (b.Length < 16) { b = "0" + b; }
            label3.Text = b;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown4.Value == 1) { DALIbuf2 |= (1 << 8); } else { DALIbuf2 &= ~(1 << 8); }
            var b = Convert.ToString(DALIbuf2, 2);
            while (b.Length < 16) { b = "0" + b; }
            label3.Text = b;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown6.Enabled = false; numericUpDown6.Value = 0;
            DALIbuf2 &= ~0x00FF;
            int temp = 0x00;
            switch (comboBox4.SelectedIndex)
            {
                case 0: temp = 0x00; break;
                case 1: temp = 0x01; break;
                case 2: temp = 0x02; break;
                case 3: temp = 0x03; break;
                case 4: temp = 0x04; break;
                case 5: temp = 0x05; break;
                case 6: temp = 0x06; break;
                case 7: temp = 0x07; break;
                case 8: temp = 0x08; break;
                case 9: temp = 0x10; numericUpDown6.Enabled = true; break;
                case 10: temp = 0x20; break;
                case 11: temp = 0x21; break;
                case 12: temp = 0x2A; break;
                case 13: temp = 0x2B; break;
                case 14: temp = 0x2C; break;
                case 15: temp = 0x2D; break;
                case 16: temp = 0x2E; break;
                case 17: temp = 0x2F; break;
                case 18: temp = 0x40; numericUpDown6.Enabled = true; break;
                case 19: temp = 0x50; numericUpDown6.Enabled = true; break;
                case 20: temp = 0x60; numericUpDown6.Enabled = true; break;
                case 21: temp = 0x70; numericUpDown6.Enabled = true; break;
                case 22: temp = 0x80; break;
                case 23: temp = 0x90; break;
                case 24: temp = 0x91; break;
                case 25: temp = 0x92; break;
                case 26: temp = 0x93; break;
                case 27: temp = 0x94; break;
                case 28: temp = 0x95; break;
                case 29: temp = 0x96; break;
                case 30: temp = 0x97; break;
                case 31: temp = 0x98; break;
                case 32: temp = 0x99; break;
                case 33: temp = 0x9A; break;
                case 34: temp = 0x9B; break;
                case 35: temp = 0xA0; break;
                case 36: temp = 0xA1; break;
                case 37: temp = 0xA2; break;
                case 38: temp = 0xA3; break;
                case 39: temp = 0xA4; break;
                case 40: temp = 0xA5; break;
                case 41: temp = 0xB0; numericUpDown6.Enabled = true; break;
                case 42: temp = 0xC0; break;
                case 43: temp = 0xC1; break;
                case 44: temp = 0xC2; break;
                case 45: temp = 0xC3; break;
                case 46: temp = 0xC4; break;
            }
            DALIbuf2 |= temp;
            var b = Convert.ToString(DALIbuf2, 2);
            while (b.Length < 16) { b = "0" + b; }
            label3.Text = b;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown6.Enabled == true)
            {
                DALIbuf2 &= ~0x000F;
                int temp = Convert.ToInt16(numericUpDown6.Value);
                DALIbuf2 |= temp;
                var b = Convert.ToString(DALIbuf2, 2);
                while (b.Length < 16) { b = "0" + b; }
                label3.Text = b;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (checkBox3.Checked == false)
                {
                    answerlength = 8; parcel = 1;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x01; //starting point low byte
                    toslave[4] = (byte)(DALIbuf2 >> 8);
                    toslave[5] = (byte)DALIbuf2;
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        var h = Convert.ToString(toslave[i], 16);
                        while (h.Length < 2) { h = "0" + h; }
                        h += ' ';
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = h;
                    }
                    richTextBox1.AppendText(Environment.NewLine);

                    var d = Convert.ToString(DALIbuf2, 2);
                    while (d.Length < 16) { d = "0" + d; }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);
                }
                else
                {
                    answerlength = 8; parcel = 1;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x03; //starting point low byte
                    toslave[4] = (byte)(DALIbuf2 >> 8);
                    toslave[5] = (byte)DALIbuf2;
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        var h = Convert.ToString(toslave[i], 16);
                        while (h.Length < 2) { h = "0" + h; }
                        h += ' ';
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = h;
                    }
                    richTextBox1.AppendText(Environment.NewLine);

                    var d = Convert.ToString(DALIbuf2, 2);
                    while (d.Length < 16) { d = "0" + d; }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 7; parcel = 3;
                toslave[0] = 0x01; //address
                toslave[1] = 0x03; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x02; //starting point low byte
                toslave[4] = 0x00;
                toslave[5] = 0x01;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Yellow;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            answerlength = 8; parcel = 0;
            DALIbuf1 &= ~0x00FF;
            DALIbuf1 |= trackBar1.Value;
            var s = Convert.ToString(DALIbuf1, 2);
            while (s.Length < 16) { s = "0" + s; }
            label8.Text = s;

            if (serialPort1.IsOpen)
            {
                answerlength = 8;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = (byte)(DALIbuf1 >> 8);
                toslave[5] = (byte)DALIbuf1;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(DALIbuf1, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown8.Value == 1) { DALIbuf1 |= (1 << 15); } else { DALIbuf1 &= ~(1 << 15); }
            var b = Convert.ToString(DALIbuf1, 2);
            while (b.Length < 16) { b = "0" + b; }
            label8.Text = b;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            DALIbuf1 &= ~0x7E00;
            int temp = Convert.ToInt16(numericUpDown10.Value) * 512;
            DALIbuf1 |= temp;
            var b = Convert.ToString(DALIbuf1, 2);
            while (b.Length < 16) { b = "0" + b; }
            label8.Text = b;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 0;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = (byte)(DALIbuf1 >> 8);
                toslave[5] = (byte)DALIbuf1;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(DALIbuf1, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            DALIbuf3 &= ~0xFF00;
            int temp = Convert.ToInt16(numericUpDown7.Value) * 256;
            DALIbuf3 |= temp;
            var b = Convert.ToString(DALIbuf3, 2);
            while (b.Length < 16) { b = "0" + b; }
            label9.Text = b;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            DALIbuf3 &= ~0x00FF;
            int temp = Convert.ToInt16(numericUpDown9.Value);
            DALIbuf3 |= temp;
            var b = Convert.ToString(DALIbuf3, 2);
            while (b.Length < 16) { b = "0" + b; }
            label9.Text = b;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (checkBox2.Checked == false)
                {
                    answerlength = 8; parcel = 2;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x01; //starting point low byte
                    toslave[4] = (byte)(DALIbuf3 >> 8);
                    toslave[5] = (byte)DALIbuf3;
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        var h = Convert.ToString(toslave[i], 16);
                        while (h.Length < 2) { h = "0" + h; }
                        h += ' ';
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = h;
                    }
                    richTextBox1.AppendText(Environment.NewLine);

                    var d = Convert.ToString(DALIbuf3, 2);
                    while (d.Length < 16) { d = "0" + d; }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);
                }
                else if (checkBox2.Checked == true)
                {
                    answerlength = 8;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x03; //starting point low byte
                    toslave[4] = (byte)(DALIbuf3 >> 8);
                    toslave[5] = (byte)DALIbuf3;
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        var h = Convert.ToString(toslave[i], 16);
                        while (h.Length < 2) { h = "0" + h; }
                        h += ' ';
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = h;
                    }
                    richTextBox1.AppendText(Environment.NewLine);

                    var d = Convert.ToString(DALIbuf3, 2);
                    while (d.Length < 16) { d = "0" + d; }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 2;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x03; //starting point low byte
                toslave[4] = 0xA5;
                toslave[5] = 0x00;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xA500, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 2;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x03; //starting point low byte
                toslave[4] = 0xA7;
                toslave[5] = 0x00;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xA700, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            adr = 0;
            timer2.Start();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 2;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = 0xA1;
                toslave[5] = 0x00;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xA100, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 2;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = 0xA9;
                toslave[5] = 0x00;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xA900, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 7; parcel = 3;
                toslave[0] = 0x01; //address
                toslave[1] = 0x03; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x02; //starting point low byte
                toslave[4] = 0x00;
                toslave[5] = 0x01;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Yellow;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (adr == 0)
                {
                    answerlength = 8; parcel = 2;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x01; //starting point low byte
                    toslave[4] = 0xB1;
                    toslave[5] = (byte)(Convert.ToUInt32(numericUpDown11.Value));
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        var h = Convert.ToString(toslave[i], 16);
                        while (h.Length < 2) { h = "0" + h; }
                        h += ' ';
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = h;
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                    UInt16 some = Convert.ToUInt16((toslave[4] * 256) + toslave[5]); var d = Convert.ToString(some, 2);
                    while (d.Length < 16) { d = "0" + d; }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);
                }
                else if (adr == 1)
                {
                    answerlength = 8; parcel = 2;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x01; //starting point low byte
                    toslave[4] = 0xB3;
                    toslave[5] = (byte)(Convert.ToUInt32(numericUpDown12.Value));
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        var h = Convert.ToString(toslave[i], 16);
                        while (h.Length < 2) { h = "0" + h; }
                        h += ' ';
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = h;
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                    UInt16 some = Convert.ToUInt16((toslave[4] * 256) + toslave[5]); var d = Convert.ToString(some, 2);
                    while (d.Length < 16) { d = "0" + d; }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);
                }
                else if (adr == 2)
                {
                    answerlength = 8; parcel = 2;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x01; //starting point low byte
                    toslave[4] = 0xB5;
                    toslave[5] = (byte)(Convert.ToUInt32(numericUpDown13.Value));
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        var h = Convert.ToString(toslave[i], 16);
                        while (h.Length < 2) { h = "0" + h; }
                        h += ' ';
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectedText = h;
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                    UInt16 some = Convert.ToUInt16((toslave[4] * 256) + toslave[5]); var d = Convert.ToString(some, 2);
                    while (d.Length < 16) { d = "0" + d; }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = d;
                    richTextBox2.AppendText(Environment.NewLine);

                    timer2.Stop();
                }
            }
            adr++;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = 0xB3;
                toslave[5] = 0xFF;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xB3FF, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = 0xB5;
                toslave[5] = 0xFF;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xB5FF, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 2;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x03; //starting point low byte
                toslave[4] = 0x20;
                toslave[5] = 0x00;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xA900, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 7; parcel = 4;
                toslave[0] = 0x01; //address
                toslave[1] = 0x03; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x04; //starting point low byte
                toslave[4] = 0x00;
                toslave[5] = 0x01;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xA900, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 2;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = 0xAB;
                toslave[5] = 0x00;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);

                var d = Convert.ToString(0xAB00, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 2;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = 0xB7;
                UInt16 add = Convert.ToUInt16(numericUpDown14.Value * 2);
                add |= 0x01;
                toslave[5] = (byte)(add);
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                for (int i = 0; i < 8; i++)
                {
                    byte[] b = new byte[] { toslave[i] };
                    var h = Convert.ToString(toslave[i], 16);
                    while (h.Length < 2) { h = "0" + h; }
                    h += ' ';
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectedText = h;
                }
                richTextBox1.AppendText(Environment.NewLine);
                UInt16 some = Convert.ToUInt16((toslave[4] * 256) + toslave[5]); var d = Convert.ToString(some, 2);
                while (d.Length < 16) { d = "0" + d; }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = d;
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button21.Enabled = true;
            address = 0x00FFFFFF;
            probe = 0x00FFFFFF;
            big_step = 0;
            small_step = 0;
            timer3.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label11.Text = String.Format("{0:X}", probe);
            if (big_step == 0) //initializing and randomizing
            {
                //Reset
                if (small_step == 0)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0x7E;
                        toslave[5] = 0x20;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        richTextBox2.SelectionColor = Color.Red;
                        richTextBox2.SelectedText = "Reseting...";
                        richTextBox2.AppendText(Environment.NewLine);
                        small_step++;
                        System.Threading.Thread.Sleep(200);
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Initialize
                else if (small_step == 1)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x03; //starting point low byte
                        toslave[4] = 0xA5;
                        toslave[5] = 0x00;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        richTextBox2.SelectionColor = Color.Red;
                        richTextBox2.SelectedText = "Initializing...";
                        richTextBox2.AppendText(Environment.NewLine);
                        small_step++;
                        System.Threading.Thread.Sleep(200);
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Randomizing
                else if (small_step == 2)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x03; //starting point low byte
                        toslave[4] = 0xA7;
                        toslave[5] = 0x00;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        richTextBox2.SelectionColor = Color.Red;
                        richTextBox2.SelectedText = "Randomizing...";
                        richTextBox2.AppendText(Environment.NewLine);
                        richTextBox2.SelectionColor = Color.Red;
                        richTextBox2.SelectedText = "Search...";
                        richTextBox2.AppendText(Environment.NewLine);
                        small_step = 0; big_step++;
                        System.Threading.Thread.Sleep(200);
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }

            else if (big_step == 1)
            {
                //Address high byte
                if (small_step == 0)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB1;
                        toslave[5] = (byte)(probe >> 16);
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Address middle byte
                else if (small_step == 1)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB3;
                        toslave[5] = (byte)(probe >> 8);
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Address low byte
                else if (small_step == 2)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB5;
                        toslave[5] = (byte)probe;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Comparing
                else if (small_step == 3)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xA9;
                        toslave[5] = 0x00;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //reading DALI
                else if (small_step == 4)
                {
                    daliread = 0x00;
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 7; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x03; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x02; //starting point low byte
                        toslave[4] = 0x00;
                        toslave[5] = 0x01;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //processing received data
                else if (small_step == 5)
                {
                    if (daliread != 0x00)
                    {
                        address = probe;
                        probe = probe / 2;
                        big_step = 1; small_step = 0;
                    }
                    else if (probe == 0x00FFFFFF && daliread == 0x00)
                    {
                        timer3.Stop();
                        richTextBox2.SelectionColor = Color.Red;
                        richTextBox2.SelectedText = "No devices found!";
                        richTextBox2.AppendText(Environment.NewLine);
                    }
                    else { big_step = 2; small_step = 0; }
                }
            }

            else if (big_step == 2)
            {
                //Address high byte
                if (small_step == 0)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB1;
                        toslave[5] = (byte)(probe >> 16);
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Address middle byte
                else if (small_step == 1)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB3;
                        toslave[5] = (byte)(probe >> 8);
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Address low byte
                else if (small_step == 2)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB5;
                        toslave[5] = (byte)probe;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Comparing
                else if (small_step == 3)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xA9;
                        toslave[5] = 0x00;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //reading DALI
                else if (small_step == 4)
                {
                    daliread = 0x00;
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 7; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x03; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x02; //starting point low byte
                        toslave[4] = 0x00;
                        toslave[5] = 0x01;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //processing received data
                else if (small_step == 5)
                {
                    if (daliread != 0xFF) { probe++; big_step = 3; small_step = 0; }
                    else { big_step = 1; small_step = 0; }
                }
            }

            else if (big_step == 3)
            {
                //Address high byte
                if (small_step == 0)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB1;
                        toslave[5] = (byte)(probe >> 16);
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Address middle byte
                else if (small_step == 1)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB3;
                        toslave[5] = (byte)(probe >> 8);
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Address low byte
                else if (small_step == 2)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xB5;
                        toslave[5] = (byte)probe;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //Comparing
                else if (small_step == 3)
                {
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 8; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x06; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x01; //starting point low byte
                        toslave[4] = 0xA9;
                        toslave[5] = 0x00;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //reading DALI
                else if (small_step == 4)
                {
                    daliread = 0x00;
                    if (serialPort1.IsOpen)
                    {
                        answerlength = 7; parcel = 5;
                        toslave[0] = 0x01; //address
                        toslave[1] = 0x03; //function code
                        toslave[2] = 0x00; //starting point high byte
                        toslave[3] = 0x02; //starting point low byte
                        toslave[4] = 0x00;
                        toslave[5] = 0x01;
                        UInt16 checksum = ModRTU_CRC(toslave, 6);
                        toslave[6] = (byte)(checksum >> 8);
                        toslave[7] = (byte)checksum;
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                            }
                            catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        small_step++;
                    }
                    else { timer3.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                //processing received data
                else if (small_step == 5)
                {
                    if (daliread == 0xFF) { address = probe; big_step = 4; small_step = 0; }
                    else { temp = (address - probe) / 2; if (temp == 0) { temp = 1; }; probe = probe - 1 + temp; big_step = 2; small_step = 0; }
                }
            }

            else if (big_step == 4)
            {
                label7.Text = Convert.ToString(Convert.ToInt16(label7.Text) + 1);
                timer3.Stop();
                button18.Enabled = true;
                button20.Enabled = true;
                groupBox8.Enabled = true;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            timer3.Stop();
            button4.Enabled = true;
            button18.Enabled = false;
            button20.Enabled = false;
            groupBox8.Enabled = false;
            if (serialPort1.IsOpen)
            {
                answerlength = 8; parcel = 5;
                toslave[0] = 0x01; //address
                toslave[1] = 0x06; //function code
                toslave[2] = 0x00; //starting point high byte
                toslave[3] = 0x01; //starting point low byte
                toslave[4] = 0xA1;
                toslave[5] = 0x00;
                UInt16 checksum = ModRTU_CRC(toslave, 6);
                toslave[6] = (byte)(checksum >> 8);
                toslave[7] = (byte)checksum;
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                    }
                    catch { MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                richTextBox2.SelectionColor = Color.Red;
                richTextBox2.SelectedText = "Search terminated!";
                richTextBox2.AppendText(Environment.NewLine);
            }
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            tim4cnt = 0;
            timer4.Start();
            button18.Enabled = false;
            groupBox8.Enabled = false;
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //set address
            if (tim4cnt == 0)
            {
                if (serialPort1.IsOpen)
                {
                    answerlength = 8; parcel = 5;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x01; //starting point low byte
                    toslave[4] = 0xB7;
                    UInt16 add = Convert.ToUInt16(numericUpDown2.Value * 2);
                    add |= 0x01;
                    toslave[5] = (byte)(add);
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { timer4.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = "Setting address...";
                    richTextBox2.AppendText(Environment.NewLine);
                    tim4cnt++;
                }
                else { timer4.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            //withdraw
            else if (tim4cnt == 1)
            {
                if (serialPort1.IsOpen)
                {
                    answerlength = 8; parcel = 5;
                    toslave[0] = 0x01; //address
                    toslave[1] = 0x06; //function code
                    toslave[2] = 0x00; //starting point high byte
                    toslave[3] = 0x01; //starting point low byte
                    toslave[4] = 0xAB;
                    toslave[5] = 0x00;
                    UInt16 checksum = ModRTU_CRC(toslave, 6);
                    toslave[6] = (byte)(checksum >> 8);
                    toslave[7] = (byte)checksum;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            serialPort1.Write(new byte[] { toslave[i] }, 0, 1);
                        }
                        catch { timer4.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    richTextBox2.SelectionColor = Color.Red;
                    richTextBox2.SelectedText = "Withdrawing device...";
                    richTextBox2.AppendText(Environment.NewLine);
                    timer4.Stop();
                }
                else { timer4.Stop(); MessageBox.Show("Port problem!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            address = 0x00FFFFFF;
            probe = 0x00FFFFFF;
            big_step = 1;
            small_step = 0;
            timer3.Start();
        }
    }
}
