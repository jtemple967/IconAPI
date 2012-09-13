using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Xml;

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
	
	public class Permissions
	{
		private Collection<Permission> permissions;
		private string role;
		private XmlDocument xmlDoc;
		private string httpRequest;
		
		public Permissions(IconAuth auth)
		{
			xmlDoc = new XmlDocument();
			permissions = new Collection<Permission>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>user</Module>" +
														"<Section>permissions</Section>",
														null,
														auth);
			// Send the request
			HttpWebResponse response = HttpRequestHandler.SendRequest(httpRequest);
			// Load the response into an XML document
			xmlDoc.Load(new StreamReader(response.GetResponseStream()));
			// Extract the security token from the response
			HttpRequestHandler.GetSession(xmlDoc, ref auth);
			// Load the permissions
			LoadPermissions();
			// Get the role
			XmlNode wrkNode = xmlDoc.SelectSingleNode("role");
			role = wrkNode.Value;
		}
		
		public Collection<Permission> Entries
		{
			get { return permissions; }
		}
		
		public string Role
		{
			get { return role; }
		}
		
		private void LoadPermissions()
		{
			// Get the permission nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/Permissions");
			foreach (XmlNode node in nodes)
			{
				// Create a permission entry and add to the collection
				Permission entry = new Permission();
				entry.Module = node.Name;
				XmlNodeList sectionNodes = node.ChildNodes;
				foreach (XmlNode sectionNode in sectionNodes)
				{
					entry.Section = sectionNode.Name;
					XmlNode wrkNode = sectionNode.SelectSingleNode("create");
					bool wrkBool = false;
					bool.TryParse(wrkNode.Value, out wrkBool);
					entry.Create = wrkBool;
					wrkNode = sectionNode.SelectSingleNode("read");
					bool.TryParse(wrkNode.Value, out wrkBool);
					entry.Read = wrkBool;
					wrkNode = sectionNode.SelectSingleNode("update");
					bool.TryParse(wrkNode.Value, out wrkBool);
					entry.Update = wrkBool;
					wrkNode = sectionNode.SelectSingleNode("delete");
					bool.TryParse(wrkNode.Value, out wrkBool);
					entry.Delete = wrkBool;
				}
			}
		}
	}
					
	
	public class Permission
	{
		public string Module { get; set; }
		public string Section { get; set; }
		public bool Create { get; set; }
		public bool Read { get; set; }
		public bool Update { get; set; }
		public bool Delete { get; set; }
		
		public Permission(string _module = "", string _section = "", bool _create = false, 
							bool _read = false, bool _update = false, bool _delete = false)
		{
			Module = _module;
			Section = _section;
			Create = _create;
			Read = _read; 
			Update = _update;
			Delete = _delete;
		}
	}
	
}

