<%@ Page Title="Urho - Kaikki Suoritukset" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="all.aspx.cs" Inherits="all" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <h1>Viimeisimmät Suoritukset</h1>
    <p>
        Valitse aikaväli:
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>Alku:</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="txtStart" runat="server"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell>
                    <asp:Calendar ID="cldStart"
                                  runat="server"
                                  Visible="false"
                                  OnSelectionChanged="cldStart_SelectionChanged" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Loppu:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="txtEnd" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell>
                    <asp:Calendar ID="cldEnd"
                                  runat="server"
                                  Visible="false"
                                  OnSelectionChanged="cldEnd_SelectionChanged" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button runat="server" Text="Hae Viimeisimmät" OnClick="btnGetAll_Click" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button runat="server" Text="Hae Päivämäärällä" OnClick="btnGet_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </p>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:HyperLinkField
                HeaderText="Nimi"
                DataTextField="Nimi"
                DataNavigateUrlFormatString="~/own.aspx?name={0}"
                DataNavigateUrlFields="Nimi" />
            <asp:BoundField DataField="Pvm" HeaderText="Päivämäärä" />
            <asp:BoundField DataField="Laji" HeaderText="Laji" />
            <asp:BoundField DataField="Kesto" HeaderText="Kesto(min)" />
        </Columns>
    </asp:GridView>
    <div style="display: none;">
        <asp:Button ID="btnTxtStartEventHandler" runat="server" OnClick ="btnTxtStartEventHandler_Click" />
        <asp:Button ID="btnTxtEndEventHandler" runat="server" OnClick ="btnTxtEndEventHandler_Click" />
    </div>
</asp:Content>

