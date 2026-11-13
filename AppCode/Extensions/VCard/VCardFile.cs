namespace AppCode.Extensions.VCard
{
  /// <summary>
  /// Download-ready vCard file returned by <see cref="VCardService"/>.
  /// </summary>
  public class VCardFile
  {
    public byte[] Contents { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
  }
}
