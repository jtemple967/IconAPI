using System;

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
	
	
}

