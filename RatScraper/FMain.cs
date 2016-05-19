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
        internal StopViewManager stopViewManager { get; set; }
        internal HalfRouteViewManager routeViewManager { get; set; }
        internal StopView selectedStopView { get; set; }

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
            this.stopViewManager = new StopViewManager(stopP, null, null, this.StopView_Click);
            this.MouseWheel += this.stopViewManager.MyScrollPanel.MouseWheelScroll_EventHandler;
            this.stopFilterTB_TextChanged(sender, e);

            this.routeViewManager = new HalfRouteViewManager(routeP, null, null, this.RouteView_Click);

            stopFilterTB.BackColor = MyGUIs.Background.Normal.Color;
            this.RegisterControlsToMoveForm(this.titleTL);
        }

        private void stopFilterTB_TextChanged(object sender, EventArgs e)
        {
            stopFilterT.Enabled = false;
            stopFilterT.Enabled = true;
        }

        private void clearSearchB_Click(object sender, EventArgs e)
        {
            stopFilterTB.Text = string.Empty;
        }

        internal void stopFilterT_Tick(object sender, EventArgs e)
        {
            stopFilterT.Enabled = false;
            ListOfIDObjects<Stop> stops = new ListOfIDObjects<Stop>();
            string query = this.stopFilterTB.Text.ToUpperInvariant();
            foreach (Stop stop in this.Database.Stops)
                if ((query.Equals(string.Empty) || stop.Name.ToUpperInvariant().Contains(query)) && stops.GetItemByName(stop.Name) == null)
                    stops.Add(stop);
            this.stopViewManager.SetStops(stops);
            stopFilterIV.TextDescription = string.Format("Ai filtrat {0} / {1} stații", stops.Count, this.Database.Stops.Count);
        }

        internal void StopView_Click(object sender, EventArgs e)
        {
            if (sender == null)
            {
                this.stopViewManager.StopViews.CheckControlAndUncheckAllOthers(null);
                routeIV.TextDescription = " ";
                this.routeViewManager.SetHalfRoutes(new List<HalfRoute>());
                this.stopTimeLB.Items.Clear();
                return;
            }
            this.selectedStopView = sender as StopView;
            this.stopViewManager.StopViews.CheckControlAndUncheckAllOthers(this.selectedStopView);
            routeIV.TextDescription = string.Format("Ești la {0}.", this.selectedStopView.Stop.Name);
            this.routeViewManager.SetHalfRoutes(this.Database.GetHalfRoutesByStopName(this.selectedStopView.Stop.Name));
        }

        internal void RouteView_Click(object sender, EventArgs e)
        {
            HalfRouteView hrView = sender as HalfRouteView;
            hrView.Checked = !hrView.Checked;

            List<HalfRoute> halfRoutes = new List<HalfRoute>();
            foreach (HalfRouteView iHRView in this.routeViewManager.HalfRouteViews)
                if (iHRView.Checked)
                    halfRoutes.Add(iHRView.HalfRoute);
            stopTimeLB.Items.Clear();
            List<KeyValuePair<HalfRoute, StopTime>> stopTimes = Database.GetStopTimes(halfRoutes, this.selectedStopView.Stop, DateTime.Now, new TimeSpan(4, 0, 0));
            foreach (KeyValuePair<HalfRoute, StopTime> stopTime in stopTimes)
                stopTimeLB.Items.Add(stopTime.Key.Route.ID + " - " + stopTime.Value);
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
