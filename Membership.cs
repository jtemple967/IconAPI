using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Xml;

namespace IconCMO
{
	
	public class DirectoryIndex
	{
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<DirectoryIndexEntry> entries;
			
		public DirectoryIndex(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			auth = _auth;
			filter = _filter;
			sort = _sort;
			xmlDoc = new XmlDocument();
			entries = new Collection<DirectoryIndexEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>directoryindex</Section>",
														filter,
														auth,
														sort);
			// Send the request
			HttpWebResponse response = HttpRequestHandler.SendRequest(httpRequest);
			// Load the response into an XML document
			xmlDoc.Load(new StreamReader(response.GetResponseStream()));
			// Extract the security token from the response
			HttpRequestHandler.GetSession(xmlDoc, ref auth);
			// Load the directory index entries
			LoadIndexEntries();
			
		}

		public DirectoryIndex(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public Collection<DirectoryIndexEntry> Entries
		{
			get { return entries; }
		}
		
		private void LoadIndexEntries()
		{
			// Get the directoryindex nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/directoryindex");
			foreach (XmlNode node in nodes)
			{
				// Create a directory index entry and add to the collection
				DirectoryIndexEntry entry = new DirectoryIndexEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("status");
				entry.Status = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("first_name");
				entry.FirstName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("last_name");
				entry.LastName = wrkNode.InnerText;
				// Get the members for the directory index entry
				XmlNodeList memberNodes = node.SelectNodes("members");
				foreach (XmlNode memberNode in memberNodes)
				{
					DirectoryIndexMember member = new DirectoryIndexMember();
					wrkNode = node.SelectSingleNode("id");
					member.Id = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("status");
					member.Status = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("first_name");
					member.FirstName = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("last_name");
					member.LastName = wrkNode.InnerText;
					entry.Members.Add(member);
				}
				entries.Add(entry);
			}
			
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
	}
	
	public class DirectoryIndexEntry
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Collection<DirectoryIndexMember> Members { get; set; }
		
		public DirectoryIndexEntry()
		{
			Id = string.Empty;
			Status = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			Members = new Collection<DirectoryIndexMember>();
		}
		
	}
	
	public class DirectoryIndexMember
	{
		public string Id {get; set;}
		public string Status { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public DirectoryIndexMember()
		{
			Id = string.Empty;
			Status = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
		}
		
	}
	
	public class Directory
	{
				
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<DirectoryEntry> entries;
		
		public Directory(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			
			auth = _auth;
			filter = _filter;
			sort = _sort;
			xmlDoc = new XmlDocument();
			entries = new Collection<DirectoryEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>directory</Section>",
														filter,
														auth,
														sort);
			// Send the request
			HttpWebResponse response = HttpRequestHandler.SendRequest(httpRequest);
			// Load the response into an XML document
			xmlDoc.Load(new StreamReader(response.GetResponseStream()));
			// Extract the security token from the response
			HttpRequestHandler.GetSession(xmlDoc, ref auth);
			// Load the directory entries
			LoadEntries();
			
		}

		public Directory(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public Collection<DirectoryEntry> Entries
		{
			get { return entries; }
		}
		
		private void LoadEntries()
		{
			// Get the directoryindex nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/directoryindex");
			foreach (XmlNode node in nodes)
			{
				// Create a directory index entry and add to the collection
				DirectoryEntry entry = new DirectoryEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("status");
				entry.Status = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("first_name");
				entry.FirstName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("last_name");
				entry.LastName = wrkNode.InnerText;
				// Get the members for the directory index entry
				XmlNodeList memberNodes = node.SelectNodes("members");
				foreach (XmlNode memberNode in memberNodes)
				{
					DirectoryMember member = new DirectoryMember();
					wrkNode = node.SelectSingleNode("id");
					member.Id = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("status");
					member.Status = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("first_name");
					member.FirstName = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("last_name");
					member.LastName = wrkNode.InnerText;
					entry.Members.Add(member);
				}
				entries.Add(entry);
			}
			
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
	}
	
	public class DirectoryEntry
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MailTo { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Email { get; set; }
		public bool EmailUnlisted { get; set; }
		public bool Permission { get; set; }
		public string Phone { get; set; }
		public bool PhoneUnlisted { get; set; }
		public Collection<DirectoryMember> Members { get; set; }
		public Collection<DirectoryPhone> Phones { get; set; }
		public Collection<DirectoryEmail> Emails { get; set; }
		
		public DirectoryEntry()
		{
			Id = string.Empty;
			Status = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			Members = new Collection<DirectoryMember>();
			Phones = new Collection<DirectoryPhone>();
			Emails = new Collection<DirectoryEmail>();
		}
		
	}
	
	public class DirectoryPhone 
	{
		public string Id { get; set; }
		public string Phone { get; set; }
		public string Extension { get; set; }
		public string Provider { get; set; }
		public bool PhoneUnlisted { get; set; }
		
		public DirectoryPhone()
		{
			Id = string.Empty;
			Phone = string.Empty;
			Extension = string.Empty;
			Provider = string.Empty;
			PhoneUnlisted = false;
		}
	
	}
	
	public class DirectoryEmail
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public bool EmailUnlisted { get; set; }
		public DirectoryEmail()
		{
			Id = string.Empty;
			Email = string.Empty;
			EmailUnlisted = false;
		}
	}

	public class DirectoryMember
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string WorkPhone { get; set; }
		public string WorkPhoneExtension { get; set; }
		public bool WorkPhoneUnlisted { get; set; }
		public DateTime BirthDate { get; set; }
		public bool Primary { get; set; }
		public string Picture { get; set; }
		public string Thumbnail { get; set; }
		public Collection<DirectoryPhone> Phones { get; set; }
		public Collection<DirectoryEmail> Emails { get; set; }
		
		public DirectoryMember()
		{
			Id = string.Empty;
			Status = string.Empty;	
			Title = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			WorkPhone = string.Empty;
			WorkPhoneExtension = string.Empty;
			WorkPhoneUnlisted = false;
			BirthDate = DateTime.MinValue;
			Primary = false;
			Picture = string.Empty;
			Thumbnail = string.Empty;
			Phones = new Collection<DirectoryPhone>();
			Emails = new Collection<DirectoryEmail>();
		}
	}
			
}

