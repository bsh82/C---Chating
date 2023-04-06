using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{

    delegate void SetTextDelegate(string s);
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        TcpListener chatServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080); // Socket() + Bind()
        public static ArrayList clientSocketArray = new ArrayList();

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //������ �������� ���
                if (lblMsg.Tag.ToString() == "Stop")
                {
                    chatServer.Start(); //���� ����
                    Thread waitThread = new Thread(new ThreadStart(AcceptClient)); // Ŭ���̾�Ʈ ���� ��� ������ 
                    waitThread.Start(); // ������ ����

                    lblMsg.Text = "���� ���� ��";
                    lblMsg.Tag = "Start";
                    btnStart.Text = "���� ����";
                }
                else
                {
                    chatServer.Stop();
                    foreach (Socket socket in Form1.clientSocketArray)
                    {
                        socket.Close(); 
                    }
                    clientSocketArray.Clear();

                    lblMsg.Text = "���� ���� ��";
                    lblMsg.Tag = "Stop";
                    btnStart.Text = "���� ����";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ������ �� �����ϴ�. :" + ex.Message);
            }
        }

        private void AcceptClient() 
        {
            Socket socketClient = null;
            while (true) // listen()
            {
                try
                {
                    socketClient = chatServer.AcceptSocket(); // Accept()

                    ClientHandler clientHandler = new ClientHandler();
                    clientHandler.ClientHandler_Setup(this, socketClient, this.txtChatMsg);
                    Thread thd_ChatProcess = new Thread(new ThreadStart(clientHandler.Chat_Process)); // ������ Ŭ���̾�Ʈ�� �����ϴ� ������
                    thd_ChatProcess.Start();
                }
                catch (System.Exception)
                {
                    Form1.clientSocketArray.Remove(socketClient);
                    break;
                }
            }

        }

        // �ؽ�Ʈ�ڽ��� ��ȭ������ ���� �޼ҵ�
        public void SetText(string text)
        {
            // t.InvokeRequired�� true�� ��ȯ�ϸ�
            // Invoke �޼ҵ� ȣ���� �ʿ�� �ϴ� ���°� �� ���� �����尡 UI�����尡 �ƴ�
            // �� �� Invoek�� ��Ű�� UI �����尡 ��������Ʈ�� ������ ���ҵ带 �������ش�.
            // false�� ��ȯ�ϸ� UI �����尡 �����ϴ� ���� ��Ʈ�ѷ� ���� �����ص� ������ ���� ���´�.
            if (this.txtChatMsg.InvokeRequired)
            {
                SetTextDelegate d = new SetTextDelegate(SetText); // ��������Ʈ ����
                this.Invoke(d, new object[] { text }); // ��������Ʈ�� ���� ���� ����.
                // �� ��� UI �����带 ���� SetText�� ȣ����
            }
            else
            {
                this.txtChatMsg.AppendText(text); // �ؽ�Ʈ�ڽ��� ���� ��
            }
        }


        public class ClientHandler
        {
            private TextBox txtChatMsg;
            private Socket socketClient;
            private NetworkStream netStream;
            private StreamReader strReader;
            private Form1 form1;

            public void ClientHandler_Setup(Form1 form1, Socket socketClient, TextBox txtChatMsg)
            {
                this.txtChatMsg = txtChatMsg; // ä�� �޽��� ����� ���� TextBox
                this.socketClient = socketClient; // Ŭ���̾�Ʈ ���Ӽ���, �̸� ���� ��Ʈ���� ����� ä���Ѵ�.
                this.netStream = new NetworkStream(socketClient);
                Form1.clientSocketArray.Add(socketClient); // Ŭ���̾�Ʈ ���Ӽ����� List�� ����
                this.strReader = new StreamReader(netStream);
                this.form1 = form1;
            }

            public void Chat_Process()
            {
                while (true)
                {
                    try
                    {
                        // ���ڿ��� ����
                        string lstMessage = strReader.ReadLine();
                        if(lstMessage != null && lstMessage != "")
                        {
                            form1.SetText(lstMessage + "\r\n");
                            byte[] byteSend_Data = Encoding.Default.GetBytes(lstMessage + "\r\n");
                            lock (Form1.clientSocketArray)
                            {
                                foreach(Socket socket in Form1.clientSocketArray)
                                {
                                    NetworkStream stream = new NetworkStream(socket);
                                    stream.Write(byteSend_Data, 0, byteSend_Data.Length);
                                }
                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show("ä�� ���� :" + ex.ToString());
                        Form1.clientSocketArray.Remove(socketClient);
                        break;
                    }
                }
            }

        }

        private void lblMsg_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            chatServer.Stop();  
        }
    }

}