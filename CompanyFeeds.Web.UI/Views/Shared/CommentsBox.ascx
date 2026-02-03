<%@ Control Language="C#" Inherits="CompanyFeeds.Web.UI.Views.Shared.CommentsBox" %>
<div id="comments">
	<h3>Comments</h3>
	<asp:Repeater runat="server" ID="rep">
		<HeaderTemplate><ul></HeaderTemplate>
		<ItemTemplate>
			<li>
				<div>
					<strong>
					<a href="#c<%# Eval("Id") %>" id="c<%# Eval("Id") %>" name="c<%# Eval("Id") %>">#</a>
					<%# Eval("Date", "{0:D}") %>
					by 
					<%# Convert.ToInt32(Eval("User.Id")) > 0 ? Html.ActionLinkFormatted(Eval("User.Name").ToString(), "Detail", "Users", new{id=Convert.ToInt32(Eval("User.Id"))}, null) : "<span>" + Eval("User.Name") + "</span>"%>
					</strong>
				</div>
				<p><%# Eval("Value") %></p>
			</li>
		</ItemTemplate>
		<FooterTemplate></ul></FooterTemplate>
	</asp:Repeater>
	
	<div id="fb-root"></div><script src="http://connect.facebook.net/en_US/all.js#appId=203002446400259&amp;xfbml=1"></script><fb:comments href="<%="http://" + Request.Url.Host +  Request.Url.PathAndQuery %>" num_posts="3" width="620"></fb:comments>
	<UC:HtmlContainer runat="server" id="pnlForm" Visible="false">
		<script type="text/javascript">
		//old ms mvc ajax
		function submitComment(form)
		{
			if (validateComments())
			{
				$("input, label, textarea", form).attr("disabled", "disabled");
				
				var params = new Array();
				params["entryId"] = $("#entryId").val();
				params["value"] = $("#commentValue").val();
				params["notify"] = $("#commentNotify:checked").length > 0 ? "true" : "false";
				if ($("#commentName").length > 0)
				{
					params["name"] = $("#commentName").val();
					params["email"] = $("#commentEmail").val();
				}
				MvcAjax.MvcHelpers.invokePost("/comments/add/", params, submitCommentCallback);
			}
		}
		
		function validateComments()
		{
			if ($("#commentValue").val().length == 0)
			{
				return false;
			}
			if ($("#commentName").length > 0)
			{
				if ($("#commentName").val().length == 0 || $("#commentEmail").val().length == 0)
				{
					alert("Name and Email must not be blank.");
					return false;
				}
			}
			return true;
		}
		
		function submitCommentCallback(result)
		{
			$("#commentSent").show("slow");
		}		
		</script>
		<form onsubmit="submitComment(this);return false;" action="">
			<h4>Leave a comment</h4>
			<UC:HtmlContainer runat="server" id="pnlNoMember" visible="false">
				<div class="item">
					<label for="commentName">Name</label>
					<input type="text" id="commentName" class="text" />
				</div>
				<div class="item">
					<label for="commentEmail">Email</label>
					<input type="text" id="commentEmail" class="text" />
				</div>
			</UC:HtmlContainer>
			<div>
				<textarea id="commentValue" rows="" cols=""></textarea>
			</div>
			<div>
				<label for="commentNotify">Notify me of followup comments by email</label>
				<input type="checkbox" checked="checked" id="commentNotify" />
			</div>
			<br />
			<div><input type="submit" class="button" value="Send &gt;&gt;" /></div>
			<div id="commentSent" style="display:none;">
				<p style="padding-top: 20px;"><strong>The message has been sent</strong></p>
			</div>
		</form>
	</UC:HtmlContainer>
</div>