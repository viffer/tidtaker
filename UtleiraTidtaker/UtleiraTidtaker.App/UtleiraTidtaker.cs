using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using UtleiraTidtaker.DataReader.Repository;
using UtleiraTidtaker.Lib.Model;
using UtleiraTidtaker.Lib.Repository;

namespace UtleiraTidtaker.App
{
    using System.Threading;

    public partial class UtleiraTidtaker : Form
    {
        private ExcelRepository _excelRepository;
        private AthleteRepository _athleteRepository;
        private RaceAthletes _raceAthletes;
        private Stopwatch _stopwatch;
        private int _exitPressCount;

        public UtleiraTidtaker()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += UtleiraTidtaker_DragEnter;
            this.DragDrop += UtleiraTidtaker_DragDrop;
            this.textRaces.MouseUp += TextRaces_MouseUp;
            this.textAthletes.MouseUp += TextAthletes_MouseUp;
            this.listSheetnames.Click += ListSheetnames_Click;
            this.KeyPress += UtleiraTidtaker_OnKeyPress;
            _stopwatch = new Stopwatch();
            dateTimePicker1.Value = new DateTime(2018, 06, 17, 11, 0, 0);
        }

        private void UtleiraTidtaker_OnKeyPress(object sender, KeyPressEventArgs keyPressEventArgs)
        {
            switch ((int)keyPressEventArgs.KeyChar)
            {
                case 27:
                    toolStripStatusLabel1.Text = "Press ESC once more to exit application";
                    _exitPressCount++;
                    if (_exitPressCount < 2) return;
                    this.Close();
                    this.Dispose();
                    break;
                case 15:
                    this.openFileDialog1.InitialDirectory = "D:\\Users\\to\\Downloads\\";
                    this.openFileDialog1.FileName = "";
                    this.openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                    var result = this.openFileDialog1.ShowDialog();
                    if (result != DialogResult.OK) return;
                    OpenFile(this.openFileDialog1.FileName);
                    break;
                default:
                    toolStripStatusLabel1.Text = string.Format("{0}", (int)keyPressEventArgs.KeyChar);
                    break;
            }
        }

        private void UtleiraTidtaker_DragDrop(object sender, DragEventArgs e)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            var file = (string[])e.Data.GetData(DataFormats.FileDrop);
            _excelRepository = new ExcelRepository(file[0]);
            listSheetnames.Items.Clear();
            foreach (var sheetName in _excelRepository.GetSheetNames())
            {
                listSheetnames.Items.Add(sheetName);
            }
            listSheetnames.SelectedItem = listSheetnames.Items[0];
        }

        private void UtleiraTidtaker_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
        }
        
        private void OpenFile(string path)
        {
            toolStripStatusLabel1.Text = $"Reading file: {path}";
            Application.DoEvents();
            _excelRepository = new ExcelRepository(path);
            listSheetnames.Items.Clear();
            foreach (var sheetName in _excelRepository.GetSheetNames())
            {
                listSheetnames.Items.Add(sheetName);
            }
            listSheetnames.SelectedItem = listSheetnames.Items[0];
        }

        private void listSheetnames_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = _excelRepository.Load(listSheetnames.SelectedItem.ToString());
            dataGridView1.DataSource = data;

            _athleteRepository = new AthleteRepository(data, dateTimePicker1.Value);

            var races = _athleteRepository.GetRaces().ToList();
            textRaces.Text = Newtonsoft.Json.JsonConvert.SerializeObject(races);

            Application.DoEvents();

            _raceAthletes = new RaceAthletes(_athleteRepository.GetAthletes(), dateTimePicker1.Value, _excelRepository.GetFiletime());

            textAthletes.Text = Newtonsoft.Json.JsonConvert.SerializeObject(_raceAthletes);

            _stopwatch.Stop();
            toolStripStatusLabel1.Text = string.Format("{0:HH:mm:ss} - {1:F}", DateTime.Now, _stopwatch.Elapsed.TotalSeconds);
        }

        private void ListSheetnames_Click(object sender, EventArgs e)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        private void TextAthletes_MouseUp(object sender, EventArgs e)
        {
            textAthletes.SelectAll();
            if (!string.IsNullOrEmpty(textAthletes.Text)) Clipboard.SetText(textAthletes.Text);
        }

        private void TextRaces_MouseUp(object sender, EventArgs e)
        {
            textRaces.SelectAll();
            if (!string.IsNullOrEmpty(textRaces.Text)) Clipboard.SetText(textRaces.Text);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            var result = this.openFileDialog1.ShowDialog();
            if (result != DialogResult.OK) return;
            OpenFile(this.openFileDialog1.FileName);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
