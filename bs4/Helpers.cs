using ToSic.Razor.Blade;
using System.Linq;
using System;

public class Helpers: Custom.Hybrid.Code12
{
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


