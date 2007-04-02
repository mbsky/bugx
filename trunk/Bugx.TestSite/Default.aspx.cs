using System;
using System.Web.UI;

public partial class _Default : Page
{
    [Serializable]
    public class tmp
    {
        public string _a;
        public string _b;
        public tmp(string a, string b)
        {
            _a = a;
            _b = b;
        }
    }
    public int? SessionValue
    {
        get{ return (int?) Session["SessionValue"]; }
        set { Session["SessionValue"] = value;
            Session["TestValue"] = new tmp("lol", "hehe");
        }
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
        }
        
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        int value = Convert.ToInt32(RadioButtonList1.SelectedValue);
        long delta = DateTime.Now.Ticks/value;
    }
}