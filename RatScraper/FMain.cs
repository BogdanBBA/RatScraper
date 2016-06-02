using RatScraper.VisualComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RatScraper
{
    public partial class FMain : MyForm
    {
        public Database Database { get; private set; }
        internal StopViewManager stopViewManager1 { get; set; }
        internal StopViewManager stopViewManager2 { get; set; }
        internal HalfRouteViewManager routeViewManager { get; set; }
        internal StopTimeViewManager stopTimeViewManager { get; set; }
        internal StopView selectedStopViewA { get; set; }
        internal StopView selectedStopViewB { get; set; }

        public FMain(FMain mainForm)
            : base(null)
        {
            InitializeComponent();

            this.Database = new Database();

            string checkResult = Paths.CheckPaths(true);
            if (!checkResult.Equals(string.Empty))
            {
                MessageBox.Show(checkResult, "Missing files or folders ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            checkResult = this.Database.LoadDatabase(Paths.DatabaseFile);
            if (!checkResult.Equals(string.Empty))
            {
                MessageBox.Show(checkResult, "Database load ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            this.stopViewManager1 = new StopViewManager(stops1P, null, null, this.Stop1View_Click);
            this.MouseWheel += this.stopViewManager1.MyScrollPanel.MouseWheelScroll_EventHandler;
            this.stopFilter1TB_TextChanged(sender, e);

            this.stopViewManager2 = new StopViewManager(stops2P, null, null, this.Stop2View_Click);
            this.MouseWheel += this.stopViewManager2.MyScrollPanel.MouseWheelScroll_EventHandler;
            this.stopFilter2TB_TextChanged(sender, e);

            this.routeViewManager = new HalfRouteViewManager(routeP, null, null, this.RouteView_Click);

            this.stopTimeViewManager = new StopTimeViewManager(stopTimeP, null, null, this.StopTimeView_Click);

            this.stopFilter1TB.BackColor = MyGUIs.Background.Normal.Color;
            this.stopFilter2TB.BackColor = MyGUIs.Background.Normal.Color;
            this.RegisterControlsToMoveForm(this.titleTL);
        }

        private void stopFilter1TB_TextChanged(object sender, EventArgs e)
        {
            this.stopFilter1T.Enabled = false;
            this.stopFilter1T.Enabled = true;
        }

        private void stopFilter2TB_TextChanged(object sender, EventArgs e)
        {
            this.stopFilter2T.Enabled = false;
            this.stopFilter2T.Enabled = true;
        }

        private void clearSearch1B_Click(object sender, EventArgs e)
        {
            this.stopFilter1TB.Text = string.Empty;
        }

        private void clearFilter2B_Click(object sender, EventArgs e)
        {
            this.stopFilter2TB.Text = string.Empty;
        }

        internal void stopFilter1T_Tick(object sender, EventArgs e)
        {
            this.stopFilter1T.Enabled = false;
            this.FilterAndSetStops(this.stopFilter1TB, this.stopViewManager1, this.stopFilter1IV);
        }

        private void stopFilter2T_Tick(object sender, EventArgs e)
        {
            this.stopFilter2T.Enabled = false;
            this.FilterAndSetStops(this.stopFilter2TB, this.stopViewManager2, this.stopFilter2IV);
        }

        private void FilterAndSetStops(TextBox filterTB, StopViewManager manager, InfoView infoView)
        {
            ListOfIDObjects<Stop> stops = new ListOfIDObjects<Stop>();
            string query = filterTB.Text.Trim().ToUpperInvariant();
            foreach (Stop stop in this.Database.Stops)
                if ((query.Equals(string.Empty) || stop.Name.ToUpperInvariant().Contains(query)) && stops.GetItemByName(stop.Name) == null)
                    stops.Add(stop);
            manager.SetStops(stops);
            infoView.TextDescription = string.Format("Ai filtrat {0} / {1} stații", stops.Count, this.Database.Stops.Count);
        }

        internal void Stop1View_Click(object sender, EventArgs e)
        {
            if (sender == null)
            {
                this.stopViewManager1.StopViews.CheckControlAndUncheckAllOthers(null);
                routeIV.TextDescription = " ";
                this.routeViewManager.SetHalfRoutes(new List<HalfRoute>());
                this.stopTimeViewManager.SetStopTimeInfos(new List<KeyValuePair<HalfRoute, StopTime>>());
                return;
            }
            this.selectedStopViewA = sender as StopView;
            this.stopViewManager1.StopViews.CheckControlAndUncheckAllOthers(this.selectedStopViewA);
            this.routeIV.TextDescription = string.Format("Ești la {0}.", this.selectedStopViewA.Stop.Name);
            this.routeViewManager.SetHalfRoutes(this.Database.GetHalfRoutesByStopNames(this.selectedStopViewA.Stop.Name, this.selectedStopViewB != null ? this.selectedStopViewB.Stop.Name : null));
        }

        internal void Stop2View_Click(object sender, EventArgs e)
        {
            if (sender == null)
            {
                this.stopViewManager2.StopViews.CheckControlAndUncheckAllOthers(null);
                this.Stop1View_Click(this.selectedStopViewA, e);
                return;
            }
            this.selectedStopViewB = sender as StopView;
            this.stopViewManager2.StopViews.CheckControlAndUncheckAllOthers(this.selectedStopViewB);
            this.Stop1View_Click(this.selectedStopViewA, e);
        }

        internal void RouteView_Click(object sender, EventArgs e)
        {
            HalfRouteView hrView = sender as HalfRouteView;
            hrView.Checked = !hrView.Checked;

            List<HalfRoute> halfRoutes = new List<HalfRoute>();
            foreach (HalfRouteView iHRView in this.routeViewManager.HalfRouteViews)
                if (iHRView.Checked)
                    halfRoutes.Add(iHRView.HalfRoute);
            List<KeyValuePair<HalfRoute, StopTime>> stopTimes = Database.GetStopTimes(halfRoutes, this.selectedStopViewA.Stop, DateTime.Now, new TimeSpan(4, 0, 0));
            this.stopTimeViewManager.SetStopTimeInfos(stopTimes);
            this.stopTimeIV.TextDescription = "Ar fi " + stopTimes.Count + " curs" + (stopTimes.Count == 1 ? "ă" : "e");
        }

        private void StopTimeView_Click(object sender, EventArgs e)
        {
            //
        }

        private void updateB_Click(object sender, EventArgs e)
        {
            new FWork(this, FWork.WorkType.Update).ShowDialog(this);
        }

        private void exitB_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
