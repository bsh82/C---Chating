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
                //서버가 꺼져있을 경우
                if (lblMsg.Tag.ToString() == "Stop")
                {
                    chatServer.Start(); //서버 시작
                    Thread waitThread = new Thread(new ThreadStart(AcceptClient)); // 클라이언트 접속 대기 스레드 
                    waitThread.Start(); // 스레드 시작

                    lblMsg.Text = "서버 시작 됨";
                    lblMsg.Tag = "Start";
                    btnStart.Text = "서버 종료";
                }
                else
                {
                    chatServer.Stop();
                    foreach (Socket socket in Form1.clientSocketArray)
                    {
                        socket.Close(); 
                    }
                    clientSocketArray.Clear();

                    lblMsg.Text = "서버 중지 됨";
                    lblMsg.Tag = "Stop";
                    btnStart.Text = "서버 시작";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("서버를 시작할 수 없습니다. :" + ex.Message);
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
                    Thread thd_ChatProcess = new Thread(new ThreadStart(clientHandler.Chat_Process)); // 각각의 클라이언트를 대응하는 스레드
                    thd_ChatProcess.Start();
                }
                catch (System.Exception)
                {
                    Form1.clientSocketArray.Remove(socketClient);
                    break;
                }
            }

        }

        // 텍스트박스에 대화내용을 쓰는 메소드
        public void SetText(string text)
        {
            // t.InvokeRequired가 true를 반환하면
            // Invoke 메소드 호출을 필요로 하는 상태고 즉 현재 스레드가 UI스레드가 아님
            // 이 때 Invoek를 시키면 UI 스레드가 델리게이트에 설정된 에소드를 실행해준다.
            // false를 반환하면 UI 스레드가 접근하는 경우로 컨트롤레 직접 접근해도 문제가 없는 상태다.
            if (this.txtChatMsg.InvokeRequired)
            {
                SetTextDelegate d = new SetTextDelegate(SetText); // 델리게이트 선언
                this.Invoke(d, new object[] { text }); // 델리게이트를 통해 글을 쓴다.
                // 이 경우 UI 스레드를 통해 SetText를 호출함
            }
            else
            {
                this.txtChatMsg.AppendText(text); // 텍스트박스에 글을 씀
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
                this.txtChatMsg = txtChatMsg; // 채팅 메시지 출력을 위힌 TextBox
                this.socketClient = socketClient; // 클라이언트 접속소켓, 이를 통해 스트림을 만들어 채팅한다.
                this.netStream = new NetworkStream(socketClient);
                Form1.clientSocketArray.Add(socketClient); // 클라이언트 접속소켓을 List에 담음
                this.strReader = new StreamReader(netStream);
                this.form1 = form1;
            }

            public void Chat_Process()
            {
                while (true)
                {
                    try
                    {
                        // 문자열을 받음
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
                        MessageBox.Show("채팅 오류 :" + ex.ToString());
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