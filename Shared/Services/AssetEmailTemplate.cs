using System;

namespace Shared.Services
{
    public static class AssetEmailTemplate
    {
        public static string GetAssetEmailTemplate(string flag, string empDtls, string userName)
        {
            if (flag == "Asset Create") {
                return AssetEmailTemplate.Request();
            }
            else if (flag == "Asset Assigning") {
                return AssetEmailTemplate.Assigning(empDtls);
            }
            else if (flag == "Asset Approved") {
                return AssetEmailTemplate.Approved(empDtls);
            }
            else if (flag == "Asset User") {
                return AssetEmailTemplate.User(empDtls);
            }

            return string.Empty;
        }

        public static string Request()
        {
            var template = string.Format(@"<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear Concern,</p>
            <p style='font-family: Century Gothic, serif; font-size: 14px;'>This is to inform you that you have few assets that have been added to the stock which requires your Approval.</p>
            <p style='font-family: Century Gothic, serif; font-size: 14px;'>You are requested to log in to your ERP system to check the asset details by clicking <a href='https://hris.myrecombd.com/login'>here</a>.</p>
            <br/>
            <p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regrads</p>
            <p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>
            <p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>");
            return template;
        }


        public static string Assigning(string empDtls)
        {
            var template = "<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear Concern,</p>" +
                           "<p style='font-family: Century Gothic, serif; font-size: 14px;'>This is to inform you that a few assets have been assigned to <b>" + empDtls + "</b> that require your Approval.</p>" +
                           "<p style='font-family: Century Gothic, serif; font-size: 14px;'>You are requested to log in to your ERP system to check the asset details by clicking <a href='https://hris.myrecombd.com/login'>here</a>.</p>" +
                           "<br/>" +
                           "<p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regards</p>" +
                           "<p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>" +
                           "<p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>";
            return template;
        }


        public static string Approved(string empDtls)
        {
            var template = "<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear Concern,</p>" +
                          "<p style='font-family: Century Gothic, serif; font-size: 14px;'>This is to inform you that you have few assets that have been added to the stock.</p>" +
                          "<p style='font-family: Century Gothic, serif; font-size: 14px;'>You are requested to log in to your ERP system to check the asset details by clicking <a href='https://hris.myrecombd.com/login'>here</a>.</p>" +
                          "<br/>" +
                          "<p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regards</p>" +
                          "<p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>" +
                          "<p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>";
            return template;
        }

    

        public static string User(string empDtls)
        {
            var template = "<p style='font-family: Century Gothic, serif; font-size: 14px;'>Dear,</p>" +
                            "<p style='font-family: Century Gothic, serif; font-size: 14px;'><b>" + empDtls + "</b>,</p>" +
                            "<p style='font-family: Century Gothic, serif; font-size: 14px;'>This is to inform you that you have received assets from the Administration Department.</p>" +
                            "<p style='font-family: Century Gothic, serif; font-size: 14px;'>You are requested to log in to your ERP system to check the received asset details by clicking <a href='https://hris.myrecombd.com/login'>here</a>.</p>" +
                            "<br/>" +
                            "<p style='font-family: Century Gothic, serif; font-size: 13px;'>Best Regards</p>" +
                            "<p style='font-family: Century Gothic, serif; font-size: 13px;'><b>Recom Consulting Limited</b></p>" +
                            "<p style='font-family: Century Gothic, serif; font-size: 10px;color:#FF0000'>This is an auto-generated mail and should not be replied to.</p>";
            return template;
        }






    }

}
