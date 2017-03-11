using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

//That code is written by Suman Biswas, Calcutta, India (Email: sumanbiswas@aol.in,Website: sumanbiswas.xm.com).
//That code is running to transfer small file to client to server. by using and after doing modification any one 
//can able to make a large file transfer application in C#.Net. This is Server code.

namespace FTServerCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FTServerCode.receivedPath = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FTServerCode.receivedPath.Length > 0)
                backgroundWorker1.RunWorkerAsync();
            else
                MessageBox.Show("Porfavor seleccione una ruta de recepcion");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = FTServerCode.receivedPath;
            label3.Text = FTServerCode.curMsg;
        }

        FTServerCode obj = new FTServerCode();
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            obj.StartServer();            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                FTServerCode.receivedPath = fd.SelectedPath;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
    //FILE TRANSFER USING C#.NET SOCKET - SERVER
    class FTServerCode
    {
        IPEndPoint ipEnd;
        Socket sock;
        public FTServerCode()
        {
           ipEnd = new IPEndPoint(IPAddress.Any, 5656);
           sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
           sock.Bind(ipEnd);
        }
        public static string receivedPath;
        public static string curMsg = "Detenido";
        public  void StartServer()
        {
            try
            {
                curMsg = "Iniciando...";
                sock.Listen(100);

                curMsg = "Corriendo y esperando a recibir archivo.";
                Socket clientSock = sock.Accept();

                byte[] clientData = new byte[1024 * 5000];
                
                int receivedBytesLen = clientSock.Receive(clientData);
                curMsg = "Recibiendo datos...";

                int fileNameLen = BitConverter.ToInt32(clientData, 0);
                string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

                BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath +"/"+ fileName, FileMode.Append)); ;
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

                curMsg = "Guardando archivo...";

                bWrite.Close();
                clientSock.Close();
                curMsg = "Archivo recibido y guardado; Servidor Detenido.";
            }
            catch (Exception ex)
            {
                curMsg = "Error recibiendo archivo.";
            }
        }
    }
}