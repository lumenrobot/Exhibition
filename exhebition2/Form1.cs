using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System.Threading;
using System.IO;
using System.Diagnostics;
namespace exhebition2
{
    public partial class Form1 : Form
    {
        IModel channelCom;
        IModel channelData;
        IConnection connection;
        string commandKey = "avatar.NAO.command";
        string dataKey = "avatar.NAO.data.image";
        bool isConnected = false;
        bool isPersonExist = false;
        bool isStand = false;
        bool isChange = false;
        QueueingBasicConsumer consumer;
        string gender = "mister";
        FaceLocation loc = new FaceLocation();
        WMPLib.WindowsMediaPlayer player;
        string language = "english";
        ProcessStartInfo cmd;
        public Form1()
        {
            InitializeComponent();
            hScrollBar1.Value = 46;
            vScrollBar1.Value = 46;
        }
        
       
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.Uri = "amqp://lumen:lumen@localhost/%2F";
                connection = factory.CreateConnection();
                channelCom = connection.CreateModel();
                channelData = connection.CreateModel();
                QueueDeclareOk queue = channelData.QueueDeclare("", false, false, true, null);
                string faceKey = "lumen.visual.face.detection";
                channelData.QueueBind(queue.QueueName, "amq.topic", faceKey);
                consumer = new QueueingBasicConsumer(channelData);
                channelData.BasicConsume(queue.QueueName, true, consumer);
                isConnected = true;
                MessageBox.Show("connected to server");
            }
            catch
            {
                MessageBox.Show("unable to connect to server");
            }
        }

        private void greatB_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                if (language == "english")
                {
                    Parameter par = new Parameter { text = "good morning " + gender + ", may I know your name?" };
                    Command com = new Command { type = "texttospeech", method = "say", parameter = par };
                    sendCommand(com);
                }
                else if(language == "bahasa")
                {
                    string espeak = @"espeak -w ""D:\exhebition2\exhebition2\great.wav"" -v mb-id1 ""selamat pagi """+gender+@""", boleh saya tahu nama """+gender+@""" siapa?""";
                    cmd = new ProcessStartInfo("cmd","/c"+espeak);
                    cmd.RedirectStandardOutput = true;
                    cmd.UseShellExecute = false;
                    cmd.CreateNoWindow = true;
                    Process proc = new Process();
                    proc.StartInfo = cmd;
                    proc.Start();
                    Console.WriteLine("finish");
                    string get = proc.StandardOutput.ReadToEnd();
                    byte[] byteWav = File.ReadAllBytes(@"D:\exhebition2\exhebition2\great.wav");
                    string stringWav = Convert.ToBase64String(byteWav);
                    Parameter par = new Parameter { wavFile = stringWav };
                    Command com = new Command { type = "audiodevice", method = "sendremotebuffertooutput", parameter = par };
                    sendCommand(com);
                }
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }
        private void sendCommand(Command com)
        {
            string body = JsonConvert.SerializeObject(com);
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            channelCom.BasicPublish("amq.topic", commandKey, null, buffer);
        }

        private void priaB_CheckedChanged(object sender, EventArgs e)
        {
            if(language == "english")
                gender = "mister";
            else if(language =="bahasa")
                gender = "bapak";
        }

        private void wanita_CheckedChanged(object sender, EventArgs e)
        {
            if (language == "english")
                gender = "miss";
            else if (language == "bahasa")
                gender = "ibu";
        }

        private void intro_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                if (language == "english")
                {
                    Parameter par1 = new Parameter { text = "hello, " + gender + " " + nameText.Text };
                    Parameter par2 = new Parameter { text = "my name is lumen" };
                    Parameter par3 = new Parameter { text = "I am robot guide" };
                    Parameter par4 = new Parameter { text = "what can I help you?" };
                    Command com = new Command { type = "texttospeech", method = "say", parameter = par1 };
                    sendCommand(com);
                    com.parameter = par2;
                    sendCommand(com);
                    com.parameter = par3;
                    sendCommand(com);
                    com.parameter = par4;
                    sendCommand(com);
                }
                else if (language == "bahasa")
                {
                    string espeak = @"espeak -w ""D:\exhebition2\exhebition2\great.wav"" -v mb-id1 ""nama saya lumen, saya adalah robot pemandu pameran, ada yang bisa saya bantu?""";
                    cmd = new ProcessStartInfo("cmd","/c"+espeak);
                    cmd.RedirectStandardOutput = true;
                    cmd.UseShellExecute = false;
                    cmd.CreateNoWindow = true;
                    Process proc = new Process();
                    proc.StartInfo = cmd;
                    proc.Start();
                    Console.WriteLine("finish");
                    string get = proc.StandardOutput.ReadToEnd();
                    byte[] byteWav = File.ReadAllBytes(@"D:\exhebition2\exhebition2\great.wav");
                    string stringWav = Convert.ToBase64String(byteWav);
                    Parameter par = new Parameter { wavFile = stringWav };
                    Command com = new Command { type = "audiodevice", method = "sendremotebuffertooutput", parameter = par };
                    sendCommand(com);
                }
                
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                Parameter par1 = new Parameter { text = gender + " " + nameText.Text + ", you are now" };
                Parameter par2 = new Parameter { text = "in I T B aniversary exebition" };
                Command com = new Command { type = "texttospeech", method = "say", parameter = par1 };
                sendCommand(com);
                com.parameter = par2;
                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                Parameter par1 = new Parameter { text = "thank's for your coming" + gender + " " + nameText.Text };
                Parameter par2 = new Parameter { text = "see you later" };
                Command com = new Command { type = "texttospeech", method = "say", parameter = par1 };
                sendCommand(com);
                com.parameter = par2;
                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                Parameter par1 = new Parameter { postureName = "Stand",speed = 0.8f };
                Command com = new Command { type = "Posture", method = "goToPosture", parameter = par1 };
                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                Parameter par1 = new Parameter { postureName = "Sit", speed = 0.8f };
                Command com = new Command { type = "Posture", method = "goToPosture", parameter = par1 };
                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isStand = false;
            changed -= new isPerson(BodyControl);
            if (isConnected)
            {
                Command com = new Command { type = "motion", method = "rest" };
                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                
                Parameter par = new Parameter { jointName = new List<string> { "HeadYaw","HeadPitch" }, stiffnessess = new List<float> { 0.7f,0.7f } };
                Command com = new Command { type = "motion", method = "setstiffnesses",parameter= par };

                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            float angle = ((float)vScrollBar1.Value - 46.0f) * 29.5f / 46.0f;
            if (isConnected)
            {
                Parameter par2 = new Parameter { jointName = new List<string> { "HeadPitch" }, angles = new List<float> { toRad(angle)},speed = 0.3f };
                Command com = new Command { type = "motion", method = "setAngles", parameter = par2 };
                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }
        float toRad(float degree)
        {
            float result;
            result = degree * 2.0f * (float)Math.PI / 360.0f;
            return result;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            float angle = ((float)hScrollBar1.Value - 46.0f) * 119.5f / 46.0f;
            if (isConnected)
            {
                Parameter par2 = new Parameter { jointName = new List<string> { "HeadYaw" }, angles = new List<float> { toRad(angle) }, speed = 0.3f };
                Command com = new Command { type = "motion", method = "setAngles", parameter = par2 };
                sendCommand(com);
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void saybutton_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                if (language == "english")
                {
                    Parameter par = new Parameter { text = toSayText.Text };
                    Command com = new Command { type = "texttospeech", method = "say", parameter = par };
                    sendCommand(com);
                    toSayText.Text = "";
                }
                else if (language == "bahasa")
                {
                    string espeak = string.Format(@"espeak -w ""D:\exhebition2\exhebition2\great.wav"" -v mb-id1 ""{0}""",  toSayText.Text );
                    cmd = new ProcessStartInfo("cmd", "/c" + espeak);
                    cmd.RedirectStandardOutput = true;
                    cmd.UseShellExecute = false;
                    cmd.CreateNoWindow = true;
                    Process proc = new Process();
                    proc.StartInfo = cmd;
                    proc.Start();
                    Console.WriteLine("finish");
                    string get = proc.StandardOutput.ReadToEnd();
                    byte[] byteWav = File.ReadAllBytes(@"D:\exhebition2\exhebition2\great.wav");
                    string stringWav = Convert.ToBase64String(byteWav);
                    Parameter par = new Parameter { wavFile = stringWav };
                    Command com = new Command { type = "audiodevice", method = "sendremotebuffertooutput", parameter = par };
                    sendCommand(com);
                }
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

            Thread a = new Thread(showFaceLocation);
            a.Start();
            changed += new isPerson(BodyControl);
        }
        public delegate void isPerson(object sender, EventArgs ev);
        public event isPerson changed;
        
        private void showFaceLocation()
        {
            BasicDeliverEventArgs ev = null;
            while (true)
            {
                ev = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                string body = Encoding.UTF8.GetString(ev.Body);
                //Console.WriteLine(body);
                
                JsonSerializerSettings setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
                lock (loc)
                {
                    loc = JsonConvert.DeserializeObject<FaceLocation>(body, setting);
                }
                if (changed != null)
                {
                    changed(this, new EventArgs());
                }
            }
        }
        private void BodyControl(object sender, EventArgs e)
        {
            if (!isStand)
            {
                if (isConnected)
                {
                    Parameter par1 = new Parameter { postureName = "Stand", speed = 0.8f };
                    Command com = new Command { type = "Posture", method = "goToPosture", parameter = par1 };
                    sendCommand(com);
                    isStand = true;
                }
            }

        }
        private void timerT(int time)
        {
            Stopwatch s = new Stopwatch();
            s.Reset();
            s.Start();
            while (s.ElapsedMilliseconds < time) { }
            s.Stop();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                Parameter par = new Parameter { text = "would you like to watch me dance?" };
                Command com = new Command { type = "texttospeech", method = "say", parameter = par };
                sendCommand(com);
                toSayText.Text = "";
            }
            else
            {
                MessageBox.Show("not connected to server");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            player = new WMPLib.WindowsMediaPlayer();
            player.URL = @"D:\exhebition2\exhebition2\Tari_Sigeh_Pengunten.mp3";
            player.controls.play();
            if (isConnected)
            {
                string espeak = @"python ""D:\tari.py""";
                cmd = new ProcessStartInfo("cmd", "/c" + espeak);
                cmd.RedirectStandardOutput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;
                Process proc = new Process();
                proc.StartInfo = cmd;
                proc.Start();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            player.controls.stop();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            byte[] byteWav = File.ReadAllBytes(file.FileName);
            string stringWav = Convert.ToBase64String(byteWav);
            Parameter par = new Parameter { wavFile = stringWav };
            Command com = new Command { type = "audiodevice", method = "sendremotebuffertooutput", parameter = par };
            sendCommand(com);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            language = "english";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            language = "bahasa";
        }
    }
}
