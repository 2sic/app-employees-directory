#if NETCOREAPP // Oqtane
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#else // DNN
using System.Web;
using System.Web.Http;
#endif
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using ToSic.Razor.Blade;

public class VCardController : Custom.Hybrid.Api12
{
  [HttpGet]
  [AllowAnonymous]
  public dynamic Get(string id)
  {
    var personEntity = App.Query["PersonUrlKey"].List.FirstOrDefault();
    if (personEntity == null) throw new Exception("Can't find person with id " + id);
    var person = AsDynamic(personEntity);

    var card = new VCard
    {
        FirstName = person.FirstName,
        LastName = person.LastName,
        Organization = Settings.CompanyName,
        JobTitle = person.Function,
        StreetAddress = person.Street,
        Zip = person.ZipCode,
        City = person.City,
        CountryName = person.Country,
        Phone = person.Phone,
        PhoneCompany = Settings.CompanyPhone,
        // Mobile = person.,// queryString["mobile"],
        Email = person.EMail,
        Url = Settings.CompanyUrl,
    };

    if (Text.Has(person.Image))
      card.PhotoPath = Link.Image(GetAbsoluteUrl(person.Image), width: 92, height: 92, quality: 0, format: "jpg");

    var mimeType = "text/vcard";
#if NETCOREAPP // Oqtane
    var response = Response;
#else // DNN
    var response = HttpContext.Current.Response;
#endif
    response.ContentType = mimeType;
    var fileName = card.FirstName + " " + card.LastName;
    if (string.IsNullOrWhiteSpace(fileName))
        fileName = card.Organization;
    if (string.IsNullOrWhiteSpace(fileName))
        fileName = "contact";
    fileName += ".vcf";

    //// source: http://stackoverflow.com/questions/93551/how-to-encode-the-filename-parameter-of-content-disposition-header-in-http
    //string contentDisposition;
    //if (Request.Browser.Browser == "IE")    // removed version check
    //    contentDisposition = "attachment; filename=" + Uri.EscapeDataString(fileName);
    //else if (context.Request.Browser.Browser == "Safari")
    //    contentDisposition = "attachment; filename=" + fileName;
    //else
    //    contentDisposition = "attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName);
    //response.AddHeader("Content-Disposition", contentDisposition);

    ////response.AddHeader("Content-Disposition", "attachment; fileName=" + fileName + ".vcf");

    var cardString = card.ToString();
    var inputEncoding = Encoding.Default;
    var outputEncoding = Encoding.GetEncoding(28591); // ISO-8859-1
    var cardBytes = inputEncoding.GetBytes(cardString);
    var outputBytes = Encoding.Convert(inputEncoding, outputEncoding, cardBytes);

    return File(download: false, contents: outputBytes, contentType: mimeType, fileDownloadName: Path.GetFileName(fileName));
  }

  internal class VCard
  {
    private const string AddressType = "WORK";
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Organization { get; set; }
    public string JobTitle { get; set; }
    public string StreetAddress { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string CountryName { get; set; }
    public string Phone { get; set; }
    public string PhoneCompany { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Url { get; set; }
    public string PhotoPath { get; set; }
    public override string ToString()
    {
      var builder = new StringBuilder();
      builder.AppendLine("BEGIN:VCARD");
      builder.AppendLine("VERSION:2.1");
      // Name
      builder.AppendLine("N;CHARSET=iso-8859-1:" + LastName + ";" + FirstName);
      // Full name
      if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(FirstName))
          builder.AppendLine("FN;CHARSET=iso-8859-1:" + Organization);
      else
          builder.AppendLine("FN;CHARSET=iso-8859-1:" + FirstName + " " + LastName);
      // Address
      builder.Append("ADR;" + AddressType + ";PREF;CHARSET=iso-8859-1:;;");
      builder.Append(StreetAddress + ";");
      builder.Append(City + ";;");
      builder.Append(Zip + ";");
      builder.AppendLine(CountryName);
      // Other data
      builder.AppendLine("ORG;CHARSET=iso-8859-1:" + Organization);
      builder.AppendLine("TITLE;CHARSET=iso-8859-1:" + JobTitle);
      builder.AppendLine("TEL;" + AddressType + ";VOICE;CHARSET=iso-8859-1:" + Phone);
      if (!string.IsNullOrWhiteSpace(PhoneCompany))
        builder.AppendLine("X-MS-TEL;VOICE;COMPANY:" + PhoneCompany);
      builder.AppendLine("TEL;CELL;VOICE:" + Mobile);
      builder.AppendLine("URL;" + AddressType + ":" + Url);
      builder.AppendLine("EMAIL;PREF;INTERNET:" + Email);

      // Add image
      if (PhotoPath != null)
      {
        builder.AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:");
        builder.AppendLine(CreateThumbnail(PhotoPath));
        builder.AppendLine(string.Empty);
      }

      builder.AppendLine("END:VCARD");

      return builder.ToString();
    }
  }

  internal static string CreateThumbnail(string absoluteUrl)
  {
      // web request to image, so we can use image resizer for image preparation
      var httpClient = new HttpClient();
      var byteArray = httpClient.GetByteArrayAsync(absoluteUrl).Result;
      return System.Convert.ToBase64String(byteArray);
  }

  internal string GetAbsoluteUrl(string relativeUrl)
  {
#if NETCOREAPP // Oqtane
    var request = Request;
#else // DNN
    var request = HttpContext.Current.Request.Url;
#endif
    return string.Concat(
      request.Scheme,
      "://",
      request.Host,
      relativeUrl);
  }
}
