using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace CapstoneDProfessor
{
    public partial class MainForm : MetroFramework.Forms.MetroForm // 상속 클래스 변경 -> 디자인 변경
    {
        public static BackgroundWorker _background = new BackgroundWorker();

        static DataTable table = null;
        DataRow row = null;

        static string rehak;
        static string rename;
         static public string fp;
        static public string fn;
        static public string rcf;
        SetExam f2 = new SetExam();

        DateTime now;

        int timerc = 0;
        
        public MainForm()
        {
            InitializeComponent();
            _background.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_background_RunWorkerCompleted);
            
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            ServerProgram serverProgram = ServerProgram.GetInstance();
            serverProgram.Start();

            lblsubName.Text = "과목명";
            lblName.Text = "교수명";
            lblTime.Text = "시간"; 
            
            table = new DataTable();

            table.Columns.Add("hak");
            table.Columns.Add("name");
            table.Columns.Add("state");


            dgStudent.DataSource = table;

        }
        void _background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataGridUpdate();
            DataGridRefresh();
        }
        private void DataGridUpdate()
        {
            if (dgStudent.InvokeRequired)
            {
                dgStudent.Invoke(new MethodInvoker(delegate { dgStudent.DataSource = table; }));
            }
            else
                dgStudent.DataSource = table;

            
        }
        private void DataGridRefresh()
        {
            if(dgStudent.InvokeRequired)
            {
                dgStudent.Invoke(new MethodInvoker(delegate { dgStudent.Refresh(); }));
            }
            else
                dgStudent.Refresh();
        }
        void Adddata()
        {
            row = table.NewRow();

            row["hak"] = rehak;
            row["name"] = rename;
            row["state"] = "진행";

            table.Rows.Add(row);
            _background.RunWorkerAsync();
        }
        public void Setuser(string a, string b)
        {
            rehak = a;
            rename = b;
            Adddata();  
        }
        private void dgStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btnsetExam_Click(object sender, EventArgs e) // 시험 설정 ->과목명,시험시간,감독교수 입력
        {
            using (SetExam setexam = new SetExam())
            {
                setexam.FormSend += new SetExam.FormSendEvent(setex);
                setexam.ShowDialog();
            }   
        }
        private void setex(string sub,string name,int time)
        {
            timer1.Stop();

            lblsubName.Text = sub;
            lblName.Text = name;

            int hour;
            int min;

            hour = time / 60;
            min = time % 60;
            lblTime.Text = String.Format("{0}:{1}:{2}",hour.ToString("D2"),min.ToString("D2"),"00" );

            timerc = time;

        }
        private void btnStart_Click(object sender, EventArgs e) //시험 시작
        {
            //블랙리스트에 해당하는 프로세스 중지, 타이머 시작
            timer1.Stop();

            now = DateTime.Now.AddMinutes(timerc);

            timer1.Interval = 1000;
            timer1.Start();
        }

        public string sendfilepath()
        {
            return fp;
        }
        public string sentfilename()
        {
            return fn;
        }
        public string recvfname()
        {
            return rcf;
        }
        private void btnSend_Click(object sender, EventArgs e) //문제 전송
        {
            using( SendFile sendFile = new SendFile())
            {
                sendFile.FormSendF += new SendFile.FormSendFileEvent(RcvFile);
                sendFile.GetPath = fp;
                sendFile.ShowDialog();

            }


            // OpenFileDialog openFileDialog = new OpenFileDialog(); 
            // openFileDialog.Filter = "Text Files(*.txt)|*.txt|All Files(*.*)|*.*"; 


            btnStart.Enabled = true;
            //문제 파일 선택, 파일 전송

        }
        private void RcvFile(string dirpath,string filename,string recvfilename)
        {
            fp = dirpath;
            fn = filename;
            rcf = recvfilename;

        }
        private void btnBlack_Click(object sender, EventArgs e) //블랙 리스트
        {
            //금지할 프로세스 선택
           
        }
        private void btnWhite_Click(object sender, EventArgs e) //화이트 리스트
        {
            //허용할 프로세스 선택
        }
        private void Timer_Tick(object sender, EventArgs e) //타이머 시작 메소드 : timer1.Start();
        {
            TimeSpan t = now - DateTime.Now;
            Math.Abs(t.Seconds);
            lblTime.Text = String.Format("{0}", t.ToString("hh':'mm':'ss"));
        }
        
        private void lblTime_Click(object sender, EventArgs e)
        {
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            MessageBox.Show("선택");
        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}

