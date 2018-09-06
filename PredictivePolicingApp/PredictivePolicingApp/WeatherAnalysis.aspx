<%@ Page Title="" Language="C#" MasterPageFile="~/Policing_Master.Master" AutoEventWireup="true" CodeBehind="WeatherAnalysis.aspx.cs" Inherits="PredictivePolicingApp.WeatherAnalysis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid" id="wrapper">
        <div class="row">
            <main class="col-xs-12 col-sm-8 col-lg-9 col-xl-10 pt-3 pl-4 ml-auto">
                <header class="page-header row justify-center">
                    <div class="col-md-6 col-lg-8">
                        <h1 class="float-left text-center text-md-left">Weather Analysis</h1>
                    </div>
                    <div class="dropdown user-dropdown col-md-6 col-lg-4 text-center text-md-right">
                        <button type="button" class="btn btn-lg btn-primary" runat="server" onserverclick="RefreshPage_Click">Refresh</button>
                    </div>
                    <div class="clear"></div>
                </header>
                
                <section class="row">
                    <div class="col-sm-12">
                        <section class="row">
                            <div class="col-12">
                                
                                <form class="form" action="#">
                                    <%-- The Weather Analysis Results --%>
                                    <div id="weatherResults" runat="server" contenteditable="true">
                                        
                                    </div>
                                </form>
                                
                            </div>
                        </section>
                    </div>
                </section>
            </main>

        </div>
    </div>
</asp:Content>
