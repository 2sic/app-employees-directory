using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppCode.Extensions.VCard
{
  /// <summary>
  /// Creates vCard files from data supplied by any 2sxc app.
  /// </summary>
  public class VCardService : Custom.Hybrid.CodeTyped
  {
    public async Task<VCardFile> CreateAsync(VCard card)
    {
      if (card == null)
        throw new ArgumentNullException(nameof(card));

      var photoBase64 = string.IsNullOrWhiteSpace(card.PhotoBase64)
        ? await DownloadPhotoAsync(card.PhotoUrl)
        : card.PhotoBase64;

      return new VCardFile
      {
        Contents = new UTF8Encoding(false).GetBytes(Serialize(card, photoBase64)),
        ContentType = "text/vcard",
        FileName = BuildFileName(card),
      };
    }

    public string Serialize(VCard card, string photoBase64 = null)
    {
      if (card == null)
        throw new ArgumentNullException(nameof(card));

      return new StringBuilder()
        .AppendLine("BEGIN:VCARD")
        .AppendLine("VERSION:3.0")
        .AppendLine($"N:{card.LastName.Escape()};{card.FirstName.Escape()};;;")
        .AppendLine($"FN:{GetDisplayName(card).Escape()}")
        .AppendLineIfValue("ORG:", card.Organization)
        .AppendLineIfValue("TITLE:", card.JobTitle)
        .AppendLineIfAny(
          $"ADR;TYPE=WORK,PREF:;;{card.StreetAddress.Escape()};{card.City.Escape()};{card.Region.Escape()};{card.Zip.Escape()};{card.CountryName.Escape()}",
          card.StreetAddress,
          card.City,
          card.Region,
          card.Zip,
          card.CountryName
        )
        .AppendLineIfValue("TEL;TYPE=WORK,VOICE:", card.Phone)
        .AppendLineIfValue("X-MS-TEL;TYPE=VOICE,COMPANY:", card.PhoneCompany)
        .AppendLineIfValue("TEL;TYPE=CELL,VOICE:", card.Mobile)
        .AppendLineIfValue("EMAIL;TYPE=PREF,INTERNET:", card.Email)
        .AppendLineIfValue("URL;TYPE=WORK:", card.Url)
        .AppendLineIfRawValue(
          $"PHOTO;ENCODING=b;TYPE={GetPhotoType(card)}:",
          (photoBase64 ?? card.PhotoBase64)?.Trim()
        )
        .AppendLine("END:VCARD")
        .ToString();
    }

    public async Task<string> DownloadPhotoAsync(string absoluteUrl)
    {
      if (string.IsNullOrWhiteSpace(absoluteUrl))
        return null;

      try
      {
        using (var client = new HttpClient())
          return Convert.ToBase64String(
            await client.GetByteArrayAsync(absoluteUrl)
          );
      }
      catch (Exception ex)
      {
        Log.Add($"Could not download the vCard photo from: {absoluteUrl}");
        Log.Exception(ex);
        return null;
      }
    }

    private static string GetDisplayName(VCard card)
    {
      var name = $"{card.FirstName} {card.LastName}".Trim();

      return string.IsNullOrWhiteSpace(name)
        ? card.Organization ?? "Contact"
        : name;
    }

    private static string GetPhotoType(VCard card)
      => string.IsNullOrWhiteSpace(card.PhotoType)
        ? "JPEG"
        : card.PhotoType.ToUpperInvariant();

    private static string BuildFileName(VCard card)
    {
      var requested = string.IsNullOrWhiteSpace(card.FileName)
        ? GetDisplayName(card)
        : card.FileName.Trim();

      var invalidCharacters = Path.GetInvalidFileNameChars();
      var safeName = new string(
          requested
            .Where(character => !invalidCharacters.Contains(character))
            .ToArray()
        )
        .Trim()
        .Trim('.');

      if (string.IsNullOrWhiteSpace(safeName))
        safeName = "contact";

      return safeName.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase)
        ? safeName
        : $"{safeName}.vcf";
    }

  }
}

