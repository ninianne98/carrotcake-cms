﻿<%@ Page Title="Logon" Language="C#" MasterPageFile="MasterPages/Public.Master" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Carrotware.CMS.UI.Admin.c3_admin.Logon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
	<asp:Panel ID="panelLogin" runat="server" DefaultButton="loginTemplate$cmdLogon">
		<asp:Login ID="loginTemplate" runat="server" OnLoggingIn="loginTemplate_LoggingIn" OnLoggedIn="loginTemplate_LoggedIn">
			<LayoutTemplate>
				<div style="text-align: left;">
					<table style="width: 300px;">
						<tr>
							<td class="tableback">
								<div style="height: 25px; width: 50px; border: 1px solid #ffffff;">
								</div>
							</td>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								&nbsp;<b class="caption">username</b>&nbsp;<br />
								<asp:TextBox ID="UserName" runat="server" Width="180px" MaxLength="60" ValidationGroup="loginTemplate" />
							</td>
							<td class="tableback" rowspan="2">
								<div style="height: 50px; width: 75px; text-align: right; border: 1px solid #ffffff;">
									<a href="/">
										<img class="imgNoBorder" src="/c3-admin/images/house_go.png" alt="Homepage" title="Homepage" /></a>
								</div>
							</td>
						</tr>
						<tr>
							<td class="tableback">
								<div style="height: 25px; width: 25px; border: 1px solid #ffffff;">
								</div>
							</td>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								<br />
								&nbsp;<b class="caption">password</b>&nbsp;<br />
								<asp:TextBox ID="Password" runat="server" TextMode="Password" Width="180px" MaxLength="60" ValidationGroup="loginTemplate" />
							</td>
						</tr>
						<tr>
							<td class="tableback">
								<div style="height: 25px; width: 25px; border: 1px solid #ffffff;">
								</div>
							</td>
							<td class="tableback">
								&nbsp;
							</td>
							<td class="tableback">
								<asp:Button ID="cmdLogon" runat="server" Text="Logon" CommandName="Login" OnClick="cmdLogon_Click" ValidationGroup="loginTemplate" />
							</td>
							<td class="tableback">
								&nbsp;
							</td>
						</tr>
					</table>
					<div style="width: 400px; text-align: left; clear: both;">
						<div class="ui-widget" id="divMsg" runat="server">
							<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
								<p>
									<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
									<asp:Label ID="FailureText" runat="server" EnableViewState="False" />
								</p>
							</div>
						</div>
						<div>
							<p>
								<a href="ForgotPassword.aspx">Forgot Password?</a>
							</p>
						</div>
					</div>
				</div>
			</LayoutTemplate>
		</asp:Login>
	</asp:Panel>
</asp:Content>
