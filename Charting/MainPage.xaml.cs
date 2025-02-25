using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Charting.wsNavChart;
using System.ServiceModel;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls.DataVisualization.Charting.Primitives;
using System.Windows.Controls.DataVisualization;
using System.Text.RegularExpressions;
using OxyPlot.Silverlight;
using OxyPlot;

namespace Charting
{
    public class SolidColorBrushGenerated
    {
        readonly Color[] _colors = new[] { Colors.Black, Colors.Magenta, Colors.Purple, Colors.DarkGray, Colors.Brown, Colors.Yellow, Colors.Orange, Colors.Green, Colors.Red, Colors.Blue };

        public SolidColorBrush GetBrush(int i)
        {
            return new SolidColorBrush(_colors[i]);
        }
    } 

    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            var proxy = new NAVChartSoapClient();
            proxy.FetchMutualFundCompleted += new EventHandler<FetchMutualFundCompletedEventArgs>(proxy_FetchMutualFundCompleted);
            proxy.FetchIndicesCompleted += new EventHandler<FetchIndicesCompletedEventArgs>(proxy_FetchIndicesCompleted);
            busyIndicator.IsBusy = true;
            cmdDateRng.SelectedIndex = 1;
            cmbMfs.Items.Add("Fetching Mutual funds...");
            cmbMfs.SelectedIndex = 0;
            cmbIndices.Items.Add("Fetching Indices...");
            cmbIndices.SelectedIndex = 0;
            lblInfo.Content = "";
            imgExc.Visibility = System.Windows.Visibility.Collapsed;
            proxy.FetchMutualFundAsync("");
            proxy.FetchIndicesAsync();
        }

        void proxy_FetchMutualFundCompleted(object sender, FetchMutualFundCompletedEventArgs e)
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

        void proxy_FetchIndicesCompleted(object sender, FetchIndicesCompletedEventArgs e)
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
            proxy.FetchSchemesCompleted += new EventHandler<FetchSchemesCompletedEventArgs>(proxy_FetchSchemesCompleted);
            proxy.FetchSchemesAsync(cmbMfs.SelectedValue.ToString());
            cmbSchemes.ItemsSource = null;
            cmbSchemes.Items.Add("Fetching Scheme names...");
        }

        void proxy_FetchSchemesCompleted(object sender, FetchSchemesCompletedEventArgs e)
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
            proxy.FetchIndicesAgainstSchemeCompleted += new EventHandler<FetchIndicesAgainstSchemeCompletedEventArgs>(proxy_FetchIndicesAgainstSchemeCompleted);
            proxy.FetchIndicesAgainstSchemeAsync(cmbSchemes.SelectedValue.ToString());
            lblInfo.Content = "Fetching Scheme Index...";
        }

        void proxy_FetchIndicesAgainstSchemeCompleted(object sender, FetchIndicesAgainstSchemeCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            if (e.Error != null)
            {
                lblInfo.Content = "Can't fetch Scheme Index...";
                return;
            }
            lblInfo.Content = "";
            var sch = new SelectedItems { Id = ((NameAndId)cmbSchemes.SelectedItem).Id, IsIndex = false, Name = ((NameAndId)cmbSchemes.SelectedItem).Name, IsChecked = true };
            if (lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.Id == sch.Id).Count() > 0) return;
            lstSelectedItems.Items.Add(sch);
            foreach (var v in e.Result)
            {
                var item = new SelectedItems {  Id = v.Id, IsIndex = true, Name = v.Name, IsChecked = true };
                if (lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.Id == item.Id).Count() > 0) continue;
                lstSelectedItems.Items.Add(item);
            }
            btnAddSchemes.IsEnabled = true;
        }

        private void btnAddIndices_Click(object sender, RoutedEventArgs e)
        {
            if (cmbIndices.SelectedIndex < 0) return;
            var ind = new SelectedItems {  Id = ((NameAndId)cmbIndices.SelectedItem).Id, IsIndex = true, Name = ((NameAndId)cmbIndices.SelectedItem).Name, IsChecked = true };
            if (lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.Id == ind.Id).Count() > 0) return;
            lstSelectedItems.Items.Add(ind);
        }

        private void lstSelectedItems_KeyDown(object sender, KeyEventArgs e)
        {
            var lstToDel = new List<SelectedItems>();
            if (e.Key == Key.Delete)
                foreach (SelectedItems v in lstSelectedItems.SelectedItems)
                    lstToDel.Add(v);
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
            for (int i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                ListBoxItem item = this.lstSelectedItems.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                CheckBox tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                tagregCheckBox.IsChecked = true;
            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                ListBoxItem item = this.lstSelectedItems.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                CheckBox tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                tagregCheckBox.IsChecked = false;
            }
        }

        private T FindFirstElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parentElement); if (count == 0)
                return null;
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i);
                if (child != null && child is T)
                { return (T)child; }
                else
                {
                    var result = FindFirstElementInVisualTree<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var count = 0;
            for (int i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                ListBoxItem item = this.lstSelectedItems.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (item == null) return;
                CheckBox tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                if (tagregCheckBox != null && tagregCheckBox.IsChecked.Value) count++;
            }
            if (count == lstSelectedItems.Items.Count) chkSelectAll.IsChecked = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //var count = 0;
            //for (int i = 0; i < lstSelectedItems.Items.Count; i++)
            //{
            //    ListBoxItem item = this.lstSelectedItems.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
            //    if (item == null) return;
            //    CheckBox tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
            //    if (tagregCheckBox != null && tagregCheckBox.IsChecked.Value) count++;
            //}
            //if (count != lstSelectedItems.Items.Count) chkSelectAll.IsChecked = false;
        }

        void proxy_FilterDataCompleted(object sender, FilterDataCompletedEventArgs e)
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
            var items = lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.IsChecked).Select(x => new wsNavChart.SelectedItems { Id = x.Id, Name = x.Name, IsIndex = x.IsIndex, IsChecked = x.IsChecked }).ToArray();
            var notFound = items.Where(x => e.Result.Where(y => y.Id == x.Id).Count() == 0).Select(x => x);
            if (notFound.Count() > 0)
            {
                var str = notFound.Select(x => x.Name).Aggregate((x, y) => x + ", " + y);
                ToolTipService.SetToolTip(imgExc, "Data not found for " + str);
                imgExc.Visibility = System.Windows.Visibility.Visible;
            }
            string enddate = Convert.ToDateTime(dtpTo.SelectedDate.Value).ToString("dd MMM yyyy");
            string startdate = Convert.ToDateTime(dtpFrom.SelectedDate.Value).ToString("dd MMM yyyy");
            List<string> arrColumn_Names = new List<string>();
            foreach (wsNavChart.SelectedItems dr in e.Result)
                    arrColumn_Names.Add(dr.Name);
            Names = arrColumn_Names.ToArray();
            lblInfo.Content = "Fetching Data....";
            btnGenerate.IsEnabled = false;
            var proxy = new NAVChartSoapClient();
            proxy.FetchNavCompleted += new EventHandler<FetchNavCompletedEventArgs>(proxy_FetchNavCompleted);
            proxy.FetchNavAsync(e.Result, startdate, enddate);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            busyIndicator.IsBusy = true;
            imgExc.Visibility = System.Windows.Visibility.Collapsed;
            Chart1.UpdateLayout();
            UpdateChart(0);
            if (dtpFrom.SelectedDate == null || dtpTo.SelectedDate == null)
            {
                lblInfo.Content = "Please select date properly";
                return;
            }
            var count = 0;
            for (int i = 0; i < lstSelectedItems.Items.Count; i++)
            {
                ListBoxItem item = this.lstSelectedItems.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (item == null) continue;
                CheckBox tagregCheckBox = FindFirstElementInVisualTree<CheckBox>(item);
                if (tagregCheckBox == null) continue;
                if (tagregCheckBox.IsChecked.Value) count++;
            }
            if (count == 0) return;
            string enddate = Convert.ToDateTime(dtpTo.SelectedDate.Value).ToString("dd MMM yyyy");
            string startdate = Convert.ToDateTime(dtpFrom.SelectedDate.Value).ToString("dd MMM yyyy");
            var proxy = new NAVChartSoapClient();
            proxy.FilterDataCompleted += new EventHandler<FilterDataCompletedEventArgs>(proxy_FilterDataCompleted);
            var items = lstSelectedItems.Items.Cast<SelectedItems>().Where(x => x.IsChecked).Select(x => new wsNavChart.SelectedItems { Id = x.Id, Name=x.Name, IsIndex=x.IsIndex , IsChecked = x.IsChecked}).ToArray();
            lblInfo.Content = "Checking for data.";
            proxy.FilterDataAsync(items, startdate, enddate);
        }

        string[] Names { get; set; }

        string escapeSpecialCharacters(string strInput)
        {
            foreach (char c in @"\-~!@#$%^*()_+{}:|""?`;',./[]]+")
                strInput = strInput.Replace(c.ToString(), "\\" + c);
            return strInput.Replace(" ", "").Replace("&", "_x0026_");
        }

        void proxy_FetchNavCompleted(object sender, FetchNavCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            btnGenerate.IsEnabled = true;
            Chart1.Series.Clear();
            if (e.Error != null)
            {
                lblInfo.Content = e.Error.Message;
                Chart1.IsEnabled = false;
                return;
            }
            Plot plot1 = new Plot();
            var tmp = new PlotModel("Silverlight demo");
            tmp.Axes.Add(new global::OxyPlot.LinearAxis(AxisPosition.Left) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            tmp.Axes.Add(new global::OxyPlot.DateTimeAxis(AxisPosition.Bottom) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, StringFormat="dd MMM yy" });
            System.Windows.Controls.DataVisualization.Charting.LineSeries ser2;
            var i = 0;
            foreach (string name in Names)
            {
                var data = new List<ChartData>();
                Regex r = new Regex(escapeSpecialCharacters(name) + ">(.*?)<.*?date>(.*?)<", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                foreach (Match m in r.Matches(e.Result.Nodes[1].ToString()))
                    data.Add(new ChartData { Date = Convert.ToDateTime(m.Groups[2].Captures[0].Value), YValue = Convert.ToDouble(m.Groups[1].Captures[0].Value) });
                var style = new System.Windows.Style { TargetType = typeof(Polyline) };
                style.Setters.Add(new Setter(Shape.StrokeThicknessProperty, polyLine.Value));
                ser2 = new System.Windows.Controls.DataVisualization.Charting.LineSeries();
                ser2.ItemsSource = null;
                ser2.Name = "ser2" + name;
                ser2.Title = name;
                ser2.IndependentValueBinding = new System.Windows.Data.Binding("Date");
                ser2.DependentValueBinding = new System.Windows.Data.Binding("YValue");
                ser2.ItemsSource = data;
                var dpStyle = new Style();
                dpStyle.TargetType = typeof(LineDataPoint);
                dpStyle.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrushGenerated().GetBrush(i)));
                dpStyle.Setters.Add(new Setter(OpacityProperty, 0));
                ser2.DataPointStyle = dpStyle;
                ser2.PolylineStyle = style;
                ser2.AnimationSequence = AnimationSequence.FirstToLast;
                ser2.IsSelectionEnabled = true;
                Chart1.Series.Add(ser2);
                i++;

                global::OxyPlot.LineSeries ls = new global::OxyPlot.LineSeries();
                ls.ItemsSource = data;
                ls.DataFieldX = "Date";
                ls.DataFieldY = "YValue";
                ls.Smooth = true;
                tmp.Series.Add(ls);
            }
            plot1.Model = tmp;
            lowerBorder.Child = plot1;

            Chart1.IsEnabled = true;
            lblInfo.Content = "Graph generated [All value rebased against 100 for comparison]";
        }

        #region properties which provide convenient access to visual tree elements

        protected System.Windows.Controls.DataVisualization.Charting.DataPointSeries LineSeries
        {
            get
            {
                return ((System.Windows.Controls.DataVisualization.Charting.DataPointSeries)Chart1.Series[0]);
            }
        }

        protected StackPanel LocationIndicator
        {
            get
            {
                return ChartRoot.FindName("LocationIndicator") as StackPanel;
            }
        }

        protected EdgePanel ChartArea
        {
            get
            {
                return ChartRoot.FindName("ChartArea") as EdgePanel;
            }
        }

        protected Grid PlotArea
        {
            get
            {
                return ChartRoot.FindName("PlotArea") as Grid;
            }
        }

        protected Grid Crosshair
        {
            get
            {
                return ChartRoot.FindName("Crosshair") as Grid;
            }
        }

        protected Grid ChartRoot
        {
            get
            {
                // chart area is within a different namescope to this page, therefore
                // we must navigate the visual tree to locate it
                return VisualTreeHelper.GetChild(Chart1, 0) as Grid;
            }
        }

        protected System.Windows.Controls.DataVisualization.Charting.LinearAxis YAxis
        {
            get
            {
                return (System.Windows.Controls.DataVisualization.Charting.LinearAxis)Chart1.Axes[0];
            }
        }

        protected System.Windows.Controls.DataVisualization.Charting.DateTimeAxis XAxis
        {
            get
            {
                return (System.Windows.Controls.DataVisualization.Charting.DateTimeAxis)Chart1.Axes[1];
            }
        }

        #endregion

        #region utility methods

        /// <summary>
        /// Transforms the supplied position on the plot area grid into a point within
        /// the plot area coordinate system
        /// </summary>
        private KeyValuePair<DateTime, double> GetPlotAreaCoordinates(Point position)
        {
            IComparable yAxisHit = ((IRangeAxis)YAxis).GetValueAtPosition(
                new UnitValue(PlotArea.ActualHeight - position.Y, Unit.Pixels));

            IComparable xAxisHit = ((IRangeAxis)XAxis).GetValueAtPosition(
                new UnitValue(position.X, Unit.Pixels));

            return new KeyValuePair<DateTime, double>((DateTime)xAxisHit, (double)yAxisHit);
        }

        #endregion

        #region event handlers

        private void CrosshairContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (LineSeries.ItemsSource == null)
                return;

            Point mousePos = e.GetPosition(PlotArea);
            KeyValuePair<DateTime, double> crosshairLocation = GetPlotAreaCoordinates(mousePos);

            LocationIndicator.DataContext = crosshairLocation;
            Crosshair.DataContext = mousePos;
        }

        private void CrosshairContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            SetCrossHairVisibility(true);
        }

        private void CrosshairContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            SetCrossHairVisibility(false);
        }

        private void SetCrossHairVisibility(bool visible)
        {
            LocationIndicator.Children[0].Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            Crosshair.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ChartRoot.Cursor = visible ? Cursors.None : Cursors.Arrow;
        }

        #endregion

        private ScrollViewer scrollArea;
        private ScrollViewer ScrollArea
        {
            get
            {
                if (scrollArea == null)
                {
                    scrollArea = GetLogicalChildrenBreadthFirst(Chart1).Where(element => element.Name.Equals("ScrollArea")).FirstOrDefault() as ScrollViewer;
                }
                return scrollArea;
            }
        }
        private void ZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Debug.Assert(ChartArea != null && ScrollArea != null, "Zoom should not be called before layout has occurred");

            double zoom = e.NewValue;

            UpdateChart(zoom);
        }
        private void UpdateChart(double zoom)
        {
            ChartArea.Width = ScrollArea.ViewportWidth + (ScrollArea.ViewportWidth * zoom / 100.0);
            try
            {
                (Crosshair.Children[1] as Line).X2 = ChartArea.ActualWidth + 2;
                (Crosshair.Children[0] as Line).Y2 = ChartArea.ActualWidth + 2;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        private IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(FrameworkElement parent)
        {
            //Debug.Assert(parent != null, "The parent cannot be null.");

            Queue<FrameworkElement> queue =
                new Queue<FrameworkElement>(GetVisualChildren(parent).OfType<FrameworkElement>());

            while (queue.Count > 0)
            {
                FrameworkElement element = queue.Dequeue();
                yield return element;

                foreach (FrameworkElement visualChild in GetVisualChildren(element).OfType<FrameworkElement>())
                {
                    queue.Enqueue(visualChild);
                }
            }
        }
        private IEnumerable<DependencyObject> GetVisualChildren(DependencyObject parent)
        {
            //Debug.Assert(parent != null, "The parent cannot be null.");

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int counter = 0; counter < childCount; counter++)
            {
                yield return VisualTreeHelper.GetChild(parent, counter);
            }
        }
        private void NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (Chart1 == null) return;
            //int i = 0;
            //foreach (LineSeries ser2 in Chart1.Series)
            //{
            //    var style = new System.Windows.Style { TargetType = typeof(Polyline) };
            //    style.Setters.Add(new Setter(Shape.StrokeThicknessProperty, polyLine.Value));
            //    var dpStyle = new Style();
            //    dpStyle.TargetType = typeof(LineDataPoint);
            //    dpStyle.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrushGenerated().GetBrush(i)));
            //    dpStyle.Setters.Add(new Setter(OpacityProperty, 0));
            //    ser2.DataPointStyle = dpStyle;
            //    ser2.PolylineStyle = style;
            //    ser2.UpdateLayout();
            //    i++;
            //}
            //if (Chart1 != null)
            //{
            //    UpdateChart(0);
            //}
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            (lowerBorder.Child as Plot).ZoomAt((lowerBorder.Child as Plot).Model.Axes[0], 50);
        }
    }
}