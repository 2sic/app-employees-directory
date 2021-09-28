// Add namespaces to enable security in Oqtane & Dnn despite the differences
#if NETCOREAPP
using Microsoft.AspNetCore.Authorization; // .net core [AllowAnonymous] & [Authorize]
using Microsoft.AspNetCore.Mvc;           // .net core [HttpGet] / [HttpPost] etc.
#else
using System.Web.Http;		// this enables [HttpGet] and [AllowAnonymous]
using DotNetNuke.Web.Api;	// this is to verify the AntiForgeryToken
#endif
using System.Linq;
using System.Xml;
using System.IO;
using System;
using ToSic.Razor.Blade;

[AllowAnonymous]			// define that all commands can be accessed without a login
public class PeopleDictionaryController : Custom.Hybrid.Api12
{
  	private const string PhotoPathBase = "~";

    [HttpGet]
    public void VCard()
	{
		var card = new VCardModel
		{
			FirstName = CmsContext.Page.Parameters["FirstName"],
			LastName = CmsContext.Page.Parameters["LastName"],
			Organization = CmsContext.Page.Parameters["Organization"],
			JobTitle = CmsContext.Page.Parameters["JobTitle"],
			StreetAddress = CmsContext.Page.Parameters["Address"],
			Zip = CmsContext.Page.Parameters["Zip"],
			City = CmsContext.Page.Parameters["City"],
			CountryName = CmsContext.Page.Parameters["Country"],
			Phone = CmsContext.Page.Parameters["Phone"],
			PhoneCompany = CmsContext.Page.Parameters["PhoneCompany"],
			Mobile = CmsContext.Page.Parameters["Mobile"],
			Email = CmsContext.Page.Parameters["Email"],
			Url = CmsContext.Page.Parameters["Url"]
		};
		if (!string.IsNullOrWhiteSpace(CmsContext.Page.Parameters["Photo"]))
			card.PhotoPath = context.Server.MapPath(PhotoPathBase + CmsContext.Page.Parameters["Photo"]);

		var response = context.Response;
		response.ContentType = "text/vcard";
		var fileName = card.FirstName + " " + card.LastName;
		if (string.IsNullOrWhiteSpace(fileName))
			fileName = card.Organization;
		if (string.IsNullOrWhiteSpace(fileName))
			fileName = "contact";
		fileName += ".vcf";


		// source: http://stackoverflow.com/questions/93551/how-to-encode-the-filename-parameter-of-content-disposition-header-in-http
		string contentDisposition;
		if (context.Request.Browser.Browser == "IE")	// removed version check
			contentDisposition = "attachment; filename=" + Uri.EscapeDataString(fileName);
		else if (context.Request.Browser.Browser == "Safari")
			contentDisposition = "attachment; filename=" + fileName;
		else
			contentDisposition = "attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName);
		response.AddHeader("Content-Disposition", contentDisposition);

		//response.AddHeader("Content-Disposition", "attachment; fileName=" + fileName + ".vcf");

		var cardString = card.ToString();
		var inputEncoding = Encoding.Default;
		var outputEncoding = Encoding.GetEncoding(28591);
		var cardBytes = inputEncoding.GetBytes(cardString);
		var outputBytes = Encoding.Convert(inputEncoding, outputEncoding, cardBytes);

		response.OutputStream.Write(outputBytes, 0, outputBytes.Length);
	}

	public bool IsReusable { get { return false; } }

	internal class VCardModel
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
				builder.AppendLine(CreateThumbnail(PhotoPath, 92, 92));
				builder.AppendLine(string.Empty);
			}

			builder.AppendLine("END:VCARD");

			return builder.ToString();
		}
	}

	internal static string CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
	{
		var loBmp = new Bitmap(lcFilename);

		int lnNewWidth;
		int lnNewHeight;
		// If the image is smaller than a thumbnail
		if (loBmp.Width < lnWidth && loBmp.Height < lnHeight)
		{
			lnNewWidth = loBmp.Width;
			lnNewHeight = loBmp.Height;
		}
		else
		{
			decimal lnRatio;
			if (loBmp.Width > loBmp.Height)
			{
				lnRatio = (decimal)lnWidth / loBmp.Width;
				lnNewWidth = lnWidth;
				var lnTemp = loBmp.Height * lnRatio;
				lnNewHeight = (int)lnTemp;
			}
			else
			{
				lnRatio = (decimal)lnHeight / loBmp.Height;
				lnNewHeight = lnHeight;
				var lnTemp = loBmp.Width * lnRatio;
				lnNewWidth = (int)lnTemp;
			}
		}

		var bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
		var g = Graphics.FromImage(bmpOut);
		g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
		g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
		g.DrawImage(loBmp, 0, 0, lnNewWidth, lnNewHeight);
		loBmp.Dispose();

		byte[] byteArray;
		using (var stream = new MemoryStream())
		{
			// Save the bitmap as a JPG file with zero quality level compression.
			// Source: http://msdn.microsoft.com/en-us/library/bb882583.aspx
			var myEncoderParameters = new EncoderParameters(1);
			var myEncoder = System.Drawing.Imaging.Encoder.Quality;
			var myEncoderParameter = new EncoderParameter(myEncoder, 93L);
			myEncoderParameters.Param[0] = myEncoderParameter;

			var jgpEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
			bmpOut.Save(stream, jgpEncoder, myEncoderParameters);
			stream.Close();

			byteArray = stream.ToArray();
		}

		return Convert.ToBase64String(byteArray);
	}
}
