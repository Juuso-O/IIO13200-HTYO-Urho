<%@ Page Title="Urho - Lisää Suoritus" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="add.aspx.cs" Inherits="add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <h1>Kirjaa liikuntasuorituksesi</h1>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" Text="Nimi" />
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSport" runat="server" Text="Laji" />
            </td>
            <td>
                <asp:DropDownList ID="ddlSport" runat="server" OnSelectedIndexChanged="ddlSport_SelectedIndexChanged" AutoPostBack="True" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNewSportName" runat="server" Text="Uuden lajin nimi" Visible="false"/>
            </td>
            <td>
                <asp:TextBox ID="txtNewSport" runat="server" Visible="false" />
            </td>
            <td>
                <asp:Button ID="btnAddNewSport" runat="server" Text="Lisää laji" OnClick="btnAddNewSport_Click" Visible="false"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDate" runat="server" Text="Pvm" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDuration" runat="server" Text="Kesto(min)" />
            </td>
            <td>
                <asp:TextBox ID="txtDuration" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" />
    <br />
    <asp:Button ID="btnAdd" runat="server" Text="Tallenna Suoritus" OnClick="btnAdd_Click" />
    <asp:Button ID="btnConfirmedAdd" runat="server" Text="Tallenna Suoritus" OnClick="btnConfirmedAdd_Click" Visible="false"/>
</asp:Content>

