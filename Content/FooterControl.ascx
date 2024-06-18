<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FooterControl.ascx.vb" Inherits="AgentsOnline_FooterControl" %>


<ul class="socialGlyph-footer">
    <li><a href="http://facebook.com/Dubairpts" target="_blank"><span class="circles font-icon-social-facebook" aria-hidden="true"></span></a></li>
    <li><a href="http://twitter.com/dubairpts" target="_blank"><span class="circles font-icon-social-twitter" aria-hidden="true"></span></a></li>
    <li><a href="https://www.youtube.com/channel/UCksl36Mv1Tx_oQ11fH2zNrQ" target="_blank"><span class="circles font-icon-social-youtube" aria-hidden="true"></span></a></li>
    <%--<li><a href="#"><span class="circles font-icon-social-vimeo" aria-hidden="true"></span></a></li>--%>
  </ul>
  <center>
  <style type="text/css">
    .footerlink:hover {
    color: #FFFFFF !important;
    }
    .footerlink:active {
    color: #FFFFFF !important;
    }
  
  </style>
  <a class="footerlink" href="TermsAndConditions.aspx" > Terms And Condtions </a> &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a class="footerlink" href="PrivacyPolicy.aspx" > Privacy Policy </a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a class="footerlink" href="ContactUs.aspx"> Contact Us </a>
  </center>
  <div align="center" class="copyright">Elevate Tourism Dubai - Copyright <%= DateTime.Now.Year %></div>

