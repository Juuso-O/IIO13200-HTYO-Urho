<%@ Page Title="Urho - Top-liikkujat" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="top.aspx.cs" Inherits="top" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <h1>Top-liikkujat</h1>
    <p>
        Järjestä:
        <asp:Button ID="btnDuration" runat="server" Text="Kokonaisajan" OnClick="btnDuration_Click" />
        <asp:Button ID="btnTimes" runat="server" Text="Kertojen" OnClick="btnTimes_Click" />
         mukaan
    </p>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:HyperLinkField
                HeaderText="Nimi"
                DataTextField="Nimi"
                DataNavigateUrlFormatString="~/own.aspx?name={0}"
                DataNavigateUrlFields="Nimi" />
            <asp:BoundField DataField="Kerrat" HeaderText="Kerrat" />
            <asp:BoundField DataField="Kesto" HeaderText="Kesto" />
        </Columns>
    </asp:GridView>
</asp:Content>

