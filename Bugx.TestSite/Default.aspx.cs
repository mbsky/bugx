using System;
using System.Web.UI;
using Bugx.Test.Model;

public partial class _Default : Page
{
    public int? SessionValue
    {
        get{ return (int?) Session["SessionValue"]; }
        set { Session["SessionValue"] = value; }
    }

    public Category CurrentCategory
    {
        get{ return (Category) Session["CurrentCategory"]; }
        set { Session["CurrentCategory"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack && SessionValue == null)
        {
            throw new Exception("Session expired");
        }
        if (SessionValue == null)
        {
            SessionValue = 125;
            CurrentCategory = Sample.BuildCategory();
        }
        
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        int value = Convert.ToInt32(RadioButtonList1.SelectedValue);
        long delta = DateTime.Now.Ticks/value;
    }
}