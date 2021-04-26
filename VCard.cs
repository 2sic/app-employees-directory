public class VCard: Custom.Hybrid.Code12
{
  public string Link(dynamic employee) {
    return App.Path + "/vCard.ashx"
      + "?FirstName=" + employee.FirstName
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