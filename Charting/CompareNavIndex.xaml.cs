using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using System.Xml.Linq;
using Charting.wsNavChart;
using System.ComponentModel;
using System.Text.RegularExpressions;
using OxyPlot.Silverlight;
using OxyPlot;

namespace Charting
{
    public partial class CompareNavIndex
    {
        private string[] Names { get; set; }

        public CompareNavIndex()
        {
            InitializeComponent();
            var proxy = new NAVChartSoapClient();
            proxy.FetchMutualFundCompleted += proxy_FetchMutualFundCompleted;
            proxy.FetchIndicesCompleted += proxy_FetchIndicesCompleted;
            busyIndicator.IsBusy = true;
            cmdDateRng.SelectedIndex = 1;
            cmbMfs.Items.Add("Fetching Mutual funds...");
            cmbMfs.SelectedIndex = 0;
            cmbIndices.Items.Add("Fetching Indices...");
            cmbIndices.SelectedIndex = 0;
            lblInfo.Content = "";
            imgExc.Visibility = Visibility.Collapsed;
            proxy.FetchMutualFundAsync("");
            proxy.FetchIndicesAsync();
        }

        private void proxy_FetchMutualFundCompleted(object sender, FetchMutualFundCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            if (e.Error != null)
            {
                lblInfo.Content = "Can't fetch MF names.";
                btnGenerate.IsEnabled = false;
                busyIndicator.IsBusy = false;
                return;
            }
            cmbMfs.Items.Clear();
            cmbMfs.ItemsSource = e.Result;
            cmbMfs.DisplayMemberPath = "Name";
            cmbMfs.SelectedValuePath = "Id";
            btnGenerate.IsEnabled = true;
        }

        private void proxy_FetchIndicesCompleted(object sender, FetchIndicesCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblInfo.Content = "Can't fetch Index names.";
                btnGenerate.IsEnabled = false;
                return;
            }
            cmbIndices.Items.Clear();
            cmbIndices.ItemsSource = e.Result;
            cmbIndices.DisplayMemberPath = "Name";
            cmbIndices.SelectedValuePath = "Id";
            btnGenerate.IsEnabled = true;
        }

        private void cmbMfs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMfs.SelectedValue == null) return;
            busyIndicator.IsBusy = true;
            var proxy = new NAVChartSoapClient();
            proxy.FetchSchemesCompleted += proxy_FetchSchemesCompleted;
            proxy.FetchSchemesAsync(cmbMfs.SelectedValue.ToString());
            cmbSchemes.ItemsSource = null;
            cmbSchemes.Items.Add("Fetching Scheme names...");
        }

        private void proxy_FetchSchemesCompleted(object sender, FetchSchemesCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            if (e.Error != null)
            {
                lblInfo.Content = "Can't fetch Scheme names.";
                btnGenerate.IsEnabled = false;
                return;
            }
            cmbSchemes.Items.Clear();
            cmbSchemes.ItemsSource = null;
            cmbSchemes.ItemsSource = e.Result;
            cmbSchemes.DisplayMemberPath = "Name";
            cmbSchemes.SelectedValuePath = "Id";
            btnGenerate.IsEnabled = true;
        }

        private void btnAddSchemes_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSchemes.SelectedIndex < 0) return;
            busyIndicator.IsBusy = true;
            btnAddSchemes.IsEnabled = false;
            var proxy = new NAVChartSoapClient();
            proxy.FetchIndicesAgainstSchemeCompleted += proxy_FetchIndicesAgainstSchemeCompleted;
            proxy.FetchIndicesAgainstSchemeAsync(cmbSchemes.SelectedValue.ToString());
            lblInfo.Content = "Fetching Scheme Index...";
        }

        private void proxy_FetchIndicesAgainstSchemeCompleted(object sender,
                                                              FetchIndicesAgainstSchemeCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            if (e.Error != null)
            {
                lblInfo.Content = "Can't fetch Scheme Index...";
                return;
            }
            lblInfo.Content = "";
            var sch = new SelectedItems
                          {
                              Id = ((NameAndId)cmbSchemes.SelectedItem).Id,
                              IsIndex = false,
                              Name = ((NameAndId)cmbSchemes.SelectedItem).Name,
                              IsChecked = true
                          };
            if (lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.Id == sch.Id).Count() > 0) return;
            lstSelectedItems.Items.Add(sch);
            foreach (var v in e.Result)
            {
                var item = new SelectedItems { Id = v.Id, IsIndex = true, Name = v.Name, IsChecked = true };
                if (lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.Id == item.Id).Count() > 0) continue;
                lstSelectedItems.Items.Add(item);
            }
            btnAddSchemes.IsEnabled = true;
        }
        private void btnAddIndices_Click(object sender, RoutedEventArgs e)
        {
            if (cmbIndices.SelectedIndex < 0) return;
            var ind = new SelectedItems
                          {
                              Id = ((NameAndId)cmbIndices.SelectedItem).Id,
                              IsIndex = true,
                              Name = ((NameAndId)cmbIndices.SelectedItem).Name,
                              IsChecked = true
                          };
            if (lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.Id == ind.Id).Count() > 0) return;
            lstSelectedItems.Items.Add(ind);
        }

        private void lstSelectedItems_KeyDown(object sender, KeyEventArgs e)
        {
            var lstToDel = new List<SelectedItems>();
            if (e.Key == Key.Delete)
                lstToDel.AddRange(lstSelectedItems.SelectedItems.Cast<SelectedItems>());
            foreach (var v in lstToDel)
                lstSelectedItems.Items.Remove(v);
        }

        private void cmdDateRng_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dateParts = ((ComboBoxItem)cmdDateRng.SelectedItem).Content.ToString().Split(' ');
            dtpTo.SelectedDate = DateTime.Today;
            if (dateParts[1].StartsWith("W"))
                dtpFrom.SelectedDate = DateTime.Today.AddDays(-7 * Convert.ToInt32(dateParts[0]));
            else if (dateParts[1].StartsWith("D"))
                dtpFrom.SelectedDate = DateTime.Today.AddDays(-Convert.ToInt32(dateParts[0]));
            else if (dateParts[1].StartsWith("M"))
                dtpFrom.SelectedDate = DateTime.Today.AddMonths(-Convert.ToInt32(dateParts[0]));
            else if (dateParts[1].StartsWith("Y"))
                dtpFrom.SelectedDate = DateTime.Today.AddYears(-Convert.ToInt32(dateParts[0]));
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                if (lstSelectedItems.ItemContainerGenerator == null) continue;
                var item = lstSelectedItems.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                var tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                tagregCheckBox.IsChecked = true;
            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                var itemContainerGenerator = lstSelectedItems.ItemContainerGenerator;
                if (itemContainerGenerator == null) continue;
                var item = itemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                var tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                tagregCheckBox.IsChecked = false;
            }
        }

        private static T FindFirstElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parentElement);
            if (count == 0)
                return null;
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                var result = FindFirstElementInVisualTree<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var count = 0;
            for (var i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                var itemContainerGenerator = lstSelectedItems.ItemContainerGenerator;
                if (itemContainerGenerator == null) continue;
                var item = itemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (item == null) return;
                var tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                if (tagregCheckBox != null && tagregCheckBox.IsChecked != null && tagregCheckBox.IsChecked.Value)
                    count++;
            }
            if (count == lstSelectedItems.Items.Count) chkSelectAll.IsChecked = true;
        }

        private void proxy_FilterDataCompleted(object sender, FilterDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblInfo.Content = "Can't connect server.";
                busyIndicator.IsBusy = false;
                return;
            }
            if (e.Result == null || e.Result.Count() == 0)
            {
                lblInfo.Content = "Data not available.";
                busyIndicator.IsBusy = false;
                return;
            }
            if (e.Result.Count() > 10)
            {
                lblInfo.Content = "You can select max 10 schemes/indices at one time.";
                busyIndicator.IsBusy = false;
                return;
            }
            var items =
                lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.IsChecked).Select(
                    x =>
                    new wsNavChart.SelectedItems { Id = x.Id, Name = x.Name, IsIndex = x.IsIndex, IsChecked = x.IsChecked }).ToArray();
            var notFound = items.Where(x => e.Result.Where(y => y.Id == x.Id).Count() == 0).Select(x => x);
            if (notFound.Count() > 0)
            {
                var str = notFound.Select(x => x.Name).Aggregate((x, y) => x + ", " + y);
                ToolTipService.SetToolTip(imgExc, "Data not found for " + str);
                imgExc.Visibility = Visibility.Visible;
            }
            var enddate = Convert.ToDateTime(dtpTo.SelectedDate.Value).ToString("dd MMM yyyy");
            var startdate = Convert.ToDateTime(dtpFrom.SelectedDate.Value).ToString("dd MMM yyyy");
            Names = e.Result.Select(dr => dr.Name).ToArray();
            lblInfo.Content = "Fetching Data....";
            btnGenerate.IsEnabled = false;
            var proxy = new NAVChartSoapClient();
            proxy.FetchNavCompleted += proxy_FetchNavCompleted;
            proxy.FetchNavAsync(e.Result, startdate, enddate);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            busyIndicator.IsBusy = true;
            imgExc.Visibility = Visibility.Collapsed;
            if (dtpFrom.SelectedDate == null || dtpTo.SelectedDate == null || lstSelectedItems.Items.Count == 0)
            {
                lblInfo.Content = "Please select date properly";
                return;
                busyIndicator.IsBusy = false;
            }
            var count = 0;
            for (var i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                var itemContainerGenerator = lstSelectedItems.ItemContainerGenerator;
                if (itemContainerGenerator == null) continue;
                var item = itemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (item == null) continue;
                var tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                if (tagregCheckBox == null) continue;
                if (tagregCheckBox.IsChecked != null && tagregCheckBox.IsChecked.Value) count++;
            }
            if (count == 0) {
                busyIndicator.IsBusy = false;
                lblInfo.Content = "Please select at least one scheme/index properly";
                return;
            }
            var enddate = Convert.ToDateTime(dtpTo.SelectedDate.Value).ToString("dd MMM yyyy");
            var startdate = Convert.ToDateTime(dtpFrom.SelectedDate.Value).ToString("dd MMM yyyy");
            var proxy = new NAVChartSoapClient();
            proxy.FilterDataCompleted += proxy_FilterDataCompleted;
            var items =
                lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.IsChecked).Select(
                    x =>
                    new wsNavChart.SelectedItems { Id = x.Id, Name = x.Name, IsIndex = x.IsIndex, IsChecked = x.IsChecked }).ToArray();
            lblInfo.Content = "Checking for data.";
            proxy.FilterDataAsync(items, startdate, enddate);
        }

        private static string escapeSpecialCharacters(string strInput)
        {
            strInput = @"\-~!@#$%^*()_+{}:|""?`;',./[]]+".Aggregate(strInput,
                                                                    (current, c) =>
                                                                    current.Replace(c.ToString(), "\\" + c));
            return strInput.Replace(" ", "").Replace("&", "_x0026_");
        }

        private void proxy_FetchNavCompleted(object sender, FetchNavCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            btnGenerate.IsEnabled = true;
            if (e.Error != null)
            {
                lblInfo.Content = e.Error.Message;
                return;
            }
            var tmp = new PlotModel
                          {
                              LegendPlacement = LegendPlacement.Outside,
                              LegendOrientation = LegendOrientation.Horizontal,
                              LegendPosition = LegendPosition.BottomCenter
                          };
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left)
                             {
                                 MajorGridlineStyle = LineStyle.Solid,
                                 MinorGridlineStyle = LineStyle.Dot,
                                 Title = "Value (rebased against 100)"
                             });
            tmp.Axes.Add(new DateTimeAxis(AxisPosition.Bottom)
                             {
                                 MajorGridlineStyle = LineStyle.Solid,
                                 MinorGridlineStyle = LineStyle.Dot,
                                 StringFormat = "dd MMM yy",
                                 Title = "Date"
                             });
            foreach (var ls in from name in Names
                               let r = new Regex(escapeSpecialCharacters(name) + ">(.*?)<.*?date>(.*?)<",
                                                 RegexOptions.Singleline | RegexOptions.IgnoreCase)
                               let data = (from Match m in r.Matches(e.Result.Nodes[1].ToString())
                                           select new ChartData
                                                      {
                                                          Date = Convert.ToDateTime(m.Groups[2].Captures[0].Value),
                                                          YValue = Convert.ToDouble(m.Groups[1].Captures[0].Value)
                                                      }).ToList()
                               select new LineSeries
                                          {
                                              ItemsSource = data,
                                              DataFieldX = "Date",
                                              DataFieldY = "YValue",
                                              Smooth = true,
                                              Title = name,
                                              TrackerFormatString =
                                                  "Date: {2:dd MMM yyyy}" + Environment.NewLine + "Value: {4}",
                                              StrokeThickness = 1,
                                              MarkerType = MarkerType.Plus,
                                              MarkerSize = 3,
                                          })
            {
                tmp.Series.Add(ls);
            }
            var plot1 = new Plot
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Background = new SolidColorBrush(Colors.Transparent),
                                Model = tmp
                            };
            lowerBorder.Child = plot1;
            lblInfo.Content = "Graph generated [All value rebased against 100 for comparison]";
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var plot1 = lowerBorder.Child as Plot;
            if (plot1 == null) return;
            var dlg = new SaveFileDialog { Filter = "Png Files (.png)|*.png|All Files (*.*)|*.*" };
            if (dlg.ShowDialog() == true)
                plot1.SaveAsPng(dlg);

        }

        private void btnPdf_Click(object sender, RoutedEventArgs e)
        {
            //var dlg1 = new SaveFileDialog { Filter = "Pdf Files (.pdf)|*.pdf|All Files (*.*)|*.*" }; // for
            //if (dlg1.ShowDialog() == true)                                                           // local
            //new Charting.Pdf.PdfExport().ExportToPdf(dlg1, lowerBorder.Child);                       // testing
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fileStream = store.OpenFile("Charting.Export.xap", FileMode.Open, FileAccess.Read);
                var sri = new StreamResourceInfo(fileStream, "application/binary");
                var manifestStream = Application.GetResourceStream(sri, new Uri("AppManifest.xaml", UriKind.Relative)).Stream;
                var appManifest = new StreamReader(manifestStream).ReadToEnd();
                var deploymentRoot = XDocument.Parse(appManifest).Root;
                var deploymentParts = (from assemblyParts in deploymentRoot.Elements().Elements() select assemblyParts).ToList();
                Assembly assembly = null;
                foreach (var source in deploymentParts.Select(xElement => xElement.Attribute("Source").Value))
                {
                    fileStream = store.OpenFile("Charting.Export.xap", FileMode.Open, FileAccess.Read);
                    var streamInfo = Application.GetResourceStream(new StreamResourceInfo(fileStream, "application/binary"), new Uri(source, UriKind.Relative));
                    if (source == "Charting.Export.dll")
                        assembly = new AssemblyPart().Load(streamInfo.Stream);
                    else
                        new AssemblyPart().Load(streamInfo.Stream);
                }
                var o = assembly.CreateInstance("Charting.Pdf.PdfExport");
                var classType = o.GetType();
                var method = classType.GetMethod("ExportToPdf");
                var dlg1 = new SaveFileDialog { Filter = "Pdf Files (.pdf)|*.pdf|All Files (*.*)|*.*" };
                if (dlg1.ShowDialog() == true)
                    method.Invoke(o, new object[] { dlg1, lowerBorder.Child });
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists("Charting.Export.xap")) return;
                var wc = new WebClient();
                wc.OpenReadCompleted += wc_OpenReadCompleted;
                //wc.OpenReadAsync(new Uri("Charting.Export.xap", UriKind.Relative)); // for local testing
                wc.OpenReadAsync(new Uri("http://mfiframes.mutualfundsindia.com/getXapFile.ashx?fileName=ClientBin/Charting.Export.xap", UriKind.RelativeOrAbsolute)); // for live link
            }
        }

        private static void WriteStream(Stream stream, IsolatedStorageFileStream fileStream)
        {
            var buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                fileStream.Write(buffer, 0, bytesRead);
        }

        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null) return;
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fileStream = store.CreateFile("Charting.Export.xap");
                WriteStream(e.Result, fileStream);
                fileStream.Close();
            }
        }
    }

    public class ChartData
    {
        public DateTime Date { get; set; }
        public double YValue { get; set; }
    }

    public class SelectedItems : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsIndex { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked) return;
                _isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}