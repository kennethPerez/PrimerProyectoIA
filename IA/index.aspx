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
                  console.log(response);
                  for (i = 0; i < response.data.length; i++) {
                      var temp = "";
                      if (response.data[i].message != undefined) {
                          temp += response.data[i].message;
                      }
                      if (response.data[i].story != undefined) {
                          if (temp != "") {
                              temp += " - " + response.data[i].story;
                          }
                          else {
                              temp += response.data[i].story;
                          }
                      }
                      respuesta += temp + "\n\n";

                  }
                  document.getElementById("text_area").innerHTML += respuesta;
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
              FB.logout(function () { document.location.reload(); });
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

            <div class="collapse navbar-collapse">
                <ul class="nav navbar-nav">
                    <li><a href="#">Analisis de Texto</a></li>
                </ul>
            </div>
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
                <asp:Button ID="button1" runat="server" OnClick="categorizar" Text="prueba categorizacion" />
                
            </div>
            <hr>
            <center><asp:FileUpload ID="FileUpload" runat="server" ></asp:FileUpload></center>
            

        </form>
        <button onclick="loginFB()">Cargar post</button>
        <div id="imagen"> </div>
        <div id="informacion"> </div>

    </div>

 

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
</body>
</html>
