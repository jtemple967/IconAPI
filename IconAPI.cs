using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Data;

namespace IconAPI
{
	
	public class IconAuth
	{
		private string phone;
		private string userName;
		private string password;
		private string session;
		
		public IconAuth(string _phone, string _userName, string _password)
		{
			phone = _phone;
			userName = _userName;
			password = _password;
			session = string.Empty;
		}
		
		public string Phone
		{
			get { return phone; }
			set { 
					phone = value; 
					session = string.Empty;
				}
		}
		
		public string UserName
		{
			get { return userName; }
			set { 	
					userName = value; 
					session = string.Empty;
				}
		}
		
		public string Password
		{
			get { return password; }
			set { 	
					password = value; 
					session = string.Empty;
				}
		}
		
		public string Session
		{
			get { return session; }
			set { 
					session = value; 
				}
		}
		
		public string GetAuthXML()
		{
			string auth = string.Empty;

			auth = "<Auth>";
			if (session != string.Empty)
				auth += "<Session>" + session + "</Session>";
			else
			{
				auth += "<Phone>" + phone + "</Phone>" + 
					"<Username>" + userName + "</Username>" +
					"<Password>" + password + "</Password>";
			}
			auth += "</Auth>";
			
			return auth;	
		}
	}
	
	public class Directoryindex
	{
		private IconAuth auth;
		private DataSet directoryDS;
		private string httpRequest;
		
		public DataSet Directory
		{
			get { return directoryDS; }
		}
		
		public static bool Validator(object sender, X509Certificate certificate, 
		                             X509Chain chain, SslPolicyErrors sslErrs)
		{
			return true;
		}
		
		public Directoryindex(IconAuth _auth)
		{
			auth = _auth;
			directoryDS = new DataSet();
			
			// Form the http request
			httpRequest = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
							"<Icon>" +
							auth.GetAuthXML() +
							"<Request>" +
								"<Module>membership</Module>" +
								"<Section>directoryindex</Section>" +
							"</Request>" +
							"</Icon>";

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
			directoryDS.ReadXml(new StreamReader(response.GetResponseStream()));
			
		}
	
	}
}

