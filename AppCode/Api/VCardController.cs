#if NETCOREAPP // Oqtane
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#else // DNN
// 2sxclint:disable:no-web-namespace
using System.Web.Http;
using FromQueryAttribute = System.Web.Http.FromUriAttribute;
#endif
using System;
using System.Threading.Tasks;
using AppCode.Extensions.VCard;

public class VCardController : Custom.Hybrid.ApiTyped
{
  [HttpGet]
  [AllowAnonymous]
  public async Task<object> Get([FromQuery] string id)
  {
    var person = AsItem(App.GetQuery("PersonUrlKey"));
    if (person == null) throw new Exception("Can't find person with id " + id);

    // This is the only app-specific part: map any source fields to a VCard.
    var card = new VCard {
      FirstName = person.String("FirstName"),
      LastName = person.String("LastName"),
      Organization = App.Settings.String("CompanyName"),
      JobTitle = person.String("Function"),
      StreetAddress = person.String("Street"),
      Zip = person.String("ZipCode"),
      City = person.String("City"),
      CountryName = person.String("Country"),
      Phone = person.String("Phone"),
      PhoneCompany = App.Settings.String("CompanyPhone"),
      Mobile = person.String("Mobile"),
      Email = person.String("EMail"),
      Url = App.Settings.Url("CompanyUrl"),
      PhotoUrl = string.IsNullOrWhiteSpace(person.String("Image"))
        ? null
        : Link.Image(person.Url("Image"), width: 250, height: 250, quality: 80, format: "jpg", type: "full"),
    };

    var result = await GetService<VCardService>().CreateAsync(card);
    return File(download: true, contents: result.Contents, contentType: result.ContentType, fileDownloadName: result.FileName);
  }
}

