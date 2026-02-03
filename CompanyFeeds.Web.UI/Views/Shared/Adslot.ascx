<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
	var values = new RouteValueDictionary(Model);
	var width = (int?)values["Width"];
	var height = (int?)values["Height"];
	//var slotId = Convert.ToString(values["SlotId"]);
	if (width == null || width == null)
	{
		throw new ArgumentException("Width and Height cannot be null on the model passed to Adslot partial view.");
	}
	if (!ViewData.Get<bool>("HideAdvertising", false))
	{
	    if (width == 728 && height == 90)
	    {
%>
            <script id="mNCC" language="javascript">  medianet_width = '728'; medianet_height = '90'; medianet_crid = '771785483';  </script>  <script id="mNSC" src="http://contextual.media.net/nmedianet.js?cid=8CUQ749VL" language="javascript"></script> 
<%
	    }
        else if (width == 160 && height == 600)
        {
            //Master right
%>
            <script id="mNCC" language="javascript">  medianet_width='160';  medianet_height= '600';  medianet_crid='984948686';  </script>  <script id="mNSC" src="http://contextual.media.net/nmedianet.js?cid=8CUQ749VL" language="javascript"></script> 
<%
        }
	    else if (width == 300 && height == 250) 
	    {
%>
            <script id="mNCC" language="javascript">  medianet_width = '300'; medianet_height = '250'; medianet_crid = '371734240';  </script>  <script id="mNSC" src="http://contextual.media.net/nmedianet.js?cid=8CUQ749VL" language="javascript"></script>
<%
	    }
	}
%>


<%-- CHITIKA
        <script type="text/javascript">
            ch_client = "jorgebg";
            ch_width = <%=width%>;
        ch_height = <%=height%>;
            ch_type = "mpu";
            ch_sid = "Chitika Default";
            ch_color_site_link = "0000CC";
            ch_color_title = "0000CC";
            ch_color_border = "FFFFFF";
            ch_color_text = "000000";
            ch_color_bg = "FFFFFF";
        </script>
        <script src="http://scripts.chitika.net/eminimalls/amm.js" type="text/javascript">
        </script>

--%>