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

    <script type="text/javascript" src="//www.google.com/jsapi"></script>
</head>
<body>

    <script>

          window.fbAsyncInit = function () {
              FB.init({
                  appId: '981065308595932', 
                  status: true, 
                  posts: true, 
                  cookie: true, 
                  xfbml: true,
                  version: 'v2.5'
              });              
          };


          function getUserInfo() {
              FB.api('/me/picture?type=normal', function (response) {
                  var str = "<img src='" + response.data.url + "'/>";
                  document.getElementById("imagen").innerHTML += str;
              });

              FB.api('/me', function (response) {
                  var str = "<b>Name</b> : " + response.name + "<br>";
                  str += "<b>id: </b>" + response.id + "<br>";
                  str += "<input type='button' value='Logout' onclick='Logout();'/>";
                  document.getElementById("informacion").innerHTML = str;
              });

              FB.api("/me/feed?limit=9999999", function (response) {
                  var respuesta = "";
                  for (i = 0; i < response.data.length; i++) {
                      if (response.data[i].message != undefined) {
                          respuesta += "{'post':'"+response.data[i].message+"'}^";
                      }
                  }
                  document.getElementById("text_area").innerHTML += respuesta;
                  document.getElementById("post").innerHTML = "True";
              });
          }

          function loginFB() {
              FB.login(function (response)
              {
                  if (response.authResponse) {
                      getUserInfo();
                  } else {
                      console.log('User cancelled login or did not fully authorize.');
                  }
              },
              {
                  scope: 'email, user_photos, user_posts',
                  auth_type: 'rerequest'
              });

          }

          function Logout() {
              FB.logout(function () { document.location.reload(); document.getElementById("post").innerHTML = "False"; });
          }


          (function (d) {
              var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
              if (d.getElementById(id)) { return; }
              js = d.createElement('script'); js.id = id; js.async = true;
              js.src = "//connect.facebook.net/en_US/all.js";
              ref.parentNode.insertBefore(js, ref);
          }(document));


      </script>   

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
        </div>
    </nav>

    <div class="container">
        
        <center>
            <br>
            <button onclick="loginFB()">Cargar post</button>
            <div id="imagen"> </div>
            <div id="informacion"> </div>
            <div style="display:none;" id="post">False</div>
        </center>

        <form id="form1" runat="server">
            <br>
            <center>
                <div class="intro">
                    <asp:Button runat="server" Text="Cargar Archivo" OnClick="CargarArchivo_Click" />
                    <asp:Button runat="server" Text="Cargar Html" OnClick="HtmlFile_Click" />
                    <asp:Button ID="ParseURL" runat="server" OnClick="ParseURL_Click" Text="Cargar URL" />
                    <asp:Button ID="btnXml" runat="server" OnClick="Xml_Click" Text="UnZipXML" />
                    <asp:Button ID="btnJson" runat="server" OnClick="Json_Click" Text="UnZipJson" />
                    <asp:Button ID="buttonCarpeta" runat="server" OnClick="carpeta_click" Text="Cargar Carpeta" />
                    <asp:Button ID="buttonTwitter" runat="server" OnClick="twitter_click" Text="Cargar tweets" />
                    <asp:Button ID="buttonLanguage" runat="server" OnClick="idioma_click" Text="Detectar Idioma" />
                    <asp:Button ID="buttonNaiveBayes" runat="server" OnClick="categorizar" Text="Categorizar" /> 
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Facebook" oncheckedchanged="facebook" />
                    <!--<asp:Button ID="button1" runat="server" OnClick="aprender" Text="Agregar Muestra y aprender palabras" />
                    <asp:Button ID="button2" runat="server" OnClick="aprenderMuestra" Text="Agregar Muestra" />      -->           
                </div>
            </center>
            <hr>
            <center><asp:FileUpload ID="FileUpload" runat="server" ></asp:FileUpload></center>
          
            <hr>
            <div class="Text-Area">    
                <asp:TextBox ID="text_area" runat="server" class="texteditor" TextMode="MultiLine"></asp:TextBox>
            </div>           
            
            <hr>
            <asp:Label Font-Size="Large" ForeColor="#5c7fa0" ID="language" runat="server"></asp:Label>
            <div class="col-md-12">
                <div id="chartResult" class="col-md-6">  <asp:Literal ID="ltResult" runat="server"></asp:Literal></div>                    
                <div id="chartBase" class="col-md-6">  <asp:Literal ID="ltBase" runat="server"></asp:Literal></div>
            </div>
            <br />    
            <hr />
            <br />
            <div class="col-md-12">
                <div class="col-md-6"> 
                    <br>
                    <br>
                    <br>
                    <br>
                    <asp:Label Font-Size="Large" ForeColor="#5c7fa0" ID="usuariosAnalizados" runat="server"></asp:Label>
                    <br>
                    <asp:Label Font-Size="Large" ForeColor="#5c7fa0" ID="teewsAnalizados" runat="server"></asp:Label>
                </div>                    
                <div id="chartJson" class="col-md-6">  <asp:Literal ID="LiteralMessage" runat="server"></asp:Literal></div>
            </div> 

            <br />    
            <hr />
            <br />
            <asp:Label Font-Size="Large" ForeColor="#5c7fa0" ID="categoria" runat="server"></asp:Label>
            <br />
            <asp:Label Font-Size="Large" ForeColor="#5c7fa0" ID="response" runat="server"></asp:Label>
            <div class="col-md-12">                  
                <div id="chartCateTexto" class="col-md-6">  <asp:Literal ID="LiteralCateTexto" runat="server"></asp:Literal></div>
            </div>

        </form>   

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>    
</body>
</html>
