<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="IA.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>I Proyecto IA</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css">
    <style>body{padding-top:50px;}.starter-template{padding:40px 15px;text-align:center;}</style>
    <link rel="stylesheet" type="text/css" href="~/Style/css.css" media="all" />
</head>
<body>

    <nav class="navbar navbar-inverse navbar-fixed-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#">I Proyecto IA</a>
            </div>

            <div class="collapse navbar-collapse">
                <ul class="nav navbar-nav">
                    <li><a href="#">Analisis de Texto</a></li>
                </ul>
            </div><!--.nav-collapse -->
        </div>
    </nav>

    <div class="container">
        
        <form id="form1" runat="server">

            <div class="Text-Area">    
                <asp:TextBox ID="text_area" runat="server" class="texteditor" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="intro">
                <asp:Button runat="server" Text="Cargar Archivo" OnClick="CargarArchivo_Click" />
                <asp:Button runat="server" Text="Cargar Html" OnClick="HtmlFile_Click" />
                <asp:Button ID="ParseURL" runat="server" OnClick="ParseURL_Click" Text="Cargar URL" />
                <asp:Button ID="btnXml" runat="server" OnClick="Xml_Click" Text="UnZipXML" />
                <asp:Button ID="btnJson" runat="server" OnClick="Json_Click" Text="UnZipJson" />
                <asp:Button ID="buttonCarpeta" runat="server" OnClick="carpeta_click" Text="Cargar Carpeta" />
                <asp:Button ID="buttonTwitter" runat="server" OnClick="twitter_click" Text="Cargar tweets" />
            </div>
            <hr>
            <center><asp:FileUpload ID="FileUpload" runat="server" ></asp:FileUpload></center>
            

        </form>

    </div>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
</body>
</html>
