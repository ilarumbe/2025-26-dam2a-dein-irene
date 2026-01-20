using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace P2526_Irene_Biblioteca.Views
{
    public partial class ReportWindow : Window
    {
        private readonly ReportViewer reportViewer = new ReportViewer();

        public ReportWindow(string windowTitle,
                            string rdlcFileName,
                            string dataSourceName,
                            IEnumerable data,
                            Dictionary<string, string> parameters = null)
        {
            InitializeComponent();

            TitleText.Text = windowTitle;
            Title = windowTitle;

            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.Dock = System.Windows.Forms.DockStyle.Fill;

            FormsHost.Child = reportViewer;

            LoadReport(rdlcFileName, dataSourceName, data, parameters);
        }

        private void LoadReport(string rdlcFileName,
                                string dataSourceName,
                                IEnumerable data,
                                Dictionary<string, string> parameters)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string reportPath = Path.Combine(baseDir, "Reports", rdlcFileName);

            if (!File.Exists(reportPath))
            {
                MessageBox.Show("No se encontró el informe: " + reportPath, "Informe",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = reportPath;

            var rds = new ReportDataSource(dataSourceName, data);
            reportViewer.LocalReport.DataSources.Add(rds);

            if (parameters != null && parameters.Count > 0)
            {
                var list = new List<ReportParameter>();
                foreach (var kv in parameters)
                {
                    list.Add(new ReportParameter(kv.Key, kv.Value ?? ""));
                }
                reportViewer.LocalReport.SetParameters(list);
            }

            reportViewer.RefreshReport();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
