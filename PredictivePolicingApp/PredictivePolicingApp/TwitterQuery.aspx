<%@ Page Title="" Language="C#" MasterPageFile="~/Policing_Master.Master" AutoEventWireup="true" CodeBehind="TwitterQuery.aspx.cs" Inherits="PredictivePolicingApp.TwitterQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid" id="wrapper">
        <div class="row">
            <main class="col-xs-12 col-sm-8 col-lg-9 col-xl-10 pt-3 pl-4 ml-auto">
                <header class="page-header row justify-center">
                    <div class="col-md-6 col-lg-8">
                        <h1 class="float-left text-center text-md-left">Twitter Query</h1>
                    </div>
                    <div class="dropdown user-dropdown col-md-6 col-lg-4 text-center text-md-right">
                    </div>
                    <div class="clear"></div>
                </header>
                <section class="row">
                    <div class="col-sm-12">
                        <section class="row">
                            <div class="col-12">
                                <div class="card mb-4">
                                    <div class="card-block">
                                        <h3 class="card-title">Twitter Parameters</h3>
                                        <form class="form" action="#">
                                            <div class="form-group row">
                                                <label class="col-md-3 col-form-label">Category Query</label>
                                                <div class="col-md-9">
                                                    <input class="form-control" type="text" name="placeholder" placeholder="Crime">
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-md-3 col-form-label">Twitter Handle Query</label>
                                                <div class="col-md-9">
                                                    <input class="form-control" type="text" name="placeholder" placeholder="@CrimeSA">
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-md-3 col-form-label">Keyword Query</label>
                                                <div class="col-md-9">
                                                    <input class="form-control" type="text" name="placeholder" placeholder="Robbery">
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </section>
                <section class="row">
                    <div class="col-sm-12">
                        <section class="row">
                            <div class="col-12">
                                <div class="card mb-4">
                                    <div class="card-block">
                                        <h3 class="card-title">Twitter Feed</h3>
                                        <form class="form" action="#">
                                            <%-- The Twitter Feed --%>
                                            <div id="queryFeed" runat="server" contenteditable="true">

                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </section>
            </main>


        </div>
    </div>
</asp:Content>
