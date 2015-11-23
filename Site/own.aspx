<%@ Page Title="Urho - Omat Suoritukset" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="own.aspx.cs" Inherits="own" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <h1>Omat Suoritukset</h1>
    <asp:Label ID="lblUser" runat="server" />
    <asp:DropDownList ID="ddlUser" runat="server" Visible="false" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" ViewStateMode="Enabled" AutoPostBack="True" />
    <asp:Button ID="btnLogOut" runat="server" Visible="false" OnClick="btnLogOut_Click" Text="Valitse joku muu"/>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:HyperLinkField
                HeaderText="Nimi"
                DataTextField="Nimi"
                DataNavigateUrlFormatString="~/add.aspx?id={0}"
                DataNavigateUrlFields="Id" />
            <asp:BoundField DataField="Nimi" HeaderText="Nimi" />
            <asp:BoundField DataField="Pvm" HeaderText="Päivämäärä" />
            <asp:BoundField DataField="Laji" HeaderText="Laji" />
            <asp:BoundField DataField="Kesto" HeaderText="Kesto(min)" />
        </Columns>
    </asp:GridView>
</asp:Content>

