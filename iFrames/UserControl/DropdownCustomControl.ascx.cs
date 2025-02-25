using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace iFrames.Pages
{
    public partial class DropdownCustomControl : System.Web.UI.UserControl
    {
        [Themeable(true)]
        public DropDownList ddlfund
        {
            get
            {
                return this.drpDownFund;
            }
            set
            {
                this.drpDownFund = value;
            }
        }

        public DropDownList ddlscheme
        {
            get
            {
                return this.drpDownScheme;
            }
            set
            {
                this.drpDownScheme = value;
            }
        }
        public Label lblfund
        {
            get
            {
                return this.LblFund;
            }
            set
            {
                this.LblFund = value;
            }
        }
        public Label lblscheme
        {
            get
            {
                return this.LblScheme;
            }
            set
            {
                this.LblScheme = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void drpDownFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            MethodInfo mi = this.Page.GetType().GetMethod("ddlfund_selectedindexchanged", BindingFlags.Public | BindingFlags.Instance);
            if (mi != null)
            {
                mi.Invoke(this.Parent.Page, new object[] { null, null });
            }
        }
    }
}