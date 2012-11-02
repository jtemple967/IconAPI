using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;	
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml;	

namespace IconCMO
{
	public static class HttpRequestHandler
	{
		public static HttpWebResponse SendRequest(string httpRequest)
		{
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://secure3.iconcmo.com/api/");
			request.ContentType = "application/xml";
			request.KeepAlive = false;
			request.ProtocolVersion = HttpVersion.Version10;
			request.Method = "POST";
			byte[] postBytes = Encoding.ASCII.GetBytes(httpRequest);
			request.ContentLength = postBytes.Length;
			ServicePointManager.ServerCertificateValidationCallback = Validator;
			Stream requestStream = null;
			try
			{
				requestStream = request.GetRequestStream();
			}
			catch (WebException e)
			{
				
			}
			requestStream.Write(postBytes, 0, postBytes.Length);
			requestStream.Close();
			
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			return response;
		}
		
		public static bool Validator(object sender, X509Certificate certificate, 
		                             X509Chain chain, SslPolicyErrors sslErrs)
		{
			return true;
		}
		
		public static string BuildRequest(string requestData, IconFilter _filter, IconAuth _auth, IconSort _sort)
		{
			string filter, sort;
			if (_filter == null)
				filter = string.Empty;
			else
				filter = _filter.GetFilterXML();
			
			if (_sort == null)
				sort = string.Empty;
			else
				sort = _sort.GetSortXML();
	
			return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
					"<Icon>" +
					_auth.GetAuthXML() +
					"<Request>" +
						requestData + 
						filter + 
						sort +
					"</Request>" +
					"</Icon>";
		}
		
		public static void GetSession(XmlDocument doc, ref IconAuth _auth)
		{
			XmlNode node = doc.SelectSingleNode("/iconresponse/session");
			_auth.Session = node.InnerText;
		}
		
	}
	
	public class ImageDownloader
	{
		private Collection<string> imageList;
		private string localImageCache;
		
		public ImageDownloader(Collection<string> _imageList, string _localImageCache)
		{
			imageList = _imageList;
			localImageCache = _localImageCache;
		}
		
		public void DownloadImages()
		{
			WebClient client = new WebClient();
			foreach (string image in imageList)
			{
				string targetFile = localImageCache + Path.DirectorySeparatorChar + Path.GetFileName(image);
				if (!File.Exists(targetFile))
					client.DownloadFile(new Uri(image), targetFile);
			}
		}
	}
				
}

