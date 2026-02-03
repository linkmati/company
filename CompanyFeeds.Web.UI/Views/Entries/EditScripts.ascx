<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.BaseViewUserControl" %>
<script src="//tinymce.cachefly.net/4.0/tinymce.min.js"></script>
<script type="text/javascript">
	tinymce.init({
		selector: "#Content",
		content_css: "/styles/common.css?2014",
		statusbar: false,
		menubar : false,
		height: 220,
		plugins: "link"
	});
	$(document).ready(function(){
		if (!$("#CompanyName").is(":disabled"))
		{
			site.initAutocomplete('#CompanyName', '<%=Url.Action("Search", "Companies") %>');
			//Fix roundtrip problem
			if ($("#CompanyName").val() != "")
			{
				$("#CompanyId").val("");
			}
			$("#entryForm").bind("submit", function(){
				var submit = verifyCompany();
				if (submit)
				{
					$("#entryForm input[type='submit']").attr("disabled", "disabled");
				}
				return submit;
			});
		}
	});
</script>
<script type="text/javascript">
	function verifyCompany()
	{		
		var input = $("#CompanyName");
		var companyName = input.val();
		var submit = false;
		if ($.trim(companyName) != "")
		{
			if (input.data("autocomplete").selectedItem != null)
			{
				var companyId = input.data("autocomplete").selectedItem.Key;
				$("#CompanyId").val(companyId);
				submit = true;
			}
			else
			{
				//Check using a request
				$.post('<%=Url.Action("Get", "Companies") %>', {companyName:companyName}, function (id){
					if (id == 0)
					{
						if (confirm(resources.companyDoesNotExist))
						{
							submitEntryForm();
						}
					}
					else
					{
						$("#CompanyId").val(id);
						submitEntryForm();
					}
				});
			}
		}
		else
		{
			$("#CompanyId").val("0");
			submit = true;
		}
		return submit;
	}
				
	function submitEntryForm()
	{
		$("#entryForm input[type='submit']").attr("disabled", "disabled");
		$("#entryForm").unbind("submit").submit();
	}
</script>