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

        public FMain(FMain mainForm)
            : base(null)
        {
            InitializeComponent();
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            this.Database = new Database();

            this.RegisterControlsToMoveForm(this.titleLabel1);
        }
    }
}
