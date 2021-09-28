using ToSic.Razor.Blade;
using System.Linq;
using System;

public class VCardLink: Custom.Hybrid.Code12
{
  // TODO: @2mh - this is a clear string that comes back, shouldn't be dynamic
  public dynamic VCardParams(dynamic employee) {
    return "?FirstName=" + employee.FirstName
      + "&LastName=" + employee.LastName
      + "&Organization=" + App.Settings.CompanyName
      + "&JobTitle=" + employee.Function
      + "&Address=" + employee.Street
      + "&Zip=" + employee.ZipCode
      + "&City=" + employee.City
      + "&Country=" + employee.Country
      + "&Phone=" + employee.Phone
      + "&PhoneCompany=" + App.Settings.CompanyPhone
      + "&Email=" + employee.EMail
      + "&Url=" + App.Settings.CompanyUrl
      + "&Photo=" + employee.Image;
  }

}


