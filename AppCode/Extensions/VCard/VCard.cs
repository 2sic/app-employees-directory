namespace AppCode.Extensions.VCard
{
  /// <summary>
  /// Contact data used to create a vCard.
  /// </summary>
  public class VCard
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Organization { get; set; }
    public string JobTitle { get; set; }
    public string StreetAddress { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string CountryName { get; set; }
    public string Phone { get; set; }
    public string PhoneCompany { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Url { get; set; }

    /// <summary>
    /// Absolute URL of an image to download and embed in the vCard.
    /// The URL itself won't be included in the vCard. A supplied
    /// <see cref="PhotoBase64"/> takes precedence.
    /// </summary>
    public string PhotoUrl { get; set; }
    public string PhotoBase64 { get; set; }
    public string PhotoType { get; set; } = "JPEG";
    public string FileName { get; set; }
  }
}
